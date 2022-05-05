using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppTrackerMVC.Models.ViewModels
{
    public class ApplicationDetailViewModel
    {
       public List<Interview> Interviews { get; set; }
        public List<Application> Applications { get; set; }
        public Application Application { get; set; }

        public Interview Interview { get; set; }

        public List<Tag> Tags { get; set; }
        public ApplicationTag ApplicationTag { get; set; }
       

    }
}
