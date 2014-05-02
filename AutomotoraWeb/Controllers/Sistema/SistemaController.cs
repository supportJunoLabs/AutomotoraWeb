using AutomotoraWeb.Controllers.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomotoraWeb.Controllers.Sistema {
    public class SistemaController : BaseController {

        public static string BCONTROLLER = "sistema";  
        public static string INDEX = "index";  

        public ActionResult Index() {
            return View();
        }

        public override string getParentControllerName() {
            return BCONTROLLER;
        }

        protected override void setearUltimoModulo() {
            //para este controller que no es ningun modulo, dejo el original.
        }
    }
}
