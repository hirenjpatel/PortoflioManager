using PMWS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMWS.Repositories
{
    public interface IPortoflioRepository
    {
        IEnumerable<Portfolio> GetPortfolios();

        Portfolio GetPortfolio(int id);

        void Insert(Portfolio portfolio);

        void Update(Portfolio portfolio);

        void Delete(Portfolio portfolio);
        void Delete(int id);
        void SaveChanges(Portfolio portfolio)





        
    }
}