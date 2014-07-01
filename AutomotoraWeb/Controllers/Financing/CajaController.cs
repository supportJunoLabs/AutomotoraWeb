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

namespace AutomotoraWeb.Controllers.Financing
{
    public class CajaController : FinancingController
    {
        public static string CONTROLLER = "Caja";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
        }

        #region Listados

        public ActionResult List() {

            ListadoCajasModel model = new ListadoCajasModel();
            string s = SessionUtils.generarIdVarSesion("ListadoCaja", Session[SessionUtils.SESSION_USER].ToString());
            Session[s] = model;
            model.idParametros = s;
            ViewBag.Sucursales = Sucursal.Sucursales;
            ViewBag.Monedas= Moneda.Monedas;
            ViewBag.Financistas = Financista.Financistas(Financista.FIN_TIPO_LISTADO.TODOS);
            ViewData["idParametros"] = model.idParametros;
            model.Resultado = _obtenerListado(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult List(ListadoCajasModel model) {
            Session[model.idParametros] = model; //filtros actualizados
            ViewData["idParametros"] = model.idParametros;
            ViewBag.Sucursales = Sucursal.Sucursales;
            ViewBag.Monedas = Moneda.Monedas;
            ViewBag.Financistas = Financista.Financistas(Financista.FIN_TIPO_LISTADO.TODOS);
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
        }

        public ActionResult ListGrillaCheques(string idParametros) {
            ListadoCajasModel model = (ListadoCajasModel)Session[idParametros];
            model.Resultado = _obtenerListado(model);
            ViewData["idParametros"] = model.idParametros;
            return PartialView("_listGrillaCheques", model.Resultado.MovimientosCheques);
        }

        public ActionResult ListGrillaEfectivo(string idParametros) {
            ListadoCajasModel model = (ListadoCajasModel)Session[idParametros];
            model.Resultado = _obtenerListado(model);
            ViewData["idParametros"] = model.idParametros;
            return PartialView("_listGrillaEfectivo", model.Resultado.MovimientosEfectivo);
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
            if (model.TabActual==ListadoCajasModel.TABS.EFECTIVO){
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
             if (model.TabActual==ListadoCajasModel.TABS.EFECTIVO){
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

    }
}
