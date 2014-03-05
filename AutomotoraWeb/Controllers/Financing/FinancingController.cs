using AutomotoraWeb.Controllers.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomotoraWeb.Controllers.Financing {
    public class FinancingController : BaseController {

        public static string FINANCING = "financing";
        public static string INDEX = "index";

        public ActionResult Index() {
            return View();
        }

        public override string getParentControllerName() {
            return FINANCING;
        }
    }
}
