using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PMWS.Models
{
    public class Portfolio : CreationInfo
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
      

        public virtual ICollection<Position> Positions { get; set; }
    }
}