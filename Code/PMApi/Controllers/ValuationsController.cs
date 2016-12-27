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
    public class ValuationsController : ApiController
    {
        private PortfolioManagerContext db = new PortfolioManagerContext();


        private ValuationRepo repo
            = new ValuationRepo(new PortfolioManagerContext());

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
    }
}