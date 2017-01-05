using PMApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace PMApi.Repo
{
    public class ValuationRepo
    {
        PortfolioManagerContext _db;

        public ValuationRepo(PortfolioManagerContext connection)
        {
            _db = connection;
        }

        public ValuationRepo()
        {
            _db = new PortfolioManagerContext();
        }

        public async Task<Valuation> GetValuationAsync(int id)
        {
            using (_db)
            {

                return await _db.Valuations.Where(x => x.Id == id).Include(s => s.ValuationDetails.Select(c => c.PortfolioPosition).Select(c => c.Portfolio).Select(x => x.PortfolioPositions)).FirstOrDefaultAsync();
                
            }
        }

        public Valuation GetValuation(int id)
        {
            return _db.Valuations.Where(x => x.Id == id).FirstOrDefault();

        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        public void Update(Valuation valuation)
        {
            valuation.RevisionDate = DateTime.Now;
            _db.Entry(valuation).State = EntityState.Modified;
        }

        public void Insert(Valuation valuation)
        {
            _db.Valuations.Add(valuation);
        }


     


    }
}