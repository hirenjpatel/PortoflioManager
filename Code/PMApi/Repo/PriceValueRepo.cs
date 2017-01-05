using PMApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PMApi.Repo
{
    public class PriceValueRepo
    {

        PortfolioManagerContext _db;
        public PriceValueRepo()
        {
            _db = new PortfolioManagerContext();
        }
        public PriceValueRepo(PortfolioManagerContext connection)
        {
            _db = connection;
        }

        /// <summary>
        /// Get Prices for the speicfied symbol and the price date
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public async Task<List<PriceValue>> GetPrices(string symbol, DateTime priceDate)
        {
            var prices = await (from b in _db.PriceValues
                        where b.Symbol == symbol
                        where b.PriceDate.ToString().Contains(priceDate.ToString())
                        select b).ToListAsync();


            return prices;
        }

        public  async Task<PriceValue> GetPriceValueAsync(int id)
        {
            using (_db)
            {
                return await _db.PriceValues.FirstOrDefaultAsync(x => x.Id == id);
            }
        }

        public PriceValue GetPriceValue(int id)
        {
            var Pricevalue = _db.PriceValues.Find(id);

            return Pricevalue;
        }

        public async Task UpdateAsync(PriceValue pricevalue)
        {
            using (_db)
            {

                _db.PriceValues.Attach(pricevalue);
                _db.Entry(pricevalue).State = EntityState.Modified;
                await _db.SaveChangesAsync();

            }
        }

        public void Update(PriceValue pricevalue)
        {
            pricevalue.RevisionDate = DateTime.Now;
            _db.Entry(pricevalue).State = EntityState.Modified;
        }

        public async Task InsertAsync(PriceValue pricevalue)
        {
            using (_db)
            {
                _db.PriceValues.Add(pricevalue);
                await _db.SaveChangesAsync();
            }
        }

        public void Insert(PriceValue pricevalue)
        {
            pricevalue.CreationDate = DateTime.Now;
            _db.PriceValues.Add(pricevalue);
        }

        public void Delete(PriceValue pricevalue)
        {
            _db.PriceValues.Remove(pricevalue);
        }

        public async Task DeleteAsync(int id)
        {
            using (_db)
            {
                var pricevalue = new PriceValue { Id = id };

                _db.PriceValues.Attach(pricevalue);
                _db.Entry(pricevalue).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
            }
        }

        public bool PriceValueExists(int id)
        {
            return _db.PriceValues.Count(e => e.Id == id) > 0;
        }

        public async void SaveChangesAsync(PriceValue pricevalue)
        {
            await _db.SaveChangesAsync();
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }




    }
}