using AutomotoraWeb.Controllers.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Diagnostics;
using System.Web.Mvc.Html;
using AutomotoraWeb.Utils;

namespace AutomotoraWeb.Filters {
    public class GeneralModelActionFilterAttribute : ActionFilterAttribute {


        public override void OnActionExecuting(ActionExecutingContext filterContext) {

            base.OnActionExecuting(filterContext);

            if (filterContext.Controller.GetType() != typeof(AuthenticationController)) {
                var baseController = filterContext.Controller as BaseController;

                if (baseController == null) {
                    throw new InvalidOperationException("It is not YourController !!!");
                } else {
                    filterContext.Controller.ViewBag.parentControllerName = baseController.getParentControllerName();
                }
            }

            // Obtengo de la session y de application datos a mostrar en todas las pantallas y guardo para pasar a todas las vistas
            filterContext.Controller.ViewBag.userName = (string)filterContext.HttpContext.Session.Contents[SessionUtils.SESSION_USER_NAME];
            filterContext.Controller.ViewBag.companyName = (string)filterContext.HttpContext.Application.Contents[SessionUtils.APPLICATION_COMPANY_NAME];
            filterContext.Controller.ViewBag.systemName = (string)filterContext.HttpContext.Application.Contents[SessionUtils.APPLICATION_SYSTEM_NAME];
        }

    }
}