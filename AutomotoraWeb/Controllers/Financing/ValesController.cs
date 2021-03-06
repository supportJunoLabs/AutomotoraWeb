﻿using System;
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
using Newtonsoft.Json.Linq;
using System.Globalization;
using AutomotoraWeb.Services;
using AutomotoraWeb.Helpers.Grilla;

namespace AutomotoraWeb.Controllers.Financing {
    public class ValesController : FinancingController {
        public static string CONTROLLER = "Vales";

        private Usuario _usuario() {
            return getUsuario();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);

            Usuario usuario = getUsuario();
            if (usuario == null) {
                filterContext.Result = new RedirectResult("/" + AuthenticationController.CONTROLLER + "/" + AuthenticationController.LOGIN);
                return;
            }
            ViewBag.MultiSucursal = usuario.MultiSucursal;
            ViewBag.Cuentas = CuentaBancaria.CuentasBancarias;
            ViewBag.Sucursales = Sucursal.Sucursales;
            ViewBag.Financistas = Financista.FinancistasTodos;
            ViewBag.Sucursal = usuario.Sucursal;

        }

        #region RenovarVale

        //El id Corrresonde al numero de vale
        public ActionResult Renovar(string id) {
            try {
                RenovarValeModel m = new RenovarValeModel(_usuario());
                if (!string.IsNullOrWhiteSpace(id)) {
                    m.Transaccion.Vale.Codigo = id;
                    Usuario u = getUsuario();
                    m.Transaccion.Vale.Consultar(u);
                    m.Cliente = m.Transaccion.Vale.ClienteOrigen;
                    if (!m.Cliente.ValesPendientesOperables().Contains(m.Transaccion.Vale)) {
                        ViewBag.ErrorCode = "ERR";
                        ViewBag.ErrorMessage = "Vale " + id + " no habilitado para renovar o inexistente";
                        m.Transaccion.Vale = new Vale();
                        return View(new RenovarValeModel(_usuario()));
                    }
                    m.Sugerido = m.Transaccion.Vale.CobroSugerido();
                    m.Transaccion.Importe = m.Sugerido.CobroSugerido;
                }
                ViewBag.SoloLectura = true;
                return View(m);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        //El id corresponde al cliente, cuando entro con el cliente elegido
        public ActionResult RenovarCliente(int? id) {
            try {
                RenovarValeModel m = new RenovarValeModel(_usuario());
                if (id != null) {
                    m.Cliente.Codigo = id ?? 0;
                    m.Cliente.Consultar();
                }
                ViewBag.SoloLectura = true;
                return View("Renovar", m);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View("Renovar");
            }
        }

        //desde el javascript de cambio en ddl clientes
        public ActionResult ValesRenovablesCliente(int? idCliente) {
            RenovarValeModel m = new RenovarValeModel(_usuario());
            if (idCliente != null && idCliente > 0) {
                m.Cliente.Codigo = idCliente ?? 0;
            }
            ViewBag.SoloLectura = true;
            return PartialView("_valesRenovarCliente", m);
        }

        //desde el javascript de cambio en ddl vales
        public ActionResult DetallesValeRenovacion(string idVale) {
            if (!string.IsNullOrWhiteSpace(idVale)) {
                RenovarValeModel m = new RenovarValeModel(_usuario());
                m.Transaccion.Vale.Codigo = idVale;
                Usuario u = getUsuario();
                m.Transaccion.Vale.Consultar(u);
                m.Sugerido = m.Transaccion.Vale.CobroSugerido();
                m.Transaccion.Importe = m.Sugerido.CobroSugerido;
                ViewBag.SoloLectura = true;
                return PartialView("_valeRenovacion", m);
            } else {
                return PartialView("_valeRenovacion", new RenovarValeModel(_usuario()));
            }
        }

        [HttpPost]
        public JsonResult FechaRenovacionModif(AuxValeRenovacion datos) {
            Vale v = new Vale();
            v.Codigo = datos.IdVale;
            Usuario u = getUsuario();
            v.Consultar(u);
            DateTime fecha = DateTime.ParseExact(datos.FechaRenov, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            ValeCobroSugerido sug = v.CobroSugerido(fecha);

            AuxValeRenovacionSug result = new AuxValeRenovacionSug();
            result.CodMoneda = sug.CobroSugerido.Moneda.Codigo;
            result.Total = sug.CobroSugerido.Monto;

            result.TotalTexto = sug.CobroSugerido.ImporteTexto;
            result.InteresesTexto = sug.Intereses.ImporteTexto;

            return this.Json(result);
        }

        [HttpPost]
        public ActionResult Renovar(RenovarValeModel m) {

            ViewBag.SoloLectura = true;

            this.eliminarValidacionesIgnorables("Transaccion.Vale", MetadataManager.IgnorablesDDL(new Vale()));
            this.eliminarValidacionesIgnorables("Transaccion.Sucursal", MetadataManager.IgnorablesDDL(new Sucursal()));
            this.eliminarValidacionesIgnorables("Cliente", MetadataManager.IgnorablesDDL(new Cliente()));
            this.eliminarValidacionesIgnorables("Transaccion.Importe.Moneda", MetadataManager.IgnorablesDDL(new Moneda()));

            //Sacar la validacion del Vale porque sale con texto feo y hacerla manualmente
            ModelState.Remove("Transaccion.Vale.Codigo");
            ModelState.Remove("Transaccion.Sucursal.Codigo");

            if (m.Transaccion.Vale == null || string.IsNullOrWhiteSpace(m.Transaccion.Vale.Codigo)) {
                ModelState.AddModelError("Transaccion.Vale.Codigo", "El Vale es requerido");
            }
            if (m.Transaccion.Sucursal == null || m.Transaccion.Sucursal.Codigo <= 0) {
                ModelState.AddModelError("Transaccion.Sucursal.Codigo", "La Sucursal es requerida");
            }

            if (ModelState.IsValid) {
                try {
                    m.Transaccion.setearAuditoria(m.UserName, m.IP);
                    m.Transaccion.Ejecutar();
                    return RedirectToAction("ReciboRenovacion", ValesController.CONTROLLER, new { id = m.Transaccion.NroRecibo });
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(m);
                }
            }
            Usuario u = getUsuario();
            m.Transaccion.Vale.Consultar(u);
            m.Cliente.Consultar();
            return View(m);
        }

        //private bool TransaccionConsultable(Transaccion tr) {
        //    if (tr == null || tr.NroRecibo == 0) return true;
        //    Usuario usuario = getUsuario();
        //    if (!SecurityService.Instance.verInfoAntigua(usuario) && tr.Antiguo) {
        //        return false;
        //    }
        //    return true;
        //}

        public ActionResult ReciboRenovacion(int id) {
            try {
                ViewData["idParametros"] = id;
                Usuario u = getUsuario();
                TRValeRenovacion tr = (TRValeRenovacion)Transaccion.ObtenerTransaccion(id, u);
                return View("ReciboRenovacion", tr);
            } catch (UsuarioException exc) {
                ViewData["idParametros"] = 0;
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private XtraReport _generarReciboRenovacion(int id) {
            Usuario u = getUsuario();
            TRValeRenovacion tr = (TRValeRenovacion)Transaccion.ObtenerTransaccion(id, u);
            List<TRValeRenovacion> ll = new List<TRValeRenovacion>();
            ll.Add(tr);
            XtraReport rep = new DXReciboRenovarVale();
            rep.DataSource = ll;
            return rep;
        }

        public ActionResult ReciboRenovacionPartial(int idParametros) {
            XtraReport rep = _generarReciboRenovacion(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reciboRenovacion");
        }

        public ActionResult ReciboRenovacionExport(int idParametros) {
            XtraReport rep = _generarReciboRenovacion(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        #endregion

        #region ConsultaVales

        private List<Vale> _valesCliente(Cliente c) {
            Usuario usuario = getUsuario();
            return c.ValesNoAnulados(usuario);
        }

        //private bool ValeConsultable(Vale v) {
        //    if (v == null || string.IsNullOrEmpty(v.Codigo)) return true;
        //    Usuario usuario = getUsuario();
        //    if (!SecurityService.Instance.verInfoAntigua(usuario) && v.Antiguo) {
        //        return false;
        //    }
        //    return true;
        //}

        //El id Corrresonde al numero de vale
        public ActionResult ConsultaVale(string id) {
            ConsultaValeModel m = new ConsultaValeModel();
            try {
                List<Vale> lista = new List<Vale>();
                m.Vale = new Vale();
                m.Cliente = new Cliente();
                if (!string.IsNullOrWhiteSpace(id)) {
                    m.Vale.Codigo = id;
                    Usuario u = getUsuario();
                    m.Vale.Consultar(u);
                    m.Cliente = m.Vale.ClienteOrigen;
                    lista = _valesCliente(m.Cliente);
                }
                ViewBag.ValesCliente = lista;
                ViewBag.SoloLectura = true;
                return View("ConsultaVale", m);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(m);
            }
        }

        //El id corresponde al cliente, cuando entro con el cliente elegido
        public ActionResult ConsultaValesCliente(int? id) {
            ConsultaValeModel m = new ConsultaValeModel();
            try {
                List<Vale> lista = new List<Vale>();
                m.Vale = new Vale();
                m.Cliente = new Cliente();
                if (id != null) {
                    m.Cliente.Codigo = id ?? 0;
                    m.Cliente.Consultar();
                    lista = _valesCliente(m.Cliente);
                }
                ViewBag.ValesCliente = lista;
                ViewBag.SoloLectura = true;
                return View("ConsultaVale", m);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View("ConsultaVale", m);
            }
        }

        //desde el javascript de cambio en ddl clientes
        public ActionResult ValesCliente(int? idCliente) {
            ConsultaValeModel m = new ConsultaValeModel();
            if (idCliente != null && idCliente > 0) {
                m.Cliente = new Cliente();
                m.Cliente.Codigo = idCliente ?? 0;
                ViewBag.ValesCliente = _valesCliente(m.Cliente);
            } else {
                ViewBag.ValesCliente = new List<Vale>();
            }
            ViewBag.SoloLectura = true;
            return PartialView("_valesCliente", m);
        }


        //desde el javascript de cambio en ddl vales
        public ActionResult DetallesVale(string idVale) {
            Vale v = new Vale();
            ViewBag.SoloLectura = true;
            if (!string.IsNullOrWhiteSpace(idVale)) {
                v.Codigo = idVale;
                Usuario u = getUsuario();
                v.Consultar(u);
            }
            return PartialView("_datosDetalleVale", v);
        }


        //tambien desde el javascript de cambio en ddl vales, para los historicos
        public ActionResult DetallesValesAnteriores(string idVale) {
            Vale v = new Vale();
            ViewBag.SoloLectura = true;
            if (!string.IsNullOrWhiteSpace(idVale)) {
                v.Codigo = idVale;
            }
            return PartialView("_valesAnteriores", v);
        }


        public ActionResult ReportVale(string id) {
            if (string.IsNullOrWhiteSpace(id)) {
                return RedirectToAction("ConsultaVale");
            }
            Vale v = new Vale();
            v.Codigo = id;
            Usuario u = getUsuario();
            v.Consultar(u);
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
            Usuario u = getUsuario();
            v.Consultar(u);
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
            try {
                model.Vale = new Vale();
                Usuario usuario = getUsuario();
                model.Sucursal = usuario.Sucursal;
                return View(model);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(model);
            }
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de cuotas. Devuelve la partial del tab de cuotas
        public ActionResult ValesDescontablesGrilla(GridLookUpModel model) {
            model.Opciones = Vale.ValesDescontables();
            return PartialView("_selectValeDesc", model);
        }

        [HttpPost]
        public ActionResult Descontar(TRValeDescontar tr) {

            this.eliminarValidacionesIgnorables("Sucursal", MetadataManager.IgnorablesDDL(new Sucursal()));
            this.eliminarValidacionesIgnorables("Importe.Moneda", MetadataManager.IgnorablesDDL(new Moneda()));
            this.eliminarValidacionesIgnorables("Cuenta", MetadataManager.IgnorablesDDL(new CuentaBancaria()));
            this.eliminarValidacionesIgnorables("Vale", MetadataManager.IgnorablesDDL(new Vale()));


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
            try {
                ViewData["idParametros"] = id;
                Usuario u = getUsuario();
                TRValeDescontar tr = (TRValeDescontar)Transaccion.ObtenerTransaccion(id, u);
                return View("ReciboDescuento", tr);
            } catch (UsuarioException exc) {
                ViewData["idParametros"] = 0;
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private XtraReport _generarReciboDescuento(int id) {
            Usuario u = getUsuario();
            TRValeDescontar tr = (TRValeDescontar)Transaccion.ObtenerTransaccion(id, u);
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

        private void _obtenerListado(ListadoValesModel model) {
            Usuario usuario = getUsuario();
            model.obtenerListado(usuario);
        }

        //Se invoca desde la url del browser o desde el menu principal, o referencias externas. Devuelve la pagina completa
        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult ListVales() {
            ListadoValesModel model = new ListadoValesModel();
            try {
                string s = SessionUtils.generarIdVarSesion("ListadoVales", getUserName());
                Session[s] = model;
                model.idParametros = s;
                ViewData["idParametros"] = model.idParametros;
                _obtenerListado(model);
                return View(model);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(model);
            }
        }

        [HttpPost]
        //Se invoca desde el boton actualizar o imprimir.
        public ActionResult ListVales(ListadoValesModel model) {
            try {
                Session[model.idParametros] = model; //filtros actualizados
                ViewData["idParametros"] = model.idParametros;
                this.eliminarValidacionesIgnorables("Filtro.Financista", MetadataManager.IgnorablesDDL(new Financista()));
                if (ModelState.IsValid) {
                    if (model.Accion == ListadoValesModel.ACCIONES.IMPRIMIR) {
                        return this.ReportVales(model);
                    }
                    _obtenerListado(model);
                }
                return View(model);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(model);
            }
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de cuotas. Devuelve la partial del tab de cuotas
        public ActionResult ListGrillaVales(string idParametros) {
            ListadoValesModel model = (ListadoValesModel)Session[idParametros];
            ViewData["idParametros"] = model.idParametros;
            _obtenerListado(model);
            return PartialView("_listGrillaVales", model.Resultado.Vales);
        }

        public ActionResult ReportVales(ListadoValesModel model) {
            return View("ReportVales", model);
        }

        private XtraReport _generarReporteVales(string idParametros) {
            ListadoValesModel model = (ListadoValesModel)Session[idParametros];
            _obtenerListado(model);
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
            try {
                model.Vale = new Vale();
                model.Sucursal = (getUsuario()).Sucursal;
                return View(model);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(model);
            }
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de cuotas. Devuelve la partial del tab de cuotas
        public ActionResult ValesRechazablesGrilla(GridLookUpModel model) {
            model.Opciones = new TRValeRechazar().ValesRechazables();
            return PartialView("_selectValeRechazar", model);
        }

        [HttpPost]
        public ActionResult Rechazar(TRValeRechazar tr) {
            this.eliminarValidacionesIgnorables("Vale", MetadataManager.IgnorablesDDL(new Vale()));
            this.eliminarValidacionesIgnorables("Sucursal", MetadataManager.IgnorablesDDL(new Sucursal()));

            //Sacar la validacion del Vale porque sale con texto feo y hacerla manualmente
            ModelState.Remove("Vale.Codigo");
            ModelState.Remove("Sucursal.Codigo");

            if (tr.Vale == null || string.IsNullOrWhiteSpace(tr.Vale.Codigo)) {
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
            try {
                Usuario u = getUsuario();
                TRValeRechazar tr = (TRValeRechazar)Transaccion.ObtenerTransaccion(id, u);
                ViewData["idParametros"] = id;
                return View("ReciboRech", tr);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                ViewData["idParametros"] = 0;
                return View();
            }
        }

        private XtraReport _generarReciboRech(int id) {
            Usuario u = getUsuario();
            TRValeRechazar tr = (TRValeRechazar)Transaccion.ObtenerTransaccion(id,u);
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

        #region CobrarVale

        private void prepararSessionCobranza(Transaccion tr, string sop) {
            string idSession = SessionUtils.generarIdVarSesion(sop, getUserName()) + "|";
            Session[idSession] = tr;
            ViewData["idSession"] = idSession;
            Session[idSession + SessionUtils.CHEQUES] = tr.Pago.Cheques;
            Session[idSession + SessionUtils.EFECTIVO] = tr.Pago.Efectivos;
            Session[idSession + SessionUtils.MOV_BANCARIO] = tr.Pago.PagosBanco;
        }

        private TRValeCobro iniCobro() {
            TRValeCobro tr = new TRValeCobro();
            tr.ClienteOp = new Cliente();
            tr.ClienteOp.Codigo = 0;
            tr.Vale = new Vale();
            tr.Fecha = DateTime.Now;
            Usuario usuario = getUsuario();
            tr.Sucursal = usuario.Sucursal;
            prepararSessionCobranza(tr, "CobroVale");
            return tr;
        }

        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Cobrar() {
            //cobrar una cuota
            TRValeCobro tr = iniCobro();
            tr.Tipo = TRValeCobro.TIPO.VALE;
            return View("Cobrar", tr);
        }

        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult CobrarAC() {
            //cobrar a cuenta de cuota
            TRValeCobro tr = iniCobro();
            tr.Tipo = TRValeCobro.TIPO.PARCIAL;
            return View("Cobrar", tr);
        }

        //cuando se selecciona un cliente de la ddl de clientes
        public ActionResult ValesCobrarCliente(int idCliente, string idSession) {
            TRValeCobro tr = (TRValeCobro)Session[idSession];
            tr.ClienteOp = new Cliente();
            tr.ClienteOp.Codigo = idCliente;
            tr.Vale = new Vale();
            return PartialView("_seleccionValeCobrar", tr);
        }

        //se invoca al seleccionar el vale a cobrar
        public ActionResult DetallesCobro(string idVale, string idSession) {
            TRValeCobro tr = (TRValeCobro)Session[idSession];
            tr.Vale = new Vale();
            tr.Vale.Codigo = idVale;
            Usuario u = getUsuario();
            tr.Vale.Consultar(u);

            if (tr.Tipo == TRValeCobro.TIPO.VALE) {
                ValeCobroSugerido sug = tr.Vale.CobroSugerido();
                tr.Importe = new Importe(sug.CobroSugerido);
            }
            return PartialView("_detalleCobroVale", tr);
        }

        //Al confirmar el cobro de un vale
        [HttpPost]
        public ActionResult Cobrar(TRValeCobro tr, string idSession) {
            ViewData["idSession"] = idSession;

            TRValeCobro tr0 = (TRValeCobro)Session[idSession];

            tr.Vale = tr0.Vale;
            tr.ClienteOp = tr0.ClienteOp;
            tr.Tipo = tr0.Tipo;
            tr.Fecha = DateTime.Now.Date;

            Session[idSession] = tr;

            tr.Importe.Moneda = tr0.Vale.Importe.Moneda;

            ModelState.Remove("Importe.Moneda");

            this.eliminarValidacionesIgnorables("Sucursal", MetadataManager.IgnorablesDDL(new Sucursal()));
            this.eliminarValidacionesIgnorables("Importe.Moneda", MetadataManager.IgnorablesDDL(new Moneda()));
            this.eliminarValidacionesIgnorables("Vale", MetadataManager.IgnorablesDDL(new Vale()));
            this.eliminarValidacionesIgnorables("ClienteOp", MetadataManager.IgnorablesDDL(new Cliente()));

            //Lo hago aca al principio para que si hay error la tr vuelva con medios de pago con los valores anteriores.
            tr.Pago.AgregarCheques((IEnumerable<Cheque>)Session[idSession + SessionUtils.CHEQUES]);
            tr.Pago.AgregarMovsBanco((IEnumerable<MovBanco>)Session[idSession + SessionUtils.MOV_BANCARIO]);
            tr.Pago.AgregarEfectivos((IEnumerable<Efectivo>)Session[idSession + SessionUtils.EFECTIVO]);

            if (ModelState.IsValid) {
                try {
                    tr.Ejecutar();
                    return RedirectToAction("ReciboVale", ValesController.CONTROLLER, new { id = tr.NroRecibo });
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View("Cobrar", tr);
                }
            }
            return View("Cobrar", tr);
        }

        [HttpPost]
        public ActionResult CobrarAC(TRValeCobro tr, string idSession) {
            return Cobrar(tr, idSession);
        }

        public ActionResult ReciboVale(int id) {
            try {
                Usuario u = getUsuario();
                TRValeCobro tr = (TRValeCobro)Transaccion.ObtenerTransaccion(id, u);
                ViewData["idParametros"] = id;
                return View("ReciboVale", tr);
            } catch (UsuarioException exc) {
                ViewData["idParametros"] = 0;
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private XtraReport _generarReciboVale(int id) {
            Usuario u = getUsuario();
            TRValeCobro tr = (TRValeCobro)Transaccion.ObtenerTransaccion(id, u);
            List<TRValeCobro> ll = new List<TRValeCobro>();
            ll.Add(tr);
            XtraReport rep = new DXReciboCobroVale();
            rep.DataSource = ll;
            return rep;
        }

        public ActionResult ReciboValePartial(int idParametros) {
            XtraReport rep = _generarReciboVale(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reciboVale");
        }

        public ActionResult ReciboValeExport(int idParametros) {
            XtraReport rep = _generarReciboVale(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        #endregion

        #region TransferirVale

        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Pasar() {
            //pasar vale
            TRValeTransferencia tr = new TRValeTransferencia();
            tr.Vale = new Vale();
            tr.Fecha = DateTime.Now;
            Usuario usuario = getUsuario();
            tr.Sucursal = usuario.Sucursal;
            prepararSessionCobranza(tr, "PasarVale");
            return View("Pasar", tr);
        }

        //se invoca al seleccionar el vale a cobrar
        public ActionResult DetallesTransferencia(string idVale, string idSession) {
            TRValeTransferencia tr = (TRValeTransferencia)Session[idSession];
            tr.Vale = new Vale();
            tr.Vale.Codigo = idVale;
            Usuario u = getUsuario();
            tr.Vale.Consultar(u);
            return PartialView("_detalleTransfVale", tr);
        }


        //Al confirmar el cobro de un vale
        [HttpPost]
        public ActionResult Pasar(TRValeTransferencia tr, string idSession) {
            ViewData["idSession"] = idSession;

            TRValeTransferencia tr0 = (TRValeTransferencia)Session[idSession];

            tr.Vale = tr0.Vale;
            tr.Fecha = DateTime.Now.Date;

            Session[idSession] = tr;
            this.eliminarValidacionesIgnorables("Sucursal", MetadataManager.IgnorablesDDL(new Sucursal()));
            this.eliminarValidacionesIgnorables("Importe.Moneda", MetadataManager.IgnorablesDDL(new Moneda()));
            this.eliminarValidacionesIgnorables("Vale", MetadataManager.IgnorablesDDL(new Vale()));
            this.eliminarValidacionesIgnorables("Destinatario", MetadataManager.IgnorablesDDL(new Financista()));

            //Lo hago aca al principio para que si hay error la tr vuelva con medios de pago con los valores anteriores.
            tr.Pago.AgregarCheques((IEnumerable<Cheque>)Session[idSession + SessionUtils.CHEQUES]);
            tr.Pago.AgregarMovsBanco((IEnumerable<MovBanco>)Session[idSession + SessionUtils.MOV_BANCARIO]);
            tr.Pago.AgregarEfectivos((IEnumerable<Efectivo>)Session[idSession + SessionUtils.EFECTIVO]);

            if (ModelState.IsValid) {
                try {
                    tr.Ejecutar();
                    return RedirectToAction("ReciboTransfVale", ValesController.CONTROLLER, new { id = tr.NroRecibo });
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View("Pasar", tr);
                }
            }
            return View("Pasar", tr);
        }

        public ActionResult ReciboTransfVale(int id) {
            try {
                Usuario u = getUsuario();
                TRValeTransferencia tr = (TRValeTransferencia)Transaccion.ObtenerTransaccion(id, u);
                ViewData["idParametros"] = id;
                return View("ReciboTransfVale", tr.Vale);
            } catch (UsuarioException exc) {
                ViewData["idParametros"] = 0;
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private XtraReport _generarReciboTransfVale(int id) {
            Usuario u = getUsuario();
            TRValeTransferencia tr = (TRValeTransferencia)Transaccion.ObtenerTransaccion(id, u);
            List<TRValeTransferencia> ll = new List<TRValeTransferencia>();
            ll.Add(tr);
            XtraReport rep = new DXReciboTransfVale();
            rep.DataSource = ll;
            return rep;
        }

        public ActionResult ReciboTransfValePartial(int idParametros) {
            XtraReport rep = _generarReciboTransfVale(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reciboTransfVale");
        }

        public ActionResult ReciboTransfValeExport(int idParametros) {
            XtraReport rep = _generarReciboTransfVale(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        #endregion

    }

    public class AuxValeRenovacion {
        public string IdVale { get; set; }
        public string FechaRenov { get; set; }
    }

    public class AuxValeRenovacionSug {
        public int CodMoneda { get; set; }
        public double Total { get; set; }

        public string TotalTexto { get; set; }
        public string InteresesTexto { get; set; }

    }
}
