using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PMWS.Models
{
    public class Valuation
    {

        [Required]
        public int Id { get; set; }

        public string ValuationName { get; set; }


    }
}