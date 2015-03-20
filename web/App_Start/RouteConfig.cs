using System.Web.Mvc;
using System.Web.Routing;

namespace Thyme.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.RouteExistingFiles = true;
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("RssFeedRoot", "rss", new { controller = "Blog", action = "GenerateRSS" });
            routes.MapRoute("RssFeedBlog", "blog/rss", new { controller = "Blog", action = "GenerateRSS" });
            routes.MapRoute("BlogPost", "blog/{slug}", new { controller = "Blog", action = "ViewPost", slug = "" });
            routes.MapRoute("GitHubUpdated", "GitRepoUpdated", new { controller = "Update", action = "UpdateCacheFromGitHub" });
            routes.MapRoute("ForceRepoRefresh", "ForceRepoRefresh", new { controller = "Blog", action = "ForceRepoRefresh" });
            routes.MapRoute("SyncDiskToCache", "SyncDiskToCache", new { controller = "Blog", action = "SyncDiskToCache" });
            routes.MapRoute("Front", "", new { controller = "Blog", action = "ListRecentPosts" });
            routes.MapRoute("About", "about", new { controller = "Home", action = "About" });
            routes.MapRoute("AllPosts", "allposts", new { controller = "Blog", action = "ListRecentPosts", showAll = true });
            routes.MapRoute("PostSearch", "PostSearch", new { controller = "Blog", action = "PostSearch", searchKeywords = string.Empty });
            routes.MapRoute("RefreshSearch", "RefreshSearch", new { controller = "Blog", action = "RefreshSearchIndex" });
            routes.MapRoute("SearchBlogPosts", "search/{keywords}", new { controller = "Blog", action = "SearchBlogPosts", keywords = string.Empty });
            routes.MapRoute("Error", "Error", new { controller = "Home", action = "Error" });
            routes.MapRoute("GitHubContributions", "GitHubContributions", new { controller = "Home", action = "GitHubContributions" });
            routes.MapRoute(name: "Default", url: "{controller}/{action}/{id}",
                defaults: new { controller = "Blog", action = "ListRecentPosts" }
            );
        }
    }
}
