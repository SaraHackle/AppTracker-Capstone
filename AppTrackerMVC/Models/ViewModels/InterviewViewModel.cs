using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppTrackerMVC.Models.ViewModels
{
    public class InterviewViewModel
    {
        public Interview Interview { get; set; }
        public List<Application> Applications { get; set; }
        
    }
}
