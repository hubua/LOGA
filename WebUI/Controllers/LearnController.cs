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

        public ActionResult Letter([DefaultValue(1)] int lid)
        {
            if (!GeorgianABC.IsValidLearnIndex(lid))
            {
                return RedirectToAction("Letter", new { lid = 1 });
            }

            GeorgianLetter letter = GeorgianABC.GetLetterByLearnIndex(lid);
            return View("Letter", letter);
        }

        [HttpGet]
        public ActionResult Translate([DefaultValue(2)] int lid, string shuffle)
        {
            if (!GeorgianABC.IsValidLearnIndex(lid) || lid == 1) // TODO: Change magic numbers and strings to consts
            {
                return RedirectToAction("Translate", new { lid = 2 });
            }

            var words = GeorgianABC.GetWordsToTranslateForLetter(lid, (shuffle?.ToUpper() == "YES"));

            Session[SESSION_WORDS_TO_TRANSLATE] = words;

            bool capitalize = HttpContextStorage.GetUserSettings(HttpContext).LearnAsomtavruli;
            return View("Translate", new Translate(words.Keys.First(), words.Keys.First().ToKhucuri(capitalize)));

        }

        [HttpPost]
        public ActionResult Translate(int lid, string hdnMxedruli, string tbTranslation) // TODO: TextBox from model
        {
            var WordsToTranslate = (Dictionary<string, bool?>)Session[SESSION_WORDS_TO_TRANSLATE]; // TODO: Move to property
            if (WordsToTranslate == null)
            {
                return RedirectToAction("Translate", new { lid = lid });
            }

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
                bool capitalize = HttpContextStorage.GetUserSettings(HttpContext).LearnAsomtavruli;
                return View("Translate", new Translate(word, word.ToKhucuri(capitalize), correctCount, incorrectCount));
            }
            else
            {
                TempData["CorrectCount"] = correctCount;
                TempData["IncorrectCount"] = incorrectCount;
                return RedirectToAction("TranslateResults", new { lid = lid });
            }
        }

        [HttpGet]
        public ActionResult TranslateResults(int lid)
        {
            string letterMxedruli = GeorgianABC.GetLetterByLearnIndex(lid).Mkhedruli;
            string letterKhucuri = GeorgianABC.GetLetterByLearnIndex(lid).Nuskhuri;

            int correctCount = Convert.ToInt32(TempData["CorrectCount"]);
            int incorrectCount = Convert.ToInt32(TempData["IncorrectCount"]);
            if (correctCount > incorrectCount)
            {
                HttpContextStorage.SetUserLearnProgressLId(HttpContext, lid);
            }

            return View("TranslateResults", new Translate(letterMxedruli, letterKhucuri, correctCount, incorrectCount));
        }

        /*
        Does not generate multiline returns
        [HttpGet]
        public ActionResult GetTextImage(string text, int size)
        {
            byte[] result;
            using (var font = new System.Drawing.Font("Sylfaen", size, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point))
            {
                var graphics = System.Drawing.Graphics.FromImage(new System.Drawing.Bitmap(1, 1));
                int w = (int)graphics.MeasureString(text, font).Width;
                int h = (int)graphics.MeasureString(text, font).Height;

                using (var bitmap = new System.Drawing.Bitmap(w, h))
                {
                    graphics = System.Drawing.Graphics.FromImage(bitmap);
                    graphics.Clear(System.Drawing.Color.White);
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                    graphics.DrawString(text, font, System.Drawing.Brushes.Black, 0, 0);
                    graphics.Flush();
                    using (var ms = new System.IO.MemoryStream())
                    {
                        bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        //msResult.Position = 0;
                        result = ms.ToArray();
                    }
                }
            }
            
            Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            Response.Cache.SetValidUntilExpires(false);
            Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            
            return new FileContentResult(result, "image/png");
        }
        */

    }
}