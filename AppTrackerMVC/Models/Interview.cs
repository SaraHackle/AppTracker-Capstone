using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppTrackerMVC.Models
{
    public class Interview
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Interview Date")]
        public DateTime InterviewDate { get; set; }

        [DisplayName("Additional Info")]
        public string AdditionalInfo { get; set; }
        public int ApplicationId { get; set; }
        public Application Application { get; set; }
    }
}
