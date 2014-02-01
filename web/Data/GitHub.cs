using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Thyme.Web.Models;
using Thyme.Web.Helpers;
using System.IO;
using System.Text;
namespace Thyme.Web.Data
{
    public class GitHub
    {
        ApiConnection apiConn;
        public GitHub()
        {
            apiConn = new ApiConnection(new Connection(new ProductHeaderValue(Config.BlogName.CreateSlug())));
        }
        public async Task<IEnumerable<BlogPost>> GetAllBlogPosts()
        {
            var treesClient = new TreesClient(apiConn);
            string masterTreeSha = await GetMasterTreeSha();
            var tree = await treesClient.Get(Config.GitHubOwner, Config.GitHubRepo, masterTreeSha);

            return Task.WhenAll(tree.Tree.Select(x => await ConvertTreeItemToBlogPost(x)));
        }

        public async Task<BlogPost> ConvertTreeItemToBlogPost(TreeItem treeItem)
        {
            var blogPostBody = await GetBlobContents(treeItem.Sha);
            return new BlogPost
            {
                Body = Encoding.UTF8.GetString(Convert.FromBase64String(blogPostBody.Content)),
                FileName = Path.GetFileNameWithoutExtension(treeItem.Path),
                SHA = treeItem.Sha,
                Url = treeItem.Url.AbsoluteUri
            };
        }

        public async Task<Blob> GetBlobContents(string sha)
        {
            var blobClient = new BlobsClient(apiConn);
            return await blobClient.Get(Config.GitHubOwner, Config.GitHubRepo, sha);
        }

        public async Task<string> GetMasterTreeSha()
        {
            var client = new RepositoriesClient(apiConn);
            var branches = await client.GetAllBranches(Config.GitHubOwner, Config.GitHubRepo);
            return branches.First().Commit.Sha;
        }

    }
}