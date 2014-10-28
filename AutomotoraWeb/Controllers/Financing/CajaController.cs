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
using AutomotoraWeb.Controllers.General;
using AutomotoraWeb.Services;

namespace AutomotoraWeb.Controllers.Financing {
    public class CajaController : FinancingController {
        public static string CONTROLLER = "Caja";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            ViewBag.Sucursales = Sucursal.Sucursales;
            ViewBag.Monedas = Moneda.Monedas;
            ViewBag.Financistas = Financista.FinancistasTodos;
            Usuario usuario = getUsuario();
            if (usuario == null) {
                filterContext.Result = new RedirectResult("/" + AuthenticationController.CONTROLLER + "/" + AuthenticationController.LOGIN);
                return;
            }
            ViewBag.MultiSucursal = usuario.MultiSucursal;
        }

        #region Listados

         [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult List() {
            ListadoCajasModel model = new ListadoCajasModel();
            try {
                string s = SessionUtils.generarIdVarSesion("ListadoCaja", getUserName());
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
            this.eliminarValidacionesIgnorables("Filtro.Sucursal", MetadataManager.IgnorablesDDL(new Sucursal()));
            this.eliminarValidacionesIgnorables("Filtro.Financista", MetadataManager.IgnorablesDDL(new Financista()));
            this.eliminarValidacionesIgnorables("Filtro.Moneda", MetadataManager.IgnorablesDDL(new Moneda()));
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
            Usuario usuario = getUsuario();
            //bool verInfoAntigua = SecurityService.Instance.verInfoAntigua(usuario);
            model.AcomodarFiltro();
            ListadoMovimientosCaja listado = ListadoMovimientosCaja.obtenerListado(model.Filtro, usuario);
            //para reflejar el periodo por si hubo restriccion por infoantigua
            model.Desde = model.Filtro.Desde;
            model.Hasta = model.Filtro.Hasta;
            return listado;
        }

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

         [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Entrada() {
            TRCajaEntrada tr = new TRCajaEntrada();
            Usuario usuario = getUsuario();
            tr.Sucursal = usuario.Sucursal;
            tr.Fecha = DateTime.Now.Date;

            string idSession = SessionUtils.generarIdVarSesion("EntradaCaja", getUserName())+"|";
            ViewData["idSession"] = idSession;
            Session[idSession + SessionUtils.CHEQUES] = tr.Pago.Cheques;
            Session[idSession + SessionUtils.EFECTIVO] = tr.Pago.Efectivos;

            return View("Entrada", tr);
        }

       
        [HttpPost]
        public ActionResult Entrada(TRCajaEntrada tr, string idSession) {
            ViewData["idSession"] = idSession;
            this.eliminarValidacionesIgnorables("Sucursal", MetadataManager.IgnorablesDDL( new Sucursal()));
            
            if (ModelState.IsValid) {
                try {
                    tr.Fecha = DateTime.Now;
                    tr.Pago.AgregarCheques((IEnumerable<Cheque>)Session[idSession + SessionUtils.CHEQUES]);
                    tr.Pago.AgregarEfectivos((IEnumerable<Efectivo>)Session[idSession + SessionUtils.EFECTIVO]);

                    tr.Ejecutar();
                    return RedirectToAction("ReciboCaja", CajaController.CONTROLLER, new { id = tr.NroRecibo });
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View("Entrada", tr);
                }
            }
            return View("Entrada", tr);
        }

         [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Salida() {
            TRCajaSalida tr = new TRCajaSalida();
            Usuario usuario = getUsuario();
            tr.Sucursal = usuario.Sucursal;
            tr.Fecha = DateTime.Now.Date;

            string idSession = SessionUtils.generarIdVarSesion("SalidaCaja", getUserName()) + "|";
            ViewData["idSession"] = idSession;
            Session[idSession + SessionUtils.EFECTIVO] = tr.Pago.Efectivos;

            return View("Salida", tr);
        }


       public ActionResult ChequesRetirablesSucGrilla(int idSucursal) {
            Sucursal suc = new Sucursal();
            suc.Codigo = idSucursal;
            var cheques = Cheque.ChequesRetirablesSucursal(suc);
            return PartialView("_selectChequeSalida", cheques);
        }


        public ActionResult SucursalSalidaChanged(int? idSucursal) {
            Sucursal suc = new Sucursal();
            suc.Codigo = idSucursal ?? 0;
            var cheques = Cheque.ChequesRetirablesSucursal(suc);
            return PartialView("_selectChequeSalida", cheques);
        }

        [HttpPost]
        public ActionResult Salida(TRCajaSalida tr, string idSession, string chequesIds) {
            ViewData["idSession"] = idSession;
            this.eliminarValidacionesIgnorables("Sucursal", MetadataManager.IgnorablesDDL(new Sucursal()));
            Usuario u = getUsuario();

            if (ModelState.IsValid) {
                try {
                    tr.Fecha = DateTime.Now;
                    tr.Pago.AgregarEfectivos((IEnumerable<Efectivo>)Session[idSession + SessionUtils.EFECTIVO]);

                    string[] ach = chequesIds.Split(new Char[] { ',' });
                    foreach (string s in ach) {
                        if (!string.IsNullOrWhiteSpace(s)) {
                            Cheque och = new Cheque();
                            och.Codigo = Int32.Parse(s);
                            och.Consultar(u);
                            tr.Pago.AgregarCheque(och);
                        }
                    }

                    tr.Ejecutar();
                    return RedirectToAction("ReciboCaja", CajaController.CONTROLLER, new { id = tr.NroRecibo });
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View("Salida", tr);
                }
            }
            return View("Salida", tr);
        }

        //private bool TransaccionConsultable(Transaccion tr) {
        //    if (tr == null || tr.NroRecibo == 0) return true;
        //    Usuario usuario = getUsuario();
        //    if (!SecurityService.Instance.verInfoAntigua(usuario) && tr.Antiguo) {
        //        return false;
        //    }
        //    return true;
        //}

        public ActionResult ReciboCaja(int id) {
            try {
                ViewData["idParametros"] = id;
                Usuario u = getUsuario();
                Transaccion tr = (Transaccion)Transaccion.ObtenerTransaccion(id, u);
                return View("ReciboCaja");
            } catch (UsuarioException exc) {
                ViewData["idParametros"] = 0;
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private XtraReport _generarReciboCaja(int id) {
            Usuario u = getUsuario();
            Transaccion tr = (Transaccion)Transaccion.ObtenerTransaccion(id,u);
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
