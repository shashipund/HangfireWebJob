using HangfireWebJobs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HangfireWebJobs.Services
{
    public class LoggingService
    {

        ErrorLogService _logService = new ErrorLogService();
        public string MethodName;
        public string ErrorMsg;
        public DateTime logDateTime;
        public LoggingService(string methodName, string errorMsg, DateTime logdatetime)
        {
            MethodName = methodName;
            ErrorMsg = errorMsg;
            logDateTime = logdatetime;
        }

        public void LogError()
        {
            tblErrorLog log = new tblErrorLog();
            log.MethodName = MethodName;
            log.LoggedTime = logDateTime;
            log.ErrorMsg = ErrorMsg;
            _logService.InsertLog(log);
        }

    }
}