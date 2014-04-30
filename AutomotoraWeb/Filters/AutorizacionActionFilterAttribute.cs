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
using AutomotoraWeb.Services;
using AutomotoraWeb.Controllers.Sales.Maintenance;

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
                } else {
                    Dictionary<string, Dictionary<string, bool>> dictionaryOptions = (Dictionary<string, Dictionary<string, bool>>)(filterContext.HttpContext.Application.Contents[SessionUtils.APPLICATION_PERMISSIBLES_CONTROLLERS_ACTIONS]);
                    string userName = (string)(filterContext.HttpContext.Session[SessionUtils.SESSION_USER_NAME]);
                    if (!SecurityService.Instance.hasAccess(actionName, controllerName, userName, dictionaryOptions)) {
                        filterContext.Result = new RedirectResult("/Error403");
                    }
                }
            }
        }

        //----------------------------------------------------------
        

    }
}
