using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Diagnostics;
using System.Web.Mvc.Html;
using AutomotoraWeb.Controllers;
using AutomotoraWeb.Controllers.General;
using AutomotoraWeb.Utils;

namespace AutomotoraWeb {

    public class AutorizacionActionFilterAttribute : ActionFilterAttribute {

        public override void OnActionExecuting(ActionExecutingContext filterContext) {

            base.OnActionExecuting(filterContext);

            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionDescriptor.ActionName;

            if (!((controllerName.ToLower() == AuthenticationController.AUTHENTICATION) && (actionName.ToLower() == AuthenticationController.LOGIN))) {
                if (filterContext.HttpContext.Session[SessionUtils.SESSION_USER_NAME] == null) {
                    // Redireccionamos la peticion a la vista de login

                    if (filterContext.HttpContext.Request.IsAjaxRequest()) {
                        filterContext.HttpContext.Response.StatusCode = 302;
                    } else {
                        filterContext.Result = new RedirectResult("/" + AuthenticationController.AUTHENTICATION + "/" + AuthenticationController.LOGIN);
                    }
                }
            }
        }


    }
}
