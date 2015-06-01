using System.Web.Mvc;
using Thyme.Web.Helpers;

namespace Thyme.Web.Controllers
{
    [HandleError]
    public class HomeController : ThymeBaseController
    {
        public ActionResult About()
        {
            return View();
        }

        public ViewResult Error() { return View(); }

        public ViewResult GitHubContributions() { return View(); }
    }
}