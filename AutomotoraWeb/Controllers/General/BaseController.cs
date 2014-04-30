using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomotoraWeb.Controllers.General {
    public abstract class BaseController : Controller {

        public static string SHOW = "show";
        public static string DETAILS = "details";
        public static string CREATE = "create";
        public static string EDIT = "edit";
        public static string DELETE = "delete";
        public static string LIST = "list";
        //public static string EXPORT_TO_PDF = "exportarPDF";
        //public static string EXPORT_TO_EXCEL = "exportarExcel";
        public static string REPORT = "report";
        public static string REPORT_PARTIAL = "reportPartial";
        public static string REPORT_EXPORT = "reportExport";
        

        public static string ERROR_CODE_SYSTEM_ERROR = "SYSTEM_ERROR";

        public abstract string getParentControllerName();

    }
}
