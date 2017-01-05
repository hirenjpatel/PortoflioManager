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
        private ValuationDetailRepo _valuationDetailRepo;

        public DateTime ValuationTime { get; set; }
        public string ValuationStatus { get; set; }

        public Valuator()
        {
            _portfolioRepo = new PortfolioRepo(_db);
            _priceValueRepo = new PriceValueRepo(_db);
            _valuationRepo = new ValuationRepo(_db);
            _valuationDetailRepo = new ValuationDetailRepo(_db);

    }

        public void RunValuation(int valuationId)
        {

            if (valuationId == 1000)
            {
                //this is a realtime valuations
                RunRealtimeValuation(valuationId);
            }

          

        }

        public void RunRealtimeValuation(int valuationId)
        {
            ValuationStatus = "Running";
            Valuation valuation = _valuationRepo.GetValuation(valuationId);
            //retrieve portfolios
            List<Portfolio> portfolios = new List<Portfolio>();
            portfolios = _portfolioRepo.GetPortoflios();

            if (portfolios.Count == 0)
                return;
            //get realtime prices
            List<PriceValue> prices = PriceEngine.GetRealTimePrices(GetAllSymbolsFromPositions(portfolios));
            //save prices
            SavePriceValues(prices);
            //valuation positions
            ValuePositions(portfolios, prices, valuation.Id);
        }

        

        /// <summary>
        /// Gets all the symbols from the specified portfolios
        /// </summary>
        /// <param name="portfolios"></param>
        /// <returns></returns>
        public string[] GetAllSymbolsFromPositions(List<Portfolio> portfolios)
        {
            List<string> symbols = new List<string>();
            foreach (Portfolio p in portfolios)
            {
                foreach (PortfolioPosition pp in p.PortfolioPositions)
                {
                    symbols.Add(pp.Symbol.Trim());
                    
                }

            }

            return symbols.ToArray();
        }

        public void SavePriceValues(List<PriceValue> prices)
        {
            foreach (PriceValue pv in prices)
            {
                _priceValueRepo.Insert(pv);
                _priceValueRepo.SaveChanges();
            }

        }

        public void ValuePositions(List<Portfolio> portfolios, List<PriceValue> prices, int valuationId)
        {
            DateTime valuationTime = prices[0].PriceDate;
            ValuationTime = valuationTime;
            //delete any valuations that exists with the same valuationTime
            _valuationDetailRepo.Delete(valuationId, ValuationTime);
            _valuationRepo.SaveChanges();


            foreach (Portfolio p in portfolios)
            {
                foreach (PortfolioPosition pp in p.PortfolioPositions)
                {
                    ValuationDetail vDetail = new ValuationDetail();
                    vDetail.PortfolioId = pp.PortfolioId;
                    vDetail.ValuationId = valuationId;
                    vDetail.ValuationTime = valuationTime;
                    vDetail.PortfolioPositionId = pp.Id;
                    vDetail.Quantity = pp.Quantity;
                    vDetail.Price = prices.Where(x => x.Symbol == pp.Symbol.Trim()).FirstOrDefault().Price;
                    vDetail.PositionValue = vDetail.Quantity * vDetail.Price;
                    _valuationDetailRepo.Insert(vDetail);
                    _valuationDetailRepo.SaveChanges();

                }
            }
            ValuationStatus = "Success";
        }
    }
}