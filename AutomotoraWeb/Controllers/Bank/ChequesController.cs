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
using DevExpress.Web.ASPxGridView;
using AutomotoraWeb.Services;
using AutomotoraWeb.Helpers.Grilla;

namespace AutomotoraWeb.Controllers.Bank {
    public class ChequesController : BankController {

        public static string CONTROLLER = "Cheques";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            Usuario usuario = getUsuario();
            if (usuario == null) {
                filterContext.Result = new RedirectResult("/" + AuthenticationController.CONTROLLER + "/" + AuthenticationController.LOGIN);
                return;
            }
            ViewBag.MultiSucursal = usuario.MultiSucursal;
            ViewBag.Sucursales = Sucursal.Sucursales;
            ViewBag.Financistas = Financista.FinancistasTodos;
            ViewBag.Cuentas = CuentaBancaria.CuentasBancarias;
        }

        #region ListadoCheques

        //Se invoca desde el menu de financiaciones,en lugar de banco.
        public ActionResult ListChequesF() {
            return RedirectToAction("ListCheques");
        }

        private void _obtenerListado(ListadoChequesModel model) {
            Usuario u = getUsuario();
            //bool verInfoAntigua = SecurityService.Instance.verInfoAntigua(usuario);
            model.obtenerListado(u);
        }


        //Se invoca desde la url del browser o desde el menu principal, o referencias externas. Devuelve la pagina completa
        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult ListCheques() {
            ListadoChequesModel model = new ListadoChequesModel();
            try {
                string s = SessionUtils.generarIdVarSesion("ListadoCheques", getUserName());
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
        public ActionResult ListCheques(ListadoChequesModel model) {
            try {
                Session[model.idParametros] = model; //filtros actualizados
                ViewData["idParametros"] = model.idParametros;
                //ViewBag.Financistas = Financista.Financistas(Financista.FIN_TIPO_LISTADO.TODOS);
                this.eliminarValidacionesIgnorables("Filtro.Financista", MetadataManager.IgnorablesDDL(new Financista()));
                this.eliminarValidacionesIgnorables("Filtro.Sucursal", MetadataManager.IgnorablesDDL(new Sucursal()));
                if (ModelState.IsValid) {
                    if (model.Accion == ListadoChequesModel.ACCIONES.IMPRIMIR) {
                        return this.ReportCheques(model);
                    }
                    _obtenerListado(model);
                }
                return View(model);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }

        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de cuotas. Devuelve la partial del tab de cuotas
        public ActionResult ListGrillaCheques(string idParametros) {
            ListadoChequesModel model = (ListadoChequesModel)Session[idParametros];
            ViewData["idParametros"] = model.idParametros;
            _obtenerListado(model);
            return PartialView("_listGrillaCheques", model.Resultado.Cheques);
        }

        public ActionResult ReportCheques(ListadoChequesModel model) {
            return View("ReportCheques", model);
        }

        private XtraReport _generarReporteCheques(string idParametros) {
            ListadoChequesModel model = (ListadoChequesModel)Session[idParametros];
            _obtenerListado(model);
            List<ListadoCheques> ll = new List<ListadoCheques>();
            ll.Add(model.Resultado);
            XtraReport rep = new DXListadoCheques();
            rep.DataSource = ll;
            setParamsToReport(rep, model);
            return rep;
        }
        private void setParamsToReport(XtraReport report, ListadoChequesModel model) {
            Parameter param = new Parameter();
            param.Name = "detalleFiltros";
            param.Type = typeof(string);
            param.Value = model.detallesFiltro();
            param.Description = "Detalle Filtros";
            param.Visible = false;
            report.Parameters.Add(param);
        }

        public ActionResult ReportChequesPartial(string idParametros) {
            XtraReport rep = _generarReporteCheques(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reportCheques");
        }

        public ActionResult ReportChequesExport(string idParametros) {
            XtraReport rep = _generarReporteCheques(idParametros);
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        #endregion

        #region ConsultaCheque

        //private bool ChequeConsultable(Cheque ch) {
        //    if (ch == null || ch.Codigo == 0) return true;
        //    Usuario usuario = getUsuario();
        //    if (!SecurityService.Instance.verInfoAntigua(usuario) && ch.Antiguo) {
        //        return false;
        //    }
        //    return true;
        //}

        private Cheque _consultarCheque(int? idCheque) {
            Cheque ch = new Cheque();
            ch.Codigo = idCheque ?? 0;
            if (idCheque != null && idCheque != 0) {
                Usuario u = getUsuario();
                ch.Consultar(u);
            }
            return ch;
        }

        public ActionResult ConsultaCheque(int? id) {
            try {
                Cheque ch = _consultarCheque(id);
               ViewData["idParametros"] = ch.Codigo;
                return View("ConsultaCheque", ch);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View("ConsultaCheque");
            }
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de movimientos del cheque. 
        public ActionResult GrillaMovsCheques(int idParametros) {
            Cheque ch = _consultarCheque(idParametros);
            ViewData["idParametros"] = ch.Codigo;
            return PartialView("_movimientosCheque", ch.Movimientos);
        }

        public ActionResult ReportCheque(int? id) {
            try {
                if (id == null || id == 0) {
                    return RedirectToAction("ConsultaCheque");
                }
                Cheque ch = new Cheque();
                ch.Codigo = id ?? 0;
                Usuario u = getUsuario();
                ch.Consultar(u);
                ViewData["idParametros"] = id;
                return View("ReportCheque", ch);
            } catch (UsuarioException exc) {
                ViewData["idParametros"] = 0;
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View("ReportCheque");
            }
        }

        public ActionResult ReportChequePartial(int idParametros) {
            XtraReport rep = _generarReporteCheque(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reportCheque");
        }

        public ActionResult ReportChequeExport(int idParametros) {
            XtraReport rep = _generarReporteCheque(idParametros);
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        private XtraReport _generarReporteCheque(int idParametros) {
            Cheque ch = new Cheque();
            ch.Codigo = idParametros;
            Usuario u = getUsuario();
            ch.Consultar(u);
            List<Cheque> ll = new List<Cheque>();
            ll.Add(ch);
            XtraReport rep = new DXReportConsultaCheque();
            rep.DataSource = ll;
            return rep;
        }


        #endregion

        #region PasarCheque

        public ActionResult PasarF() {
            return RedirectToAction("Pasar");
        }

        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Pasar() {
            TRChequePasar model = new TRChequePasar();
            prepararSession(model);
            try {
                model.TipoDestino = TRChequePasar.TIPO_DESTINO.FINANCISTA;
                model.Cheque = new Cheque();
                model.Sucursal = (getUsuario()).Sucursal;
                return View(model);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(model);
            }
        }

        private void prepararSession(Transaccion tr) {
            string idSession = SessionUtils.generarIdVarSesion("ChequeEmitido", getUserName()) + "|";
            Session[idSession] = tr;
            ViewData["idSession"] = idSession;
            Session[idSession + SessionUtils.CHEQUES] = tr.Pago.Cheques;
            Session[idSession + SessionUtils.EFECTIVO] = tr.Pago.Efectivos;
            Session[idSession + SessionUtils.MOV_BANCARIO] = tr.Pago.PagosBanco;
        }

        //Se invoca para cargar opciones de la gridlookup 
        public ActionResult ChequesTransferiblesGrilla(GridLookUpModel model) {
            model.Opciones = Cheque.ChequesTransferibles();
            return PartialView("_selectChequePasar", model);
        }

        [HttpPost]
        public ActionResult Pasar(TRChequePasar tr, string idSession) {

            ViewData["idSession"] = idSession;

            //Lo hago aca al principio para que si hay error la tr vuelva con medios de pago con los valores anteriores.
            tr.Pago.AgregarCheques((IEnumerable<Cheque>)Session[idSession + SessionUtils.CHEQUES]);
            tr.Pago.AgregarMovsBanco((IEnumerable<MovBanco>)Session[idSession + SessionUtils.MOV_BANCARIO]);
            tr.Pago.AgregarEfectivos((IEnumerable<Efectivo>)Session[idSession + SessionUtils.EFECTIVO]);

            this.eliminarValidacionesIgnorables("Cheque", MetadataManager.IgnorablesDDL(new Cheque()));
            this.eliminarValidacionesIgnorables("Sucursal", MetadataManager.IgnorablesDDL(new Sucursal()));
            this.eliminarValidacionesIgnorables("Financista", MetadataManager.IgnorablesDDL(new Financista()));
            this.eliminarValidacionesIgnorables("Importe.Moneda", MetadataManager.IgnorablesDDL(new Moneda()));

            //Sacar la validacion del cheque porque sale con texto feo y hacerla manualmente
            ModelState.Remove("Cheque.Codigo");

            if (tr.Cheque == null || tr.Cheque.Codigo <= 0) {
                ModelState.AddModelError("Cheque.Codigo", "El Cheque es requerido");
            }

            //Se valida de este lado, porque no siempre es requerido.
            if (tr.TipoDestino == TRChequePasar.TIPO_DESTINO.FINANCISTA && (tr.Financista == null || tr.Financista.Codigo <= 0)) {
                ModelState.AddModelError("Financista.Codigo", "El Financista es requerido");
            }
            if (tr.TipoDestino == TRChequePasar.TIPO_DESTINO.TERCERO && string.IsNullOrWhiteSpace(tr.Tercero)) {
                ModelState.AddModelError("Tercero", "El Tercero es requerido");
            }

            if (ModelState.IsValid) {
                try {
                    tr.Ejecutar();
                    return RedirectToAction("ReciboTransf", ChequesController.CONTROLLER, new { id = tr.NroRecibo });
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(tr);
                }
            }
            return View(tr);
        }

        public ActionResult ReciboTransf(int id) {
            try {
                Usuario u = getUsuario();
                TRChequePasar tr = (TRChequePasar)Transaccion.ObtenerTransaccion(id, u);
                return View("ReciboTransf", tr);
            } catch (UsuarioException exc) {
                ViewData["idParametros"] = id;
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private XtraReport _generarReciboTransf(int id) {
            Usuario u = getUsuario();
            TRChequePasar tr = (TRChequePasar)Transaccion.ObtenerTransaccion(id, u);
            List<TRChequePasar> ll = new List<TRChequePasar>();
            ll.Add(tr);
            XtraReport rep = new DXReciboTransfCheque();
            rep.DataSource = ll;
            return rep;
        }

        public ActionResult ReciboTransfPartial(int idParametros) {
            XtraReport rep = _generarReciboTransf(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reciboTransf");
        }

        public ActionResult ReciboTransfExport(int idParametros) {
            XtraReport rep = _generarReciboTransf(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }


        #endregion

        #region DepositarDescontarCheque

        public ActionResult Depositar() {
            TRChequeDepositarDescontar model = new TRChequeDepositarDescontar();
            try {
                model.Cheque = new Cheque();
                model.TipoDestino = TRChequeDepositarDescontar.TIPO_DESTINO.DEPOSITAR;
                model.Sucursal = (getUsuario()).Sucursal;
                return View(model);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(model);
            }
        }

        public ActionResult Descontar() {
            TRChequeDepositarDescontar model = new TRChequeDepositarDescontar();
            try {
                model.Cheque = new Cheque();
                model.TipoDestino = TRChequeDepositarDescontar.TIPO_DESTINO.DESCONTAR;
                model.Sucursal = (getUsuario()).Sucursal;
                return View(model);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(model);
            }
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de cuotas. Devuelve la partial del tab de cuotas
        public ActionResult ChequesDepositablesGrilla(GridLookUpModel model) {
            model.Opciones = Cheque.ChequesDepositables();
            return PartialView("_selectCheque", model);
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de cuotas. Devuelve la partial del tab de cuotas
        public ActionResult ChequesDescontablesGrilla(GridLookUpModel model) {
            model.Opciones = Cheque.ChequesDescontables();
            return PartialView("_selectChequeDesc", model);
        }

        private ActionResult DepositarDescontar(TRChequeDepositarDescontar tr) {

            this.eliminarValidacionesIgnorables("Sucursal", MetadataManager.IgnorablesDDL(new Sucursal()));
            if (tr.Importe != null && tr.Importe.Moneda != null) {
                this.eliminarValidacionesIgnorables("Importe.Moneda", MetadataManager.IgnorablesDDL(new Moneda()));
            }
            this.eliminarValidacionesIgnorables("Cuenta", MetadataManager.IgnorablesDDL(new CuentaBancaria()));
            this.eliminarValidacionesIgnorables("Cheque", MetadataManager.IgnorablesDDL(new Cheque()));


            //Sacar la validacion del cheque porque sale con texto feo y hacerla manualmente
            ModelState.Remove("Cheque.Codigo");

            if (tr.Cheque == null || tr.Cheque.Codigo <= 0) {
                ModelState.AddModelError("Cheque.Codigo", "El Cheque es requerido");
            }

            if (tr.TipoDestino == TRChequeDepositarDescontar.TIPO_DESTINO.DESCONTAR &&
                tr.Importe.Monto <= 0) {
                ModelState.AddModelError("Importe.Monto", "Debe especificar un importe valido");
            }

            if (ModelState.IsValid) {
                try {
                    tr.Ejecutar();
                    return RedirectToAction("ReciboDeposito", ChequesController.CONTROLLER, new { id = tr.NroRecibo });
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(tr);
                }
            }
            return View(tr);
        }

        [HttpPost]
        public ActionResult Descontar(TRChequeDepositarDescontar tr) {
            return DepositarDescontar(tr);
        }

        [HttpPost]
        public ActionResult Depositar(TRChequeDepositarDescontar tr) {
            return DepositarDescontar(tr);
        }

        public ActionResult ReciboDeposito(int id) {
            try {
                Usuario u = getUsuario();
                TRChequeDepositarDescontar tr = (TRChequeDepositarDescontar)Transaccion.ObtenerTransaccion(id, u);
                ViewData["idParametros"] = id;
                return View("ReciboDeposito", tr);
            } catch (UsuarioException exc) {
                ViewData["idParametros"] = 0;
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private XtraReport _generarReciboDeposito(int id) {
            Usuario u = getUsuario();
            TRChequeDepositarDescontar tr = (TRChequeDepositarDescontar)Transaccion.ObtenerTransaccion(id, u);
            List<TRChequeDepositarDescontar> ll = new List<TRChequeDepositarDescontar>();
            ll.Add(tr);
            XtraReport rep = new DXReciboDepositarCheque();
            rep.DataSource = ll;
            return rep;
        }

        public ActionResult ReciboDepositoPartial(int idParametros) {
            XtraReport rep = _generarReciboDeposito(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reciboDeposito");
        }

        public ActionResult ReciboDepositoExport(int idParametros) {
            XtraReport rep = _generarReciboDeposito(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        #endregion

        #region ChequesTransferirSucursal

        public ActionResult TransfSuc() {
            ChequeTransfSucModel model = new ChequeTransfSucModel();
            try {
                Sucursal suc = (getUsuario()).Sucursal;
                model.SucursalOrigen = suc;
                ViewData["idParametros"] = suc.Codigo;
                return View(model);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(model);
            }
        }

        public ActionResult SucursalOrigenChanged(int? idSucursal) {
            ViewData["idParametros"] = idSucursal;
            Sucursal suc = new Sucursal();
            suc.Codigo = idSucursal ?? 0;
            var cheques = Cheque.ChequesTransferiblesSucursal(suc);
            return PartialView("_selectChequeTransfSuc", cheques);
        }

        public ActionResult ChequesTransfSucGrilla(int idParametros) {
            ViewData["idParametros"] = idParametros;
            Sucursal suc = new Sucursal();
            suc.Codigo = idParametros;
            var cheques = Cheque.ChequesTransferiblesSucursal(suc);
            return PartialView("_selectChequeTransfSuc", cheques);
        }

        [HttpPost]
        public ActionResult TransfSuc(ChequeTransfSucModel model) {
            this.eliminarValidacionesIgnorables("SucursalOrigen", MetadataManager.IgnorablesDDL(new Sucursal()));
            this.eliminarValidacionesIgnorables("SucursalDestino", MetadataManager.IgnorablesDDL(new Sucursal()));
            ViewData["idParametros"] = model.SucursalOrigen.Codigo;
            if (ModelState.IsValid) {
                try {
                    TRChequeTransfSucursal tr = new TRChequeTransfSucursal();
                    tr.SucursalOrigen = model.SucursalOrigen;
                    tr.SucursalDestino = model.SucursalDestino;
                    tr.Observaciones = model.Observaciones;

                    string scheques = model.ChequesIds;

                    if (string.IsNullOrWhiteSpace(scheques)) {
                        ViewBag.ErrorCode = "CH001";
                        ViewBag.ErrorMessage = "No hay cheques seleccionados";
                        return View(model);
                    }

                    string[] ach = scheques.Split(new Char[] { ',' });
                    foreach (string s in ach) {
                        if (!string.IsNullOrWhiteSpace(s)) {
                            Cheque och = new Cheque();
                            och.Codigo = Int32.Parse(s);
                            tr.Cheques.Add(och);
                        }
                    }
                    if (tr.Cheques.Count == 0) {
                        ViewBag.ErrorCode = "CH001";
                        ViewBag.ErrorMessage = "No hay cheques seleccionados";
                        return View(model);
                    }

                    //Como no estoy usando una Transaccion del backend (que lo setea el filter) sino del modelo tengo que setear estos dos atributos a mano:
                    string nomUsuario = getUserName();
                    string origen = getIP();
                    tr.setearAuditoria(nomUsuario, origen);

                    tr.Ejecutar();

                    return RedirectToAction("ReciboTransfSuc", ChequesController.CONTROLLER, new { id = tr.NroRecibo });

                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(model);
                }
            }
            return View(model);
        }

        public ActionResult ReciboTransfSuc(int id) {
            try {
                Usuario u = getUsuario();
                Transaccion tr = (Transaccion)Transaccion.ObtenerTransaccion(id, u);
                ViewData["idParametros"] = id;
                return View("ReciboTransfSuc");
            } catch (UsuarioException exc) {
                ViewData["idParametros"] = 0;
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }


        private XtraReport _generarReciboTransfSuc(int id) {
            Usuario u = getUsuario();
            TRChequeTransfSucursal tr = (TRChequeTransfSucursal)Transaccion.ObtenerTransaccion(id,u);
            List<TRChequeTransfSucursal> ll = new List<TRChequeTransfSucursal>();
            ll.Add(tr);
            XtraReport rep = new DXReciboTransfSucCheque();
            rep.DataSource = ll;
            return rep;
        }

        public ActionResult ReciboTransfSucPartial(int idParametros) {
            XtraReport rep = _generarReciboTransfSuc(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reciboTransfSuc");
        }

        public ActionResult ReciboTransfSucExport(int idParametros) {
            XtraReport rep = _generarReciboTransfSuc(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        #endregion

        #region RechazarCheque

        public ActionResult Rechazar() {
            TRChequeRechazar model = new TRChequeRechazar();
            try {
                model.Cheque = new Cheque();
                model.Sucursal = (getUsuario()).Sucursal;
                return View(model);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(model);
            }
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de cuotas. Devuelve la partial del tab de cuotas
        public ActionResult ChequesRechazablesGrilla(GridLookUpModel model) {
            model.Opciones = new TRChequeRechazar().ChequesRechazables();
            return PartialView("_selectChequeRechazar", model);
        }

        [HttpPost]
        public ActionResult Rechazar(TRChequeRechazar tr) {
            this.eliminarValidacionesIgnorables("Cheque", MetadataManager.IgnorablesDDL(new Cheque()));
            this.eliminarValidacionesIgnorables("Sucursal", MetadataManager.IgnorablesDDL(new Sucursal()));

            //Sacar la validacion del cheque porque sale con texto feo y hacerla manualmente
            ModelState.Remove("Cheque.Codigo");
            ModelState.Remove("Sucursal.Codigo");

            if (tr.Cheque == null || tr.Cheque.Codigo <= 0) {
                ModelState.AddModelError("Cheque.Codigo", "El Cheque es requerido");
            }
            if (tr.Sucursal == null || tr.Sucursal.Codigo <= 0) {
                ModelState.AddModelError("Sucursal.Codigo", "La Sucursal es requerida");
            }

            if (ModelState.IsValid) {
                try {
                    tr.Ejecutar();
                    return RedirectToAction("ReciboRech", ChequesController.CONTROLLER, new { id = tr.NroRecibo });
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
                ViewData["idParametros"] = id;
                Usuario u = getUsuario();
                TRChequeRechazar tr = (TRChequeRechazar)Transaccion.ObtenerTransaccion(id,u);
                return View("ReciboRech", tr);
            } catch (UsuarioException exc) {
                ViewData["idParametros"] = 0;
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private XtraReport _generarReciboRech(int id) {
            Usuario u = getUsuario();
            TRChequeRechazar tr = (TRChequeRechazar)Transaccion.ObtenerTransaccion(id,u);
            List<TRChequeRechazar> ll = new List<TRChequeRechazar>();
            ll.Add(tr);
            XtraReport rep = new DXReciboRechazarCheque();
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

        #region CanjearChequeRechazado

        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult CanjeRechazado() {
            TRChequeRechazadoCanje model = new TRChequeRechazadoCanje();
            prepararSession(model);
            try {
                model.Cheque = new Cheque();
                model.Sucursal = (getUsuario()).Sucursal;
                return View(model);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult CanjeRechazado(TRChequeRechazadoCanje tr, string idSession) {

            ViewData["idSession"] = idSession;

            //Lo hago aca al principio para que si hay error la tr vuelva con medios de pago con los valores anteriores.
            tr.Pago.AgregarCheques((IEnumerable<Cheque>)Session[idSession + SessionUtils.CHEQUES]);
            tr.Pago.AgregarMovsBanco((IEnumerable<MovBanco>)Session[idSession + SessionUtils.MOV_BANCARIO]);
            tr.Pago.AgregarEfectivos((IEnumerable<Efectivo>)Session[idSession + SessionUtils.EFECTIVO]);

            this.eliminarValidacionesIgnorables("Cheque", MetadataManager.IgnorablesDDL(new Cheque()));
            this.eliminarValidacionesIgnorables("Sucursal", MetadataManager.IgnorablesDDL(new Sucursal()));
            this.eliminarValidacionesIgnorables("Importe.Moneda", MetadataManager.IgnorablesDDL(new Moneda()));

            //Sacar la validacion del cheque porque sale con texto feo y hacerla manualmente
            ModelState.Remove("Cheque.Codigo");

            if (tr.Cheque == null || tr.Cheque.Codigo <= 0) {
                ModelState.AddModelError("Cheque.Codigo", "El Cheque es requerido");
            }

            if (ModelState.IsValid) {
                try {
                    tr.Ejecutar();
                    return RedirectToAction("ReciboCanje", ChequesController.CONTROLLER, new { id = tr.NroRecibo });
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(tr);
                }
            }
            return View(tr);
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de cuotas. Devuelve la partial del tab de cuotas
        public ActionResult ChequesCanjeablesGrilla(GridLookUpModel model) {
            model.Opciones = Cheque.ChequesRechazados();
            return PartialView("_selectChequeCanjear", model);
        }

        public ActionResult ReciboCanje(int id) {
            try {
                ViewData["idParametros"] = id;
                Usuario u = getUsuario();
                TRChequeRechazadoCanje tr = (TRChequeRechazadoCanje)Transaccion.ObtenerTransaccion(id,u);
                return View("ReciboCanje", tr);
            } catch (UsuarioException exc) {
                ViewData["idParametros"] = id;
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private XtraReport _generarReciboCanje(int id) {
            Usuario u = getUsuario();
            TRChequeRechazadoCanje tr = (TRChequeRechazadoCanje)Transaccion.ObtenerTransaccion(id,u);
            List<TRChequeRechazadoCanje> ll = new List<TRChequeRechazadoCanje>();
            ll.Add(tr);
            XtraReport rep = new DXReciboCanjeCheque();
            rep.DataSource = ll;
            return rep;
        }

        public ActionResult ReciboCanjePartial(int idParametros) {
            XtraReport rep = _generarReciboCanje(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reciboCanje");
        }

        public ActionResult ReciboCanjeExport(int idParametros) {
            XtraReport rep = _generarReciboCanje(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        #endregion



    }
}
