using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using PMApi.Models;
using PMApi.Repo;
using PMApi.PriceEngine;
using System.Web.Configuration;

namespace PMApi.Controllers
{
    public class ValuationsController : ApiController
    {
        private PortfolioManagerContext db = new PortfolioManagerContext();


        private ValuationRepo repo
            = new ValuationRepo(new PortfolioManagerContext());
        private ValuationDetailRepo valuationDetailRepo
           = new ValuationDetailRepo(new PortfolioManagerContext());
        private PortfolioRepo portfoliolRepo
          = new PortfolioRepo(new PortfolioManagerContext());

        // GET: api/Valuations
        public IQueryable<Valuation> GetValuations()
        {
            return db.Valuations;
        }


        // GET: api/Valuations/5
        [ResponseType(typeof(Valuation))]
        public async Task<IHttpActionResult> GetValuation(int id)
        {
            Valuation valuation = await repo.GetValuationAsync(id);
            if (valuation == null)
            {
                return NotFound();
            }

            return Ok(valuation);
        }

        [HttpGet]
        [Route("api/valuations/RunValuation/{id}")]
        [ResponseType(typeof(string))]
        public async Task<IHttpActionResult> RunValuation(int id)
        {
            var config_priceengine = WebConfigurationManager.AppSettings["PriceEngine"];
            var priceengineName = string.IsNullOrEmpty(config_priceengine) ? "GOOGLE" : config_priceengine;

            IPriceEngine priceEngine;
            if(priceengineName.ToUpper() == "GOOGLE")
            {
                priceEngine = new GooglePriceEngine();
            }
            else
            {
                priceEngine = new YahooPriceEngine();
            }


            Valuator valuator = new Valuator(priceEngine);
            valuator.RunValuation(1000);
            return Ok(valuator);
        }

        [HttpGet]
        [Route("api/valuations/Intraday/{valuationId}/{portfolioId}/{begtime}/{endtime}")]
        [ResponseType(typeof(ValuationSummary))]
        public async Task<IHttpActionResult> Intraday(int valuationId, int portfolioId, DateTime begtime, DateTime endtime)
        {
            ValuationSummary summary = new ValuationSummary();
            summary.BegTime = begtime;
            summary.EndTime = endtime;

            summary.Portfolio = await portfoliolRepo.GetPortoflioAsync(portfolioId);
            summary.ValuationDetail = await valuationDetailRepo.GetValuationDetailsAsync(portfolioId, valuationId, begtime, endtime);
            summary.ValuationDetailSummary = summary.ValuationDetail.GroupBy(x => x.ValuationTime).ToDictionary(x => x.Key, x => x.Sum(global => global.PositionValue));
            // summary.IntraDayChangeValuationDetailSummary = GenerateIntraDayChangeValuationDetailSummary(summary.ValuationDetailSummary);
            summary.IntraDayChangeValuationDetailSummary = summary.ValuationDetail.GroupBy(x => x.ValuationTime).ToDictionary(x => x.Key, x => x.Sum(y => y.Quantity * y.IntradayChange));
            if (summary == null)
            {
                return NotFound();
            }

            return Ok(summary);
        }

        [HttpGet]
        [Route("api/valuations/ValuationSummary/{valuationId}/{portfolioId}/{begtime}/{endtime}")]
        [ResponseType(typeof(ValuationSummary))]
        public async Task<IHttpActionResult> ValuationSummary(int valuationId, int portfolioId, DateTime begtime, DateTime endtime)
        {
            ValuationSummary summary = new ValuationSummary();
            summary.BegTime = begtime;
            summary.EndTime = endtime;
           
            summary.Portfolio = await portfoliolRepo.GetPortoflioAsync(portfolioId);
            summary.ValuationDetail = await valuationDetailRepo.GetValuationDetailsAsync(portfolioId, valuationId, begtime, endtime);
            summary.ValuationDetailSummary = summary.ValuationDetail.GroupBy(x => x.ValuationTime).ToDictionary(x => x.Key, x =>x.Sum(global =>global.PositionValue));
            summary.IntraDayChangeValuationDetailSummary = GenerateIntraDayChangeValuationDetailSummary(summary.ValuationDetailSummary);
            //summary.IntraDayChangeValuationDetailSummary = summary.ValuationDetail.GroupBy(x => x.ValuationTime).ToDictionary(x => x.Key, x => x.Sum(y => y.Quantity * y.IntradayChange));
            if (summary == null)
            {
                return NotFound();
            }

            return Ok(summary);
        }

        // PUT: api/Valuations/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutValuation(int id, Valuation valuation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != valuation.Id)
            {
                return BadRequest();
            }

            db.Entry(valuation).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ValuationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Valuations
        [ResponseType(typeof(Valuation))]
        public async Task<IHttpActionResult> PostValuation(Valuation valuation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Valuations.Add(valuation);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = valuation.Id }, valuation);
        }

        // DELETE: api/Valuations/5
        [ResponseType(typeof(Valuation))]
        public async Task<IHttpActionResult> DeleteValuation(int id)
        {
            Valuation valuation = await db.Valuations.FindAsync(id);
            if (valuation == null)
            {
                return NotFound();
            }

            db.Valuations.Remove(valuation);
            await db.SaveChangesAsync();

            return Ok(valuation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ValuationExists(int id)
        {
            return db.Valuations.Count(e => e.Id == id) > 0;
        }


        /// <summary>
        /// Generates the intra day change for the valaution by comparing the previous value
        /// </summary>
        public Dictionary<DateTime, decimal> GenerateIntraDayChangeValuationDetailSummary(Dictionary<DateTime, decimal> ValuationDetailSummary)
        {
            Dictionary<DateTime, decimal> IntraDayChangeValuationDetailSummary = new Dictionary<DateTime, decimal>();

            bool firstItem = true;
            decimal intraDayChange = 0;
            decimal portfolioValue = 0;
            foreach (KeyValuePair<DateTime, decimal> entry in ValuationDetailSummary)
            {
                if (firstItem)
                {
                    //this is the first item int he list so we do not have a previous records to compare with
                    //so set IntraDay Change will be 0
                    IntraDayChangeValuationDetailSummary.Add(entry.Key, intraDayChange);
                    portfolioValue = entry.Value;
                    firstItem = false;
                }
                else
                {
                    //subtract value from previous value to get intraday change
                    IntraDayChangeValuationDetailSummary.Add(entry.Key, (entry.Value - portfolioValue));
                }
            }

            return IntraDayChangeValuationDetailSummary;
        }
    }
}