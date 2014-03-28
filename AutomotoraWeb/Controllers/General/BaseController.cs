﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomotoraWeb.Controllers.General {
    public abstract class BaseController : Controller {

        /*
         * public ActionResult Show() {
            //TODO: Cargo en el viewBug cosas datos a mostrar en el layout (nombre empresa, nombre usuario, etc)

            return getShowViewAndModel();
        }

        protected abstract ActionResult getShowViewAndModel();

        public abstract string getParentControllerName();
         * */

        public static string SHOW = "show";
        public static string DETAILS = "details";
        public static string CREATE = "create";
        public static string EDIT = "edit";
        public static string DELETE = "delete";

        public static string ERROR_CODE_SYSTEM_ERROR = "SYSTEM_ERROR";

        public abstract string getParentControllerName();

    }
}