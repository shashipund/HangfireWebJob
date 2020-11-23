using Hangfire;
using HangfireWebJobs.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace HangfireWebJobs.Models
{
    public class CreatePriorityJob
    {
        Connection con = null; 
        ServerConnection serverCon = null;
        string _query;
        bool _status;
        DataTable dt;
        public void GetPriorityList()
        {
            List<PriorityModel> list = null;
            using (LT_SERVER_DBEntities entity=new LT_SERVER_DBEntities ())
            {
                list = (from t1 in entity.Priorities
                            select new PriorityModel
                            {
                                ID=t1.ID,
                                Frequency=t1.Frequency
                            }).ToList();
            }
            //Console.WriteLine("Method Called !!!");
            CreateRecurringJob(list);
        }

        private void CreateRecurringJob(List<PriorityModel> list)
        {
            foreach (var item in list)
            {
                switch (item.ID)
                {
                    case 1 :
                        RecurringJob.AddOrUpdate(() => GetTablePriorityRecords1(item.ID), Cron.MinuteInterval(Convert.ToInt32(item.Frequency)));
                        break;
                    case 2:
                        RecurringJob.AddOrUpdate(() => GetTablePriorityRecords2(item.ID), Cron.HourInterval(Convert.ToInt32(item.Frequency)));
                        break;
                    case 3:
                        RecurringJob.AddOrUpdate(() => GetTablePriorityRecords3(item.ID), Cron.HourInterval(Convert.ToInt32(item.Frequency)));
                        break;
                    case 4:
                        RecurringJob.AddOrUpdate(() => GetTablePriorityRecords4(item.ID), Cron.HourInterval(Convert.ToInt32(item.Frequency)));
                        break;
                    case 5:
                        RecurringJob.AddOrUpdate(() => GetTablePriorityRecords5(item.ID), Cron.HourInterval(Convert.ToInt32(item.Frequency)));
                        break;
                }
            }
        }

        public void GetTablePriorityRecords1(int priorityID)
        {
            try
            {
                using (LT_SERVER_DBEntities entity = new LT_SERVER_DBEntities())
                {
                    List<TablePriority> list = entity.TablePriorities.Where(x => x.PriorityID == priorityID).ToList<TablePriority>();
                    GetTableData(list);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public void GetTablePriorityRecords2(int priorityID)
        {
            try
            {
                using (LT_SERVER_DBEntities entity = new LT_SERVER_DBEntities())
                {
                    List<TablePriority> list = entity.TablePriorities.Where(x => x.PriorityID == priorityID).ToList<TablePriority>();
                    GetTableData(list);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public void GetTablePriorityRecords3(int priorityID)
        {
            try
            {
                using (LT_SERVER_DBEntities entity = new LT_SERVER_DBEntities())
                {
                    List<TablePriority> list = entity.TablePriorities.Where(x => x.PriorityID == priorityID).ToList<TablePriority>();
                    GetTableData(list);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public void GetTablePriorityRecords4(int priorityID)
        {
            try
            {
                using (LT_SERVER_DBEntities entity = new LT_SERVER_DBEntities())
                {
                    List<TablePriority> list = entity.TablePriorities.Where(x => x.PriorityID == priorityID).ToList<TablePriority>();
                    GetTableData(list);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public void GetTablePriorityRecords5(int priorityID)
        {
            try
            {
                using (LT_SERVER_DBEntities entity = new LT_SERVER_DBEntities())
                {
                    List<TablePriority> list = entity.TablePriorities.Where(x => x.PriorityID == priorityID).ToList<TablePriority>();
                    GetTableData(list);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void GetTableData(List<TablePriority> list)
        {
            try
            {
                TestBenchService service = new TestBenchService();
                foreach (var item in list)
                {
                    TestBenchDetail _testBenchDetails = service.GetTestBenchInfo(item.TestBenchID);
                    string ip = _testBenchDetails.IPAddress;
                    string port = _testBenchDetails.PortNo.ToString();
                    string dbname = _testBenchDetails.DBName;
                    string user = _testBenchDetails.DBUser;
                    string pass = _testBenchDetails.DBPassword;
                    string source = ip + ", " + port;
                    con = new Connection(source, dbname, user, pass);

                    _query = "SELECT *  " +
                            " FROM " +item.TableName; 
                    con.SqlQuery(_query);
                    dt = con.QueryEx();
                    con.closeConnection();
                    InsertData(dt, item.TableName);
                }
            }
            catch (Exception ex)
            {

            }
        }
        

        private void InsertData(DataTable table, string tableName)
        {
            try
            {
                serverCon = new ServerConnection();
                string col = "";
                foreach (DataColumn colName in dt.Columns)
                {
                    col = col + colName.ColumnName + ",";
                }
                //_query = "INSERT INTO "+tableName + " VALUES(+;

                //con.SqlQuery(_query);
                //con.NonQueryEx();
                con.closeConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}