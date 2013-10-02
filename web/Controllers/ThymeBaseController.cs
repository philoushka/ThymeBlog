using System.Configuration;
using System.Web.Mvc;

namespace Thyme.Web.Controllers
{
    public class ThymeBaseController:Controller
    {

        public ThymeBaseController()
        {
            ViewBag.TwitterAccountName = ConfigurationManager.AppSettings["TwitterAcct"];
            ViewBag.SiteName = ConfigurationManager.AppSettings["BlogSiteName"];
            ViewBag.MyName = ConfigurationManager.AppSettings["MyName"];
        }
    }
}