using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGA.WebUI.Models
{
    public class Translate
    {
        public string Mxedruli { get; }
        public string Khucuri { get; }
        public int CorrectCount { get; }
        public int IncorrectCount { get; }
        public bool IsPreviousTranslatedCorrectly { get; }

        public Translate(string mxedruli, string khucuri, int correctCount, int incorrectCount, bool isPreviousTranslatedCorrectly)
        {
            Mxedruli = mxedruli;
            Khucuri = khucuri;
            CorrectCount = correctCount;
            IncorrectCount = incorrectCount;
            IsPreviousTranslatedCorrectly = isPreviousTranslatedCorrectly;
        }

        public Translate(string mxedruli, string khucuri, int correctCount, int incorrectCount) : this(mxedruli, khucuri, correctCount, incorrectCount, false)
        {

        }

        public Translate(string mxedruli, string khucuri) : this(mxedruli, khucuri, 0, 0, false)
        {

        }
    }
}