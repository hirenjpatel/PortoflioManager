using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PMApi.Models
{
    public class PortfolioPosition : CreationInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Portfolio")]
        public int PortfolioId { get; set; }

        public string Symbol { get; set; }

        public decimal Quantity { get; set; }
        public decimal CostBasis { get; set; }


        public virtual Portfolio Portfolio {get; set;}
    }
}