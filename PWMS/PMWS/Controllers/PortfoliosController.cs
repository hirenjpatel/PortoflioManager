using PMWS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PMWS.Controllers
{
    public class PortfoliosController : ApiController
    {
        Portfolio[] portfolios = new Portfolio[]
      {
            new Portfolio { Id = 1, Name = "Scottrade", Description = "Scottrade Portfolio"},
            new Portfolio { Id = 2, Name = "Merrill Edge", Description = "Merrill Edge Portfolio" },
            new Portfolio { Id = 3, Name = "Virtual", Description = "Hardware" }
      };

        public IEnumerable<Portfolio> GetAllPortfolios()
        {
            return portfolios;
        }

        public IHttpActionResult GetPortfolio(int id)
        {
            var product = portfolios.FirstOrDefault((p) => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
    }
}
