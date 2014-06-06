using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
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

        public string GetCurrentMasterSha()
        {
            var masterTreeSha = GetMasterTreeSha();
            Task.WaitAll(masterTreeSha);
            return masterTreeSha.Result;
        }

        public IEnumerable<BlogPost> GetItemsForBranchCommit(GitHubPostedCommit posted)
        {
            var commit = GetCommit(posted.commits.First().id);
            var tree = GetTree(commit.Tree.Sha);
            foreach (var item in tree.Tree.Where(x => posted.commits.First().ItemsToKeep.Contains(x.Path)))
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

        public IEnumerable<BlogPost> GetAllBlogPosts()
        {
            string masterTreeSha = GetCurrentMasterSha();
            var tree = GetTree(masterTreeSha);

            foreach (var item in tree.Tree.Where(x => x.Path.EndsWith(".md")))
            {
                yield return ConvertTreeItemToBlogPost(item).Result;
            }

            SaveTreeItemBlobsToDisk(tree);
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

                local.SaveLocalItem(new SaveItem { Contents = Convert.FromBase64String(blob.Content), Dir = subTree.Path, FileName = item.Path });
            }
        }

        public async Task<BlogPost> ConvertTreeItemToBlogPost(TreeItem treeItem)
        {
            var blob = await GetBlobContents(treeItem.Sha);
            var blobContents = blob;

            string fileContents = Encoding.UTF8.GetString(Convert.FromBase64String(blobContents.Content));

            BlogPostMetaProperties metaProps = BlogPostParsing.ParseValuesFromComment(fileContents);

            return new BlogPost
            {
                Body = BlogPostParsing.RemovePostHeader(fileContents),
                FileName = Path.GetFileNameWithoutExtension(treeItem.Path),
                SHA = treeItem.Sha,
                Url = treeItem.Url.AbsoluteUri,
                Title = metaProps.Title,
                Intro = metaProps.Intro,
                PublishedOn = (metaProps.PublishedOn.HasValue()) ? DateTime.Parse(metaProps.PublishedOn) : new Nullable<DateTime>(),
                UrlSlug = Path.GetFileNameWithoutExtension(treeItem.Path)
            };
        }

        public async Task<Blob> GetBlobContents(string sha)
        {
            var blobClient = new BlobsClient(apiConn);
            var blob = blobClient.Get(Config.GitHubOwner, Config.GitHubRepo, sha);
            Task.WaitAll(blob);
            return blob.Result;
        }

        public async Task<string> GetMasterTreeSha()
        {
            var client = new RepositoriesClient(apiConn);
            var branches = client.GetAllBranches(Config.GitHubOwner, Config.GitHubRepo);
            Task.WaitAll(branches);
            return branches.Result.First().Commit.Sha;
        }
    }
}