using AutomotoraWeb.Services;
using AutomotoraWeb.Utils;
using DLL_Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TwitterBootstrapMVC;

namespace AutomotoraWeb {
    // Nota: para obtener instrucciones sobre cómo habilitar el modo clásico de IIS6 o IIS7, 
    // visite http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication {
        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Application.Add(SessionUtils.APPLICATION_COMPANY_NAME, CompanyService.getCompanyName());
            Application.Add(SessionUtils.APPLICATION_SYSTEM_NAME, CompanyService.getSystemName());
            Application.Add(SessionUtils.APPLICATION_PERMISSIBLES_CONTROLLERS_ACTIONS, SecurityService.Instance.getPermissiblesControllerAction());

            Bootstrap.Configure();
        }
    }
}