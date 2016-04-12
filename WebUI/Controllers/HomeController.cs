using LOGA.WebUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LOGA.WebUI.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
            ViewData["ActiveMenu"] = "Home";
        }

        // GET: Home
        public ActionResult Index()
        {
            


            return View("Index");
        }

        [HttpPost]
        public ActionResult UpdateSettings(UserSettings settings)
        {
            HttpContextStorage.SetUserSettings(HttpContext, settings);
            return PartialView("UserSettingsPartial", settings);
        }

        public ActionResult LogError()
        {
            throw new ApplicationException("Sample error message", new ApplicationException("Sample inner error message"));
        }
    }
}