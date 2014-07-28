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
using AutomotoraWeb.Controllers.General;

namespace AutomotoraWeb.Controllers.Financing
{
    public class ValesController : FinancingController
    {
        public static string CONTROLLER = "Vales";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);

            Usuario usuario = (Usuario)(Session[SessionUtils.SESSION_USER]);
            if (usuario == null) {
                filterContext.Result = new RedirectResult("/" + AuthenticationController.CONTROLLER + "/" + AuthenticationController.LOGIN);
                return;
            }
            ViewBag.MultiSucursal = usuario.MultiSucursal;
            ViewBag.Cuentas = CuentaBancaria.CuentasBancarias;
            ViewBag.Sucursales = Sucursal.Sucursales;
            ViewBag.Financistas = Financista.FinancistasTodos;

        }

        #region ConsultaVales

        //El id Corrresonde al numero de vale
        public ActionResult ConsultaVale(string id) {
            ConsultaValeModel m = new ConsultaValeModel();
            m.Vale = new Vale();
            m.Cliente = new Cliente();
            if (!string.IsNullOrWhiteSpace(id)) {
                m.Vale.Codigo = id;
                m.Vale.Consultar();
                m.Cliente = m.Vale.ClienteOrigen;
            }
            ViewBag.SoloLectura = true;
            return View("ConsultaVale", m);
        }

        //El id corresponde al cliente, cuando entro con el cliente elegido
        public ActionResult ConsultaValesCliente(int? id) {
            ConsultaValeModel m = new ConsultaValeModel();
            m.Vale = new Vale();
            m.Cliente = new Cliente();
            if (id != null) {
                m.Cliente.Codigo = id??0;
                m.Cliente.Consultar();
            }
            ViewBag.SoloLectura = true;
            return View("ConsultaVale", m);
        }

        //desde el javascript de cambio en ddl clientes
        public ActionResult ValesCliente(int idCliente) {
            ConsultaValeModel m = new ConsultaValeModel();
            m.Cliente = new Cliente();
            m.Cliente.Codigo = idCliente;
            ViewBag.SoloLectura = true;
            return PartialView("_valesCliente", m);
        }


        //desde el javascript de cambio en ddl vales
        public ActionResult DetallesVale(string idVale) {
            ConsultaValeModel m = new ConsultaValeModel();
            m.Vale = new Vale();
            m.Vale.Codigo = idVale;
            m.Vale.Consultar();
            m.Cliente = m.Vale.ClienteOrigen;
            ViewBag.SoloLectura = true;
            return PartialView("_datosDetalleVale", m);
        }

        public ActionResult ReportVale(string id) {
            if (string.IsNullOrWhiteSpace(id)) {
                return RedirectToAction("ConsultaVale");
            }
            Vale v = new Vale();
            v.Codigo = id;
            //v.Consultar();
            ViewData["idParametros"] = id;
            return View("ReportVale", v);
        }

        public ActionResult ReportValePartial(string idParametros) {
            XtraReport rep = _generarReporteVale(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reportVale");
        }

        public ActionResult ReportValeExport(string idParametros) {
            XtraReport rep = _generarReporteVale(idParametros);
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        private XtraReport _generarReporteVale(string idParametros) {
            Vale v = new Vale();
            v.Codigo = idParametros;
            v.Consultar();
            List<Vale> ll = new List<Vale>();
            ll.Add(v);
            XtraReport rep = new DXReportConsultaVale();
            rep.DataSource = ll;
            return rep;
        }

        #endregion

        #region DescontarVale

        public ActionResult Descontar() {
            TRValeDescontar model = new TRValeDescontar();
            model.Vale = new Vale();
            Usuario usuario = (Usuario)(Session[SessionUtils.SESSION_USER]);
            model.Sucursal = usuario.Sucursal;
            return View(model);
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de cuotas. Devuelve la partial del tab de cuotas
        public ActionResult ValesDescontablesGrilla(GridLookUpModel model) {
            model.Opciones = Vale.ValesDescontables();
            return PartialView("_selectValeDesc", model);
        }

        [HttpPost]
        public ActionResult Descontar(TRValeDescontar tr) {

            this.eliminarValidacionesIgnorables("Sucursal", MetadataManager.IgnorablesDDL(tr.Sucursal));
            this.eliminarValidacionesIgnorables("Importe.Moneda", MetadataManager.IgnorablesDDL(tr.Importe.Moneda));
            this.eliminarValidacionesIgnorables("Cuenta", MetadataManager.IgnorablesDDL(tr.Cuenta));
            this.eliminarValidacionesIgnorables("Vale", MetadataManager.IgnorablesDDL(tr.Vale));


            //Sacar la validacion del vale porque sale con texto feo y hacerla manualmente
            ModelState.Remove("Vale.Codigo");

            if (tr.Vale == null || string.IsNullOrWhiteSpace(tr.Vale.Codigo)) {
                ModelState.AddModelError("Vale.Codigo", "El Vale es requerido");
            }

            if (tr.Importe.Monto <= 0) {
                ModelState.AddModelError("Importe.Monto", "Debe especificar un importe valido");
            }

            if (ModelState.IsValid) {
                try {
                    tr.Ejecutar();
                    return RedirectToAction("ReciboDescuento", ValesController.CONTROLLER, new { id = tr.NroRecibo });
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(tr);
                }
            }
            return View(tr);
        }

        public ActionResult ReciboDescuento(int id) {
            ViewData["idParametros"] = id;
            return View("ReciboDescuento");
        }

        private XtraReport _generarReciboDescuento(int id) {
            TRValeDescontar tr = (TRValeDescontar)Transaccion.ObtenerTransaccion(id);
            List<TRValeDescontar> ll = new List<TRValeDescontar>();
            ll.Add(tr);
            XtraReport rep = new DXReciboDescontarVale();  
            rep.DataSource = ll;
            return rep;
        }

        public ActionResult ReciboDescuentoPartial(int idParametros) {
            XtraReport rep = _generarReciboDescuento(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reciboDescuento");
        }

        public ActionResult ReciboDescuentoExport(int idParametros) {
            XtraReport rep = _generarReciboDescuento(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        #endregion


        #region ListadoVales

        //Se invoca desde el menu de financiaciones,en lugar de banco.
        public ActionResult ListValesF() {
            return RedirectToAction("ListVales");
        }


        //Se invoca desde la url del browser o desde el menu principal, o referencias externas. Devuelve la pagina completa
        public ActionResult ListVales() {
            ListadoValesModel model = new ListadoValesModel();
            string s = SessionUtils.generarIdVarSesion("ListadoVales", Session[SessionUtils.SESSION_USER].ToString());
            Session[s] = model;
            model.idParametros = s;
            ViewData["idParametros"] = model.idParametros;
            model.obtenerListado();
            return View(model);
        }

        [HttpPost]
        //Se invoca desde el boton actualizar o imprimir.
        public ActionResult ListVales(ListadoValesModel model) {
            Session[model.idParametros] = model; //filtros actualizados
            ViewData["idParametros"] = model.idParametros;
            this.eliminarValidacionesIgnorables("Filtro.Financista", MetadataManager.IgnorablesDDL(model.Filtro.Financista));
            if (ModelState.IsValid) {
                if (model.Accion == ListadoValesModel.ACCIONES.IMPRIMIR) {
                    return this.ReportVales(model);
                }
                model.obtenerListado();
            }
            return View(model);
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de cuotas. Devuelve la partial del tab de cuotas
        public ActionResult ListGrillaVales(string idParametros) {
            ListadoValesModel model = (ListadoValesModel)Session[idParametros];
            ViewData["idParametros"] = model.idParametros;
            model.obtenerListado();
            return PartialView("_listGrillaVales", model.Resultado.Vales);
        }

        public ActionResult ReportVales(ListadoValesModel model) {
            return View("ReportVales", model);
        }

        private XtraReport _generarReporteVales(string idParametros) {
            ListadoValesModel model = (ListadoValesModel)Session[idParametros];
            model.obtenerListado();
            List<ListadoVales> ll = new List<ListadoVales>();
            ll.Add(model.Resultado);
            XtraReport rep = new DXListadoVales();
            rep.DataSource = ll;
            setParamsToReport(rep, model);
            return rep;
        }
        private void setParamsToReport(XtraReport report, ListadoValesModel model) {
            Parameter param = new Parameter();
            param.Name = "detalleFiltros";
            param.Type = typeof(string);
            param.Value = model.detallesFiltro();
            param.Description = "Detalle Filtros";
            param.Visible = false;
            report.Parameters.Add(param);

            string s = "LISTADO VALES";
            Parameter param1 = new Parameter();
            param1.Name = "tituloReporte";
            param1.Type = typeof(string);
            param1.Value = s;
            param1.Description = "Titulo Reporte";
            param1.Visible = false;
            report.Parameters.Add(param1);

        }

        public ActionResult ReportValesPartial(string idParametros) {
            XtraReport rep = _generarReporteVales(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reportVales");
        }

        public ActionResult ReportValesExport(string idParametros) {
            XtraReport rep = _generarReporteVales(idParametros);
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        #endregion

        #region RechazarVale

        public ActionResult Rechazar() {
            TRValeRechazar model = new TRValeRechazar();
            model.Vale = new Vale();
            model.Sucursal = ((Usuario)(Session[SessionUtils.SESSION_USER])).Sucursal;
            return View(model);
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de cuotas. Devuelve la partial del tab de cuotas
        public ActionResult ValesRechazablesGrilla(GridLookUpModel model) {
            model.Opciones = new TRValeRechazar().ValesRechazables();
            return PartialView("_selectValeRechazar", model);
        }

        [HttpPost]
        public ActionResult Rechazar(TRValeRechazar tr) {
            this.eliminarValidacionesIgnorables("Vale", MetadataManager.IgnorablesDDL(tr.Vale));
            this.eliminarValidacionesIgnorables("Sucursal", MetadataManager.IgnorablesDDL(tr.Sucursal));

            //Sacar la validacion del Vale porque sale con texto feo y hacerla manualmente
            ModelState.Remove("Vale.Codigo");
            ModelState.Remove("Sucursal.Codigo");

            if (tr.Vale == null || string.IsNullOrWhiteSpace(tr.Vale.Codigo )) {
                ModelState.AddModelError("Vale.Codigo", "El Vale es requerido");
            }
            if (tr.Sucursal == null || tr.Sucursal.Codigo <= 0) {
                ModelState.AddModelError("Sucursal.Codigo", "La Sucursal es requerida");
            }

            if (ModelState.IsValid) {
                try {
                    tr.Ejecutar();
                    return RedirectToAction("ReciboRech", ValesController.CONTROLLER, new { id = tr.NroRecibo });
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(tr);
                }
            }
            return View(tr);
        }

        public ActionResult ReciboRech(int id) {
            ViewData["idParametros"] = id;
            return View("ReciboRech");
        }

        private XtraReport _generarReciboRech(int id) {
            TRValeRechazar tr = (TRValeRechazar)Transaccion.ObtenerTransaccion(id);
            List<TRValeRechazar> ll = new List<TRValeRechazar>();
            ll.Add(tr);
            XtraReport rep = new DXReciboRechazarVale();
            rep.DataSource = ll;
            return rep;
        }

        public ActionResult ReciboRechPartial(int idParametros) {
            XtraReport rep = _generarReciboRech(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reciboRech");
        }

        public ActionResult ReciboRechExport(int idParametros) {
            XtraReport rep = _generarReciboRech(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }


        #endregion

    }
}
