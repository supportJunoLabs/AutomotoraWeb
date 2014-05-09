using AutomotoraWeb.Controllers.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomotoraWeb.Controllers.Financing {
    public class FinancingController : BaseController {

        public static string BCONTROLLER = "financing";
        public static string INDEX = "index";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            ViewBag.NombreEntidad = "Financista";
            ViewBag.NombreEntidades = "Financistas";
            base.OnActionExecuting(filterContext);
        }

        public ActionResult Index() {
            return View();
        }

        public override string getParentControllerName() {
            return BCONTROLLER;
        }
    }
}
