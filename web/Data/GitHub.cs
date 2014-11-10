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

        public async Task<IEnumerable<BlogPost>> GetItemsForBranchCommit(GitHubPostedCommit posted)
        {
            var commit = await GetCommit(posted.commits.First().id);
            var tree = await GetTree(commit.Tree.Sha);
            var itemsToKeep = posted.commits.First().ItemsToKeep.ToList();
            var posts = new List<BlogPost>();
            foreach (TreeItem item in tree.Tree.Where(x => itemsToKeep.Contains(x.Path)))
            {
                var blob = await GetBlobContents(item.Sha);
                byte[] blogPostFileBytes = Convert.FromBase64String(blob.Content);
                EnsureExistsOnDisk(new DiskSaveItem { FileContents = blogPostFileBytes, SubDirectory = "", FileName = item.Path });

                var post = await ConvertTreeItemToBlogPost(item);
                posts.Add(post);
            }
       
            return posts;
        }

        public async Task<Commit> GetCommit(string commitSha)
        {
            var commitClient = new CommitsClient(apiConn);
            var commit = await commitClient.Get(Config.GitHubOwner, Config.GitHubRepo, commitSha);
            return commit;
        }

        public async Task<TreeResponse> GetTree(string treeSha)
        {
            var treesClient = new TreesClient(apiConn);
            var tree = await treesClient.Get(Config.GitHubOwner, Config.GitHubRepo, treeSha);

            return tree;
        }

        /// <summary>
        /// Gets all the blog posts from GitHub. Saves them to disk.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<BlogPost>> SaveAllBlogPostsFromGitHubToDisk()
        {
            string masterTreeSha = await GetCurrentMasterSha();
            var tree = await GetTree(masterTreeSha);
            var posts = new List<BlogPost>();
            foreach (TreeItem item in tree.Tree.Where(x => x.Path.EndsWith(".md", StringComparison.CurrentCultureIgnoreCase)))
            {
                var blob = await GetBlobContents(item.Sha);
                byte[] blogPostFileBytes = Convert.FromBase64String(blob.Content);
                EnsureExistsOnDisk(new DiskSaveItem { FileContents = blogPostFileBytes, SubDirectory = "", FileName = item.Path });
                BlogPost post = await ConvertTreeItemToBlogPost(item);
                posts.Add(post);
            }

            await SaveTreeItemBlobsToDisk(tree);

            return posts;
        }

        public void EnsureExistsOnDisk(DiskSaveItem saveItem)
        {
            LocalFileCache local = new LocalFileCache();
            local.SaveLocalItem(saveItem);
        }

        public async Task SaveTreeItemBlobsToDisk(TreeResponse tree)
        {
            foreach (var subTree in tree.Tree.Where(x => x.Type == TreeType.Tree))
            {
                await SaveAllItemsFromTree(subTree);
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

                local.SaveLocalItem(new DiskSaveItem { FileContents = Convert.FromBase64String(blob.Content), SubDirectory = subTree.Path, FileName = item.Path });
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