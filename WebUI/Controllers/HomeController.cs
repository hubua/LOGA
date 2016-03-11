using LOGA.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LOGA.WebUI.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var abc = GeorgianABC.LettersIndex;

            return View("Index", abc);
        }

        public ActionResult Learn(int id)
        {
            var abc = GeorgianABC.LettersIndex;

            if (id < 1 || id > abc.Count)
            {
                id = 1;
            }

            return View("Learn", abc.ToList()[id].Value);
        }
    }
}