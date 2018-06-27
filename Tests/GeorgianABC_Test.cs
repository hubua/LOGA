using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LOGA.WebUI.Models;
using System.Collections.Generic;
using System.Linq;
using LOGA.WebUI.Services;

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
            GeorgianABCService.Initialize(@".\..\..\..\..\WebUI\Services\Data\");
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
            Assert.IsTrue(GeorgianABCService.GetNextLetterLearnIndex(1) == 2);
            Assert.IsTrue(GeorgianABCService.GetNextLetterLearnIndex(2) == 3);
            Assert.IsTrue(GeorgianABCService.GetNextLetterLearnIndex(33) == 101);
            Assert.IsTrue(GeorgianABCService.GetNextLetterLearnIndex(105) == 1);

            Assert.IsTrue(GeorgianABCService.GetNextLetterLearnIndex(105, true) == 104);
            Assert.IsTrue(GeorgianABCService.GetNextLetterLearnIndex(101, true) == 33);
            Assert.IsTrue(GeorgianABCService.GetNextLetterLearnIndex(2, true) == 1);
            Assert.IsTrue(GeorgianABCService.GetNextLetterLearnIndex(1, true) == 105);
        }

        [TestMethod]
        public void GetWordsToTranslateForLetter_Shuffled_Test()
        {   
            foreach (var l in GeorgianABCService.LettersDictionary)
            {
                var lid = l.Value.LearnOrder;
                var words = GeorgianABCService.GetWordsToTranslateForLetter(lid);
                var wordsShuffled = GeorgianABCService.GetWordsToTranslateForLetter(lid, true);
                
                Assert.IsTrue(Enumerable.SequenceEqual(words.OrderBy(item => item.Word), wordsShuffled.OrderBy(item => item.Word), new WordsToTranslateComparer()));
            }
        }

        
        
    }
}
