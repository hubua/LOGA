using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LOGAWebApp.Models;
using System.IO;

namespace LOGAWebApp.Services
{
    public static class GeorgianABCService
    {

        private static readonly Random random = new Random(DateTime.Now.Millisecond);

        public static readonly Dictionary<char, GeorgianLetter> LettersDictionary = new Dictionary<char, GeorgianLetter>();

        public const int FIRST_LETTER_LID = 1;
        public const int FIRST_LETTER_TRANSLATION_LID = 2;
        public const int LAST_LETTER_LID = 105;

        public const int MODERN_LETTER_COUNT = 33;
        public const int OLD_LETTER_COUNT = 5;

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

        public static List<WordToTranslate> GetWordsToTranslateForLetter(int lid, bool shuffle = false)
        {
            var letter = GetLetterByLearnIndex(lid);
            var words = shuffle ? letter.Words.OrderBy(item => random.Next()).ToList() : letter.Words.ToList();
            return words.Select(item => new WordToTranslate { Word = item, IsTranslatedCorrectly = default(bool?) }).ToList();
        }

        public static void Initialize(string csvdir)
        {
            LettersDictionary.Clear(); // In case Initialize was already called before

            var ogaCSV = File.ReadAllLines(Path.Combine(csvdir, "oga.tsv"));
            /*
             * [0]Order [1]Modern [2]Asomtavruli [3]Nuskhuri [4]AlternativeAsomtavruliSpelling [5]LatinEquivalent
             * [6]NumberEquivalent [7]LetterName [8]ReadAs [9]LearnOrder [10]LearnOrder2 [11]Words
             */
            var ogaData = ogaCSV.Skip(1).Select(item => item.Split('\t'));

            var sentencesCSV1 = File.ReadAllLines(Path.Combine(csvdir, "sentences1.txt"));
            var sentencesCSV2 = File.ReadAllLines(Path.Combine(csvdir, "sentences2.txt"));
            var sentencesData = sentencesCSV1.Concat(sentencesCSV2).Distinct().ToList();

            var letterSentences = new Dictionary<char, List<string>>();

            var letters = ogaData
                .Select(item => new KeyValuePair<char, int>(Convert.ToChar(item[1]), Convert.ToInt32(item[9])))
                .OrderBy(item => item.Value)
                .Select(item => item.Key)
                .ToList();
            letters.ForEach(letter => { letterSentences.Add(letter, new List<string>()); });

            foreach (var sentence in sentencesData)
            {
                int max = 0;
                foreach (char letter in sentence)
                {
                    int lid = letters.IndexOf(letter);
                    max = Math.Max(max, lid);
                }
                letterSentences[letters[max]].Add(sentence);
            }

            foreach (var data in ogaData)
            {
                var LetterMxedruli = Convert.ToChar(data[1]);
                var Order = Convert.ToInt32(data[0]);
                var LearnOrder = Convert.ToInt32(data[9]);
                var LearnOrder2 = Convert.ToInt32(data[10]);

                var Words = letterSentences[LetterMxedruli].OrderBy(item => item.Length).ToArray();

                var letter = new GeorgianLetter(Order, LetterMxedruli.ToString(), data[2], data[3], data[4], data[5], data[6], data[7], data[8], LearnOrder, Words);

                LettersDictionary.Add(LetterMxedruli, letter);
            }
        }
    }
    
    public static class StringTranslationExtension
    {
        public static string ToKhucuri(this string mxedruli, Writing capitalization = Writing.Mixed)
        {
#if DEBUG
            return mxedruli;
#endif

#pragma warning disable CS0162 // Unreachable code
            StringBuilder result = new StringBuilder();
#pragma warning restore

            var withCapital = capitalization == Writing.Mixed || capitalization == Writing.OnlyAsomtavruli;

            foreach (var c in mxedruli)
            {
                if (GeorgianABCService.LettersDictionary.ContainsKey(c))
                {
                    string khucuriLetter = withCapital ? GeorgianABCService.LettersDictionary[c].Asomtavruli : GeorgianABCService.LettersDictionary[c].Nuskhuri;
                    withCapital = withCapital && capitalization != Writing.Mixed; // Only first letter should be capitalized
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