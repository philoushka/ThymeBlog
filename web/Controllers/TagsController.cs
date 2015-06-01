using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Thyme.Web.Models;
using Thyme.Web.ViewModels;

namespace Thyme.Web.Controllers
{
    public class TagsController : Controller
    {
        public ActionResult GetTopTags()
        {
            var topTags = GetTopBlogPostTags();
            return PartialView("_Tags", topTags);
        }

        public IEnumerable<string> GetTopBlogPostTags()
        {
            using (var repo = new BlogPostRepo())
            {
                return repo.PublishedPosts
                        .SelectMany(x => x.Tags)
                         .GroupBy(x => x)
                         .Select(g => new
                         {
                             Tag = g.Key,
                             NumPosts = g.Count()
                         })
                         .OrderByDescending(x => x.NumPosts)
                         .Take(10)
                         .Select(x => x.Tag).ToList();
            }
        }

        public async Task<ActionResult> ListPostsForTag(string tag)
        {
            using (var repo = new BlogPostRepo())
            {
                var postsWithTag = repo.ListPostsWithTag(tag.Trim()).ToList();
                return View("PostsWithTag", new PostsWithTag_vm { Tag = tag.Trim(), Posts = postsWithTag });
            }
        }
    }
}