using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Thyme.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(name:"BlogPost",url: "blog/{slug}", defaults:new { controller="Home", action="ViewPost" , slug=""});
            routes.MapRoute("ForceRepoRefresh", "ForceRepoRefresh", new { controller = "Home", action = "ForceRepoRefresh" });
            routes.MapRoute("Front", "", new { controller = "Home", action = "Index" });
            routes.MapRoute("about", "about", new { controller = "Home", action = "About" });
             
            routes.MapRoute(name: "Default", url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
