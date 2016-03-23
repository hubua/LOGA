using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LOGA.WebUI.Models;

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
        public void GetRandomWordsToTranslateForLetter_Test()
        {
            var a = GeorgianABC.GetRandomWordsToTranslateForLetter(4);
            Assert.IsTrue(a.Count == 4);
        }

        [TestMethod]
        public void GetRandomWordsToTranslateForLetters_Test()
        {
            var a = GeorgianABC.GetRandomWordsToTranslateForLetters(4);
            Assert.IsTrue(a.Length == 8);
        }
    }
}
