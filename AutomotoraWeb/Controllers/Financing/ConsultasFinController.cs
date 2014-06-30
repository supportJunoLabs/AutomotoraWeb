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
using AutomotoraWeb.Utils;

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
        public ActionResult ListSitCliente(int? idCliente) {
            Cliente c = _consultarCliente(idCliente);
            return View(c);
        }


        [HttpPost]
        //se invoca desde el boton actualizar e imprimir. Devuelve la pagina completa
        public ActionResult ListSitCliente(Cliente model, string btnSubmit) {
            int? idCliente = model.Codigo;
            Cliente c = _consultarCliente(idCliente);
            if (btnSubmit == "Imprimir" && idCliente != null && idCliente != 0) {
                return this.ReportSitCliente(c);
            }
             return View(c);
        }

        //Se invoca por json al actualizar la ddl de clientes, devuelve solo la partial de contenido.
        public ActionResult ListSitClientePartial(int? idCliente) {
            Cliente c = _consultarCliente(idCliente);
            SituacionCliente sit = new SituacionCliente();
            sit.generarSituacion(c);
            return PartialView("_listSitCliente", sit);
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de cuotas. Devuelve la partial del tab de cuotas
        public ActionResult GrillaCuotasCliente(int? idParametros) {
            Cliente c = _consultarCliente(idParametros);
            SituacionCliente sit = new SituacionCliente();
            sit.generarSituacion(c);
            return PartialView("_listGrillaSitClienteCuotas", sit);
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de vales. Devuelve la partial del tab de vales
        public ActionResult GrillaValesCliente(int? idParametros) {
            Cliente c = _consultarCliente(idParametros);
            SituacionCliente sit = new SituacionCliente();
            sit.generarSituacion(c);
            return PartialView("_listGrillaSitClienteVales", sit);
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de cheques. Devuelve la partial del tab de cheques
        public ActionResult GrillaChequesCliente(int? idParametros) {
            Cliente c = _consultarCliente(idParametros);
            SituacionCliente sit = new SituacionCliente();
            sit.generarSituacion(c);
            return PartialView("_listGrillaSitClienteCheques", sit);
        }

        public ActionResult ReportSitCliente(Cliente c) {
            return View("ReportSitCliente", c);
        }

        private SituacionCliente _obtenerDatos(Cliente c) {
            SituacionCliente model = new SituacionCliente();
            model.Cliente = c;
            c.Consultar();
            return model;
        }

        public ActionResult ReportSitClientePartial(int idParametros) {
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

        public ActionResult ReportSitClienteExport(int idParametros) {
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


        #region ConsultaSitCuotas

        //Se invoca desde la url del browser o desde el menu principal, o referencias externas. Devuelve la pagina completa
        public ActionResult ListSitCuotas() {
            ListadoCuotasValesModel model = new ListadoCuotasValesModel();
            string s = SessionUtils.generarIdVarSesion("ListadoCaja", Session[SessionUtils.SESSION_USER].ToString());
            Session[s] = model;
            model.idParametros = s;
            ViewBag.Financistas = Financista.Financistas(Financista.FIN_TIPO_LISTADO.TODOS);
            ViewData["idParametros"] = model.idParametros;
            model.obtenerListado(ListadoCuotasValesModel.TIPO_LISTADO.SITUACION_CUOTAS);
            return View(model);

        }
        [HttpPost]
        //Se invoca desde el boton actualizar o imprimir.
        public ActionResult ListSitCuotas(ListadoCuotasValesModel model, string btnSubmit) {
            Session[model.idParametros] = model; //filtros actualizados
            ViewData["idParametros"] = model.idParametros;
            ViewBag.Financistas = Financista.Financistas(Financista.FIN_TIPO_LISTADO.TODOS);
            this.eliminarValidacionesIgnorables("Filtro.Financista", MetadataManager.IgnorablesDDL(model.Filtro.Financista));
            if (ModelState.IsValid) {
                if (btnSubmit == "Imprimir") {
                    return this.ReportSitCuotas(model);
                }
                model.obtenerListado(ListadoCuotasValesModel.TIPO_LISTADO.SITUACION_CUOTAS);
            }
            return View(model);
        }

        //Se invoca desde la propia grilla al paginar, filtrar etc. Devuelve solamente la grilla
        public ActionResult ListGrillaSitCuotas(string idParametros) {
            ListadoCuotasValesModel model = (ListadoCuotasValesModel)Session[idParametros];
            model.obtenerListado(ListadoCuotasValesModel.TIPO_LISTADO.SITUACION_CUOTAS);
            ViewData["idParametros"] = model.idParametros;
            return PartialView("_listGrillaSitCuotas", model.Resultado.Cuotas);
        }

        //Desde el postback de SitCuotas asociado al boton Imprimir, redirige al reporte
        public ActionResult ReportSitCuotas(ListadoCuotasValesModel model) {
            return View("reportSitCuotas", model);
        }

        public ActionResult ReportSitCuotasPartial(string idParametros) {
            ListadoCuotasValesModel model = null;
            model = (ListadoCuotasValesModel)Session[idParametros];
            XtraReport rep = new DXListadoSituacionCuotas();
            model.obtenerListado(ListadoCuotasValesModel.TIPO_LISTADO.SITUACION_CUOTAS);
            List<ListadoCuotasVales> ll = new List<ListadoCuotasVales>();
            ll.Add(model.Resultado);
            rep.DataSource = ll;
            setParamsToReport(rep, model);
            Session[idParametros] = model;
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reportSitCuotas");
        }

        public ActionResult ReportSitCuotasExport(string idParametros) {
            ListadoCuotasValesModel model = null;
            model = (ListadoCuotasValesModel)Session[idParametros];
            XtraReport rep = new DXListadoSituacionCuotas();
            List<ListadoCuotasVales> ll = new List<ListadoCuotasVales>();
            ll.Add(model.Resultado);
            rep.DataSource = ll;
            setParamsToReport(rep, model);
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }


        private void setParamsToReport(XtraReport report, ListadoCuotasValesModel model) {
            Parameter paramSystemName = new Parameter();
            paramSystemName.Name = "detalleFiltros";
            paramSystemName.Type = typeof(string);
            paramSystemName.Value = model.detallesFiltro();
            paramSystemName.Description = "Detalle Filtros";
            paramSystemName.Visible = false;
            report.Parameters.Add(paramSystemName);
        }

        #endregion
    }
}
