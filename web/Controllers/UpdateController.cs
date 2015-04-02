using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Thyme.Web.Data;
using Thyme.Web.Helpers;
using Thyme.Web.Models;

namespace Thyme.Web.Controllers
{
    [HandleError]
    public class UpdateController : Controller
    {
        [HttpPost]
        public async Task<ActionResult> UpdateCacheFromGitHub()
        {
            string postedJson = string.Empty;

            try
            {
                using (var streamReader = new System.IO.StreamReader(Request.InputStream))
                {
                    postedJson = streamReader.ReadToEnd();
                }
                GitHubPostedCommit posted = JsonConvert.DeserializeObject<GitHubPostedCommit>(postedJson);
                IEnumerable<BlogPost> newposts = Enumerable.Empty<BlogPost>();
                //try
                //{
                    var gh = new Data.GitHub();
                    newposts = await gh.GetItemsForBranchCommit(posted);

                    var localFileCache = new LocalFileCache();

                    //delete items if any.
                    foreach (string blogUrlSlug in posted.RemovedPosts)
                    {
                        localFileCache.RemovePost(blogUrlSlug);
                    }
                //}
                //catch (Exception) { }
                
                try
                {
                    await SyncAzureIndex(newposts, posted.RemovedPosts);
                }
                catch (Exception) { }


                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
            }

        }
        public async Task SyncAzureIndex(IEnumerable<BlogPost> newOrUpdates, IEnumerable<string> deletedPostSlugs)
        {
            var azureIndexer = new BlogPostSearchIndex(Config.AzureSearchService, Config.AzureSearchApiKey);
            await azureIndexer.AddToIndex(newOrUpdates.Select(x => new IndexBlogPost { Id = x.UrlSlug, BlogPostBody = x.Body }).ToArray());
            await azureIndexer.RemoveFromIndex(deletedPostSlugs.ToArray());
        }
    }
}