using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thyme.Web.Helpers;
using Thyme.Web.ViewModels;

namespace Thyme.Web.Controllers
{
    public class StatsController : Controller
    {

        public ActionResult BloggingStats()
        {
            var vm = new BloggingStats_vm { Statistics = BuildStats() };
            return PartialView("_MyStats", vm);
        }

        public Dictionary<string, string> BuildStats()
        {
            var stats = BlogStatistics.BuildFooterStats();
            return stats;
        }

    }
}