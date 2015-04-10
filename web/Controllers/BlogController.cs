using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml;
using Thyme.Web.Helpers;
using Thyme.Web.Models;
using Thyme.Web.ViewModels;
namespace Thyme.Web.Controllers
{
    [HandleError]
    public class BlogController : ThymeBaseController
    {
        public BlogController()
        {
            ViewBag.AllTags = GetTopBlogPostTags();
            ViewBag.RecentPosts = GetFeaturedRecentPosts();

        }

        public IEnumerable<string> GetTopBlogPostTags()
        {
            using (var repo = new BlogPostRepo())
            {
                var alltags = repo.PublishedPosts.SelectMany(x => x.Tags);

                var topTags = from t in alltags
                              group t by t into g
                              select new { Tag = g.Key, NumPosts = g.Count() };

                return topTags
                    .OrderByDescending(x => x.NumPosts)
                    .Take(10)
                    .Select(x => x.Tag).ToList();
            }
        }



        public ActionResult ListRecentPosts(bool showAll = false)
        {
            using (var repo = new BlogPostRepo())
            {
                int numPosts = showAll ? int.MaxValue : Helpers.Config.NumPostsFrontPage;
                var recents = repo.ListRecentBlogPosts(numPosts).ToList();
                return View("Front", new Front_vm { RecentBlogPosts = recents });
            }
        }

        public IEnumerable<BlogPost> GetFeaturedRecentPosts()
        {
            using (var repo = new BlogPostRepo())
            {
                return repo.ListRecentBlogPosts(3).ToList();
            }
        }

        public async Task<ActionResult> RefreshSearchIndex()
        {
            var azureIndexer = new BlogPostSearchIndex(Config.AzureSearchService, Config.AzureSearchApiKey);
            using (var blogPostRepo = new BlogPostRepo())
            {
                var blogPostsToIndex = blogPostRepo.PublishedPosts.Select(x => new IndexBlogPost { BlogPostBody = x.Body, Id = x.UrlSlug }).ToArray();
                await azureIndexer.AddToIndex(blogPostsToIndex);
            }
            return RedirectToRoute("Front");
        }

        /// <summary>
        /// IF the cache gets out of sync from the disk, call this to load all blog posts on disk to http cache.
        /// </summary>
        /// <returns></returns>
        public ActionResult SyncDiskToCache()
        {
            return RedirectToAction("ListRecentPosts", new { Refresh = true });
        }

        /// <summary>
        /// Force get all items from the Git repo.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ForceRepoRefresh()
        {
            using (var repo = new BlogPostRepo())
            {
                await repo.RefreshCachedBlogPosts();
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
            {
                return RedirectToRoute("SearchBlogPosts", new { keywords = searchTerms });
            }
            else
            {
                return RedirectToRoute("Front");
            }
        }

        [HttpGet]
        public async Task<ActionResult> SearchBlogPosts(string keywords)
        {
            keywords = keywords.RemoveSlugSeparators();
            ViewBag.SearchKeywords = keywords;
            var matchingPosts = new List<BlogPost>();
            var azureIndexer = new BlogPostSearchIndex(Config.AzureSearchService, Config.AzureSearchApiKey);

            using (var repo = new BlogPostRepo())
            {
                var results = await azureIndexer.SearchIndex(keywords);
                foreach (var result in results.OrderByDescending(x => x.Score))
                {
                    matchingPosts.Add(repo.GetPost(result.Document.Id));
                }
            }
            return View("SearchResults", new Search_vm { SearchKeywords = keywords, MatchingBlogPosts = matchingPosts });
        }


        [OutputCache(Duration = 21600, VaryByParam = "none")]
        public RssActionResult GenerateRSS()
        {
            using (var repo = new BlogPostRepo())
            {
                var items = repo.ListRecentBlogPosts(int.MaxValue).Select(x =>
                    new SyndicationItem(
                    title: x.Title,
                    content: x.Intro,
                    lastUpdatedTime: x.PublishedOn.Value,
                    id: GenerateBlogLink(x.UrlSlug),
                    itemAlternateLink: new Uri(GenerateBlogLink(x.UrlSlug)))
                    );

                SyndicationFeed feed = new SyndicationFeed
                {
                    Title = new TextSyndicationContent("{0}'s Blog".FormatWith(Thyme.Web.Helpers.Config.MyName)),
                    Description = new TextSyndicationContent("Blog feed for {0}".FormatWith(Request.Url.Host)),
                    Language = "en-us",
                    LastUpdatedTime = items.OrderByDescending(x => x.PublishDate).First().PublishDate.ToUniversalTime(),
                    Items = items,

                };
                feed.Links.Add(SyndicationLink.CreateAlternateLink(new Uri(GenerateBlogLink())));
                return new RssActionResult() { Feed = feed };
            }
        }

        public string GenerateBlogLink(string routeUrl = "")
        {
            if (routeUrl.HasValue())
            {
                routeUrl = Url.RouteUrl("BlogPost", new { slug = routeUrl });
            }
            return Request.Url.GetLeftPart(UriPartial.Authority) + routeUrl;
        }
    }

    public class RssActionResult : ActionResult
    {
        public SyndicationFeed Feed { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "application/rss+xml";

            Rss20FeedFormatter rssFormatter = new Rss20FeedFormatter(Feed);
            using (XmlWriter writer = XmlWriter.Create(context.HttpContext.Response.Output))
            {
                rssFormatter.WriteTo(writer);
            }
        }
    }
}