using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Basecamp.Models;

namespace Basecamp.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated) { Response.Redirect("http://advancement.macalester.edu/M", true); }
            BCX bcx = new BCX(User.Identity.Name);            
            return View(bcx);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}