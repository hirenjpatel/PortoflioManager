using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PMWS.Models
{
    public class PositionValuation
    {

        [Required]
        public string Id { get; set; }
        public int ValuationId { get; set; }
        public string ValuationTime { get; set; }
        public decimal PositionNumber { get; set; }
        ///public decimal MarketValue { get; set; }
        public decimal MarketValue { get; set; }


    }
}