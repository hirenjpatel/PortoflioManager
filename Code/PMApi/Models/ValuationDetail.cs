using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PMApi.Models
{
    public class ValuationDetail :CreationInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ValuationId { get; set; }
        public int PortfolioId { get; set; }

        public DateTime ValuationTime { get; set; }

        [ForeignKey("PortfolioPosition")]
        public int PortfolioPositionId { get; set; }

        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal IntradayChange { get; set; }
        public decimal PositionValue { get; set; }

        public virtual PortfolioPosition PortfolioPosition { get; set; }


    }
}