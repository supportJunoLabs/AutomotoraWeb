using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AutomotoraWeb {
    public class RouteConfig {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
           name: "Error404",
           url: "Error404/{action}/{id}",
           defaults: new {
               controller = "Error",
               action = "Error404",
               id = UrlParameter.Optional
           }
           );

            routes.MapRoute(
         name: "Error403",
         url: "Error403/{action}/{id}",
         defaults: new {
             controller = "Error",
             action = "Error403",
             id = UrlParameter.Optional
         }
         );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "sales", action = "index", id = UrlParameter.Optional }
            );

            routes.IgnoreRoute("Logs/elmah.axd/{*pathInfo}");
        }
    }
}