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
    public class PriceValuesController : ApiController
    {
        private PriceValueRepo repo
            = new PriceValueRepo(new PortfolioManagerContext());


        [ResponseType(typeof(IQueryable<PriceValue>))]
        public async Task<IHttpActionResult> GetPriceValue(string symbol, DateTime pricedate)
        {
            List<PriceValue> prices = await  repo.GetPrices(symbol, pricedate);
            if (prices == null)
            {
                return NotFound();
            }

            return Ok(prices);
        }

        // GET: api/PriceValues/5
        [ResponseType(typeof(PriceValue))]
        public async Task<IHttpActionResult> GetPriceValue(int id)
        {
             PriceValue priceValue = await repo.GetPriceValueAsync(id);
            if (priceValue == null)
            {
                return NotFound();
            } 

            return Ok(priceValue);
        }

        // PUT: api/PriceValues/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPriceValue(int id, PriceValue priceValue)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != priceValue.Id)
            {
                return BadRequest();
            }

          

            try
            {
                await repo.UpdateAsync(priceValue);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!repo.PriceValueExists(id))
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

        // POST: api/PriceValues
        [ResponseType(typeof(PriceValue))]
        public async Task<IHttpActionResult> PostPriceValue(PriceValue priceValue)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await repo.InsertAsync(priceValue);
          

            return CreatedAtRoute("DefaultApi", new { id = priceValue.Id }, priceValue);
        }

        // DELETE: api/PriceValues/5
        [ResponseType(typeof(PriceValue))]
        public async Task<IHttpActionResult> DeletePriceValue(int id)
        {
            PriceValue priceValue = await repo.GetPriceValueAsync(id);
            if (priceValue == null)
            {
                return NotFound();
            }

            repo.Delete(priceValue);
            repo.SaveChangesAsync(priceValue);

            return Ok(priceValue);
        }

    }
}