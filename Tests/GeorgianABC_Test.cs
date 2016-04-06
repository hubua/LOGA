using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LOGA.WebUI.Models;
using System.Collections.Generic;

namespace Tests
{
    [TestClass]
    public class GeorgianABC_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            GeorgianABC.Initialize(@".\..\..\..\WebUI\App_Data\oga.csv");
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
            int lid = new Random().Next(5, 30);
            var w = GeorgianABC.GetWordsToTranslateForLetter(lid);
            var ws = GeorgianABC.GetWordsToTranslateForLetter(lid, true);

            foreach (var item in w)
            {
                Assert.IsTrue(ws.ContainsKey(item.Key));
            }
        }
        
        
    }
}
