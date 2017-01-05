using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
namespace PMApi.Models
{
    public class Valuation :CreationInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

        public string ValuationStatus { get; set; }

        public virtual ICollection<ValuationDetail> ValuationDetails { get; set; }



    }
}