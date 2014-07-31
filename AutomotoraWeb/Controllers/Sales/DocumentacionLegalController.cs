using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutomotoraWeb.Models;
using DLL_Backend;
using AutomotoraWeb.Utils;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using DevExpress.Web.Mvc;

namespace AutomotoraWeb.Controllers.Sales {
    public class DocumentacionLegalController : SalesController {

        public static string CONTROLLER = "DocumentacionLegal";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            ViewBag.NombreEntidad = "Documentacion Legal";
            ViewBag.NombreEntidades = "Documentacion Legal";
        }

        public ActionResult DocComprobanteVenta(int id) {
            //Venta v = new Venta();
            //v.Codigo=id;
            ////v.Consultar();
            ViewData["idParametros"] = id;
            return View("ReportComprobanteVenta");
        }

        public ActionResult ReportComprobanteVentaPartial(int idParametros) {
            ViewData["idParametros"] = idParametros;
            Venta v = new Venta();
            v.Codigo = idParametros;
            v.Consultar();
            XtraReport rep = new DXReciboVenta();
            List<Venta> ll = new List<Venta>();
            ll.Add(v);
            rep.DataSource = ll;
            ViewData["Report"] = rep;
            return PartialView("_reportComprobanteVenta");
        }

        public ActionResult ReportComprobanteVentaExport(int idParametros) {
            ViewData["idParametros"] = idParametros;
            Venta v = new Venta();
            v.Codigo = idParametros;
            v.Consultar();
            XtraReport rep = new DXReciboVenta();
            List<Venta> ll = new List<Venta>();
            ll.Add(v);
            rep.DataSource = ll;
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);

        }

    }
}
