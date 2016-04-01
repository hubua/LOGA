using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LOGA.WebUI.Models
{
    public class GeorgianLetter
    {
        public string Mxedruli { get; }
        public string Asomtavruli { get; }
        public string Nuskhuru { get; }
        public string NumberEquivalent { get; }
        public string LetterName { get; }
        public string ReadAs { get; }
        public int LearnOrder { get; }
        public string[] Words  { get; }

        public GeorgianLetter(string mxedruli, string asomtavruli, string nuskhuri, string number, string name, string read, int order, string[] words)
        {
            Mxedruli = mxedruli;
            Asomtavruli = asomtavruli;
            Nuskhuru = nuskhuri;
            NumberEquivalent = number;
            LetterName = name;
            ReadAs = read;
            LearnOrder = order;
            Words = words;
        }
        
        public override string ToString()
        {
            return $"{Mxedruli} ({Asomtavruli}, {Nuskhuru}) [{ReadAs}]";
        }
    }
}