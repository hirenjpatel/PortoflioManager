using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PMApi.Repo;
using PMApi.Models;

namespace PMTests
{
    [TestClass]
    public class ValuationTests
    {
        [TestMethod]
        public void UpdateValuationTest()
        {
            ValuationRepo _repo = new ValuationRepo();

            Valuation valuation = _repo.GetValuation(1000);

            
        }

        [TestMethod]
        public void InsertValuationDetailTest()
        {
            ValuationDetailRepo _repo = new ValuationDetailRepo();

            ValuationDetail vDetail = new ValuationDetail();
            vDetail.ValuationId = 1000;
            vDetail.ValuationTime = DateTime.Now;
            vDetail.PortfolioPositionId = 1;
            vDetail.PortfolioId = 100;
            vDetail.PositionValue = 35000;
            vDetail.Quantity = 500;

            _repo.Insert(vDetail);
            _repo.SaveChanges();

        }
    
    }
}
