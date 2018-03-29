using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TVController.BE1.Helpers;

namespace TVController.BE1.Controllers
{
    [Route("shutdown")]
    public class ShutdownController : Controller
    {
        [HttpGet]
        [Route("")]
        public void Index()
        {
            var ip = Request.UserHostAddress;
            try
            {
                EventLogger.LogMessage(string.Format("IP: {0}", ip));
                Process.Start("shutdown", "/s /t 0");
            }
            catch (Exception ex)
            {
                EventLogger.LogExpetion(string.Format("IP: {0}, Ex: ", ip, ex.ToString()));
            }
        }
    }
}