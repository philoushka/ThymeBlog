using System.Configuration;
using System.Web.Mvc;

namespace Thyme.Web.Controllers
{
    [HandleError]
    public class ThymeBaseController:AsyncController
    {

        public ThymeBaseController()
        {
            ViewBag.TwitterAccountName = ConfigurationManager.AppSettings["TwitterAcct"];
            ViewBag.SiteName = ConfigurationManager.AppSettings["BlogSiteName"];
            ViewBag.MyName = ConfigurationManager.AppSettings["MyName"];
        }
    }
}