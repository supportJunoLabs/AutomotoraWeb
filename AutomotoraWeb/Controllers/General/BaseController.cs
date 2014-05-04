using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutomotoraWeb.Utils;

using AutomotoraWeb.Controllers.Bank;
using AutomotoraWeb.Controllers.Financing;
using AutomotoraWeb.Controllers.Sales;
using AutomotoraWeb.Controllers.General;
using AutomotoraWeb.Controllers.Sistema;

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

        public static string REPORT2 = "report2";
        public static string REPORT_PARTIAL2 = "reportPartial2";
        public static string REPORT_EXPORT2 = "reportExport2";


        public static string ERROR_CODE_SYSTEM_ERROR = "SYSTEM_ERROR";

        public abstract string getParentControllerName();

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            setearUltimoModulo();
        }

        protected virtual void setearUltimoModulo() {
            Session[SessionUtils.ULTIMO_MODULO] = this.getParentControllerName();
        }


        public ActionResult IndexUltimoModulo() {
            if (Session[SessionUtils.ULTIMO_MODULO] != null) {
                Destino dest = BaseController.DestinoIndexUltimoModulo(Session[SessionUtils.ULTIMO_MODULO].ToString());
                return RedirectToAction(dest.Accion, dest.Controlador);
            }
            return RedirectToAction(SistemaController.INDEX, SistemaController.BCONTROLLER);
        }

        public static Destino DestinoIndexUltimoModulo(Object sesionNomModulo) {

            if (sesionNomModulo == null) {
                return new Destino(SistemaController.INDEX, SistemaController.BCONTROLLER);
            }

            string nomModulo = sesionNomModulo.ToString().Trim();
            nomModulo = nomModulo.Trim();
            if (nomModulo == SalesController.BCONTROLLER) {
                return new Destino(SalesController.INDEX, SalesController.BCONTROLLER);
            }
            if (nomModulo == BankController.BCONTROLLER) {
                return new Destino(BankController.INDEX, BankController.BCONTROLLER);
            }
            if (nomModulo == FinancingController.BCONTROLLER) {
                return new Destino(FinancingController.INDEX, FinancingController.BCONTROLLER);
            }
            if (nomModulo == SistemaController.BCONTROLLER) {
                return new Destino(SistemaController.INDEX, SistemaController.BCONTROLLER);
            }
            return new Destino(SistemaController.INDEX, SistemaController.BCONTROLLER);
        }

    }
}
