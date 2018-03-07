using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace TVController.BE1
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{action}/{id}", //"{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
