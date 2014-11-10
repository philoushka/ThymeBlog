using System.Web.Mvc;
using Thyme.Web.Helpers;

namespace Thyme.Web.Controllers
{
    [HandleError]
    public class ThymeBaseController : AsyncController
    {
        public ThymeBaseController()
        {
            ViewBag.TwitterAccountName = Config.TwitterAcct;
            ViewBag.SiteName = Config.BlogName;
            ViewBag.MyName = Config.MyName;
            ViewBag.GitHubAccountName = Config.GitHubOwner;
            ViewBag.GoogleAnalyticsAccountNumber = Config.GoogleAnalyticsAccountNumber;
            ViewBag.GooglePlusAccountNumber = Config.GooglePlusAccountNumber;
        }
    }
}