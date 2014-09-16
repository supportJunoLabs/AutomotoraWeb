using AutomotoraWeb.Controllers.General;
using AutomotoraWeb.Models;
using AutomotoraWeb.Services;
using AutomotoraWeb.Utils;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DLL_Backend;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;

namespace AutomotoraWeb.Controllers.Financing {
    public class FinancistasController : FinancingController, IMaintenance {

        public static string CONTROLLER = "Financistas";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            ViewBag.NombreEntidad = "Financista";
            ViewBag.NombreEntidades = "Financistas";
            ViewBag.Financistas = Financista.FinancistasTodos;
            ViewBag.FinancistasPago = Financista.FinancistasActivosSinDefault;
            Usuario usuario = (Usuario)(Session[SessionUtils.SESSION_USER]);
            if (usuario == null) {
                filterContext.Result = new RedirectResult("/" + AuthenticationController.CONTROLLER + "/" + AuthenticationController.LOGIN);
                return;
            }
            ViewBag.MultiSucursal = usuario.MultiSucursal;
            ViewBag.Sucursales = Sucursal.Sucursales;
        }

        #region Mantenimiento

        public ActionResult Show([ModelBinder(typeof(DevExpressEditorsBinder))] Financista fin) {
            return View(_listaElementos());
        }

        public ActionResult ListaGrilla() {
            return PartialView("_listGrilla", _listaElementos());
        }


        //--------------------------------------    REPORT    ----------------------------------------------
        public ActionResult Report() {
            return View();
        }

        public ActionResult ReportPartial() {
            DXReportFinancistas rep = new DXReportFinancistas();
            rep.DataSource = _listaElementos();
            ViewData["Report"] = rep;
            return PartialView("_reportList");
        }

        public ActionResult ReportExport() {
            DXReportFinancistas rep = new DXReportFinancistas();
            //setParamsToReport(rep);
            rep.DataSource = _listaElementos();
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        //--------------------------------------------------------------------------------------------------

        public ActionResult Details(int id) {
            ViewBag.SoloLectura = true;
            return VistaElemento(id);
        }

        public ActionResult Create() {
            Financista fin = new Financista();
            return View(fin); //para que tenga los valores inicializados por defecto en el nuevo objeto.
        }

        public ActionResult Edit(int id) {
            return VistaElemento(id);
        }

        public ActionResult Delete(int id) {
            ViewBag.SoloLectura = true;
            return VistaElemento(id);
        }

        //-----------------------------------------------------------------------------------------------------


        private ActionResult VistaElemento(int id) {
            try {
                Financista fin = _obtenerElemento(id);
                return View(fin);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private Financista _obtenerElemento(int id) {
            Financista fin = new Financista();
            fin.Codigo = id;
            fin.Consultar();
            return fin;
        }

        private List<Financista> _listaElementos() {
            return Financista.FinancistasTodos;
        }

        //-----------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Create(Financista td) {

            if (ModelState.IsValid) {
                try {
                    td.Agregar();
                    return RedirectToAction(BaseController.SHOW);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(td);
                }
            }

            return View(td);
        }

        //-----------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Edit(Financista fin) {
            if (ModelState.IsValid) {
                try {
                    fin.ModificarDatos();
                    return RedirectToAction(BaseController.SHOW);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(fin);
                }
            }

            return View(fin);
        }



        [HttpPost]
        public ActionResult Delete(Financista fin) {
            ViewBag.SoloLectura = true;
            if (ModelState.IsValid) {
                try {
                    string userName = (string)HttpContext.Session.Contents[SessionUtils.SESSION_USER_NAME];
                    string IP = HttpContext.Request.UserHostAddress;
                    fin.Eliminar(userName, IP);
                    return RedirectToAction(BaseController.SHOW);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(fin);
                }
            }

            return View(fin);
        }
        //-----------------------------------------------------------------------------------------------------

        #endregion

        #region consultaSituacionFinancista

        //Se invoca desde la url del browser o desde el menu principal, o referencias externas. Devuelve la pagina completa
        public ActionResult ConsultaSituacion(int? id) {
            try {
                Financista f = new Financista();
                f.Codigo = id ?? 0;
                if (id != null && id != 0) {
                    f.Consultar();
                }
                ViewData["idParametros"] = f.Codigo;
                return View(f);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        [HttpPost]
        //se invoca desde el boton actualizar e imprimir. Devuelve la pagina completa
        public ActionResult ConsultaSituacion(Financista model, string btnSubmit) {
            try {
                if (model != null) {
                    ViewData["idParametros"] = model.Codigo;
                }
                if (btnSubmit == "Imprimir" && model != null && model.Codigo != 0) {
                    return this.ReportSitFinancista(model);
                }
                return View(model);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(model);
            }
        }

        //Se invoca por json al actualizar la ddl de clientes, devuelve solo la partial de contenido.
        public ActionResult ConsultaSituacionPartial(int id) {
            Financista f = new Financista();
            f.Codigo = id;
            ViewData["idParametros"] = f.Codigo;
            SituacionFinancista sit = new SituacionFinancista();
            sit.generarSituacion(f);
            return PartialView("_situacionFinancista", sit);
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de cheques. Devuelve la partial del tab de cheques
        public ActionResult SitFinancistaChequesGrilla(int idParametros) {
            Financista f = new Financista();
            f.Codigo = idParametros;
            ViewData["idParametros"] = f.Codigo;
            SituacionFinancista sit = new SituacionFinancista();
            sit.generarSituacion(f);
            return PartialView("_sitFinancistaChequesGrilla", sit);
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de efectivo. Devuelve la partial del tab de efectivo
        public ActionResult SitFinancistaEfectivoGrilla(int idParametros) {
            Financista f = new Financista();
            f.Codigo = idParametros;
            ViewData["idParametros"] = f.Codigo;
            SituacionFinancista sit = new SituacionFinancista();
            sit.generarSituacion(f);
            return PartialView("_sitFinancistaEfectivoGrilla", sit);
        }

        public ActionResult ReportSitFinancista(Financista f) {
            return View("ReportSitFinancista", f);
        }

        private XtraReport _generarReport(int idParametros) {
            Financista f = new Financista();
            f.Codigo = idParametros;
            f.Consultar();
            SituacionFinancista model = new SituacionFinancista();
            model.generarSituacion(f);
            List<SituacionFinancista> ll = new List<SituacionFinancista>();
            ll.Add(model);
            XtraReport rep = new DXSituacionFinancista();
            rep.DataSource = ll;
            return rep;
        }

        public ActionResult ReportSitFinancistaPartial(int idParametros) {
            XtraReport rep = _generarReport(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reportSitFinancista");
        }

        public ActionResult ReportSitFinancistaExport(int idParametros) {
            XtraReport rep = _generarReport(idParametros);
            ViewData["idParametros"] = idParametros;
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        #endregion

        #region PagoFinancista

        public ActionResult Pago() {
            TRFinancistaPago tr = new TRFinancistaPago();
            Usuario usuario = (Usuario)(Session[SessionUtils.SESSION_USER]);
            tr.Sucursal = usuario.Sucursal;
            tr.Fecha = DateTime.Now.Date;
            if (Financista.FinancistasActivosSinDefault != null && Financista.FinancistasActivosSinDefault.Count > 0) {
                tr.Financista = Financista.FinancistasActivosSinDefault.First();
            }

            string idSession = SessionUtils.generarIdVarSesion("PagoFin", Session[SessionUtils.SESSION_USER].ToString()) + "|";
            ViewData["idSession"] = idSession;
            PagoFinancistaModel model = new PagoFinancistaModel();
            model.Transaccion = tr;
            model.listaCheques = tr.Financista.PagosChequesPendientes();
            model.listaEfectivo = tr.Financista.PagosEfectivoPendientes();
            Session[idSession + SessionUtils.EFECTIVO] = model.listaEfectivo;
            Session[idSession + SessionUtils.CHEQUES] = model.listaCheques;

            return View("Pago", model);
        }

        //para actualizar la grilla de pagos cuando cambia el financista elegido
        public ActionResult FinancistaPagoChanged(int? idFinancista, string idSession) {
            Financista fin = new Financista();
            fin.Codigo = idFinancista ?? 0;
            PagoFinancistaModel model = new PagoFinancistaModel();
            model.Transaccion = new TRFinancistaPago();
            model.Transaccion.Financista = fin;
            model.listaCheques = model.Transaccion.Financista.PagosChequesPendientes();
            model.listaEfectivo = model.Transaccion.Financista.PagosEfectivoPendientes();
            Session[idSession + SessionUtils.EFECTIVO] = model.listaEfectivo;
            Session[idSession + SessionUtils.CHEQUES] = model.listaCheques;
            return PartialView("_detallePago", model);
        }



        //para actualizar la grilla cheques desde el postback de la grilla
        public ActionResult ChequesPagoFinGrilla(int idFinancista, string idSession) {
            Financista fin = new Financista();
            fin.Codigo = idFinancista;
            var cheques = fin.PagosChequesPendientes();
            return PartialView("_selectChequePago", Session[idSession + SessionUtils.CHEQUES]);
        }

        //para actualizar la grilla cheques desde el postback de la grilla
        public ActionResult EfectivoPagoFinGrilla(int idFinancista, string idSession) {
            return PartialView("_selectEfectivoPago", Session[idSession + SessionUtils.EFECTIVO]);
        }

        private void _validarEfectivo(FinancistaPagoEfectivo pef, FinancistaPagoEfectivo orig) {

            //eliminarValidacionesIgnorablesEfectivo(efectivo);

            //ModelState.Remove("Importe.ImporteEnMonedaDefault.Monto");

            ModelState.Remove("ImporteOrig.Moneda");
            ModelState.Remove("ImportePagoAnt.Moneda");
            ModelState.Remove("ImportePagoActual.Moneda");
            ModelState.Remove("ImporteSaldo.Moneda");

            ////Sacar la validacion de moneda no nula porque da mensaje feo, hacerla manualmente
            //ModelState.Remove("Importe.Moneda.Codigo");
            //if (efectivo.Importe.Moneda == null || efectivo.Importe.Moneda.Codigo <= 0) {
            //    ModelState.AddModelError("Importe.Moneda.Codigo", "La moneda es requerida");
            //}

            //validar el importe
            pef.ImportePagoActual.Moneda = orig.ImporteOrig.Moneda;
            if (pef.ImportePagoActual.Monto < 0) {
                ModelState.AddModelError("ImportePagoActual.Monto", "El monto debe ser un valor positivo o cero");
            }
            if (pef.ImportePagoActual.Monto > orig.ImporteSaldo.Monto) {
                ModelState.AddModelError("ImportePagoActual.Monto", "El monto ("+pef.ImportePagoActual+") no puede superar al saldo ("+orig.ImporteSaldo+")");
            }
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult GrillaEfectivoEdit(FinancistaPagoEfectivo pef, string idSession) {
            List<FinancistaPagoEfectivo> lista = (List<FinancistaPagoEfectivo>)Session[idSession + SessionUtils.EFECTIVO];
            try {
                FinancistaPagoEfectivo efectivoEditado =
                            (from c in lista
                             where (c.Codigo == pef.Codigo)
                             select c).First<FinancistaPagoEfectivo>();

                _validarEfectivo(pef, efectivoEditado);
                if (ModelState.IsValid) {
                    efectivoEditado.ImportePagoActual.Monto = pef.ImportePagoActual.Monto;
                } else {
                    ViewData["EditError"] = "Corrija los valores incorrectos";
                }
            } catch (Exception e) {
                ViewData["EditError"] = e.Message;
            }
            return PartialView("_selectEfectivoPago", lista);
        }

        [HttpPost]
        public ActionResult Pago(PagoFinancistaModel model, string idSession ) {

            ViewData["idSession"] = idSession;
            model.listaCheques =(List<FinancistaPagoCheque>)  Session[idSession + SessionUtils.CHEQUES];
            model.listaEfectivo = (List<FinancistaPagoEfectivo>) Session[idSession + SessionUtils.EFECTIVO];

            string scheques = model.chequesIds??"";
            string sefectivo = model.efectivosIds??"";

            //Por ahora para que coincida con que no vuelve nada seleccionado. Esperar caso de devexpress
            //model.chequesIds = "";
            //model.efectivosIds = "";

            this.eliminarValidacionesIgnorables("Transaccion.Financista", MetadataManager.IgnorablesDDL(model.Transaccion.Financista));
            this.eliminarValidacionesIgnorables("Transaccion.Sucursal", MetadataManager.IgnorablesDDL(model.Transaccion.Sucursal));

            if (ModelState.IsValid) {
                try {

                    //Como no estoy usando una Transaccion del backend (que lo setea el filter) sino del modelo tengo que setear estos dos atributos a mano:
                    string nomUsuario = Session[SessionUtils.SESSION_USER_NAME].ToString();
                    string origen = HttpContext.Request.UserHostAddress;
                    model.Transaccion.setearAuditoria(nomUsuario, origen);

                    List<FinancistaPagoEfectivo> lef = (List<FinancistaPagoEfectivo>) Session[idSession + SessionUtils.EFECTIVO];
                    List<FinancistaPagoCheque> lch = (List<FinancistaPagoCheque>) Session[idSession + SessionUtils.CHEQUES];

                    string[] ach = scheques.Split(new Char[] { ',' });
                    foreach (string s in ach) {
                        if (!string.IsNullOrWhiteSpace(s)) {
                            FinancistaPagoCheque och = new FinancistaPagoCheque();
                            och.Codigo = Int32.Parse(s);
                            int i = lch.IndexOf(och);
                            if (i < 0) { 
                                ViewBag.ErrorCode = och.Codigo.ToString();
                                ViewBag.ErrorMessage = "No se encontro uno de los cheques seleccionados. Operacion cancelada";
                                return View("Pago", model);
                            }
                            model.Transaccion.ChequesEntregados.Add(lch[i]);
                        }
                    }
                    string[] ech = sefectivo.Split(new Char[] { ',' });
                    foreach (string s in ech) {
                        if (!string.IsNullOrWhiteSpace(s)) {
                            FinancistaPagoEfectivo och = new FinancistaPagoEfectivo();
                            och.Codigo = Int32.Parse(s);
                            int i = lef.IndexOf(och);
                            if (i < 0) {
                                ViewBag.ErrorCode = och.Codigo.ToString();
                                ViewBag.ErrorMessage = "No se encontro uno de los pagos en efectivo seleccionados. Operacion cancelada";
                                return View("Pago", model);
                            }
                            model.Transaccion.EfectivoEntregado.Add(lef[i]);
                        }
                    }

                    if ((model.Transaccion.EfectivoEntregado.Count + model.Transaccion.ChequesEntregados.Count) == 0) {
                        ViewBag.ErrorCode = "100";
                        ViewBag.ErrorMessage = "No hay pagos seleccionados. Operacion cancelada";
                        return View("Pago", model);
                    }

                    model.Transaccion.Ejecutar();
                    return RedirectToAction("ReciboPago", FinancistasController.CONTROLLER, new { id = model.Transaccion.NroRecibo });
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View("Pago", model);
                }
            }
            return View("Pago", model);

        }

        public ActionResult ReciboPago(int id) {
            try {
                TRFinancistaPago tr = (TRFinancistaPago)Transaccion.ObtenerTransaccion(id);
                ViewData["idParametros"] = id;
                return View("ReciboPago", tr.Financista);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private XtraReport _generarReciboPago(int id) {
            TRFinancistaPago tr = (TRFinancistaPago)Transaccion.ObtenerTransaccion(id);
            List<TRFinancistaPago> ll = new List<TRFinancistaPago>();
            ll.Add(tr);
            XtraReport rep = new DXReciboFinancistaPago();
            rep.DataSource = ll;
            return rep;
        }

        public ActionResult ReciboPagoPartial(int idParametros) {
            XtraReport rep = _generarReciboPago(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reciboPago");
        }

        public ActionResult ReciboPagoExport(int idParametros) {
            XtraReport rep = _generarReciboPago(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        #endregion

    }
}
