using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PMApi.Repo;
using PMApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace PMTests
{
    [TestClass]
    public class PortfolioTests
    {
        [TestMethod]
        public void GetPortfoliosTest()
        {
            PortfolioRepo _repo = new PortfolioRepo();
            List<Portfolio> _portfolios = new List<Portfolio>();

            _portfolios =  _repo.GetPortoflios();
            IEnumerable<string>[] symbols = _portfolios.Select(x => x.PortfolioPositions.Select(c => c.Symbol)).ToArray();
            string[] symbolArray = symbols.Cast<string>().ToArray();
        }
    }
}
