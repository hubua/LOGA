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
#if DEBUG
            System.Threading.Thread.Sleep(2000);
#endif
            HttpContextStorage.SetUserSettings(HttpContext, settings);

            if (Request.IsAjaxRequest())
            {
                var data = new { HasDisplayName = HttpContextStorage.HasProgressSaved(HttpContext), DisplayName = HttpContextStorage.GetUserSettings(HttpContext).DisplayName };
                return Json(data); // JsonResult
            }
            else
            {
                return PartialView("UserSettingsPartial", settings); // TODO: return correct view when not js enabled
            }
        }

        public ActionResult DisplayAll()
        {
            GeorgianABC.Initialize(Server.MapPath(@"~\App_Data\")); // Re-initializing dictionary
            return View(GeorgianABC.LettersDictionary.OrderBy(item => item.Value.LearnOrder));
        }

        public ActionResult LogError()
        {
            throw new ApplicationException("Sample error message", new ApplicationException("Sample inner error message"));
        }
    }
}