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

        public ActionResult Learn(int id)
        {
            if (!GeorgianABC.IsValidLetterIndex(id))
            {
                return RedirectToAction("Learn", new { id = 1 });
            }

            GeorgianLetter letter = GeorgianABC.GetLetterByLearnIndex(id);
            return View("LearnLetter", letter);
        }

        [HttpPost]
        public ActionResult LetterRemembered(int id, int learnorder) // public ActionResult LetterRemembered([DefaultValue(0)] int id)
        {
            if (id == 1)
            {
                return Learn(id + 1);
            }
            else
            {
                GeorgianLetter letter = GeorgianABC.GetLetterByLearnIndex(learnorder + 1);
                return View("Translate", letter.Words[0]);
            }
        }

        public ActionResult LetterVerifyTranslation(string mxedruli, string translation)
        {
            return null;
        }
    }
}