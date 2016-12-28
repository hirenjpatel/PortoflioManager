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
        public void GetRealTimePricesTest()
        {
            PriceValueRepo _repo = new PriceValueRepo();
            PriceEngine engine = new PriceEngine();
            List<PriceValue> prices = engine.GetRealTimePrices(new string[] { "AXP", "MSFT" });

            foreach (PriceValue pv in prices)
            {
               
                _repo.Insert(pv);
                _repo.SaveChanges();
            }
        }
    }
}
