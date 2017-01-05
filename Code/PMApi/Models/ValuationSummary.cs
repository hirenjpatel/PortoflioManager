using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMApi.Models
{
    public class ValuationSummary
    {

        public Portfolio Portfolio { get; set; }

      

        public DateTime BegTime { get; set; }
        public DateTime EndTime { get; set; }

        public List<ValuationDetail> ValuationDetail { get; set; }

        public Dictionary<DateTime, decimal> ValuationDetailSummary { get; set; }
        public Dictionary<DateTime, decimal> IntraDayChangeValuationDetailSummary { get; set; }


       
    }
}