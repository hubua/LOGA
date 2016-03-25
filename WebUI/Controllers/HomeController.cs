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
            if (!GeorgianABC.IsValidLearnIndex(lid))
            {
                return RedirectToAction("LearnLetter", new { lid = 1 });
            }

            GeorgianLetter letter = GeorgianABC.GetLetterByLearnIndex(lid);
            return View("LearnLetter", letter);
        }

        [HttpGet]
        public ActionResult Translate(int lid) // public ActionResult LetterRemembered([DefaultValue(0)] int id)
        {
            if (!GeorgianABC.IsValidLearnIndex(lid))
            {
                return RedirectToAction("Translate", new { lid = 1 });
            }

            var words = GeorgianABC.GetWordsToTranslateForLetter(lid);

            Session["WordsToTranslate"] = words;
            
            return View("Translate", new Translate(words.Keys.First(), words.Keys.First().ToKhucuri()));
            
        }

        [HttpPost]
        public ActionResult Translate(int lid, string hdnMxedruli, string tbTranslation) // TODO: TextBox from model
        {
            var WordsToTranslate = (Dictionary<string, bool?>)Session["WordsToTranslate"]; // TODO: Move to property

            var isCorrectTranslation = (hdnMxedruli == tbTranslation);
            WordsToTranslate[hdnMxedruli] = isCorrectTranslation;

            ModelState.Clear();

            string word = String.Empty;
            int correctCount = 0;
            int incorrectCount = 0;
            foreach (var item in WordsToTranslate)
            {
                if (!item.Value.HasValue)
                {
                    word = item.Key;
                    break;
                }
                else
                {
                    correctCount += Convert.ToInt32(item.Value.Value);
                    incorrectCount += Convert.ToInt32(!item.Value.Value);
                }
            }

            if (!String.IsNullOrEmpty(word))
            {
                return View("Translate", new Translate(word, word.ToKhucuri(), correctCount, incorrectCount));
            }
            else
            {
                TempData["CorrectCount"] = correctCount;
                TempData["IncorrectCount"] = incorrectCount;
                return RedirectToAction("TranslateResults", new { lid = lid });
            }
        }

        public ActionResult TranslateResults(int lid)
        {
            string letterMxedruli = GeorgianABC.GetLetterByLearnIndex(lid).Mxedruli;
            string letterKhucuri = GeorgianABC.GetLetterByLearnIndex(lid).Nuskhuru;

            int correctCount = Convert.ToInt32(TempData["CorrectCount"]);
            int incorrectCount = Convert.ToInt32(TempData["IncorrectCount"]);

            return View("TranslateResults", new Translate(letterMxedruli, letterKhucuri, correctCount, incorrectCount));
        }

        public ActionResult LogError()
        {
            throw new ApplicationException("Sample error message", new ApplicationException("Sample inner error message"));
        }
    }
}