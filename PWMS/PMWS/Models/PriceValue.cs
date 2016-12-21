using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PMWS.Models
{
    public class PriceValue : CreationInfo
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Symbol { get; set; }
        [Required]
        public DateTime PriceDate { get; set; }
        [Required]
        public decimal Price { get; set; }
        public decimal LowPrice { get; set; }
        public decimal HighPrice { get; set; }
        public decimal OpenPrice { get; set; }

    }
}