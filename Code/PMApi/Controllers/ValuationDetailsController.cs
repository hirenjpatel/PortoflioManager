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

namespace PMApi.Controllers
{
    public class ValuationDetailsController : ApiController
    {
        private PortfolioManagerContext db = new PortfolioManagerContext();

        // GET: api/ValuationDetails
        public IQueryable<ValuationDetail> GetValuationDetails()
        {
            return db.ValuationDetails;
        }

        // GET: api/ValuationDetails/5
        [ResponseType(typeof(ValuationDetail))]
        public async Task<IHttpActionResult> GetValuationDetail(int id)
        {
            ValuationDetail valuationDetail = await db.ValuationDetails.FindAsync(id);
            if (valuationDetail == null)
            {
                return NotFound();
            }

            return Ok(valuationDetail);
        }

        // PUT: api/ValuationDetails/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutValuationDetail(int id, ValuationDetail valuationDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != valuationDetail.Id)
            {
                return BadRequest();
            }

            db.Entry(valuationDetail).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ValuationDetailExists(id))
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

        // POST: api/ValuationDetails
        [ResponseType(typeof(ValuationDetail))]
        public async Task<IHttpActionResult> PostValuationDetail(ValuationDetail valuationDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ValuationDetails.Add(valuationDetail);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = valuationDetail.Id }, valuationDetail);
        }

        // DELETE: api/ValuationDetails/5
        [ResponseType(typeof(ValuationDetail))]
        public async Task<IHttpActionResult> DeleteValuationDetail(int id)
        {
            ValuationDetail valuationDetail = await db.ValuationDetails.FindAsync(id);
            if (valuationDetail == null)
            {
                return NotFound();
            }

            db.ValuationDetails.Remove(valuationDetail);
            await db.SaveChangesAsync();

            return Ok(valuationDetail);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ValuationDetailExists(int id)
        {
            return db.ValuationDetails.Count(e => e.Id == id) > 0;
        }

    }
}