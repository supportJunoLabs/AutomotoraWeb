using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DLL_Backend;
using AutomotoraWeb.Models;
using AutomotoraWeb.Utils;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Parameters;

namespace AutomotoraWeb.Controllers.Financing {
    public class CajaController : FinancingController {
        public static string CONTROLLER = "Caja";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            ViewBag.Sucursales = Sucursal.Sucursales;
            ViewBag.Monedas = Moneda.Monedas;
            ViewBag.Financistas = Financista.FinancistasTodos;
        }

        #region Listados

        public ActionResult List() {
            ListadoCajasModel model = new ListadoCajasModel();
            try {
                string s = SessionUtils.generarIdVarSesion("ListadoCaja", Session[SessionUtils.SESSION_USER].ToString());
                Session[s] = model;
                model.idParametros = s;
                ViewData["idParametros"] = model.idParametros;
                model.Resultado = _obtenerListado(model);
                return View(model);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(model);
            }

        }

        [HttpPost]
        public ActionResult List(ListadoCajasModel model) {
            try{
            Session[model.idParametros] = model; //filtros actualizados
            ViewData["idParametros"] = model.idParametros;
            this.eliminarValidacionesIgnorables("Filtro.Sucursal", MetadataManager.IgnorablesDDL(model.Filtro.Sucursal));
            this.eliminarValidacionesIgnorables("Filtro.Financista", MetadataManager.IgnorablesDDL(model.Filtro.Financista));
            this.eliminarValidacionesIgnorables("Filtro.Moneda", MetadataManager.IgnorablesDDL(model.Filtro.Moneda));
            if (ModelState.IsValid) {
                if (model.Accion == ListadoCajasModel.ACCION.IMPRIMIR) {
                    return this.Report(model);
                }
                model.TabActual = ListadoCajasModel.TABS.EFECTIVO;//En cada refresh vuelvo al inicial
                model.Resultado = _obtenerListado(model);
            }
            return View(model);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(model);
            }
        }

        public ActionResult ListGrillaCheques(string idParametros) {
            ListadoCajasModel model = (ListadoCajasModel)Session[idParametros];
            model.Resultado = _obtenerListado(model);
            ViewData["idParametros"] = model.idParametros;
            return PartialView("_listGrillaCheques", model);
        }

        public ActionResult ListGrillaEfectivo(string idParametros) {
            ListadoCajasModel model = (ListadoCajasModel)Session[idParametros];
            model.Resultado = _obtenerListado(model);
            ViewData["idParametros"] = model.idParametros;
            return PartialView("_listGrillaEfectivo", model);
        }


        private ListadoMovimientosCaja _obtenerListado(ListadoCajasModel model) {
            model.AcomodarFiltro();
            return ListadoMovimientosCaja.obtenerListado(model.Filtro);
        }

        #endregion

        #region Reportes
        public ActionResult Report(ListadoCajasModel model) {
            return View("report", model);
        }

        public ActionResult ReportPartial(string idParametros) {
            ListadoCajasModel model = null;
            model = (ListadoCajasModel)Session[idParametros];
            XtraReport rep = null;
            if (model.TabActual == ListadoCajasModel.TABS.EFECTIVO) {
                rep = new DXListadoMovsCajaEfectivo();
            } else {
                rep = new DXListadoMovsCajaCheques();
            }

            model.Resultado = _obtenerListado(model);
            List<ListadoMovimientosCaja> ll = new List<ListadoMovimientosCaja>();
            ll.Add(model.Resultado);
            rep.DataSource = ll;
            setParamsToReport(rep, model);
            Session[idParametros] = model;
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_report");
        }

        public ActionResult ReportExport(string idParametros) {
            ListadoCajasModel model = null;
            model = (ListadoCajasModel)Session[idParametros];
            XtraReport rep = null;
            if (model.TabActual == ListadoCajasModel.TABS.EFECTIVO) {
                rep = new DXListadoMovsCajaEfectivo();
            } else {
                rep = new DXListadoMovsCajaCheques();
            }
            model.Resultado = _obtenerListado(model);
            setParamsToReport(rep, model);
            List<ListadoMovimientosCaja> ll = new List<ListadoMovimientosCaja>();
            ll.Add(model.Resultado);
            rep.DataSource = ll;
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }


        private void setParamsToReport(XtraReport report, ListadoCajasModel model) {
            Parameter paramSystemName = new Parameter();
            paramSystemName.Name = "detalleFiltros";
            paramSystemName.Type = typeof(string);
            paramSystemName.Value = model.detallesFiltro();
            paramSystemName.Description = "Detalle Filtros";
            paramSystemName.Visible = false;
            report.Parameters.Add(paramSystemName);
        }
        #endregion


        #region EntradaSalida

        public ActionResult ReciboCaja(int id) {
            try {
                //Transaccion tr = (Transaccion)Transaccion.ObtenerTransaccion(id);
                ViewData["idParametros"] = id;
                return View("ReciboCaja");
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private XtraReport _generarReciboCaja(int id) {
            Transaccion tr = (Transaccion)Transaccion.ObtenerTransaccion(id);
            List<Transaccion> ll = new List<Transaccion>();
            ll.Add(tr);
            XtraReport rep = new DXReciboEntradaSalidaCaja();
            rep.DataSource = ll;
            return rep;
        }

        public ActionResult ReciboCajaPartial(int idParametros) {
            XtraReport rep = _generarReciboCaja(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reciboCaja");
        }

        public ActionResult ReciboCajaExport(int idParametros) {
            XtraReport rep = _generarReciboCaja(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        #endregion

    }
}
