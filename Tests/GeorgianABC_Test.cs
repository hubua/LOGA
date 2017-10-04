using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LOGA.WebUI.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace Tests
{
    public class WordsToTranslateComparer : IEqualityComparer<WordToTranslate>
    {
        public bool Equals(WordToTranslate x, WordToTranslate y)
        {
            return x.Word == y.Word;
        }

        public int GetHashCode(WordToTranslate obj)
        {
            throw new NotImplementedException();
        }
    }


    [TestClass]
    public class GeorgianABC_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            GeorgianABC.Initialize(@".\..\..\..\WebUI\App_Data\");
        }

        [TestMethod]
        public void Translation_Test()
        {
            string mkhedruli = "ბებო ენა";
            string asomtavruli = "Ⴁⴄⴁⴍ ⴄⴌⴀ";
            string nuskhuri = "ⴁⴄⴁⴍ ⴄⴌⴀ";
            Assert.IsTrue(mkhedruli.ToKhucuri(true) == asomtavruli);
            Assert.IsTrue(mkhedruli.ToKhucuri(false) == nuskhuri);
        }

        [TestMethod]
        public void GetNextLetterLearnIndex_Test()
        {
            Assert.IsTrue(GeorgianABC.GetNextLetterLearnIndex(1) == 2);
            Assert.IsTrue(GeorgianABC.GetNextLetterLearnIndex(2) == 3);
            Assert.IsTrue(GeorgianABC.GetNextLetterLearnIndex(33) == 101);
            Assert.IsTrue(GeorgianABC.GetNextLetterLearnIndex(105) == 1);

            Assert.IsTrue(GeorgianABC.GetNextLetterLearnIndex(105, true) == 104);
            Assert.IsTrue(GeorgianABC.GetNextLetterLearnIndex(101, true) == 33);
            Assert.IsTrue(GeorgianABC.GetNextLetterLearnIndex(2, true) == 1);
            Assert.IsTrue(GeorgianABC.GetNextLetterLearnIndex(1, true) == 105);
        }

        [TestMethod]
        public void GetWordsToTranslateForLetter_Shuffled_Test()
        {
            foreach (var l in GeorgianABC.LettersDictionary)
            {
                var lid = l.Value.LearnOrder;
                var words = GeorgianABC.GetWordsToTranslateForLetter(lid);
                var wordsShuffled = GeorgianABC.GetWordsToTranslateForLetter(lid, true);
                
                Assert.IsTrue(Enumerable.SequenceEqual(words.OrderBy(item => item.Word), wordsShuffled.OrderBy(item => item.Word), new WordsToTranslateComparer()));
            }
        }

        
        
    }
}
