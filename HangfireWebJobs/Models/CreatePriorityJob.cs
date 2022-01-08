using Hangfire;
using HangfireWebJobs.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace HangfireWebJobs.Models
{
    public class CreatePriorityJob
    {
        private TestBenchService _service = new TestBenchService();
        Connection con = null;
        ServerConnection serverCon = null;
        string _query, query;
        bool _status;
        DataTable dt;
        public void GetPriorityList()
        {
            try
            {
                List<PriorityModel> list = null;
                using (LT_SERVER_DBEntities entity = new LT_SERVER_DBEntities())
                {
                    list = (from t1 in entity.Priorities
                            select new PriorityModel
                            {
                                ID = t1.ID,
                                Frequency = t1.Frequency,
                                PriorityName = t1.PriorityName
                            }).ToList();
                }
                //Console.WriteLine("Method Called !!!");
                CreateRecurringJob(list);
            }
            catch (Exception ex)
            {
                LoggingService service2 = new LoggingService("CreatePriorityJob/GetPriorityList", ex.InnerException.Message, System.DateTime.Now);
                service2.LogError();
            }
        }

        private void CreateRecurringJob(List<PriorityModel> list)
        {
            foreach (var item in list)
            {
                switch (item.PriorityName)
                {
                    case "A":
                        RecurringJob.AddOrUpdate(() => GetTablePriorityRecords1(item.ID), Cron.HourInterval(Convert.ToInt32(item.Frequency)));
                        break;
                    case "B":
                        RecurringJob.AddOrUpdate(() => GetTablePriorityRecords2(item.ID), Cron.HourInterval(Convert.ToInt32(item.Frequency)));
                        break;
                    case "C":
                        RecurringJob.AddOrUpdate(() => GetTablePriorityRecords3(item.ID), Cron.HourInterval(Convert.ToInt32(item.Frequency)));
                        break;
                    default:
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
        private void GetTableData(List<TablePriority> list)
        {
            try
            {

                TestBenchService service = new TestBenchService();
                foreach (var item in list)
                {
                    TestBenchDetail _testBenchDetails = _service.GetTestBenchInfo(Convert.ToInt32(item.TestBenchID));
                    string ip = _testBenchDetails.IPAddress;
                    string port = _testBenchDetails.PortNo.ToString();
                    string dbname = _testBenchDetails.DBName;
                    string user = _testBenchDetails.DBUser;
                    string pass = _testBenchDetails.DBPassword;
                    string source = ip + ", " + port;

                    con = new Connection(source, dbname, user, pass);
                    if (item.TableName.Trim() == "BBT_HV_Production Report")
                    {
                        try
                        {
                            serverCon = new ServerConnection();
                            query = "SELECT convert(varchar, MAX([Date_Time]), 103) AS DATE FROM [BBT_HV_Production Report]";
                            serverCon.SqlQuery(query);

                            string MaxDate = serverCon.DataReaderSingleRow();
                            serverCon.closeConnection();

                            if (!String.IsNullOrEmpty(MaxDate))
                            {
                                LoggingService s114 = new LoggingService("CreatePriorityJob/GetTableData", "Max Date for BBT_HV_Production Report = " + MaxDate, System.DateTime.Now);
                                s114.LogError();
                                _query = "SELECT *  " +
                                        " FROM [" + item.TableName + "] WHERE Date_Time >='" + MaxDate + "'";
                            }
                            else
                            {
                                _query = "SELECT *  " +
                                        " FROM [" + item.TableName + "]";
                            }
                            LoggingService s15 = new LoggingService("CreatePriorityJob/GetTableData", "Select Query for HV " + _query, System.DateTime.Now);
                            s15.LogError();
                            con.SqlQuery(_query);
                            dt = con.QueryEx();
                            con.closeConnection();
                            LoggingService s1 = new LoggingService("CreatePriorityJob/GetTableData", "Total Records fetch from remote machine table " + item.TableName + " are " + dt.Rows.Count, System.DateTime.Now);
                            s1.LogError();

                            InsertData(dt, item.TableName);
                        }
                        catch (Exception ex)
                        {
                            LoggingService service1222 = new LoggingService("CreatePriorityJob/GetTableData", "Exception in Connection Remote machine : " + ex.InnerException.Message, System.DateTime.Now);
                            service1222.LogError();
                        }
                    }
                    if (item.TableName.Trim() == "BBT_IR_Production Report")
                    {
                        try
                        {
                            serverCon = new ServerConnection();
                            query = "SELECT convert(varchar, MAX([Date_Time]), 103) AS DATE FROM [BBT_IR_Production Report]";
                            LoggingService s111 = new LoggingService("CreatePriorityJob/GetTableData", "Query for Max Date = " + query, System.DateTime.Now);
                            s111.LogError();
                            serverCon.SqlQuery(query);

                            string MaxDate = serverCon.DataReaderSingleRow();
                            serverCon.closeConnection();

                            LoggingService s1111 = new LoggingService("CreatePriorityJob/GetTableData", "Max Date for BBT_IR_Production Report = " + MaxDate, System.DateTime.Now);
                            s1111.LogError();
                            if (!String.IsNullOrEmpty(MaxDate))
                            {
                                _query = "SELECT *  " +
                                        " FROM [" + item.TableName + "] WHERE Date_Time >='" + MaxDate+"'";
                            }
                            else
                            {
                                _query = "SELECT *  " +
                                        " FROM [" + item.TableName + "]";
                            }
                            LoggingService s15 = new LoggingService("CreatePriorityJob/GetTableData", "Select Query for IR " + _query, System.DateTime.Now);
                            s15.LogError();

                            con.SqlQuery(_query);
                            dt = con.QueryEx();
                            con.closeConnection();
                            LoggingService s1 = new LoggingService("CreatePriorityJob/GetTableData", "Total Records fetch from remote machine table " + item.TableName + " are " + dt.Rows.Count, System.DateTime.Now);
                            s1.LogError();

                            InsertData(dt, item.TableName);
                        }
                        catch (Exception ex)
                        {
                            LoggingService service1 = new LoggingService("CreatePriorityJob/GetTableData", "Exception in Connection Remote machine : " + ex.InnerException.Message, System.DateTime.Now);
                            service1.LogError();
                        }
                    }
                    if (item.TableName.Trim() == "BBT_IR_Settings")
                    {
                        try
                        {
                            _query = "SELECT *  " +
                                    " FROM [" + item.TableName + "]";
                            con.SqlQuery(_query);
                            dt = con.QueryEx();
                            con.closeConnection();
                            LoggingService s1 = new LoggingService("CreatePriorityJob/GetTableData", "Total Records fetch from remote machine table " + item.TableName + " are " + dt.Rows.Count, System.DateTime.Now);
                            s1.LogError();

                            InsertData(dt, item.TableName);
                        }
                        catch (Exception ex)
                        {
                            LoggingService service1 = new LoggingService("CreatePriorityJob/GetTableData", "Exception in Connection Remote machine : " + ex.InnerException.Message, System.DateTime.Now);
                            service1.LogError();
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                LoggingService service = new LoggingService("CreatePriorityJob/GetTableData", "Exception in Connection Remote machine : " + ex.InnerException.Message, System.DateTime.Now);
                service.LogError();
            }
        }


        private void InsertData(DataTable table, string tableName)
        {
            try
            {

                int count = 0;
                if (tableName.Trim() == "BBT_HV_Production Report")
                {
                    LoggingService service34 = new LoggingService("CreatePriorityJob/InsertData", "In BBT_HV insert condition ", System.DateTime.Now);
                    service34.LogError();

                    serverCon = new ServerConnection();
                    //SqlBulkCopy bulk = serverCon.SqlBulkQuery();
                    //serverCon.DestinationTable("[BBT_HV_Production Report]");
                    //bulk.ColumnMappings.Add(dt.Columns[0].ToString(), "Barcode");
                    //bulk.ColumnMappings.Add(dt.Columns[1].ToString(), "Date_Time");
                    //bulk.ColumnMappings.Add(dt.Columns[2].ToString(), "Project No");
                    //bulk.ColumnMappings.Add(dt.Columns[3].ToString(), "Month Code");
                    //bulk.ColumnMappings.Add(dt.Columns[4].ToString(), "Tag No");
                    //bulk.ColumnMappings.Add(dt.Columns[5].ToString(), "Stack No");
                    //bulk.ColumnMappings.Add(dt.Columns[6].ToString(), "Voltage");
                    //bulk.ColumnMappings.Add(dt.Columns[7].ToString(), "Step 1");
                    //bulk.ColumnMappings.Add(dt.Columns[8].ToString(), "Step 2");
                    //bulk.ColumnMappings.Add(dt.Columns[9].ToString(), "Step 3");
                    //bulk.ColumnMappings.Add(dt.Columns[10].ToString(), "Step 4");
                    //bulk.ColumnMappings.Add(dt.Columns[11].ToString(), "Step 5");
                    //bulk.ColumnMappings.Add(dt.Columns[12].ToString(), "Step 6");
                    //bulk.ColumnMappings.Add(dt.Columns[13].ToString(), "HV Result");
                    //bulk.ColumnMappings.Add(dt.Columns[14].ToString(), "Date");
                    foreach (DataRow row in dt.Rows)
                    {
                        _query = "INSERT INTO [dbo].[BBT_HV_Production Report]([Barcode],[Date_Time],[Project No],[Month Code],[Tag No],[Stack No],[Voltage],[Step 1],[Step 2],[Step 3],[Step 4],[Step 5],[Step 6],[HV Result],[Date]) " +
                                " VALUES (@bar,@dttime,@projno,@mon,@tag,@stack,@volt,@step1,@step2,@step3,@step4,@step5,@step6,@hv,@date)";
                        serverCon.SqlQuery(_query);
                        LoggingService service4 = new LoggingService("CreatePriorityJob/InsertData", "Query=" + _query, System.DateTime.Now);
                        service4.LogError();
                        serverCon.Cmd.Parameters.AddWithValue("@bar", row["Barcode"]);
                        serverCon.Cmd.Parameters.AddWithValue("@dttime", Convert.ToDateTime(row["Date_Time"]));
                        serverCon.Cmd.Parameters.AddWithValue("@projno", row["Project No"]);
                        serverCon.Cmd.Parameters.AddWithValue("@mon", row["Month Code"]);
                        serverCon.Cmd.Parameters.AddWithValue("@tag", row["Tag No"]);
                        serverCon.Cmd.Parameters.AddWithValue("@stack", row["Stack No"]);
                        serverCon.Cmd.Parameters.AddWithValue("@volt", row["Voltage"]);
                        serverCon.Cmd.Parameters.AddWithValue("@step1", row["Step 1"]);
                        serverCon.Cmd.Parameters.AddWithValue("@step2", row["Step 2"]);
                        serverCon.Cmd.Parameters.AddWithValue("@step3", row["Step 3"]);
                        serverCon.Cmd.Parameters.AddWithValue("@step4", row["Step 4"]);
                        serverCon.Cmd.Parameters.AddWithValue("@step5", row["Step 5"]);
                        serverCon.Cmd.Parameters.AddWithValue("@step6", row["Step 6"]);
                        serverCon.Cmd.Parameters.AddWithValue("@hv", row["HV Result"]);
                        serverCon.Cmd.Parameters.AddWithValue("@date", row["Date"]);

                        serverCon.NonQueryEx();
                        count++;

                    }
                    //serverCon.PushData(dt);
                    serverCon.closeConnection();
                    LoggingService service2 = new LoggingService("CreatePriorityJob/InsertData", "Total Records Inserted =" + count, System.DateTime.Now);
                    service2.LogError();
                }
                if (tableName.Trim() == "BBT_IR_Production Report")
                {
                    try
                    {
                        serverCon = new ServerConnection();
                        //SqlBulkCopy bulk = serverCon.SqlBulkQuery();
                        //serverCon.DestinationTable("[BBT_IR_Production Report]");
                        //bulk.ColumnMappings.Add(dt.Columns[0].ToString(), "Barcode");
                        //bulk.ColumnMappings.Add(dt.Columns[1].ToString(), "User Name");
                        //bulk.ColumnMappings.Add(dt.Columns[2].ToString(), "Date_Time");
                        //bulk.ColumnMappings.Add(dt.Columns[3].ToString(), "Project No");
                        //bulk.ColumnMappings.Add(dt.Columns[4].ToString(), "Month Code");
                        //bulk.ColumnMappings.Add(dt.Columns[5].ToString(), "Tag No");
                        //bulk.ColumnMappings.Add(dt.Columns[6].ToString(), "Stack No");
                        //bulk.ColumnMappings.Add(dt.Columns[7].ToString(), "Lower Limit");
                        //bulk.ColumnMappings.Add(dt.Columns[8].ToString(), "Upper Limit");
                        //bulk.ColumnMappings.Add(dt.Columns[9].ToString(), "Step 1");
                        //bulk.ColumnMappings.Add(dt.Columns[10].ToString(), "Step 2");
                        //bulk.ColumnMappings.Add(dt.Columns[11].ToString(), "Step 3");
                        //bulk.ColumnMappings.Add(dt.Columns[12].ToString(), "Step 4");
                        //bulk.ColumnMappings.Add(dt.Columns[13].ToString(), "Step 5");
                        //bulk.ColumnMappings.Add(dt.Columns[14].ToString(), "Step 6");
                        //bulk.ColumnMappings.Add(dt.Columns[15].ToString(), "Step 7");
                        //bulk.ColumnMappings.Add(dt.Columns[16].ToString(), "Step 8");
                        //bulk.ColumnMappings.Add(dt.Columns[17].ToString(), "Step 9");
                        //bulk.ColumnMappings.Add(dt.Columns[18].ToString(), "Step 10");
                        //bulk.ColumnMappings.Add(dt.Columns[19].ToString(), "Step 11");
                        //bulk.ColumnMappings.Add(dt.Columns[20].ToString(), "Step 12");
                        //bulk.ColumnMappings.Add(dt.Columns[21].ToString(), "Step 13");
                        //bulk.ColumnMappings.Add(dt.Columns[22].ToString(), "Step 14");
                        //bulk.ColumnMappings.Add(dt.Columns[23].ToString(), "Step 15");
                        //bulk.ColumnMappings.Add(dt.Columns[24].ToString(), "Megger Result");
                        //bulk.ColumnMappings.Add(dt.Columns[25].ToString(), "Date");

                        //serverCon.PushData(dt);

                        foreach (DataRow row in dt.Rows)
                        {


                            _query = "INSERT INTO [dbo].[BBT_IR_Production Report] ([Barcode],[User Name],[Date_Time],[Project No],[Month Code],[Tag No],[Stack No],[Lower Limit],[Upper Limit],[Step 1] " +
                                     ",[Step 2],[Step 3],[Step 4],[Step 5],[Step 6],[Step 7],[Step 8],[Step 9],[Step 10] ,[Step 11],[Step 12],[Step 13],[Step 14],[Step 15],[Megger Result],[Date]) " +
                                        " VALUES (@bar,@user,@dttime,@projno,@mon,@tag,@stack,@lower,@upper,@step1,@step2,@step3,@step4,@step5,@step6,@step7,@step8,@step9,@step10,@step11,@step12,@step13,@step14,@step15,@hv,@date)";
                            serverCon.SqlQuery(_query);
                            LoggingService service4 = new LoggingService("CreatePriorityJob/InsertData", "Query=" + _query, System.DateTime.Now);
                            service4.LogError();
                            serverCon.Cmd.Parameters.AddWithValue("@bar", row["Barcode"]);
                            serverCon.Cmd.Parameters.AddWithValue("@user", row["User Name"]);
                            serverCon.Cmd.Parameters.AddWithValue("@dttime", Convert.ToDateTime(row["Date_Time"]));
                            serverCon.Cmd.Parameters.AddWithValue("@projno", row["Project No"]);
                            serverCon.Cmd.Parameters.AddWithValue("@mon", row["Month Code"]);
                            serverCon.Cmd.Parameters.AddWithValue("@tag", row["Tag No"]);
                            serverCon.Cmd.Parameters.AddWithValue("@stack", row["Stack No"]);
                            serverCon.Cmd.Parameters.AddWithValue("@lower", row["Lower Limit"]);
                            serverCon.Cmd.Parameters.AddWithValue("@upper", row["Upper Limit"]);
                            serverCon.Cmd.Parameters.AddWithValue("@step1", row["Step 1"]);
                            serverCon.Cmd.Parameters.AddWithValue("@step2", row["Step 2"]);
                            serverCon.Cmd.Parameters.AddWithValue("@step3", row["Step 3"]);
                            serverCon.Cmd.Parameters.AddWithValue("@step4", row["Step 4"]);
                            serverCon.Cmd.Parameters.AddWithValue("@step5", row["Step 5"]);
                            serverCon.Cmd.Parameters.AddWithValue("@step6", row["Step 6"]);
                            serverCon.Cmd.Parameters.AddWithValue("@step7", row["Step 7"]);
                            serverCon.Cmd.Parameters.AddWithValue("@step8", row["Step 8"]);
                            serverCon.Cmd.Parameters.AddWithValue("@step9", row["Step 9"]);
                            serverCon.Cmd.Parameters.AddWithValue("@step10", row["Step 10"]);
                            serverCon.Cmd.Parameters.AddWithValue("@step11", row["Step 11"]);
                            serverCon.Cmd.Parameters.AddWithValue("@step12", row["Step 12"]);
                            serverCon.Cmd.Parameters.AddWithValue("@step13", row["Step 13"]);
                            serverCon.Cmd.Parameters.AddWithValue("@step14", row["Step 14"]);
                            serverCon.Cmd.Parameters.AddWithValue("@step15", row["Step 15"]);
                            serverCon.Cmd.Parameters.AddWithValue("@hv", row["Megger Result"]);
                            serverCon.Cmd.Parameters.AddWithValue("@date", row["Date"]);

                            serverCon.NonQueryEx();
                            count++;

                        }
                        serverCon.closeConnection();
                        LoggingService service2 = new LoggingService("CreatePriorityJob/InsertData", "Records Inserted =" + dt.Rows.Count, System.DateTime.Now);
                        service2.LogError();
                    }
                    catch (Exception ex)
                    {
                        LoggingService service = new LoggingService("CreatePriorityJob/InsertData", "Exception in IR Report" + ex.InnerException.Message, System.DateTime.Now);
                        service.LogError();
                    }
                }
                if (tableName.Trim() == "BBT_IR_Settings")
                {
                    LoggingService service3 = new LoggingService("CreatePriorityJob/InsertData", "In if condition for table=" + tableName, System.DateTime.Now);
                    service3.LogError();
                    serverCon = new ServerConnection();
                    foreach (DataRow row in dt.Rows)
                    {
                        _query = "INSERT INTO [dbo].[BBT_IR_Settings]([Test Selection],[Test Time],[Ramp Time],[Test Voltage],[HI Resistance Value],[LO Resistance Value])" +
                                " VALUES (@bar,@dttime,@projno,@mon,@tag,@stack)";
                        serverCon.SqlQuery(_query);
                        LoggingService service4 = new LoggingService("CreatePriorityJob/InsertData", "Query=" + _query, System.DateTime.Now);
                        service4.LogError();
                        serverCon.Cmd.Parameters.AddWithValue("@bar", row["Test Selection"]);
                        serverCon.Cmd.Parameters.AddWithValue("@dttime", Convert.ToInt32(row["Test Time"]));
                        serverCon.Cmd.Parameters.AddWithValue("@projno", Convert.ToInt32(row["Ramp Time"]));
                        serverCon.Cmd.Parameters.AddWithValue("@mon", Convert.ToInt32(row["Test Voltage"]));
                        serverCon.Cmd.Parameters.AddWithValue("@tag", Convert.ToInt32(row["HI Resistance Value"]));
                        serverCon.Cmd.Parameters.AddWithValue("@stack", Convert.ToInt32(row["LO Resistance Value"]));

                        serverCon.NonQueryEx();
                        count++;

                        LoggingService service2 = new LoggingService("CreatePriorityJob/InsertData", "Records Inserted =" + count, System.DateTime.Now);
                        service2.LogError();
                    }
                    serverCon.closeConnection();
                }

                LoggingService service1 = new LoggingService("CreatePriorityJob/InsertData", "Inserting records ended" + count, System.DateTime.Now);
                service1.LogError();
            }
            catch (Exception ex)
            {
                LoggingService service = new LoggingService("CreatePriorityJob/InsertData", "Exception in InsertData" + ex.InnerException.Message, System.DateTime.Now);
                service.LogError();
            }
        }
    }
}