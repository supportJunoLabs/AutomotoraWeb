using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DLL_Backend;
using AutomotoraWeb.Models;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;

namespace AutomotoraWeb.Controllers.Financing {
    public class ConsultasFinController : FinancingController {

        public static string CONTROLLER = "ConsultasFin";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            ViewBag.Clientes = Cliente.Clientes();
        }


        #region ConsultaSituacionCliente

        private Cliente _consultarCliente(int? idCliente) {
            Cliente c = new Cliente();
            c.Codigo = idCliente ?? 0;
            if (idCliente != null && idCliente != 0) {
                c.Consultar();
            }
            ViewData["idParametros"] = c.Codigo;
            return c;
        }

        public ActionResult SitCliente(int? idCliente) {
            Cliente c = _consultarCliente(idCliente);
            return View(c);
        }


        [HttpPost]
        public ActionResult SitCliente(Cliente model, string btnSubmit) {
            int? idCliente = model.Codigo;
            Cliente c = _consultarCliente(idCliente);
            if (btnSubmit == "Imprimir" && idCliente != null && idCliente != 0) {
                return this.ReporteSituacionCliente(c);
            }
             return View(c);
        }

        public ActionResult SitClientePartial(int? idCliente) {
            Cliente c = _consultarCliente(idCliente);
            return PartialView("_sitCliente", c);
        }

        public ActionResult GrillaCuotasCliente(int? idParametros) {
            Cliente c = _consultarCliente(idParametros);
            return PartialView("_sitClienteCuotas", c);
        }

        public ActionResult GrillaValesCliente(int? idParametros) {
            Cliente c = _consultarCliente(idParametros);
            return PartialView("_sitClienteVales", c);
        }

        public ActionResult GrillaChequesCliente(int? idParametros) {
            Cliente c = _consultarCliente(idParametros);
            return PartialView("_sitClienteCheques", c);
        }

        #endregion


        #region ConsultaVales

        private Vale _consultarVale(string idVale) {
            Vale v = new Vale();
            if (idVale != null && idVale.Trim() != "") {
                v.Codigo = idVale;
                v.Consultar();
            }
            ViewData["idParametros"] = v.Codigo;
            return v;
        }

        public ActionResult ConsultaVale(string idVale) {
            Vale v = _consultarVale(idVale);
            return View("ConsultaVale", v);
        }

        //public ActionResult ConsultaValeCliente(int? idCliente) { 

        //}

        #endregion

        #region ConsultaFinanciacion

        private Venta _consultarFinanciacion(int? idVenta) {
            Venta v = new Venta();
            v.Codigo = idVenta ?? 0;
            if (idVenta != null && idVenta != 0) {
                v.Consultar();
            }
            ViewData["idParametros"] = v.Codigo;
            return v;
        }

        public ActionResult ConsultaFinanciacion(int? idVenta) {
            Venta v = _consultarFinanciacion(idVenta);
            return View("ConsultaFinanciacion", v);
        }

        //public ActionResult ConsultaFinanciacionesCliente(int? idCliente) { 

        //}

        #endregion


        #region ReporteSituacionCliente
        public ActionResult ReporteSituacionCliente(Cliente c) {
            return View("ReportSitCliente", c);
        }

        private SituacionCliente _obtenerDatos(Cliente c) {
            SituacionCliente model = new SituacionCliente();
            model.Cliente = c;
            c.Consultar();
            

            return model;
        }

        public ActionResult _reporteSituacionCliente(int idParametros) {
            Cliente c = _consultarCliente(idParametros);
            SituacionCliente model = new SituacionCliente();
            model.generarSituacion(c);
        
             List<SituacionCliente> ll = new List<SituacionCliente>();
            ll.Add(model);

            XtraReport rep = new DXSituacionCliente();
           
            rep.DataSource = ll;
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reportSitCliente");
        }

        public ActionResult _reporteSituacionClienteExport(int idParametros) {
            Cliente c = _consultarCliente(idParametros);
            SituacionCliente model = new SituacionCliente();
            model.generarSituacion(c);

            List<SituacionCliente> ll = new List<SituacionCliente>();
            ll.Add(model);

            XtraReport rep = new DXSituacionCliente();
            rep.DataSource = ll;
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }
        #endregion


    }
}
