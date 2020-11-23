using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HangfireWebJobs.Models
{
    public class TestBenchModel
    {
        public string TestBenchID { get; set; }
        public string TestBenchName { get; set; }
        public string DBName { get; set; }
        public string IPAddress { get; set; }
        public string DBUser { get; set; }
        public string DBPassword { get; set; }
        public Nullable<int> PortNo { get; set; }
        public int ID { get; set; }
    }
}