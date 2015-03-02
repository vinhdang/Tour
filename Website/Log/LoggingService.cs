using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using System.Reflection;
namespace Website.Log
{
    public class LoggingService : ILoggingService
    {
        private static ILog log = LogManager.GetLogger
            (MethodBase.GetCurrentMethod().DeclaringType);
        public void Log(string message)
        {
            log.Info(message);
        }
        public void Error(string message)
        {
            log.Error(message);
        }
    }
    public interface ILoggingService
    {
        void Log(string message);
        void Error(string message);
    }
}