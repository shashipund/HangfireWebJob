using Hangfire;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HangfireWebJobs.Controllers
{
    public class HomeController : Controller
    {
        Connection con = null;
        string _query;
        bool _status;
        DataTable dt;
        public ActionResult Index()
        {
            
            return View();
        }


        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}