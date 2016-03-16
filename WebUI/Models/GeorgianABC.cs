using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace LOGA.WebUI.Models
{
    public static class GeorgianABC
    {
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

        public static string ToKhucuri(string mxedruli)
        {
            StringBuilder result = new StringBuilder();
            foreach (var c in mxedruli)
            {
                result.Append(LettersDictionary.ContainsKey(c) ? LettersDictionary[c].Nuskhuru : c.ToString());
            }
            return result.ToString();
        }

        public static void Initialize(string csvpath)
        {
            var ogafile = System.IO.File.ReadAllLines(HttpContext.Current.Server.MapPath(csvpath));

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
}