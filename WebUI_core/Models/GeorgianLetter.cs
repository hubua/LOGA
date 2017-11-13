using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGA.WebUI.Models
{
    public class GeorgianLetter
    {
        public int Order { get; }
        public string Mkhedruli { get; }
        public string Asomtavruli { get; }
        public string Nuskhuri { get; }
        public string AlternativeAsomtavruliSpelling { get; }
        public string LatinEquivalent { get; }
        public string NumberEquivalent { get; }
        public string LetterName { get; }
        public string ReadAs { get; }
        public int LearnOrder { get; }
        public int LearnOrder2 { get; }
        public string[] Words  { get; }

        public GeorgianLetter(int order, string mkhedruli, string asomtavruli, string nuskhuri, string alternativeasomtavrulispelling, string latinequivalent, string number, string name, string read, int learnorder, string[] words)
        {
            Order = order;
            Mkhedruli = mkhedruli;
            Asomtavruli = asomtavruli;
            Nuskhuri = nuskhuri;
            AlternativeAsomtavruliSpelling = alternativeasomtavrulispelling;
            LatinEquivalent = latinequivalent;
            NumberEquivalent = number;
            LetterName = name;
            ReadAs = read;
            LearnOrder = learnorder;
            Words = words;
        }
        
        public override string ToString()
        {
            return $"{Mkhedruli} ({Asomtavruli}, {Nuskhuri}, {LatinEquivalent}) [{ReadAs}]";
        }
    }
}