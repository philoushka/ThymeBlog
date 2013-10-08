using System.Web.Mvc;
using System.Web.Routing;

namespace Thyme.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("BlogPost", "blog/{slug}", new { controller="Blog", action="ViewPost" , slug=""});
            routes.MapRoute("ForceRepoRefresh", "ForceRepoRefresh", new { controller = "Blog", action = "ForceRepoRefresh" });
            routes.MapRoute("Front", "", new { controller = "Blog", action = "ListRecentPosts" });
            routes.MapRoute("About", "about", new { controller = "Home", action = "About" });
            routes.MapRoute("AllPosts", "allposts", new { controller = "Blog", action = "ListRecentPosts", showAll=true });
            routes.MapRoute("PostSearch", "PostSearch", new { controller = "Blog", action = "PostSearch", searchKeywords = string.Empty });
            routes.MapRoute("SearchBlogPosts", "search/{keywords}", new { controller = "Blog", action = "SearchBlogPosts", keywords = string.Empty });

            routes.MapRoute(name: "Default", url: "{controller}/{action}/{id}",
                defaults: new { controller = "Blog", action = "ListRecentPosts"}
            );
        }
    }
}
