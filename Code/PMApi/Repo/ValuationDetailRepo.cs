using PMApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace PMApi.Repo
{
    public class ValuationDetailRepo
    {
        PortfolioManagerContext _db;

        public ValuationDetailRepo(PortfolioManagerContext connection)
        {
            _db = connection;
        }

        public ValuationDetailRepo()
        {
            _db = new PortfolioManagerContext();
        }

        public async Task<ValuationDetail> GetValuationAsync(int id)
        {
            using (_db)
            {

                return await _db.ValuationDetails.Where(x => x.Id == id).FirstOrDefaultAsync();
                
            }
        }

        public ValuationDetail GetValuationDetail(int id)
        {
            //return _db.Valuations.Where(x => x.Id == id).Include(s => s.ValuationDetails.Select(c => c.PortfolioPosition).Select(c => c.Portfolio).Select(x => x.PortfolioPositions)).FirstOrDefault();
            return _db.ValuationDetails.Where(x => x.Id == id).FirstOrDefault();

        }

        public List<ValuationDetail> GetValuationDetails(int valuationId, DateTime valuationTime)
        {
            //return _db.Valuations.Where(x => x.Id == id).Include(s => s.ValuationDetails.Select(c => c.PortfolioPosition).Select(c => c.Portfolio).Select(x => x.PortfolioPositions)).FirstOrDefault();
            return _db.ValuationDetails.Where(x => x.ValuationId == valuationId && x.ValuationTime == valuationTime).ToList();

        }


        public List<ValuationDetail> GetValuationDetails(int portfolioId, int valuationId,  DateTime begTime, DateTime endTime)
        {
            //return _db.Valuations.Where(x => x.Id == id).Include(s => s.ValuationDetails.Select(c => c.PortfolioPosition).Select(c => c.Portfolio).Select(x => x.PortfolioPositions)).FirstOrDefault();
            return _db.ValuationDetails.Where(x => x.ValuationId == valuationId && x.PortfolioId == portfolioId && x.ValuationTime >= begTime && x.ValuationTime <= endTime).OrderBy(x => x.ValuationTime).ToList();

        }

        public async Task<List<ValuationDetail>> GetValuationDetailsAsync(int portfolioId, int valuationId, DateTime begTime, DateTime endTime)
        {
            //return _db.Valuations.Where(x => x.Id == id).Include(s => s.ValuationDetails.Select(c => c.PortfolioPosition).Select(c => c.Portfolio).Select(x => x.PortfolioPositions)).FirstOrDefault();
            return await _db.ValuationDetails.Where(x => x.ValuationId == valuationId && x.PortfolioId == portfolioId && x.ValuationTime >= begTime && x.ValuationTime <= endTime).OrderBy(x => x.ValuationTime).ToListAsync();

        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        public void Update(ValuationDetail valuationDetail)
        {
            valuationDetail.RevisionDate = DateTime.Now;
            _db.Entry(valuationDetail).State = EntityState.Modified;
        }

        public void Insert(ValuationDetail valuationDetail)
        {
            valuationDetail.CreationDate = DateTime.Now;
            _db.ValuationDetails.Add(valuationDetail);
        }


        public void Delete(int valuationId, DateTime valuationTime)
        {
            _db.ValuationDetails.RemoveRange(_db.ValuationDetails.Where(c => c.ValuationId == valuationId && c.ValuationTime == valuationTime));
        }



    }
}