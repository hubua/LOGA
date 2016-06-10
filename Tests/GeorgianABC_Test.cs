using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LOGA.WebUI.Models;
using System.Collections.Generic;
using System.Web.Mvc;

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
        public void Translation_Test()
        {
            string mkhedruli = "ბებო ენა";
            string asomtavruli = "Ⴁⴄⴁⴍ ⴄⴌⴀ";
            string nuskhuri = "ⴁⴄⴁⴍ ⴄⴌⴀ";
            string bechduri = "bebo ena";
            Assert.IsTrue(mkhedruli.ToKhucuri(Writing.Hand, true) == asomtavruli);
            Assert.IsTrue(mkhedruli.ToKhucuri(Writing.Hand, false) == nuskhuri);
            Assert.IsTrue(mkhedruli.ToKhucuri(Writing.Print, true) == bechduri);
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
                var w = GeorgianABC.GetWordsToTranslateForLetter(lid);
                var ws = GeorgianABC.GetWordsToTranslateForLetter(lid, true);

                foreach (var item in w)
                {
                    Assert.IsTrue(ws.ContainsKey(item.Key), lid.ToString());
                }
            }
        }

        
        
    }
}
