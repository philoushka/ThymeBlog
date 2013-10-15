using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Thyme.Web.Models;
using Thyme.Web.ViewModels;

namespace Thyme.Web.Controllers
{
    [HandleError]
    public class BlogController : ThymeBaseController
    {
        public ActionResult ListRecentPosts(bool showAll = false)
        {
            using (var repo = new BlogPostRepo())
            {
                int numPosts = showAll ? int.MaxValue : Helpers.Config.NumPostsFrontPage;
                return View("Front", new Front_vm { RecentBlogPosts = repo.ListRecentBlogPosts(numPosts) });
            }
        }

        public ActionResult ForceRepoRefresh()
        {
            using (var repo = new BlogPostRepo())
            {
                repo.RefreshRepo();
                return RedirectToRoute("Front");
            }
        }
        public ActionResult ViewPost(string slug)
        {            
            using (var repo = new BlogPostRepo())
            {
                BlogPost bp = repo.GetPost(slug); 
                return View(new BlogPost_vm { BlogPost = bp });
            }
        }

        [HttpPost]
        public ActionResult PostSearch()
        {            
            string searchTerms = Request.Form["searchText"].ToString().Trim().CreateSlug();
            if (searchTerms.HasValue())
                return RedirectToRoute("SearchBlogPosts", new { keywords = searchTerms }); 
            else
                return RedirectToRoute("Front");
        }

        [HttpGet]
        public ActionResult SearchBlogPosts(string keywords)
        {
            using (var repo = new BlogPostRepo())
            {
                keywords = keywords.RemoveSlugSeparators();
                IEnumerable<BlogPost> matchingPosts = repo.SearchPosts(keywords.Split(' '));
                ViewBag.SearchKeywords = keywords;
                return View("SearchResults",new Search_vm { SearchKeywords = keywords, MatchingBlogPosts = matchingPosts });
            }
        }
    }
}