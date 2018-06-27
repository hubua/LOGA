using LOGA.WebUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LOGA.WebUI.Controllers
{
    public class DownloadsController : Controller
    {

        public DownloadsController()
        {
            ViewData["ActiveMenu"] = "Downloads";
        }
        
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}