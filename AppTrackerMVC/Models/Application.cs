using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppTrackerMVC.Models
{
    public class Application
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public DateTime DateApplied { get; set; }
        public string Salary { get; set; }
        public int UserId { get; set; } 
        public UserProfile User { get; set; }
    }
}
