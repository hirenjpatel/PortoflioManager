using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PMApi.PriceEngine;

namespace PMTests
{
    [TestClass]
    public class ValuatorTests
    {
        [TestMethod]
        public void RunValuationTest()
        {
            Valuator valuator = new Valuator();

            valuator.RunValuation(1000);

        }
    }
}
