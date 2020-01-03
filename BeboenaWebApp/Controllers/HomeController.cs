using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BeboenaWebApp.Services;
using Microsoft.AspNetCore.Hosting;
using BeboenaWebApp.Models;
using BeboenaWebApp;
using BeboenaWebApp.Helpers;

namespace BeboenaWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment hostingEnvironment;

        public HomeController(IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        // GET: Home
        public ActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        public ActionResult UpdateSettings(UserSettings settings) //TODO show progress spinner
        {
#if DEBUG
            System.Threading.Thread.Sleep(1000);
#endif
            HttpContextStorage.SetUserSettings(HttpContext, settings);

            var data = new { SaveLearnProgress = HttpContextStorage.GetUserSettings(HttpContext).SaveLearnProgress };
            return Json(data); // JsonResult
        }

        public ActionResult ReloadAll()
        {
            var dirinfo = hostingEnvironment.ContentRootFileProvider.GetFileInfo("/Services/Data");
            var dir = dirinfo.PhysicalPath;
            GeorgianABCService.Initialize(dir); // Re-initializing dictionary
            return View(GeorgianABCService.LettersDictionary.OrderBy(item => item.Value.LearnOrder));
        }

        public ActionResult LogError()
        {
            throw new ApplicationException("Sample error message", new ApplicationException("Sample inner error message"));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
