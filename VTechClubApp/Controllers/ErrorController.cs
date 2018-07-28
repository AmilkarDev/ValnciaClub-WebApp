using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VTechClubApp.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult GoLogin()
        {
            return RedirectToAction("Login", "Account");
        }

        public ActionResult http404()
        {
            return View("Http404");
        }
        public ActionResult http405()
        {
            return View();
        }
        public ActionResult general()
        {
            return View();
        }

    }
}