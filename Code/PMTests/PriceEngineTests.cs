using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PMApi.PriceEngine;
using System.Collections.Generic;
using PMApi.Models;

namespace PMTests
{
    [TestClass]
    public class PriceEngineTests
    {
        [TestMethod]
        public void GetRealTimePricesTest()
        {
            PriceEngine engine = new PriceEngine();
            List<PriceValue> prices = engine.GetRealTimePrices(new string[] { "AXP", "MSFT" });


        }
    }
}
