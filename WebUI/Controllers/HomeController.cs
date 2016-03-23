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

        public ActionResult LearnLetter(int lid)
        {
            if (!GeorgianABC.IsValidLetterIndex(lid))
            {
                return RedirectToAction("LearnLetter", new { lid = 1 });
            }

            GeorgianLetter letter = GeorgianABC.GetLetterByLearnIndex(lid);
            return View("LearnLetter", letter);
        }

        [HttpGet]
        public ActionResult Translate(int lid) // public ActionResult LetterRemembered([DefaultValue(0)] int id)
        {
            if (!GeorgianABC.IsValidLetterIndex(lid))
            {
                return RedirectToAction("Translate", new { lid = 1 });
            }


            // TODO: no need to shuffle
            var allwords = GeorgianABC.GetRandomWordsToTranslateForLetter(lid);
            var firstword = GeorgianABC.GetFirstWordToTranslateForLetter(lid);
            allwords.Remove(firstword);

            Session["WordsToTranslate"] = allwords;
            
            return View("Translate", new Translate(firstword, firstword.ToKhucuri()));
            
        }

        [HttpPost]
        public ActionResult Translate(int lid, string hdnMxedruli, string tbTranslation) // TODO: TextBox from model
        {
            var WordsToTranslate = (Dictionary<string, bool?>)Session["WordsToTranslate"];

            var correct = (hdnMxedruli == tbTranslation);
            WordsToTranslate[hdnMxedruli] = correct;

            if (correct)
            {
                TempData["Result"] = "correct";
            }

            ModelState.Clear();


            string word = String.Empty;
            foreach (var item in WordsToTranslate)
            {
                if (item.Value == null)
                {
                    word = item.Key;
                    break;
                }
            }


            return View("Translate", new Translate(word, word.ToKhucuri()));
        }

        public ActionResult LogError()
        {
            throw new ApplicationException("Sample error message", new ApplicationException("Sample inner error message"));
        }
    }
}