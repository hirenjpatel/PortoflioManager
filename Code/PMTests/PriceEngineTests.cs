using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PMApi.PriceEngine;
using System.Collections.Generic;
using PMApi.Models;
using PMApi.Repo;

namespace PMTests
{
    [TestClass]
    public class PriceEngineTests
    {
        [TestMethod]
        public void YahooGetPrices()
        {
            PriceValueRepo _repo = new PriceValueRepo();
            YahooPriceEngine engine = new YahooPriceEngine();
            List<PriceValue> prices = engine.GetPrices(new string[] { "AXP", "MSFT" });

           Assert.AreEqual(2, prices.Count);
        }

        [TestMethod]
        public void GoogleGetPrices()
        {
            PriceValueRepo _repo = new PriceValueRepo();
            GooglePriceEngine engine = new GooglePriceEngine();
            List<PriceValue> prices = engine.GetPrices(new string[] { "AXP", "MSFT" });

            Assert.AreEqual(2, prices.Count);
        }



    }
}
