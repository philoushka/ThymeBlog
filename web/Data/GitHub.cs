using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
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
            apiConn = new ApiConnection(new Connection(new ProductHeaderValue(Config.BlogName.CreateSlug())));
            apiConn.Connection.Credentials = new Credentials(Config.GitHubOAuthToken);
        }

        public string GetCurrentMasterSha()
        {
            var masterTree = GetMasterTreeSha();
            Task.WaitAll(masterTree);
            return masterTree.Result;
        }

        public IEnumerable<BlogPost> GetAllBlogPosts()
        {
            string masterTreeSha = GetCurrentMasterSha();

            var treesClient = new TreesClient(apiConn);
            var tree = treesClient.Get(Config.GitHubOwner, Config.GitHubRepo, masterTreeSha);
            Task.WaitAll(tree);
            var tree2 = tree.Result;

            List<BlogPost> posts = new List<BlogPost>();
            foreach (var item in tree2.Tree.Where(x => x.Path.EndsWith(".md")))
            {
                posts.Add(ConvertTreeItemToBlogPost(item).Result);                
            }
            
            foreach (var subTree in tree2.Tree.Where(x => x.Type == TreeType.Tree))
            {
               Task.WaitAll(SaveAllItemsFromTree(subTree));
            }
            return posts;
        }

        public async Task SaveAllItemsFromTree(TreeItem subTree)
        {
            var treesClient = new TreesClient(apiConn);
            var tree = treesClient.Get(Config.GitHubOwner, Config.GitHubRepo, subTree.Sha);
            Task.WaitAll(tree);
            LocalFileCache local = new LocalFileCache();
            foreach (var item in tree.Result.Tree.Where(x=>local.LocalItemExists(subTree.Path, x.Path)==false))
            {                
                var blob = GetBlobContents(item.Sha);
                Task.WaitAll(blob);
                var blobItem = blob.Result;                
                local.SaveLocalItem(new SaveItem { Contents = Convert.FromBase64String(blobItem.Content), Dir = subTree.Path, FileName = item.Path });
            }
        }
            
        public async Task<BlogPost> ConvertTreeItemToBlogPost(TreeItem treeItem)
        {
            var blob = GetBlobContents(treeItem.Sha);
            Task.WaitAll(blob);
            var blobContents = blob.Result;

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