using PMApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PMApi.Repo
{
    public class PortfolioRepo
    {
        PortfolioManagerContext _db;

        public PortfolioRepo(PortfolioManagerContext connection)
        {
            _db = connection;
        }

        public async Task<Portfolio> GetPortoflioAsync(int id)
        {
            using (_db)
            {
       
                return await _db.Portfolios.Where(x => x.Id == id).Include(s => s.PortfolioPositions).FirstOrDefaultAsync();
            }
        }

        public async Task Update(Portfolio portoflio)
        {
            using (_db)
            {

                _db.Portfolios.Attach(portoflio);
                _db.Entry(portoflio).State = EntityState.Modified;
                await _db.SaveChangesAsync();

            }
        }

        public async Task InsertAsync(Portfolio portfolio)
        {
            using (_db)
            {
                _db.Portfolios.Add(portfolio);
                await _db.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (_db)
            {
                var portfolio = new Portfolio { Id = id };

                _db.Portfolios.Attach(portfolio);
                _db.Entry(portfolio).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
            }
        }
    }
}