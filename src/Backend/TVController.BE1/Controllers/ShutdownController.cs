using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TVController.BE1.Controllers
{
    [Route("shutdown")]
    public class ShutdownController : Controller
    {
        [HttpGet]
        [Route("")]
        public void Index()
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                var ip = Request.UserHostAddress;
                eventLog.Source = "TVController";
                eventLog.WriteEntry(string.Format("IP: {0}", ip), EventLogEntryType.Information, 101, 1);
            }
            Process.Start("shutdown", "/s /t 0");
        }
    }
}