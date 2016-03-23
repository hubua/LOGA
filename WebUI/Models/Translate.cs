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

        public Translate(string mxedruli, string khucuri)
        {
            Mxedruli = mxedruli;
            Khucuri = khucuri;
        }
    }
}