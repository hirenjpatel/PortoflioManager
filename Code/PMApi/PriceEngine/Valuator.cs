using PMApi.Models;
using PMApi.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMApi.PriceEngine
{
    public class Valuator
    {
        private PortfolioManagerContext _db = new PortfolioManagerContext();
        private PortfolioRepo _portfolioRepo;
        private PriceValueRepo _priceValueRepo;
        private ValuationRepo _valuationRepo;

        public Valuator()
        {
            _portfolioRepo = new PortfolioRepo(_db);
        }

        public void RunValuation(int valuationId, DateTime valuationTime)
        {

            //retrieve portfolios
            List<Portfolio> portfolios = new List<Portfolio>();
            portfolios = _portfolioRepo.GetPortoflios();
            var symbols = portfolios.Select(x => x.PortfolioPositions.Select(c => c.Symbol)).ToArray();
            //get realtime prices

            //save realtime prices

            //valuation positions

        }

     
    }
}