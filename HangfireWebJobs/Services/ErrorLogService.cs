using HangfireWebJobs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HangfireWebJobs.Models;

namespace HangfireWebJobs.Services
{
    public class ErrorLogService
    {
        public void InsertLog(tblErrorLog log)
        {
            using (LT_SERVER_DBEntities entity = new LT_SERVER_DBEntities())
            {
                entity.tblErrorLogs.Add(log);
                entity.SaveChanges();

            }
        }

        public List<tblErrorLog> GetErrorLog()
        {
            List<tblErrorLog> log = null;
            using (LT_SERVER_DBEntities entity = new LT_SERVER_DBEntities())
            {
                log = entity.tblErrorLogs.ToList();
            }
            return log;
        }

        public List<WebJobLogModel> GetWebJobLog()
        {
            IEnumerable<WebJobLogModel> priorityList = null;
            try
            {
                using (LT_SERVER_DBEntities entity = new LT_SERVER_DBEntities())
                {
                    var t = (from t1 in entity.tblWebJobLogs
                             select new WebJobLogModel
                             {
                                 ID = t1.ID,
                                 TestBenchID =  t1.TestBenchID,
                                 TableName = t1.TableName,
                                 StartDateTime = t1.StartDateTime,
                                 EndDateTime = t1.EndDateTime,
                                 Status = t1.Status
                             }).OrderByDescending(z=>z.ID).ToList();
                    priorityList = t.ToList();
                }
            }
            catch (Exception ex)
            {
                LoggingService service = new LoggingService("TestBenchService/GetPriorityList", ex.Message, System.DateTime.Now);
                service.LogError();
            }
            return priorityList.ToList();
        }

    }
}