using System.Configuration;
using System.Web.Mvc;

namespace Thyme.Web.Controllers
{
    public class HomeController : ThymeBaseController
    {
        public ActionResult About() {
            ViewBag.StackOverflowUserNumber = ConfigurationManager.AppSettings["StackOverflowUserNumber"];
            return View(); 
        }

        public ViewResult Error()
        {
            return View();
        }
    }
}