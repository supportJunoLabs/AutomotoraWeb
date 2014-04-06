using AutomotoraWeb.Controllers.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomotoraWeb.Controllers.Sales {
    public class SalesController : BaseController {

        public static string SALES = "sales";
        public static string INDEX = "index";

        public ActionResult Index() {
            return View();
        }

        public override string getParentControllerName() {
            return SALES;
        }

    }
}
