using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppTrackerMVC.Models
{
    public class ApplicationTag
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
