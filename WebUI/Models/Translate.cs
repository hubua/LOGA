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

        public Translate(string mxedruli, string khucuri, int correctCount, int incorrectCount)
        {
            Mxedruli = mxedruli;
            Khucuri = khucuri;
            CorrectCount = correctCount;
            IncorrectCount = incorrectCount;
        }

        public Translate(string mxedruli, string khucuri) : this(mxedruli, khucuri, 0, 0)
        {

        }
    }
}