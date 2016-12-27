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

namespace PMApi.Controllers
{
    public class PortfoliosController : ApiController
    {
        private PortfolioManagerContext db = new PortfolioManagerContext();

        private PortfolioRepo repo
            = new PortfolioRepo(new PortfolioManagerContext());
        // GET: api/Portfolios
        public IQueryable<Portfolio> GetPortfolios()
        {
            return db.Portfolios;
        }

        // GET: api/Portfolios/5
        [ResponseType(typeof(Portfolio))]
        public async Task<IHttpActionResult> GetPortfolio(int id)
        {
            Portfolio portfolio = await repo.GetPortoflioAsync(id);
            if (portfolio == null)
            {
                return NotFound();
            }

            return Ok(portfolio);
        }

        // PUT: api/Portfolios/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPortfolio(int id, Portfolio portfolio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != portfolio.Id)
            {
                return BadRequest();
            }

         

            try
            {
                await repo.Update(portfolio);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PortfolioExists(id))
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

        // POST: api/Portfolios
        [ResponseType(typeof(Portfolio))]
        public async Task<IHttpActionResult> PostPortfolio(Portfolio portfolio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await repo.InsertAsync(portfolio);

            return CreatedAtRoute("DefaultApi", new { id = portfolio.Id }, portfolio);
        }

        // DELETE: api/Portfolios/5
        [ResponseType(typeof(Portfolio))]
        public async Task<IHttpActionResult> DeletePortfolio(int id)
        {
            Portfolio portfolio = await repo.GetPortoflioAsync(id);
            
            await repo.DeleteAsync(id);

            return Ok(portfolio);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PortfolioExists(int id)
        {
            return db.Portfolios.Count(e => e.Id == id) > 0;
        }
    }
}