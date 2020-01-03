using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BeboenaWebApp.Models;
using System.Collections.Generic;
using System.Linq;
using BeboenaWebApp.Services;
using System.IO;
using Microsoft.Extensions.FileProviders;

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
            var provider = new PhysicalFileProvider(new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName);
            var dirinfo = provider.GetFileInfo("/Services/Data/");
            var dir = dirinfo.PhysicalPath;

            GeorgianABCService.Initialize(dir);
        }

        [TestMethod]
        public void Translation_Test()
        {
            // Test works only in Release configuration
            string mkhedruli = "ბებო ენა";
            string mixed = "Ⴁⴄⴁⴍ ⴄⴌⴀ";
            string nuskhuri = "ⴁⴄⴁⴍ ⴄⴌⴀ";
            string asomtavruli = "ႡႤႡႭ ႤႬႠ";
            Assert.IsTrue(mkhedruli.ToKhucuri(Writing.Mixed) == mixed, "Mixed");
            Assert.IsTrue(mkhedruli.ToKhucuri(Writing.OnlyNuskhuri) == nuskhuri, "OnlyNuskhuri");
            Assert.IsTrue(mkhedruli.ToKhucuri(Writing.OnlyAsomtavruli) == asomtavruli, "Asomtavruli");
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
        public void GetWordsToTranslateForLetter_Test()
        {
            // Test fails if sentences.txt contains empty string
            Assert.IsTrue(GeorgianABCService.GetWordsToTranslateForLetter(1).Count == 0);
            Assert.IsTrue(GeorgianABCService.GetWordsToTranslateForLetter(2).Count == 1);
            Assert.IsTrue(GeorgianABCService.GetWordsToTranslateForLetter(101).Count == 0);
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
