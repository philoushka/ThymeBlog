using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Linq;
using Thyme.Tests.JsonSamples;
using Thyme.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Thyme.Web.Data;
namespace Thyme.Tests
{
    [TestClass]
    public class GitHubUpdate
    {
        [TestMethod]
        public async Task CanProcessPostedWebhook()
        {

            GitHubPostedCommit posted = JsonConvert.DeserializeObject<GitHubPostedCommit>(GitHubJsonMessages.GitHubCommitValidMsg);

            var gh = new Thyme.Web.Data.GitHub();
            var newposts =  await gh.GetItemsForBranchCommit(posted);

            LocalFileCache lfc = new LocalFileCache();

            foreach (var blogPostSlug in posted.RemovedPosts)
            {
                lfc.RemovePost(blogPostSlug);
            }

            foreach (BlogPost blogPost in newposts)
            {
                lfc.SaveLocalItem(blogPost);
            }
            

        }
    }
}
