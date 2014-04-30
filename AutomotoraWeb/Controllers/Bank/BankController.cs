using AutomotoraWeb.Controllers.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomotoraWeb.Controllers.Bank {
    public class BankController : BaseController {

        public static string BCONTROLLER = "bank";
        public static string INDEX = "index";

        public ActionResult Index() {
            return View();
        }

        public override string getParentControllerName() {
            return BCONTROLLER;
        }
    }
}
