using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Linq;
using Thyme.Tests.JsonSamples;
using Thyme.Web.Models;

namespace Thyme.Tests
{
    [TestClass]
    public class GitHubUpdate
    {
        [TestMethod]
        public void CanProcessPostedWebhook()
        {

            GitHubPostedCommit posted = JsonConvert.DeserializeObject<GitHubPostedCommit>(GitHubJsonMessages.GitHubCommitValidMsg);

            var gh = new Thyme.Web.Data.GitHub();
            var newposts = gh.GetItemsForBranchCommit(posted).ToList();

            CacheState cache = new CacheState();
            cache.RemovePostsByName(posted.RemovedPosts);
            cache.AddPostsToCache(newposts);
            cache.SetCurrentBranchSha(posted.after);

        }
    }
}
