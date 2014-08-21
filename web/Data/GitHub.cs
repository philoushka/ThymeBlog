using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thyme.Web.Helpers;
using Thyme.Web.Models;

namespace Thyme.Web.Data
{
    public class GitHub
    {
        ApiConnection apiConn;
        public GitHub()
        {
            apiConn = new ApiConnection(new Connection(new Octokit.ProductHeaderValue(Config.BlogName.CreateSlug())));
            apiConn.Connection.Credentials = new Credentials(Config.GitHubOAuthToken);
        }

        public async Task<string> GetCurrentMasterSha()
        {
            var masterTreeSha = await GetMasterTreeSha();
            return masterTreeSha;
        }

        public IEnumerable<BlogPost> GetItemsForBranchCommit(GitHubPostedCommit posted)
        {
            var commit = GetCommit(posted.commits.First().id);
            var tree = GetTree(commit.Tree.Sha);
            var itemsToKeep = posted.commits.First().ItemsToKeep.ToList();
            foreach (TreeItem item in tree.Tree.Where(x => itemsToKeep.Contains(x.Path)))
            {
                yield return ConvertTreeItemToBlogPost(item).Result;
            }

            SaveTreeItemBlobsToDisk(tree);
        }

        public Commit GetCommit(string commitSha)
        {
            var commitClient = new CommitsClient(apiConn);
            var commit = commitClient.Get(Config.GitHubOwner, Config.GitHubRepo, commitSha);
            Task.WaitAll(commit);
            return commit.Result;
        }

        public TreeResponse GetTree(string treeSha)
        {
            var treesClient = new TreesClient(apiConn);
            var tree = treesClient.Get(Config.GitHubOwner, Config.GitHubRepo, treeSha);
            Task.WaitAll(tree);
            return tree.Result;
        }

        public async Task<IEnumerable<BlogPost>> GetAllBlogPosts()
        {
            string masterTreeSha = await GetCurrentMasterSha();
            var tree = GetTree(masterTreeSha);
            var posts = new List<BlogPost>();
            foreach (TreeItem item in tree.Tree.Where(x => x.Path.EndsWith(".md", StringComparison.CurrentCultureIgnoreCase)))
            {
                var blob = await GetBlobContents(item.Sha);
                byte[] blogPostFileBytes = Convert.FromBase64String(blob.Content);
                EnsureExistsOnDisk(new SaveItem { FileContents = blogPostFileBytes, SubDirectory = "", FileName = item.Path });
                BlogPost post = await ConvertTreeItemToBlogPost(item);
                posts.Add(post);
            }

            SaveTreeItemBlobsToDisk(tree);

            return posts;
        }

        public void EnsureExistsOnDisk(SaveItem saveItem)
        {
            LocalFileCache local = new LocalFileCache();
            local.SaveLocalItem(saveItem);
        }

        public void SaveTreeItemBlobsToDisk(TreeResponse tree)
        {
            foreach (var subTree in tree.Tree.Where(x => x.Type == TreeType.Tree))
            {
                Task.WaitAll(SaveAllItemsFromTree(subTree));
            }
        }

        public async Task SaveAllItemsFromTree(TreeItem subTree)
        {
            var treesClient = new TreesClient(apiConn);
            var tree = await treesClient.Get(Config.GitHubOwner, Config.GitHubRepo, subTree.Sha);
            LocalFileCache local = new LocalFileCache();
            foreach (var item in tree.Tree.Where(x => local.LocalItemExists(subTree.Path, x.Path) == false))
            {
                var blob = await GetBlobContents(item.Sha);

                local.SaveLocalItem(new SaveItem { FileContents = Convert.FromBase64String(blob.Content), SubDirectory = subTree.Path, FileName = item.Path });
            }
        }

        public async Task<BlogPost> ConvertTreeItemToBlogPost(TreeItem treeItem)
        {
            var blob = await GetBlobContents(treeItem.Sha);

            string fileContents = Encoding.UTF8.GetString(Convert.FromBase64String(blob.Content));

            return BlogPostParsing.ConvertFileToBlogPost(treeItem.Path, fileContents, treeItem.Sha, treeItem.Url.AbsoluteUri);
        }

        public async Task<Blob> GetBlobContents(string sha)
        {
            var blobClient = new BlobsClient(apiConn);
            var blob = await blobClient.Get(Config.GitHubOwner, Config.GitHubRepo, sha);

            return blob;
        }

        public async Task<string> GetMasterTreeSha()
        {
            var client = new RepositoriesClient(apiConn);
            var branches = await client.GetAllBranches(Config.GitHubOwner, Config.GitHubRepo);

            return branches.First().Commit.Sha;
        }
    }
}