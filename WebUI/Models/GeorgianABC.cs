using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGA.WebUI.Models
{
    public class GeorgianABC
    {
        public static Dictionary<string, GeorgianLetter> LettersIndex;

        public static List<GeorgianLetter> LettersOrdered
        {
            get
            {
                return LettersIndex.Values.OrderBy(item => item.LearnOrder).ToList();
            }
        }

        public static void Initialize(string csvpath)
        {
            var ogafile = System.IO.File.ReadAllLines(HttpContext.Current.Server.MapPath(csvpath));

            LettersIndex = new Dictionary<string, GeorgianLetter>();

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

                LettersIndex.Add(data[0], new GeorgianLetter(data[0], data[1], data[2], data[3], data[4], data[5], LearnOrder, Words));
            }
        }
    }
}