using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppTrackerMVC.Models
{
    public class Interview
    {
        public int Id { get; set; }
        public DateTime InterviewDate { get; set; }
        public string AdditionalInfo { get; set; }
        public int ApplicationId { get; set; }
        public Application Application { get; set; }
    }
}
