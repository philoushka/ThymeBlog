using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Thyme.Web.Data;
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

                var gh = new Data.GitHub();
                var newposts = await gh.GetItemsForBranchCommit(posted);
                var localFileCache = new LocalFileCache();

                //delete items if any.
                foreach (string blogUrlSlug in posted.RemovedPosts)
                {
                    localFileCache.RemovePost(blogUrlSlug);
                }

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
            }

        }
    }


}