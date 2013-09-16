using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thyme.Web.Models;
using Thyme.Web.ViewModels;

namespace Thyme.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult About() { return View("AboutMe"); }
        
    }
}