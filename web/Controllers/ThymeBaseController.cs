using System;
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
            ViewBag.SiteName = Config.BlogSiteName;
            ViewBag.MyName = Config.MyName;
            ViewBag.GitHubAccountName = Config.GitHubOwner;
            ViewBag.GoogleAnalyticsAccountNumber = Config.GoogleAnalyticsAccountNumber;
            ViewBag.GooglePlusAccountNumber = Config.GooglePlusAccountNumber;
            ViewBag.StackOverflowUserNumber = Config.StackOverflowUserNumber;
            ViewBag.FunQuote = Config.Quotes[(new Random()).Next(0, Config.Quotes.Length)];
        }
    }
}