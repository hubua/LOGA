﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LOGA.WebUI.Models
{
    public static class GeorgianABC
    {

        private static readonly Random random = new Random(DateTime.Now.Millisecond);

        public static Dictionary<char, GeorgianLetter> LettersDictionary;

        /*
        private static List<GeorgianLetter> LettersOrdered
        {
            get
            {
                return LettersDictionary.Values.OrderBy(item => item.LearnOrder).ToList();
            }
        }

        public static GeorgianLetter GetLetterByAlphabetIndex(int index)
        {
            return LettersDictionary.ToList()[index - 1].Value;
        }
        */

        public static bool IsValidLearnIndex(int lid)
        {
            return LettersDictionary.Values.ToList().Exists(item => item.LearnOrder == lid);
        }

        public static GeorgianLetter GetLetterByLearnIndex(int lid)
        {
            return LettersDictionary.Single(item => item.Value.LearnOrder == lid).Value;
        }

        public static int GetNextLetterLearnIndex(int lid, bool back = false)
        {
            var order = LettersDictionary.Select(item => item.Value.LearnOrder).OrderBy(item => item).ToList();
            int currentIndex = order.IndexOf(lid);
            int nextIndex;
            if (!back)
            {
                nextIndex = (currentIndex == order.Count - 1) ? 0 : currentIndex + 1;
            }
            else
            {
                nextIndex = (currentIndex == 0) ? order.Count - 1 : currentIndex - 1;
            }
            return order[nextIndex];
        }

        /// <summary>
        /// Gets firts word/sentence to translate for given Letter ID
        /// </summary>
        /// <param name="lid">Letter ID</param>
        /// <returns>String in mxedruli to translate</returns>
        public static string GetFirstWordToTranslateForLetter(int lid)
        {
            return GetLetterByLearnIndex(lid).Words[0];
        }

        public static Dictionary<string, bool?> GetWordsToTranslateForLetter(int lid, bool shuffle = false)
        {
            var letter = GetLetterByLearnIndex(lid);
            var words = shuffle ? letter.Words.OrderBy(item => random.Next()).ToList() : letter.Words.ToList();
            return words.ToDictionary(item => item, item => default(bool?));
        }

        /*
        /// <summary>
        /// Gets word/sentence to translate for given Letter ID
        /// </summary>
        /// <param name="lid">Letter ID</param>
        /// <returns>String in mxedruli to translate</returns>
        public static string GetRandomWordToTranslateForLetter(int lid)
        {
            throw new NotImplementedException();

            var letter = GetLetterByLearnIndex(lid);
            return letter.Words[random.Next(1, letter.Words.Count())];
        }

        /// <summary>
        /// Gets word/sentence to translate for letters up to given Letter ID
        /// </summary>
        /// <param name="lid">Latter letter ID</param>
        /// <returns>String in mxedruli to translate</returns>
        public static string GetRandomWordToTranslateForLetters(int lid)
        {
            throw new NotImplementedException();

            var letter = GetLetterByLearnIndex(random.Next(lid));
            return letter.Words[random.Next(1, letter.Words.Count())];
        }

        /// <summary>
        /// Gets shuffled words/sentences to translate for given Letter ID
        /// </summary>
        /// <param name="lid">Letter ID</param>
        /// <returns>Strings in mxedruli / is correct translation</returns>
        public static Dictionary<string, bool?> GetRandomWordsToTranslateForLetter(int lid)
        {
            throw new NotImplementedException();

            var letter = GetLetterByLearnIndex(lid);
            var words = letter.Words.OrderBy(item => random.Next()).ToArray();
            return words.ToDictionary(item => item, item => default(bool?));
        }

        /// <summary>
        /// Gets shuffled words/sentences to translate for letters up to given Letter ID
        /// </summary>
        /// <param name="lid"></param>
        /// <returns></returns>
        public static string[] GetRandomWordsToTranslateForLetters(int lid)
        {
            throw new NotImplementedException();

            var result = new string[0];
            

            for (int i = 1; i <= lid; i++)
            {
                var letter = GetLetterByLearnIndex(i);
                result = result.Concat(letter.Words).ToArray();
            }

            return result.OrderBy(item => random.Next()).ToArray();
        }
        */

        public static void Initialize(string csvpath)
        {
            var ogafile = System.IO.File.ReadAllLines(csvpath);

            LettersDictionary = new Dictionary<char, GeorgianLetter>();

            foreach (var item in ogafile)
            {
                var data = item.Split(','); // [0]Order [1]Modern [2]Asomtavruli [3]Nuskhuri [4]AlternativeAsomtavruliSpelling [5]LatinEquivalent [6]NumberEquivalent [7]LetterName [8]ReadAs [9]LearnOrder [10]Words

                var Order = Convert.ToInt32(data[0]);
                var LearnOrder = Convert.ToInt32(data[9]);

                var sRaw = data[10].Split(';').ToList();
                var sProc = new List<string>();
                foreach (var s in sRaw)
                {
                    if (!String.IsNullOrWhiteSpace(s))
                    {
                        sProc.Add(s.Trim());
                    }
                }
                var Words = sProc.ToArray();

                LettersDictionary.Add(Convert.ToChar(data[1]), new GeorgianLetter(Order, data[1], data[2], data[3], data[4], data[5], data[6], data[7], data[8], LearnOrder, Words));
            }
        }
    }
    
    public static class StringTranslationExtension
    {
        public static string ToKhucuri(this string mxedruli, bool withCapital = false)
        {
#if DEBUG
            return mxedruli;
#endif

#pragma warning disable CS0162 // Unreachable code
            StringBuilder result = new StringBuilder();
#pragma warning restore

            foreach (var c in mxedruli)
            {
                if (GeorgianABC.LettersDictionary.ContainsKey(c))
                {
                    string khucuriLetter = withCapital ? GeorgianABC.LettersDictionary[c].Asomtavruli : GeorgianABC.LettersDictionary[c].Nuskhuri;
                    withCapital = false; // Only first letter should be capitalized
                    result.Append(khucuriLetter);
                }
                else
                {
                    result.Append(c);
                }
            }
            return result.ToString();
        }
    }


    public static class HtmlHelperExtension
    {
        public static bool IsDebugMode(this HtmlHelper html)
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
    }

}