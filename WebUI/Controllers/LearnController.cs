using LOGA.WebUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LOGA.WebUI.Controllers
{
    public class LearnController : Controller
    {
        private static string SESSION_WORDS_TO_TRANSLATE = "SESSION_WORDS_TO_TRANSLATE";
        private static string CORRECT_COUNT = "CORRECT_COUNT";
        private static string INCORRECT_COUNT = "INCORRECT_COUNT";

        public LearnController()
        {
            ViewData["ActiveMenu"] = "Learn";
        }

        // GET: Learn
        public ActionResult Index()
        {
            var abc = GeorgianABC.LettersDictionary;

            return View("Index", abc);
        }

        public ActionResult Letter([DefaultValue(GeorgianABC.FIRST_LETTER_LID)] int lid)
        {
            if (!GeorgianABC.IsValidLearnIndex(lid))
            {
                return RedirectToAction(nameof(Letter), new { lid = GeorgianABC.FIRST_LETTER_LID });
            }

            GeorgianLetter letter = GeorgianABC.GetLetterByLearnIndex(lid);
            return View("Letter", letter);
        }

        [HttpGet] // Used by syncronous Get
        public ActionResult Translate([DefaultValue(GeorgianABC.FIRST_LETTER_TRANSLATION_LID)] int lid, bool shuffle = false)
        {
            if (!GeorgianABC.IsValidLearnIndex(lid) || lid == GeorgianABC.FIRST_LETTER_LID)
            {
                return RedirectToAction(nameof(Translate), new { lid = GeorgianABC.FIRST_LETTER_TRANSLATION_LID });
            }

            var words = GeorgianABC.GetWordsToTranslateForLetter(lid, shuffle);

            Session[SESSION_WORDS_TO_TRANSLATE] = words;

            bool capitalize = HttpContextStorage.GetUserSettings(HttpContext).LearnAsomtavruli;
            //return View("Translate", new Translate(words.Keys.First(), words.Keys.First().ToKhucuri(capitalize)));
            return View("Translate", new Translate(words[0].Word, words[0].Word.ToKhucuri(capitalize)));
        }

        [HttpPost] // Used by Ajax Post
        public ActionResult Translate(int lid, string hdnMxedruli, string tbTranslation) // TODO: TextBox from model
        {
            //var WordsToTranslate = (Dictionary<string, bool?>)Session[SESSION_WORDS_TO_TRANSLATE];
            var WordsToTranslate = (List<WordToTranslate>)Session[SESSION_WORDS_TO_TRANSLATE];
            if (WordsToTranslate == null)
            {
                return JavaScript($"window.location='{Url.Action(nameof(Translate), new { lid = lid })}'"); // instead of RedirectToAction(nameof(Translate), new { lid = lid }); because result rendered by Ajax in div
            }

            var isCorrectTranslation = (hdnMxedruli == tbTranslation);
            WordsToTranslate.Single(item => item.Word == hdnMxedruli).IsTranslatedCorrectly = isCorrectTranslation;

            ModelState.Clear();
            
            int correctCount = WordsToTranslate.Where(item => item.IsTranslatedCorrectly.HasValue && item.IsTranslatedCorrectly.Value).Count();
            int incorrectCount = WordsToTranslate.Where(item => item.IsTranslatedCorrectly.HasValue && !item.IsTranslatedCorrectly.Value).Count();

            var nextWordToTranslate = WordsToTranslate.FirstOrDefault(item => !item.IsTranslatedCorrectly.HasValue);

            if (nextWordToTranslate != null)
            {
                bool capitalize = HttpContextStorage.GetUserSettings(HttpContext).LearnAsomtavruli;
                return PartialView("TranslatePartial", new Translate(nextWordToTranslate.Word, nextWordToTranslate.Word.ToKhucuri(capitalize), correctCount, incorrectCount, isCorrectTranslation));
            }
            else
            {
                TempData[CORRECT_COUNT] = correctCount;
                TempData[INCORRECT_COUNT] = incorrectCount;
                return JavaScript($"window.location='{Url.Action(nameof(TranslateResults), new { lid = lid })}'"); // instead of RedirectToAction("TranslateResults", new { lid = lid }); because result rendered by Ajax in div
            }
        }

        [HttpGet]
        public ActionResult TranslateResults(int lid)
        {
            string letterMxedruli = GeorgianABC.GetLetterByLearnIndex(lid).Mkhedruli;
            string letterKhucuri = GeorgianABC.GetLetterByLearnIndex(lid).Nuskhuri;

            int correctCount = Convert.ToInt32(TempData[CORRECT_COUNT]);
            int incorrectCount = Convert.ToInt32(TempData[INCORRECT_COUNT]);
            if (correctCount > incorrectCount)
            {
                HttpContextStorage.SetUserLearnProgressLId(HttpContext, lid);
            }

            return View("TranslateResults", new Translate(letterMxedruli, letterKhucuri, correctCount, incorrectCount));
        }


    }
}