using LOGA.WebUI;
using LOGA.WebUI.Models;
using LOGA.WebUI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace LOGA.WebUI.Controllers
{
    public class LearnController : Controller
    {
        private static string SESSION_WORDS_TO_TRANSLATE = "SESSION_WORDS_TO_TRANSLATE";
        private static string CORRECT_COUNT = "CORRECT_COUNT";
        private static string INCORRECT_COUNT = "INCORRECT_COUNT";

        public LearnController()
        {
            ViewData["ActiveMenu"] = "Learn"; //TODO Highlight active item in UI
        }

        // GET: Learn
        public ActionResult Index()
        {
            var abc = GeorgianABCService.LettersDictionary;

            return View("Index", abc);
        }

        public ActionResult Letter([DefaultValue(GeorgianABCService.FIRST_LETTER_LID)] int lid)
        {
            if (!GeorgianABCService.IsValidLearnIndex(lid))
            {
                return RedirectToAction(nameof(Letter), new { lid = GeorgianABCService.FIRST_LETTER_LID });
            }

            GeorgianLetter letter = GeorgianABCService.GetLetterByLearnIndex(lid);
            return View("Letter", letter);
        }

        [HttpGet] // Used by syncronous Get
        public ActionResult Translate([DefaultValue(GeorgianABCService.FIRST_LETTER_TRANSLATION_LID)] int lid, bool shuffle = false)
        {
            if (!GeorgianABCService.IsValidLearnIndex(lid) || lid == GeorgianABCService.FIRST_LETTER_LID)
            {
                return RedirectToAction(nameof(Translate), new { lid = GeorgianABCService.FIRST_LETTER_TRANSLATION_LID });
            }

            var words = GeorgianABCService.GetWordsToTranslateForLetter(lid, shuffle);

            HttpContext.Session.Set<List<WordToTranslate>>(SESSION_WORDS_TO_TRANSLATE, words);

            bool capitalize = HttpContextStorage.GetUserSettings(HttpContext).LearnAsomtavruli;
            return View("Translate", new Translate(words[0].Word, words[0].Word.ToKhucuri(capitalize)));
        }

        [HttpPost] // Used by Ajax Post
        public ActionResult Translate(int lid, string hdnMxedruli, string tbTranslation)
        {
#if DEBUG
            System.Threading.Thread.Sleep(2000);
#endif
            //TODO show spinner
            var words = HttpContext.Session.Get<List<WordToTranslate>>(SESSION_WORDS_TO_TRANSLATE);
            if (words == null)
            {
                return Content($"window.location='{Url.Action(nameof(Translate), new { lid = lid })}'", "application/x-javascript"); // instead of RedirectToAction because result rendered by Ajax in div. 'x-javascript' is redirected faster then 'javascript'.
            }

            var isCorrectTranslation = (hdnMxedruli == tbTranslation);
            words.Single(item => item.Word == hdnMxedruli).IsTranslatedCorrectly = isCorrectTranslation;

            HttpContext.Session.Set<List<WordToTranslate>>(SESSION_WORDS_TO_TRANSLATE, words); //TODO remove session

            ModelState.Clear(); //TODO remove
            
            int correctCount = words.Where(item => item.IsTranslatedCorrectly.HasValue && item.IsTranslatedCorrectly.Value).Count();
            int incorrectCount = words.Where(item => item.IsTranslatedCorrectly.HasValue && !item.IsTranslatedCorrectly.Value).Count();

            var nextWordToTranslate = words.FirstOrDefault(item => !item.IsTranslatedCorrectly.HasValue);

            if (nextWordToTranslate != null)
            {
                bool capitalize = HttpContextStorage.GetUserSettings(HttpContext).LearnAsomtavruli;
                return PartialView("TranslatePartial", new Translate(nextWordToTranslate.Word, nextWordToTranslate.Word.ToKhucuri(capitalize), correctCount, incorrectCount, isCorrectTranslation));
            }
            else
            {
                TempData[CORRECT_COUNT] = correctCount;
                TempData[INCORRECT_COUNT] = incorrectCount;
                return Content($"window.location='{Url.Action(nameof(TranslateResults), new { lid = lid })}'", "application/x-javascript"); // instead of RedirectToAction because result rendered by Ajax in div. 'x-javascript' is redirected faster then 'javascript'.
            }
        }

        [HttpGet]
        public ActionResult TranslateResults(int lid)
        {
            string letterMxedruli = GeorgianABCService.GetLetterByLearnIndex(lid).Mkhedruli;
            string letterKhucuri = GeorgianABCService.GetLetterByLearnIndex(lid).Nuskhuri;

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