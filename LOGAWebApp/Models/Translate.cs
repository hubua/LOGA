using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGAWebApp.Models
{
    public class Translate
    {
        public string Mxedruli { get; }
        public string Khucuri { get; }
        public int TotalCount { get; }
        public int CorrectCount { get; }
        public int IncorrectCount { get; }
        public bool IsPreviousTranslatedCorrectly { get; }

        public Translate(string mxedruli, string khucuri, int totalCount, int correctCount, int incorrectCount, bool isPreviousTranslatedCorrectly)
        {
            Mxedruli = mxedruli;
            Khucuri = khucuri;
            TotalCount = totalCount;
            CorrectCount = correctCount;
            IncorrectCount = incorrectCount;
            IsPreviousTranslatedCorrectly = isPreviousTranslatedCorrectly;
        }

        public Translate(string mxedruli, string khucuri, int correctCount, int incorrectCount) : this(mxedruli, khucuri, -1, correctCount, incorrectCount, false)
        {

        }

        public Translate(string mxedruli, string khucuri, int totalCount) : this(mxedruli, khucuri, totalCount, 0, 0, false)
        {

        }
    }
}