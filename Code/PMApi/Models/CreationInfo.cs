using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMApi.Models
{
    public class CreationInfo
    {

        public string CreationName { get; set; }
        public DateTime? CreationDate { get; set; }
        public string RevisionName { get; set; }

        public DateTime? RevisionDate { get; set; }

    }
}