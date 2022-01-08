using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HangfireWebJobs.Models
{
    public class PriorityModel
    {
        public string PriorityName { get; set; }
        public int ID { get; set; }
        public string Frequency { get; set; }
    }
}