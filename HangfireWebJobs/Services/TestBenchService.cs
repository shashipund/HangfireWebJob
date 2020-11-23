using HangfireWebJobs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HangfireWebJobs.Services
{
    public class TestBenchService
    {
        public List<TestBenchModel> GetTestBenchList()
        {
            IEnumerable<TestBenchModel> priorityList = null;
            try
            {
                using (LT_SERVER_DBEntities entity = new LT_SERVER_DBEntities())
                {
                    var t = (from t1 in entity.TestBenchDetails
                             select new TestBenchModel
                             {
                                 ID = t1.ID,
                                 TestBenchID = t1.TestBenchID,
                                 TestBenchName = t1.TestBenchName,
                                 DBName = t1.DBName,
                                 DBPassword = t1.DBPassword,
                                 DBUser = t1.DBUser,
                                 PortNo = t1.PortNo,
                                 IPAddress = t1.IPAddress

                             }).ToList();
                    priorityList = t.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return priorityList.ToList();
        }

        public TestBenchDetail GetTestBenchInfo(string ID)
        {
            TestBenchDetail priorityList = null;
            try
            {
                using (LT_SERVER_DBEntities entity = new LT_SERVER_DBEntities())
                {
                    priorityList = entity.TestBenchDetails.Where(x => x.TestBenchID == ID).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return priorityList;
        }
    }
}