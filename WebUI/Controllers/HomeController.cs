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
        // GET: Home
        public ActionResult Index()
        {
            var abc = GeorgianABC.LettersDictionary;

            return View("Index", abc);
        }

        public ActionResult LearnLetter(int id)
        {
            if (!GeorgianABC.IsValidLetterIndex(id))
            {
                return RedirectToAction("LearnLetter", new { id = 1 });
            }

            GeorgianLetter letter = GeorgianABC.GetLetterByLearnIndex(id);
            return View("LearnLetter", letter);
        }

        [HttpGet]
        public ActionResult Translate(int id) // public ActionResult LetterRemembered([DefaultValue(0)] int id)
        {
            if (!GeorgianABC.IsValidLetterIndex(id))
            {
                return RedirectToAction("Translate", new { id = 1 });
            }
            
            return View("Translate", (object)GeorgianABC.GetFirstWordToTranslateForLetter(id).ToKhucuri());
            
        }

        [HttpPost]
        public ActionResult Translate(int id, string hdnKhucuri, string tbTranslation)
        {
            if (tbTranslation.ToKhucuri() == hdnKhucuri)
            {
                TempData["Result"] = "correct";
            }
            ModelState.Clear();
            return View("Translate", (object)GeorgianABC.GetRandomWordToTranslateForLetter(id).ToKhucuri());
        }
    }
}