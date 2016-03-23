﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace LOGA.WebUI.Models
{
    public static class GeorgianABC
    {
        private static readonly Random random = new Random(DateTime.Now.Millisecond);

        public static Dictionary<char, GeorgianLetter> LettersDictionary;

        private static List<GeorgianLetter> LettersOrdered
        {
            get
            {
                return LettersDictionary.Values.OrderBy(item => item.LearnOrder).ToList();
            }
        }

        /// <summary>
        /// Returns sequental index of letters starting from 1
        /// ა - 1
        /// ბ - 2
        /// </summary>
        /// <param name="index">Letter index</param>
        /// <returns>True if index between 1 and 38 (33 + 5)</returns>
        public static bool IsValidLetterIndex(int index)
        {
            return (index >= 1 && index <= LettersDictionary.Count);
        }

        public static GeorgianLetter GetLetterByAlphabetIndex(int index)
        {
            return LettersDictionary.ToList()[index - 1].Value;
        }

        public static GeorgianLetter GetLetterByLearnIndex(int index)
        {
            return LettersOrdered[index - 1];
        }

        public static string GetFirstWordToTranslateForLetter(int lid)
        {
            return GetLetterByLearnIndex(lid).Words[0];
        }

        /// <summary>
        /// Gets word/sentence to translate for given Letter ID
        /// </summary>
        /// <param name="lid">Letter ID</param>
        /// <returns>String in mxedruli to translate</returns>
        public static string GetRandomWordToTranslateForLetter(int lid)
        {
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

        public static void Initialize(string csvpath)
        {
            var ogafile = System.IO.File.ReadAllLines(csvpath);

            LettersDictionary = new Dictionary<char, GeorgianLetter>();

            foreach (var item in ogafile)
            {
                var data = item.Split(','); // [0]მხედრული [1]ასომთავრული [2]ნუსხური [3]რიცხვი [4]სახელი [5]იკითხება [6]მიმდევრობა [7]სიტყვები

                var LearnOrder = Convert.ToInt32(data[6]);

                var sRaw = data[7].Split(';').ToList();
                var sProc = new List<string>();
                foreach (var s in sRaw)
                {
                    if (!String.IsNullOrWhiteSpace(s))
                    {
                        sProc.Add(s.Trim());
                    }
                }
                var Words = sProc.ToArray();

                LettersDictionary.Add(Convert.ToChar(data[0]), new GeorgianLetter(data[0], data[1], data[2], data[3], data[4], data[5], LearnOrder, Words));
            }
        }
    }

    public static class StringTranslationExtension
    {
        public static string ToKhucuri(this string mxedruli, bool withCapital = false)
        {
            return mxedruli;

            StringBuilder result = new StringBuilder();
            foreach (var c in mxedruli)
            {
                if (GeorgianABC.LettersDictionary.ContainsKey(c))
                {
                    string khucuriLetter = withCapital ? GeorgianABC.LettersDictionary[c].Asomtavruli : GeorgianABC.LettersDictionary[c].Nuskhuru;
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

}