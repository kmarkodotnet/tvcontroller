using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace TVController.BE1.Helpers
{
    public class EventLogger
    {
        public static void LogMessage(string message)
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "TVController";
                eventLog.WriteEntry(message, EventLogEntryType.Information, 101, 1);
            }
        }

        public static void LogExpetion(string message)
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "TVController";
                eventLog.WriteEntry(message, EventLogEntryType.Error, 101, 1);
            }
        }

        public static void LogExpetion(Exception ex)
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                var stackTrace = ex.StackTrace;
                var message = ex.Message;
                eventLog.Source = "TVController";
                eventLog.WriteEntry(string.Format("StackTrace: {0}, Message: {1}", stackTrace, message), EventLogEntryType.Error, 101, 1);
            }
        }
    }
}