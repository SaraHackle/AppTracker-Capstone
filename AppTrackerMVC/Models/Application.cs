using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppTrackerMVC.Models
{
    public class Application
    {
        public int Id { get; set; }

        [Required]
        public string Company { get; set; }

    
        public string Location { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DisplayName("Date Applied")]
        public DateTime DateApplied { get; set; }
        public string Salary { get; set; }
        public int UserId { get; set; } 
        public UserProfile User { get; set; }
    }
}
