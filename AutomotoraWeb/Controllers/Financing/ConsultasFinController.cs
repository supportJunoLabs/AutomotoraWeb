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
            ViewBag.Financistas = Financista.FinancistasTodos;
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
        public ActionResult ListSitCliente(int? id) {
            Cliente c = _consultarCliente(id);
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
            //ViewBag.Financistas = Financista.Financistas(Financista.FIN_TIPO_LISTADO.TODOS);
            ViewData["idParametros"] = model.idParametros;
            model.TipoListado = ListadoCuotasValesModel.TIPO_LISTADO.SITUACION_CUOTAS;
            model.obtenerListado();
            return View(model);

        }
        [HttpPost]
        //Se invoca desde el boton actualizar o imprimir.
        public ActionResult ListSitCuotas(ListadoCuotasValesModel model, string btnSubmit) {
            Session[model.idParametros] = model; //filtros actualizados
            ViewData["idParametros"] = model.idParametros;
            //ViewBag.Financistas = Financista.Financistas(Financista.FIN_TIPO_LISTADO.TODOS);
            this.eliminarValidacionesIgnorables("Filtro.Financista", MetadataManager.IgnorablesDDL(model.Filtro.Financista));
            if (ModelState.IsValid) {
                //if (model.Accion==ListadoCuotasValesModel.ACCIONES.IMPRIMIR){
                if (btnSubmit == "Imprimir") {
                    return this.ReportSitCuotas(model);
                }
                model.obtenerListado();
            }
            return View(model);
        }

        //Se invoca desde la propia grilla al paginar, filtrar etc. Devuelve solamente la grilla
        public ActionResult ListGrillaSitCuotas(string idParametros) {
            ListadoCuotasValesModel model = (ListadoCuotasValesModel)Session[idParametros];
            model.obtenerListado();
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
            XtraReport rep = new DXListadoCuotas();
            model.obtenerListado();
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
            XtraReport rep = new DXListadoCuotas();
            List<ListadoCuotasVales> ll = new List<ListadoCuotasVales>();
            ll.Add(model.Resultado);
            rep.DataSource = ll;
            setParamsToReport(rep, model);
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }


    

        #endregion

        #region ConsultaCuotasPendientes

        //Se invoca desde la url del browser o desde el menu principal, o referencias externas. Devuelve la pagina completa
        public ActionResult ListCuotasPendientes() {
            ListadoCuotasValesModel model = new ListadoCuotasValesModel();
            model.TipoListado = ListadoCuotasValesModel.TIPO_LISTADO.CUOTAS_PENDIENTES;
            string s = SessionUtils.generarIdVarSesion("ListadoCuotasPend", Session[SessionUtils.SESSION_USER].ToString());
            Session[s] = model;
            model.idParametros = s;
            //ViewBag.Financistas = Financista.Financistas(Financista.FIN_TIPO_LISTADO.TODOS);
            ViewData["idParametros"] = model.idParametros;
            model.obtenerListado();
            return View(model);
        }
        [HttpPost]
        //Se invoca desde el boton actualizar o imprimir.
        public ActionResult ListCuotasPendientes(ListadoCuotasValesModel model) {
            Session[model.idParametros] = model; //filtros actualizados
            ViewData["idParametros"] = model.idParametros;
            //ViewBag.Financistas = Financista.Financistas(Financista.FIN_TIPO_LISTADO.TODOS);
            this.eliminarValidacionesIgnorables("Filtro.Financista", MetadataManager.IgnorablesDDL(model.Filtro.Financista));
            if (ModelState.IsValid) {
                if (model.Accion==ListadoCuotasValesModel.ACCIONES.IMPRIMIR){
                    return this.ReportCuotasPendientes(model);
                }
                model.obtenerListado();
                model.TabActual = ListadoCuotasValesModel.TABS.TAB1;
            }
            return View(model);
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de cuotas. Devuelve la partial del tab de cuotas
        public ActionResult ListGrillaCuotasPendientesDet(string idParametros) {
            ListadoCuotasValesModel model = (ListadoCuotasValesModel)Session[idParametros];
            ViewData["idParametros"] = model.idParametros;
            model.obtenerListado();
            return PartialView("_listGrillaCuotasPendientesDet", model.Resultado.Cuotas);
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de vales. Devuelve la partial del tab de vales
        public ActionResult ListGrillaCuotasPendientesCli(string idParametros) {
            ListadoCuotasValesModel model = (ListadoCuotasValesModel)Session[idParametros];
            ViewData["idParametros"] = model.idParametros;
            model.obtenerListado();
            return PartialView("_listGrillaCuotasPendientesCli", model.Resultado.AgrupCliente);
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de cheques. Devuelve la partial del tab de cheques
        public ActionResult ListGrillaCuotasPendientesMes(string idParametros) {
            ListadoCuotasValesModel model = (ListadoCuotasValesModel)Session[idParametros];
            ViewData["idParametros"] = model.idParametros;
            model.obtenerListado();
            return PartialView("_listGrillaCuotasPendientesMes", model.Resultado.AgrupMes);
        }

        public ActionResult ReportCuotasPendientes(ListadoCuotasValesModel model) {
            return View("ReportCuotasPendientes", model);
        }

        private XtraReport _generarReporteCuotasPendientes(string idParametros) {
            ListadoCuotasValesModel model = (ListadoCuotasValesModel)Session[idParametros];
            model.obtenerListado();
            List<ListadoCuotasVales> ll = new List<ListadoCuotasVales>();
            ll.Add(model.Resultado);
            XtraReport rep = null;
            switch (model.TabActual) {
                case ListadoCuotasValesModel.TABS.TAB3:
                    rep = new DXListadoCuotas();
                    break;
                case ListadoCuotasValesModel.TABS.TAB2:
                    rep = new DXListadoCuotasValesCli();
                    break;
                case ListadoCuotasValesModel.TABS.TAB1:
                    rep = new DXListadoCuotasValesMes();
                    break;
            }
            rep.DataSource = ll;
            setParamsToReport(rep, model);
            return rep;
        }

        public ActionResult ReportCuotasPendientesPartial(string idParametros) {
            XtraReport rep = _generarReporteCuotasPendientes(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reportCuotasPendientes");
        }

        public ActionResult ReportCuotasPendientesExport(string idParametros) {
            XtraReport rep = _generarReporteCuotasPendientes(idParametros);
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        #endregion

        #region ConsultaValesPendientes

        //Se invoca desde la url del browser o desde el menu principal, o referencias externas. Devuelve la pagina completa
        public ActionResult ListValesPendientes() {
            ListadoCuotasValesModel model = new ListadoCuotasValesModel();
            model.TipoListado = ListadoCuotasValesModel.TIPO_LISTADO.VALES_PENDIENTES;
            string s = SessionUtils.generarIdVarSesion("ListadoValesPend", Session[SessionUtils.SESSION_USER].ToString());
            Session[s] = model;
            model.idParametros = s;
            //ViewBag.Financistas = Financista.Financistas(Financista.FIN_TIPO_LISTADO.TODOS);
            ViewData["idParametros"] = model.idParametros;
            model.obtenerListado();
            return View(model);
        }
        [HttpPost]
        //Se invoca desde el boton actualizar o imprimir.
        public ActionResult ListValesPendientes(ListadoCuotasValesModel model) {
            Session[model.idParametros] = model; //filtros actualizados
            ViewData["idParametros"] = model.idParametros;
            //ViewBag.Financistas = Financista.Financistas(Financista.FIN_TIPO_LISTADO.TODOS);
            this.eliminarValidacionesIgnorables("Filtro.Financista", MetadataManager.IgnorablesDDL(model.Filtro.Financista));
            if (ModelState.IsValid) {
                if (model.Accion == ListadoCuotasValesModel.ACCIONES.IMPRIMIR) {
                    return this.ReportValesPendientes(model);
                }
                model.obtenerListado();
                model.TabActual = ListadoCuotasValesModel.TABS.TAB1;
            }
            return View(model);
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de cuotas. Devuelve la partial del tab de cuotas
        public ActionResult ListGrillaValesPendientesDet(string idParametros) {
            ListadoCuotasValesModel model = (ListadoCuotasValesModel)Session[idParametros];
            ViewData["idParametros"] = model.idParametros;
            model.obtenerListado();
            return PartialView("_listGrillaValesPendientesDet", model.Resultado.Vales);
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de vales. Devuelve la partial del tab de vales
        public ActionResult ListGrillaValesPendientesCli(string idParametros) {
            ListadoCuotasValesModel model = (ListadoCuotasValesModel)Session[idParametros];
            ViewData["idParametros"] = model.idParametros;
            model.obtenerListado();
            return PartialView("_listGrillaValesPendientesCli", model.Resultado.AgrupCliente);
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de cheques. Devuelve la partial del tab de cheques
        public ActionResult ListGrillaValesPendientesMes(string idParametros) {
            ListadoCuotasValesModel model = (ListadoCuotasValesModel)Session[idParametros];
            ViewData["idParametros"] = model.idParametros;
            model.obtenerListado();
            return PartialView("_listGrillaValesPendientesMes", model.Resultado.AgrupMes);
        }

        public ActionResult ReportValesPendientes(ListadoCuotasValesModel model) {
            return View("ReportValesPendientes", model);
        }

        private XtraReport _generarReporteValesPendientes(string idParametros) {
            ListadoCuotasValesModel model = (ListadoCuotasValesModel)Session[idParametros];
            model.obtenerListado();
            List<ListadoCuotasVales> ll = new List<ListadoCuotasVales>();
            ll.Add(model.Resultado);
            XtraReport rep = null;
            switch (model.TabActual) {
                case ListadoCuotasValesModel.TABS.TAB3:
                    rep = new DXListadoValesFin();
                    break;
                case ListadoCuotasValesModel.TABS.TAB2:
                    rep = new DXListadoCuotasValesCli();
                    break;
                case ListadoCuotasValesModel.TABS.TAB1:
                    rep = new DXListadoCuotasValesMes();
                    break;
            }
            rep.DataSource = ll;
            setParamsToReport(rep, model);
            return rep;
        }

        public ActionResult ReportValesPendientesPartial(string idParametros) {
            XtraReport rep = _generarReporteValesPendientes(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reportValesPendientes");
        }

        public ActionResult ReportValesPendientesExport(string idParametros) {
            XtraReport rep = _generarReporteValesPendientes(idParametros);
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        #endregion

        #region ConsultaCuotasValesPendientes

        //Se invoca desde la url del browser o desde el menu principal, o referencias externas. Devuelve la pagina completa
        public ActionResult ListCuotasValesPendientes() {
            ListadoCuotasValesModel model = new ListadoCuotasValesModel();
            model.TipoListado = ListadoCuotasValesModel.TIPO_LISTADO.CUOTAS_VALES_PENDIENTES;
            string s = SessionUtils.generarIdVarSesion("ListadoCuotasValesPend", Session[SessionUtils.SESSION_USER].ToString());
            Session[s] = model;
            model.idParametros = s;
            //ViewBag.Financistas = Financista.Financistas(Financista.FIN_TIPO_LISTADO.TODOS);
            ViewData["idParametros"] = model.idParametros;
            model.obtenerListado();
            return View(model);
        }
        [HttpPost]
        //Se invoca desde el boton actualizar o imprimir.
        public ActionResult ListCuotasValesPendientes(ListadoCuotasValesModel model) {
            Session[model.idParametros] = model; //filtros actualizados
            ViewData["idParametros"] = model.idParametros;
            //ViewBag.Financistas = Financista.Financistas(Financista.FIN_TIPO_LISTADO.TODOS);
            this.eliminarValidacionesIgnorables("Filtro.Financista", MetadataManager.IgnorablesDDL(model.Filtro.Financista));
            if (ModelState.IsValid) {
                if (model.Accion == ListadoCuotasValesModel.ACCIONES.IMPRIMIR) {
                    return this.ReportCuotasValesPendientes(model);
                }
                model.obtenerListado();
                model.TabActual = ListadoCuotasValesModel.TABS.TAB1;
            }
            return View(model);
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de vales. Devuelve la partial del tab de vales
        public ActionResult ListGrillaCuotasValesPendientesCli(string idParametros) {
            ListadoCuotasValesModel model = (ListadoCuotasValesModel)Session[idParametros];
            ViewData["idParametros"] = model.idParametros;
            model.obtenerListado();
            return PartialView("_listGrillaCuotasValesPendientesCli", model.Resultado.AgrupCliente);
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de cheques. Devuelve la partial del tab de cheques
        public ActionResult ListGrillaCuotasValesPendientesMes(string idParametros) {
            ListadoCuotasValesModel model = (ListadoCuotasValesModel)Session[idParametros];
            ViewData["idParametros"] = model.idParametros;
            model.obtenerListado();
            return PartialView("_listGrillaCuotasValesPendientesMes", model.Resultado.AgrupMes);
        }

        public ActionResult ReportCuotasValesPendientes(ListadoCuotasValesModel model) {
            return View("ReportCuotasValesPendientes", model);
        }

        private XtraReport _generarReporteCuotasValesPendientes(string idParametros) {
            ListadoCuotasValesModel model = (ListadoCuotasValesModel)Session[idParametros];
            model.obtenerListado();
            List<ListadoCuotasVales> ll = new List<ListadoCuotasVales>();
            ll.Add(model.Resultado);
            XtraReport rep = null;
            switch (model.TabActual) {
                case ListadoCuotasValesModel.TABS.TAB2:
                    rep = new DXListadoCuotasValesCli();
                    break;
                case ListadoCuotasValesModel.TABS.TAB1:
                    rep = new DXListadoCuotasValesMes();
                    break;
            }
            rep.DataSource = ll;
            setParamsToReport(rep, model);
            return rep;
        }

        public ActionResult ReportCuotasValesPendientesPartial(string idParametros) {
            XtraReport rep = _generarReporteCuotasValesPendientes(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reportCuotasValesPendientes");
        }

        public ActionResult ReportCuotasValesPendientesExport(string idParametros) {
            XtraReport rep = _generarReporteCuotasValesPendientes(idParametros);
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        #endregion

        private void setParamsToReport(XtraReport report, ListadoCuotasValesModel model) {
            Parameter param = new Parameter();
            param.Name = "detalleFiltros";
            param.Type = typeof(string);
            param.Value = model.detallesFiltro();
            param.Description = "Detalle Filtros";
            param.Visible = false;
            report.Parameters.Add(param);

            string s = "TITULO REPORTE";
            switch (model.TipoListado) {
                case ListadoCuotasValesModel.TIPO_LISTADO.SITUACION_CUOTAS:
                    s = "SITUACION CUOTAS";
                    break;
                case ListadoCuotasValesModel.TIPO_LISTADO.CUOTAS_PENDIENTES:
                    s = "CUOTAS PENDIENTES";
                    break;
                case ListadoCuotasValesModel.TIPO_LISTADO.VALES_PENDIENTES:
                    s = "VALES PENDIENTES";
                    break;
                case ListadoCuotasValesModel.TIPO_LISTADO.CUOTAS_VALES_PENDIENTES:
                    s = "CUOTAS Y VALES PENDIENTES";
                    break;
            }

            Parameter param1 = new Parameter();
            param1.Name = "tituloReporte";
            param1.Type = typeof(string);
            param1.Value = s;
            param1.Description = "Titulo Reporte";
            param1.Visible = false;
            report.Parameters.Add(param1);
        }

    }
}
