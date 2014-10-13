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
using AutomotoraWeb.Controllers.Sistema;

namespace AutomotoraWeb.Controllers.Financing {
    public class TransaccionesController : FinancingController {

        public static string CONTROLLER = "Transacciones";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            ViewBag.TiposRecibo = TipoRecibo.TiposRecibo();
            ViewBag.Clientes = Cliente.Clientes();
            ViewBag.Sucursales = Sucursal.Sucursales;
            ViewBag.Usuarios = Usuario.Usuarios();
            ViewBag.Financistas = Financista.FinancistasTodos;
        }

        public ActionResult ConsultaTransaccion(int id) {
            try {
                Recibo recibo = new Recibo();
                recibo.Numero = id;
                recibo.Consultar();
                return Redirect("/" + recibo.DestinoConsulta());
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return RedirectToAction("Mensaje", SistemaController.CONTROLLER, new { id = SistemaController.MSJ_ERROR });
            }
        }

        #region ListadoTransacciones


        //Se invoca desde la url del browser o desde el menu principal, o referencias externas. Devuelve la pagina completa
        public ActionResult List() {
            ListadoTransaccionesModel model = new ListadoTransaccionesModel();
            try {
                string s = SessionUtils.generarIdVarSesion("ListadoTransacciones", Session[SessionUtils.SESSION_USER].ToString());
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
        public ActionResult List(ListadoTransaccionesModel model) {
            try {
                Session[model.idParametros] = model; //filtros actualizados
                ViewData["idParametros"] = model.idParametros;
                //ViewBag.Financistas = Financista.Financistas(Financista.FIN_TIPO_LISTADO.TODOS);
                this.eliminarValidacionesIgnorables("Filtro.Financista", MetadataManager.IgnorablesDDL(new Financista()));
                this.eliminarValidacionesIgnorables("Filtro.Sucursal", MetadataManager.IgnorablesDDL(new Sucursal()));
                this.eliminarValidacionesIgnorables("Filtro.Cliente", MetadataManager.IgnorablesDDL(new Cliente()));
                this.eliminarValidacionesIgnorables("Filtro.Usuario", MetadataManager.IgnorablesDDL(new Usuario()));
                this.eliminarValidacionesIgnorables("Filtro.TipoRecibo", MetadataManager.IgnorablesDDL(new TipoRecibo()));
                if (ModelState.IsValid) {
                    if (model.Accion == ListadoTransaccionesModel.ACCIONES.IMPRIMIR) {
                        return this.ReportTransacciones(model);
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
