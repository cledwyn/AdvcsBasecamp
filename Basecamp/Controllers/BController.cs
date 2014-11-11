using Basecamp.Helpers;
using Basecamp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Basecamp.Controllers
{
    public class BController : Controller
    {
        [Authorize]
        // GET: B
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated) { Response.Redirect("http://advancement.macalester.edu/M", true); }
            BCX bcx = new BCX(User.Identity.Name);
            return View(bcx);
        }
    }
}