using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace AppTrackerMVC.Models
{
    public class Tag
    {
        public int Id { get; set; }

        [DisplayName("Tag Name")]
        public string TagName { get; set; }
    }
}
