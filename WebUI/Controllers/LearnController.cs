﻿using LOGA.WebUI.Models;
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
            
            return View("Translate", new Translate(words.Keys.First(), words.Keys.First().ToKhucuri()));
            
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
                return View("Translate", new Translate(word, word.ToKhucuri(), correctCount, incorrectCount));
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
            string letterMxedruli = GeorgianABC.GetLetterByLearnIndex(lid).Mxedruli;
            string letterKhucuri = GeorgianABC.GetLetterByLearnIndex(lid).Nuskhuru;

            int correctCount = Convert.ToInt32(TempData["CorrectCount"]);
            int incorrectCount = Convert.ToInt32(TempData["IncorrectCount"]);

            return View("TranslateResults", new Translate(letterMxedruli, letterKhucuri, correctCount, incorrectCount));
        }

        
    }
}