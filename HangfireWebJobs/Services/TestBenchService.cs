using HangfireWebJobs.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace HangfireWebJobs.Services
{
    public class TestBenchService
    {
        private Connection con;
        string _query;
        DataTable dt;
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
                LoggingService service = new LoggingService("TestBenchService/GetTestBenchList", ex.Message, System.DateTime.Now);
                service.LogError();
            }
            return priorityList.ToList();
        }

        public TestBenchDetail GetTestBenchInfo(int ID)
        {
            TestBenchDetail priorityList = null;
            try
            {
                using (LT_SERVER_DBEntities entity = new LT_SERVER_DBEntities())
                {
                    priorityList = entity.TestBenchDetails.Where(x => x.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LoggingService service = new LoggingService("TestBenchService/GetTestBenchInfo", ex.Message, System.DateTime.Now);
                service.LogError();
            }
            return priorityList;
        }

        public int GetTestBenchCount()
        {
            int count = 0;
            try
            {

                using (LT_SERVER_DBEntities entity = new LT_SERVER_DBEntities())
                {
                    count = entity.TestBenchDetails.Count();
                }
            }
            catch (Exception ex)
            {
                LoggingService service = new LoggingService("TestBenchService/GetTestBenchCount", ex.Message, System.DateTime.Now);
                service.LogError();
            }
            return count;
        }


        public TestBenchDetail GetTestBenchInfo(int ID, LT_SERVER_DBEntities entity)
        {
            TestBenchDetail priorityList = null;
            try
            {
                priorityList = entity.TestBenchDetails.Where(x => x.ID == ID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LoggingService service = new LoggingService("TestBenchService/GetTestBenchInfo", ex.Message, System.DateTime.Now);
                service.LogError();
            }
            return priorityList;
        }
        public List<PriorityModel> GetPriorityList()
        {
            IEnumerable<PriorityModel> priorityList = null;
            try
            {
                using (LT_SERVER_DBEntities entity = new LT_SERVER_DBEntities())
                {
                    var t = (from t1 in entity.Priorities
                             select new PriorityModel
                             {
                                 PriorityName = t1.PriorityName,
                                 ID = t1.ID,
                                 Frequency = t1.Frequency
                             }).ToList();
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

        public DataTable GetTestBenchTable(int id)
        {
            try
            {
                using (LT_SERVER_DBEntities entity = new LT_SERVER_DBEntities())
                {
                    TestBenchDetail _testBenchDetails = GetTestBenchInfo(id, entity);
                    string ip = _testBenchDetails.IPAddress;
                    string port = _testBenchDetails.PortNo.ToString();
                    string dbname = _testBenchDetails.DBName;
                    string user = _testBenchDetails.DBUser;
                    string pass = _testBenchDetails.DBPassword;
                    string source = ip + ", " + port;
                    con = new Connection(source, dbname, user, pass);
                    _query = "SELECT  DISTINCT * FROM  information_schema.tables;";
                    con.SqlQuery(_query);
                    dt = con.QueryEx();
                    con.closeConnection();
                }
                //SaveTableDetails(_testBenchDetails.TestBenchID, dt);
            }
            catch (Exception epx)
            {
                //LogLibrary.WriteErrorLog("Error in getData Function in From Table Class" + epx);
                Debug.WriteLine(epx);
            }
            return dt;
        }

        public string GenerateConnectionString(int testBenchID)
        {
            string databaseSource = "";
            try
            {
                TestBenchDetail _testBenchDetails = GetTestBenchInfo(testBenchID);
                string ip = _testBenchDetails.IPAddress;
                string port = _testBenchDetails.PortNo.ToString();
                string dbname = _testBenchDetails.DBName;
                string user = _testBenchDetails.DBUser;
                string pass = _testBenchDetails.DBPassword;
                string source = ".";
                databaseSource = @"Data Source=" +source + ";Initial Catalog=" + dbname + ";Integrated Security=True";


            }
            catch (Exception ex)
            {
                LoggingService service = new LoggingService("WebJob - Local_TestBenchService/GenerateConnectionString", ex.Message, System.DateTime.Now);
                service.LogError();
            }
            return databaseSource;
        }

        public ServerDataModel GenerateRemoteConnectionString(int testBenchID)
        {
            string databaseSource = "";
            ServerDataModel model = new ServerDataModel();
            try
            {
                TestBenchDetail _testBenchDetails = GetTestBenchInfo(testBenchID);
                string ip = _testBenchDetails.IPAddress;
                string port = _testBenchDetails.PortNo.ToString();
                string dbname = _testBenchDetails.DBName;
                string user = _testBenchDetails.DBUser;
                string pass = _testBenchDetails.DBPassword;
                string source = ip + ", " + port;
                databaseSource = @"Data Source=" + source + ";Initial Catalog=master;User ID=" + user + ";Password=" + pass + "";

                model.DBName = _testBenchDetails.DBName;
                model.DBSource = databaseSource;

            }
            catch (Exception ex)
            {
                LoggingService service = new LoggingService("WebJob - Local_TestBenchService/GenerateConnectionString", ex.Message, System.DateTime.Now);
                service.LogError();
                return null;
            }
            return model;
        }
        
        
    }
}