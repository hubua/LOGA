using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LOGA.WebUI.Models
{
    public class GeorgianLetter
    {
        public string Mkhedruli { get; }
        public string Asomtavruli { get; }
        public string Nuskhuri { get; }
        public string LatEquivalent { get; }
        public string NumberEquivalent { get; }
        public string LetterName { get; }
        public string ReadAs { get; }
        public int LearnOrder { get; }
        public string[] Words  { get; }

        public GeorgianLetter(string mkhedruli, string asomtavruli, string nuskhuri, string latequivalent, string number, string name, string read, int order, string[] words)
        {
            Mkhedruli = mkhedruli;
            Asomtavruli = asomtavruli;
            Nuskhuri = nuskhuri;
            LatEquivalent = latequivalent;
            NumberEquivalent = number;
            LetterName = name;
            ReadAs = read;
            LearnOrder = order;
            Words = words;
        }
        
        public override string ToString()
        {
            return $"{Mkhedruli} ({Asomtavruli}, {Nuskhuri}, {LatEquivalent}) [{ReadAs}]";
        }
    }
}