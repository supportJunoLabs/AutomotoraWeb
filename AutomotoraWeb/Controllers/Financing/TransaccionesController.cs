using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DLL_Backend;
using AutomotoraWeb.Models;
using AutomotoraWeb.Utils;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;

namespace AutomotoraWeb.Controllers.Financing
{
    public class TransaccionesController : FinancingController
    {

        public static string BCONTROLLER = "Financing";
        public static string CONTROLLER = "Transacciones";

        public override string getParentControllerName() {
            return BCONTROLLER;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            ViewBag.TiposRecibo = TipoRecibo.TiposRecibo();
            ViewBag.Clientes = Cliente.Clientes();
            ViewBag.Sucursales = Sucursal.Sucursales;
            ViewBag.Usuarios = Usuario.Usuarios();
            ViewBag.Financistas = Financista.FinancistasTodos;
        }

        public ActionResult ConsultaTransaccion(int id)
        {
            Recibo recibo = new Recibo();
            recibo.Numero = id;
            recibo.Consultar();

            return Redirect("/"+recibo.DestinoConsulta());
        }

        #region ListadoTransacciones


        //Se invoca desde la url del browser o desde el menu principal, o referencias externas. Devuelve la pagina completa
        public ActionResult List() {
            ListadoTransaccionesModel model = new ListadoTransaccionesModel();
            string s = SessionUtils.generarIdVarSesion("ListadoTransacciones", Session[SessionUtils.SESSION_USER].ToString());
            Session[s] = model;
            model.idParametros = s;
            ViewData["idParametros"] = model.idParametros;
            model.obtenerListado();
            return View(model);
        }

        [HttpPost]
        //Se invoca desde el boton actualizar o imprimir.
        public ActionResult List(ListadoTransaccionesModel model) {
            Session[model.idParametros] = model; //filtros actualizados
            ViewData["idParametros"] = model.idParametros;
            //ViewBag.Financistas = Financista.Financistas(Financista.FIN_TIPO_LISTADO.TODOS);
            this.eliminarValidacionesIgnorables("Filtro.Financista", MetadataManager.IgnorablesDDL(model.Filtro.Financista));
            this.eliminarValidacionesIgnorables("Filtro.Sucursal", MetadataManager.IgnorablesDDL(model.Filtro.Sucursal));
            this.eliminarValidacionesIgnorables("Filtro.Cliente", MetadataManager.IgnorablesDDL(model.Filtro.Cliente));
            this.eliminarValidacionesIgnorables("Filtro.Usuario", MetadataManager.IgnorablesDDL(model.Filtro.Usuario));
            this.eliminarValidacionesIgnorables("Filtro.TipoRecibo", MetadataManager.IgnorablesDDL(model.Filtro.TipoRecibo));
            if (ModelState.IsValid) {
                if (model.Accion == ListadoTransaccionesModel.ACCIONES.IMPRIMIR) {
                    return this.ReportTransacciones(model);
                }
                model.obtenerListado();
            }
            return View(model);
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de cuotas. Devuelve la partial del tab de cuotas
        public ActionResult ListGrillaTransacciones(string idParametros) {
            ListadoTransaccionesModel model = (ListadoTransaccionesModel)Session[idParametros];
            ViewData["idParametros"] = model.idParametros;
            model.obtenerListado();
            return PartialView("_listGrillaTransacciones", model.Resultado);
        }

        public ActionResult ReportTransacciones(ListadoTransaccionesModel model) {
            return View("ReportTransacciones", model);
        }

        private XtraReport _generarReporteTransacciones(string idParametros) {
            ListadoTransaccionesModel model = (ListadoTransaccionesModel)Session[idParametros];
            model.obtenerListado();
            XtraReport rep = new DXListadoTransacciones();
            rep.DataSource = model.Resultado;
            setParamsToReport(rep, model);
            return rep;
        }

        public ActionResult ReportTransaccionesPartial(string idParametros) {
            XtraReport rep = _generarReporteTransacciones(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reportTransacciones");
        }

        public ActionResult ReportTransaccionesExport(string idParametros) {
            XtraReport rep = _generarReporteTransacciones(idParametros);
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        private void setParamsToReport(XtraReport report, ListadoTransaccionesModel model) {
            Parameter param = new Parameter();
            param.Name = "detalleFiltros";
            param.Type = typeof(string);
            param.Value = model.detallesFiltro();
            param.Description = "Detalle Filtros";
            param.Visible = false;
            report.Parameters.Add(param);
        }


        #endregion

    }
}
