using AutomotoraWeb.Controllers.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Diagnostics;
using System.Web.Mvc.Html;
using AutomotoraWeb.Utils;
using AutomotoraWeb.Models;
using DLL_Backend;

namespace AutomotoraWeb.Filters {
    public class GeneralModelActionFilterAttribute : ActionFilterAttribute {


        public override void OnActionExecuting(ActionExecutingContext filterContext) {

            base.OnActionExecuting(filterContext);

            if ((filterContext.Controller.GetType() != typeof(AuthenticationController)) && (filterContext.Controller.GetType() != typeof(ErrorController))) {
                var baseController = filterContext.Controller as BaseController;

                if (baseController == null) {
                    throw new InvalidOperationException("It is not YourController !!!");
                } else {
                    filterContext.Controller.ViewBag.parentControllerName = baseController.getParentControllerName();
                }
            }

            // Obtengo de la session y de application datos a mostrar en todas las pantallas y guardo para pasar a todas las vistas
            filterContext.Controller.ViewBag.userName = (string)filterContext.HttpContext.Session.Contents[SessionUtils.SESSION_USER_NAME];
            filterContext.Controller.ViewBag.menuOptions = (Dictionary<string, Dictionary<int, MenuOptionModel>>)filterContext.HttpContext.Session.Contents[SessionUtils.SESSION_MENU_OPTIONS];
            filterContext.Controller.ViewBag.companyName = (string)filterContext.HttpContext.Application.Contents[SessionUtils.APPLICATION_COMPANY_NAME];
            filterContext.Controller.ViewBag.systemName = (string)filterContext.HttpContext.Application.Contents[SessionUtils.APPLICATION_SYSTEM_NAME];


            // Para que al pasar desde el controller al servicio los objetos del modelo (como parámetro)
            // estos contengan IP y username para auditoria, evitando tener que pasarlos por parametros en cada uno de los servicios
            foreach (Object model in filterContext.ActionParameters.Values) {

                string nomUsuario = (string)filterContext.HttpContext.Session.Contents[SessionUtils.SESSION_USER_NAME];
                string origen = filterContext.HttpContext.Request.UserHostAddress;
                
                if (model is AbstractModel) {
                    ((AbstractModel)model).IP = origen;
                    ((AbstractModel)model).UserName = nomUsuario;
                } else if (model is Auditable) {
                    ((Auditable)model).setearAuditoria(nomUsuario, origen);
                }
            }

            
        }

    }
}