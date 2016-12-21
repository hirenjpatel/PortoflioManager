using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PMWS.Models
{
    public class Position : CreationInfo
    {
        [Required]
        public string PositionNumber { get; set; }
        public int PortolioId { get; set; }
        public string Symbol { get; set; }
        public decimal Quantity { get; set; }
        ///public decimal MarketValue { get; set; }
        public decimal CostBasis { get; set; }

        public virtual Portfolio Portfolio { get; set; }


    }
}