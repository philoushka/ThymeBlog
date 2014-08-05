using System;
using Thyme.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Thyme.Web.Models;
using Newtonsoft.Json;
using Thyme.Tests.JsonSamples;
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
            var newposts = gh.GetItemsForBranchCommit(posted);
            
            CacheState cache = new CacheState();
            cache.RemovePostsByName(posted.RemovedPosts);
            cache.AddPostsToCache(newposts);
            cache.SetCurrentBranchSha(posted.after);


        }
    }
}
