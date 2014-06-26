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


        //Se invoca desde la url del browser o desde el menu principal, o referencias externas. Devuelve la pagina completa
        public ActionResult SitCliente(int? idCliente) {
            Cliente c = _consultarCliente(idCliente);
            return View(c);
        }


        [HttpPost]
        //se invoca desde el boton actualizar e imprimir. Devuelve la pagina completa
        public ActionResult SitCliente(Cliente model, string btnSubmit) {
            int? idCliente = model.Codigo;
            Cliente c = _consultarCliente(idCliente);
            if (btnSubmit == "Imprimir" && idCliente != null && idCliente != 0) {
                return this.ReporteSituacionCliente(c);
            }
             return View(c);
        }

        //Se invoca por json al actualizar la ddl de clientes, devuelve solo la partial de contenido.
        public ActionResult SitClientePartial(int? idCliente) {
            Cliente c = _consultarCliente(idCliente);
            SituacionCliente sit = new SituacionCliente();
            sit.generarSituacion(c);
            return PartialView("_sitCliente", sit);
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de cuotas. Devuelve la partial del tab de cuotas
        public ActionResult GrillaCuotasCliente(int? idParametros) {
            Cliente c = _consultarCliente(idParametros);
            SituacionCliente sit = new SituacionCliente();
            sit.generarSituacion(c);
            return PartialView("_sitClienteCuotas", sit);
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de vales. Devuelve la partial del tab de vales
        public ActionResult GrillaValesCliente(int? idParametros) {
            Cliente c = _consultarCliente(idParametros);
            SituacionCliente sit = new SituacionCliente();
            sit.generarSituacion(c);
            return PartialView("_sitClienteVales", sit);
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de cheques. Devuelve la partial del tab de cheques
        public ActionResult GrillaChequesCliente(int? idParametros) {
            Cliente c = _consultarCliente(idParametros);
            SituacionCliente sit = new SituacionCliente();
            sit.generarSituacion(c);
            return PartialView("_sitClienteCheques", sit);
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
