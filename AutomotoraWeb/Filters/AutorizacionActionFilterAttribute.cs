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

            //Agrego esto temporalmente para poder ver las paginas de error que se generan durante el login
            if (controllerName.Length >= "Error".Length && controllerName.Substring(0, "error".Length).ToLower() == "error") {
                return;
            }

            if (!((controllerName.ToLower() == AuthenticationController.CONTROLLER) && (actionName.ToLower() == AuthenticationController.LOGIN))) {
                if (filterContext.HttpContext.Session[SessionUtils.SESSION_USER_NAME] == null) {
                    // Redireccionamos la peticion a la vista de login
                    if (filterContext.HttpContext.Request.IsAjaxRequest()) {
                        filterContext.HttpContext.Response.StatusCode = 302;
                    } else {
                        filterContext.Result = new RedirectResult("/" + AuthenticationController.CONTROLLER + "/" + AuthenticationController.LOGIN);
                    }
                } else {
                    Dictionary<string, Dictionary<string, bool>> dictionaryOptions = (Dictionary<string, Dictionary<string, bool>>)(filterContext.HttpContext.Application.Contents[SessionUtils.APPLICATION_PERMISSIBLES_CONTROLLERS_ACTIONS]);
                    string userName = (string)(filterContext.HttpContext.Session[SessionUtils.SESSION_USER_NAME]);
                    if (!SecurityService.Instance.hasAccess(actionName, controllerName, userName, dictionaryOptions)) {
                        string spermiso = controllerName.ToLower() + "|" + actionName.ToLower();
                        filterContext.Result = new RedirectResult("/Error/Error403/"+spermiso);
                    }
                }
            }
        }

        //----------------------------------------------------------
        

    }
}
