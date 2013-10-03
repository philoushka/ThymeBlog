using System.Web.Mvc;
using Thyme.Web.Models;
using Thyme.Web.ViewModels;

namespace Thyme.Web.Controllers
{
    public class BlogController : ThymeBaseController
    {
         
        public ActionResult Index()
        {
            using (var repo = new BlogPostRepo())
            {                
                return View("Front", new Front_vm { RecentBlogPosts = repo.ListRecentBlogPosts(Helpers.Config.NumPostsFrontPage) });
            }
        }

        public ActionResult ForceRepoRefresh()
        {
            
            var repo = new BlogPostRepo();
            repo.RefreshRepo();
            return RedirectToAction("Index");
        }
        public ActionResult ViewPost(string slug)
        {
            BlogPost bp;
            using (var repo = new BlogPostRepo())
            {
                bp = repo.GetPost(slug);
            }
            
            return View(new BlogPost_vm { BlogPost = bp });
        }

        [HttpPost]
        public ActionResult Search(string searchKeywords)
        {
            //TODO implement this with Post Redirect Get pattern
            return RedirectToAction("Search", new { keyphrase = searchKeywords });
        }

        [HttpGet]
        public ActionResult SearchPosts(string keywords)
        {
            //TODO implement this with Post Redirect Get pattern
            ViewBag.SearchKeywords = keywords;
            return View();
        }

    }
}