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


        #region ConsultaFinanciacion

        public ActionResult ConsultaFinanciacion(int? id) {
            ConsultaVentaModel m = new ConsultaVentaModel();
            try {
                m.Venta = new Venta();
                m.Cliente = new Cliente();
                if (id != null && id > 0) {
                    m.Venta.Codigo = id ?? 0;
                    m.Venta.Consultar();
                    m.Cliente = m.Venta.Cliente;
                }
                ViewBag.SoloLectura = true;
                ViewData["idParametros"] = m.Cliente.Codigo;
                return View("ConsultaFinanciacion", m);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(m);
            }
        }

        //El id corresponde al cliente, cuando entro con el cliente elegido
        public ActionResult ConsultaFinanciacionesCliente(int? id) {
            ConsultaVentaModel m = new ConsultaVentaModel();
            try {
                m.Venta = new Venta();
                m.Cliente = new Cliente();
                if (id != null) {
                    m.Cliente.Codigo = id ?? 0;
                    m.Cliente.Consultar();
                }
                ViewBag.SoloLectura = true;
                ViewData["idParametros"] = m.Cliente.Codigo;
                return View("ConsultaFinanciacion", m);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View("ConsultaFinanciacion", m);
            }
        }

        //desde el javascript de cambio en ddl clientes
        public ActionResult VentasFinCliente(int idCliente) {
            ConsultaVentaModel m = new ConsultaVentaModel();
            m.Cliente = new Cliente();
            m.Venta = new Venta();
            m.Cliente.Codigo = idCliente;
            ViewBag.SoloLectura = true;
            ViewData["idParametros"] = m.Cliente.Codigo;
            return PartialView("_financiacionesCliente", m);
        }

        public ActionResult GrillaFinanciacionesCliente(int idParametros) {
            Cliente c = new Cliente();
            c.Codigo = idParametros;
            c.Consultar();
            GridLookUpModel model = new GridLookUpModel();
            model.Opciones = c.VentasCuotas();
            ViewData["idParametros"] = idParametros;
            return PartialView("_selectFinanciacionConsultar", model);
        }

        //desde el javascript de cambio en ddl vales
        public ActionResult DetallesFinanciacion(int idVenta) {
            ConsultaVentaModel m = new ConsultaVentaModel();
            m.Venta = new Venta();
            m.Venta.Codigo = idVenta;
            m.Venta.Consultar();
            m.Cliente = m.Venta.Cliente;
            ViewBag.SoloLectura = true;
            return PartialView("_datosDetalleFinanciacion", m);
        }

        //desde el boton imprimir de la consulta
        public ActionResult ReportFinanciacion(int id) {
            Venta model = new Venta();
            model.Codigo = id;
            ViewData["idParametros"] = id;
            return View("ReportFinanciacion", model);
        }

        public ActionResult ReportFinanciacionPartial(int idParametros) {
            Venta model = new Venta();
            model.Codigo = idParametros;
            model.Consultar();
            XtraReport rep = new DXReportConsultaFinanciacion();
            List<Venta> ll = new List<Venta>();
            ll.Add(model);
            rep.DataSource = ll;
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reportFinanciacion");
        }

        public ActionResult ReportFinanciacionExport(int idParametros) {
            Venta model = new Venta();
            model.Codigo = idParametros;
            model.Consultar();
            XtraReport rep = new DXReportConsultaFinanciacion();
            List<Venta> ll = new List<Venta>();
            ll.Add(model);
            rep.DataSource = ll;
            ViewData["idParametros"] = idParametros;
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }


        #endregion


        #region ConsultaSitCuotas

        //Se invoca desde la url del browser o desde el menu principal, o referencias externas. Devuelve la pagina completa
        public ActionResult ListSitCuotas() {
            ListadoCuotasValesModel model = new ListadoCuotasValesModel();
            try {
                string s = SessionUtils.generarIdVarSesion("ListadoCaja", Session[SessionUtils.SESSION_USER].ToString());
                Session[s] = model;
                model.idParametros = s;
                ViewData["idParametros"] = model.idParametros;
                model.TipoListado = ListadoCuotasValesModel.TIPO_LISTADO.SITUACION_CUOTAS;
                model.obtenerListado();
                return View(model);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(model);
            }

        }
        [HttpPost]
        //Se invoca desde el boton actualizar o imprimir.
        public ActionResult ListSitCuotas(ListadoCuotasValesModel model, string btnSubmit) {
            try {
                Session[model.idParametros] = model; //filtros actualizados
                ViewData["idParametros"] = model.idParametros;
                //ViewBag.Financistas = Financista.Financistas(Financista.FIN_TIPO_LISTADO.TODOS);
                this.eliminarValidacionesIgnorables("Filtro.Financista", MetadataManager.IgnorablesDDL(new Financista()));
                if (ModelState.IsValid) {
                    //if (model.Accion==ListadoCuotasValesModel.ACCIONES.IMPRIMIR){
                    if (btnSubmit == "Imprimir") {
                        return this.ReportSitCuotas(model);
                    }
                    model.obtenerListado();
                }
                return View(model);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(model);
            }
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
            try {
                model.TipoListado = ListadoCuotasValesModel.TIPO_LISTADO.CUOTAS_PENDIENTES;
                string s = SessionUtils.generarIdVarSesion("ListadoCuotasPend", Session[SessionUtils.SESSION_USER].ToString());
                Session[s] = model;
                model.idParametros = s;
                ViewData["idParametros"] = model.idParametros;
                model.obtenerListado();
                return View(model);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(model);
            }
        }
        [HttpPost]
        //Se invoca desde el boton actualizar o imprimir.
        public ActionResult ListCuotasPendientes(ListadoCuotasValesModel model) {
            try {
                Session[model.idParametros] = model; //filtros actualizados
                ViewData["idParametros"] = model.idParametros;
                //ViewBag.Financistas = Financista.Financistas(Financista.FIN_TIPO_LISTADO.TODOS);
                this.eliminarValidacionesIgnorables("Filtro.Financista", MetadataManager.IgnorablesDDL(new Financista()));
                if (ModelState.IsValid) {
                    if (model.Accion == ListadoCuotasValesModel.ACCIONES.IMPRIMIR) {
                        return this.ReportCuotasPendientes(model);
                    }
                    model.obtenerListado();
                    model.TabActual = ListadoCuotasValesModel.TABS.TAB1;
                }
                return View(model);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(model);
            }
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
            try {
                model.TipoListado = ListadoCuotasValesModel.TIPO_LISTADO.VALES_PENDIENTES;
                string s = SessionUtils.generarIdVarSesion("ListadoValesPend", Session[SessionUtils.SESSION_USER].ToString());
                Session[s] = model;
                model.idParametros = s;
                //ViewBag.Financistas = Financista.Financistas(Financista.FIN_TIPO_LISTADO.TODOS);
                ViewData["idParametros"] = model.idParametros;
                model.obtenerListado();
                return View(model);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(model);
            }
        }
        [HttpPost]
        //Se invoca desde el boton actualizar o imprimir.
        public ActionResult ListValesPendientes(ListadoCuotasValesModel model) {
            try {
                Session[model.idParametros] = model; //filtros actualizados
                ViewData["idParametros"] = model.idParametros;
                //ViewBag.Financistas = Financista.Financistas(Financista.FIN_TIPO_LISTADO.TODOS);
                this.eliminarValidacionesIgnorables("Filtro.Financista", MetadataManager.IgnorablesDDL(new Financista()));
                if (ModelState.IsValid) {
                    if (model.Accion == ListadoCuotasValesModel.ACCIONES.IMPRIMIR) {
                        return this.ReportValesPendientes(model);
                    }
                    model.obtenerListado();
                    model.TabActual = ListadoCuotasValesModel.TABS.TAB1;
                }
                return View(model);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(model);
            }
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
            try {
                model.TipoListado = ListadoCuotasValesModel.TIPO_LISTADO.CUOTAS_VALES_PENDIENTES;
                string s = SessionUtils.generarIdVarSesion("ListadoCuotasValesPend", Session[SessionUtils.SESSION_USER].ToString());
                Session[s] = model;
                model.idParametros = s;
                //ViewBag.Financistas = Financista.Financistas(Financista.FIN_TIPO_LISTADO.TODOS);
                ViewData["idParametros"] = model.idParametros;
                model.obtenerListado();
                return View(model);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(model);
            }
        }
        [HttpPost]
        //Se invoca desde el boton actualizar o imprimir.
        public ActionResult ListCuotasValesPendientes(ListadoCuotasValesModel model) {
            try {
                Session[model.idParametros] = model; //filtros actualizados
                ViewData["idParametros"] = model.idParametros;
                //ViewBag.Financistas = Financista.Financistas(Financista.FIN_TIPO_LISTADO.TODOS);
                this.eliminarValidacionesIgnorables("Filtro.Financista", MetadataManager.IgnorablesDDL(new Financista()));
                if (ModelState.IsValid) {
                    if (model.Accion == ListadoCuotasValesModel.ACCIONES.IMPRIMIR) {
                        return this.ReportCuotasValesPendientes(model);
                    }
                    model.obtenerListado();
                    model.TabActual = ListadoCuotasValesModel.TABS.TAB1;
                }
                return View(model);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(model);
            }
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
