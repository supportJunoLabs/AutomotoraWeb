using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutomotoraWeb.Models;
using DLL_Backend;
using AutomotoraWeb.Utils;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using AutomotoraWeb.Controllers.General;
using System.Globalization;
using AutomotoraWeb.Services;
using AutomotoraWeb.Helpers.Grilla;

namespace AutomotoraWeb.Controllers.Financing {
    public class CuotasController : FinancingController {

        public static string CONTROLLER = "Cuotas";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            ViewBag.NombreEntidad = "Cuota";
            ViewBag.NombreEntidades = "Cuotas";
            Usuario usuario = getUsuario();
            if (usuario == null) {
                filterContext.Result = new RedirectResult("/" + AuthenticationController.CONTROLLER + "/" + AuthenticationController.LOGIN);
                return;
            }
            ViewBag.MultiSucursal = usuario.MultiSucursal;
            ViewBag.Sucursales = Sucursal.Sucursales;
            ViewBag.FinancistasActivos = Financista.FinancistasActivos;
        }

        #region CobrarCuota

        private void prepararSessionCobranza(Transaccion tr, string sop) {
            string idSession = SessionUtils.generarIdVarSesion(sop, getUserName()) + "|";
            Session[idSession] = tr;
            ViewData["idSession"] = idSession;
            Session[idSession + SessionUtils.CHEQUES] = tr.Pago.Cheques;
            Session[idSession + SessionUtils.EFECTIVO] = tr.Pago.Efectivos;
            Session[idSession + SessionUtils.MOV_BANCARIO] = tr.Pago.PagosBanco;
        }

        public TRCuotaCobro iniCobro() {
            TRCuotaCobro tr = new TRCuotaCobro();
            tr.ClienteOp = new Cliente();
            tr.ClienteOp.Codigo = 0;
            tr.Venta = new Venta();
            tr.Venta.Codigo = 0; //aca deberia cargar el codigo la gridlookup
            tr.Fecha = DateTime.Now;
            Usuario usuario = getUsuario();
            tr.Sucursal = usuario.Sucursal;

            prepararSessionCobranza(tr, "CobroCuota");
            return tr;
        }

         [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Cobrar() {
            //cobrar una cuota
            TRCuotaCobro tr = iniCobro();
            tr.Tipo = TRCuotaCobro.TIPO.CUOTA;
            tr.cantCuotas = 1;
            return View("Cobrar", tr);
        }

         [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult CobrarAC() {
            //cobrar a cuenta de cuota
            TRCuotaCobro tr = iniCobro();
            tr.Tipo = TRCuotaCobro.TIPO.PARCIAL;
            tr.cantCuotas = 1;
            return View("Cobrar", tr);
        }

         [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult CobrarConj() {
            //cobrar a cuenta de cuota
            TRCuotaCobro tr = iniCobro();
            tr.Tipo = TRCuotaCobro.TIPO.CONJUNTO;
            return View("Cobrar", tr);
        }

        //cuando se selecciona un cliente de la ddl de clientes
        public ActionResult VentasCobrarCliente(int idCliente, string idSession) {
            TRCuotaCobro tr = (TRCuotaCobro)Session[idSession];
            tr.ClienteOp = new Cliente();
            tr.ClienteOp.Codigo = idCliente;
            tr.Venta = new Venta();
            tr.Venta.Codigo = 0;
            return PartialView("_seleccionVentaCobrar", tr);
        }

        // se invoca desde la propia gridlookup cada vez que se va a desplegar (se reusa tanto para cobrar como refinanciar)
        public ActionResult GrillaVentasCobrarCliente(string idSession) {
            Transaccion tr = (Transaccion)Session[idSession];
            Cliente cl = null;
            if (tr is TRCuotaCobro) {
                cl = ((TRCuotaCobro)tr).ClienteOp;
            } else {
                cl = ((TRRefinanciacion)tr).ClienteOp;
            }
            GridLookUpModel gmodel = new GridLookUpModel();
            gmodel.Opciones = cl.VentasCuotasPendientes();
            return PartialView("_seleccionVentaGridLookup", gmodel);
        }

        //se invoca desde la grilla de cuotas para paginar (tanto en cobrar cuota como en refinanciar)
        public ActionResult CuotasVigentesVenta(string idSession) {
            Transaccion tr = (Transaccion)Session[idSession];
            if (tr is TRCuotaCobro) {
                return PartialView("_grillacuotasVigentes", ((TRCuotaCobro)tr).Venta.Financiacion.CuotasVigentes);
            } else {
                return PartialView("_grillacuotasVigentes", ((TRRefinanciacion)tr).Venta.Financiacion.CuotasVigentes);
            }
        }

        //se invoca al seleccionar una venta de la gridlookup, por ajax
        public ActionResult DetallesCobro(int idVenta, string idSession) {
            TRCuotaCobro tr = (TRCuotaCobro)Session[idSession];
            tr.Venta = new Venta();
            tr.Venta.Codigo = idVenta;
            Usuario u = getUsuario();
            tr.Venta.Consultar(u);

            if (tr.Tipo == TRCuotaCobro.TIPO.CUOTA) {
                CuotaCobroSugerido sug = tr.Venta.Financiacion.ProximaCuota().CobroSugerido();
                tr.Importe = new Importe(sug.CobroSugerido);
            } else if (tr.Tipo == TRCuotaCobro.TIPO.CONJUNTO) {
                CuotaCobroSugerido sug = tr.Venta.Financiacion.CobroSugerido();
                tr.Importe = new Importe(sug.CobroSugerido);
                tr.cantCuotas = sug.CantCuotas;
            }
            return PartialView("_detalleCobroCuota", tr);
        }

        public JsonResult CantCuotasChanged(string cantCuotas, string idSession) {
            ViewData["idSession"] = idSession;

            try {
                TRCuotaCobro tr = (TRCuotaCobro)Session[idSession];
                int cant;
                bool cantok = Int32.TryParse(cantCuotas, out cant);
                int cantMax = tr.Venta.Financiacion.CantCuotasPendientes();
                if (!cantok || cant <= 0 || cant > cantMax) {
                    List<String> errores1 = new List<string>();
                    errores1.Add("No es una cantidad de cuotas valida");
                    return Json(new { Result = "ERROR", ErrorCode = "100", ErrorMessage = errores1.ToArray() }, JsonRequestBehavior.AllowGet);
                }
                CuotaCobroSugerido sug = tr.Venta.Financiacion.CobroSugerido(DateTime.Now.Date, cant);
                var ret = Json(new {
                    Result = "OK", Intereses = sug.Intereses.ImporteTexto,
                    ImporteCalculado = sug.CobroSugerido.ImporteTexto, Importe = sug.CobroSugerido.Monto
                }, JsonRequestBehavior.AllowGet);
                return ret;
            } catch (UsuarioException exc) {
                List<String> errores1 = new List<string>();
                errores1.Add(exc.Message);
                return Json(new { Result = "ERROR", ErrorCode = exc.Codigo, ErrorMessage = errores1.ToArray() }, JsonRequestBehavior.AllowGet);
            }

        }

        //Al confirmar el cobro de una cuota
        [HttpPost]
        public ActionResult Cobrar(TRCuotaCobro tr, string idSession) {
            ViewData["idSession"] = idSession;

            TRCuotaCobro tr0 = (TRCuotaCobro)Session[idSession];

            tr.Venta = tr0.Venta;
            tr.ClienteOp = tr0.ClienteOp;
            tr.Tipo = tr0.Tipo;
            if (tr.Tipo != TRCuotaCobro.TIPO.CONJUNTO) {
                tr.cantCuotas = tr0.cantCuotas;
            }
            tr.Fecha = DateTime.Now.Date;

            Session[idSession] = tr;

            CuotaCobroSugerido sug = tr.Venta.Financiacion.ProximaCuota().CobroSugerido();
            tr.Importe.Moneda = sug.CobroSugerido.Moneda;

            ModelState.Remove("Venta.Sucursal");
            ModelState.Remove("Venta.Importe");
            ModelState.Remove("Importe.Moneda");
            ModelState.Remove("Venta.Vehiculo");
            ModelState.Remove("Venta.Cliente");

            this.eliminarValidacionesIgnorables("Sucursal", MetadataManager.IgnorablesDDL(new Sucursal()));
            this.eliminarValidacionesIgnorables("Importe.Moneda", MetadataManager.IgnorablesDDL(new Moneda()));
            this.eliminarValidacionesIgnorables("Venta", MetadataManager.IgnorablesDDL(new Venta()));
            this.eliminarValidacionesIgnorables("ClienteOp", MetadataManager.IgnorablesDDL(new Cliente()));

            //Lo hago aca al principio para que si hay error la tr vuelva con medios de pago con los valores anteriores.
            tr.Pago.AgregarCheques((IEnumerable<Cheque>)Session[idSession + SessionUtils.CHEQUES]);
            tr.Pago.AgregarMovsBanco((IEnumerable<MovBanco>)Session[idSession + SessionUtils.MOV_BANCARIO]);
            tr.Pago.AgregarEfectivos((IEnumerable<Efectivo>)Session[idSession + SessionUtils.EFECTIVO]);

            if (ModelState.IsValid) {
                try {
                    tr.Ejecutar();
                    return RedirectToAction("ReciboCuota", CuotasController.CONTROLLER, new { id = tr.NroRecibo });
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View("Cobrar", tr);
                }
            }
            return View("Cobrar", tr);
        }

        [HttpPost]
        public ActionResult CobrarAC(TRCuotaCobro tr, string idSession) {
            return Cobrar(tr, idSession);
        }

        [HttpPost]
        public ActionResult CobrarConj(TRCuotaCobro tr, string idSession) {
            return Cobrar(tr, idSession);
        }

        //private bool TransaccionConsultable(Transaccion tr) {
        //    if (tr == null || tr.NroRecibo == 0) return true;
        //    Usuario usuario = getUsuario();
        //    if (!SecurityService.Instance.verInfoAntigua(usuario) && tr.Antiguo) {
        //        return false;
        //    }
        //    return true;
        //}

        public ActionResult ReciboCuota(int id) {
            try {
                Usuario u = getUsuario();
                TRCuotaCobro tr = (TRCuotaCobro)Transaccion.ObtenerTransaccion(id,u);
                ViewData["idParametros"] = id;
                return View("ReciboCuota", tr);
            } catch (UsuarioException exc) {
                ViewData["idParametros"] = 0;
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private XtraReport _generarReciboCuota(int id) {
            Usuario u = getUsuario();
            TRCuotaCobro tr = (TRCuotaCobro)Transaccion.ObtenerTransaccion(id,u);
            List<TRCuotaCobro> ll = new List<TRCuotaCobro>();
            ll.Add(tr);
            XtraReport rep = new DXReciboCobroCuotas();
            rep.DataSource = ll;
            return rep;
        }

        public ActionResult ReciboCuotaPartial(int idParametros) {
            XtraReport rep = _generarReciboCuota(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reciboCuota");
        }

        public ActionResult ReciboCuotaExport(int idParametros) {
            XtraReport rep = _generarReciboCuota(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        #endregion

        #region TransferirCuotas

         [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Pasar() {
            //pasar cuotas a un financista
            TRCuotaTransferencia tr = new TRCuotaTransferencia();

            string idSession = SessionUtils.generarIdVarSesion("pasarCuotas", getUserName()) + "|";
            Session[idSession] = tr;
            ViewData["idSession"] = idSession;
            Session[idSession + SessionUtils.CHEQUES] = tr.Pago.Cheques;
            Session[idSession + SessionUtils.EFECTIVO] = tr.Pago.Efectivos;
            Session[idSession + SessionUtils.MOV_BANCARIO] = tr.Pago.PagosBanco;

            tr.Fecha = DateTime.Now;
            Usuario usuario = getUsuario();
            tr.Sucursal = usuario.Sucursal;

            return View("Pasar", tr);
        }

        // se invoca desde la propia gridlookup cada vez que se va a desplegar
        public ActionResult GrillaVentasTransferibles(string idSession) {
            GridLookUpModel gmodel = new GridLookUpModel();
            gmodel.Opciones = Financiacion.FinanciacionesTransferibles();
            return PartialView("_seleccionVentaPasarGridLookup", gmodel);
        }

        //se invoca al seleccionar una venta de la gridlookup, por ajax
        public ActionResult DetalleVentaPasar(string idSubfin, string idSession) {

            var datos = idSubfin.Split('-');
            ViewData["idSession"] = idSession;

            TRCuotaTransferencia tr = new TRCuotaTransferencia();
            Session[idSession] = tr;
            tr.SubFinanciacion = new SubFinanciacion();
            tr.SubFinanciacion.Venta = new Venta();
            tr.SubFinanciacion.Venta.Codigo = Int32.Parse(datos[0]);

            tr.SubFinanciacion.Financista = new Financista();
            tr.SubFinanciacion.Financista.Codigo = Int32.Parse(datos[1]);

            Usuario u = getUsuario();
            tr.SubFinanciacion.Consultar(u);

            Session[idSession + SessionUtils.CHEQUES] = tr.Pago.Cheques;
            Session[idSession + SessionUtils.EFECTIVO] = tr.Pago.Efectivos;
            Session[idSession + SessionUtils.MOV_BANCARIO] = tr.Pago.PagosBanco;

            return PartialView("_pasar", tr);
        }

        //se invoca automaticamente desde la grilla de cuotas
        public ActionResult GrillaCuotasTransf(string idSession) {
            TRCuotaTransferencia tr = (TRCuotaTransferencia)Session[idSession];
            return PartialView("_grillaCuotasTransf", tr.SubFinanciacion.CuotasPendientes());
        }

        [HttpPost]
        public ActionResult Pasar(TRCuotaTransferencia model, string idSession, string cuotasIds) {

            ViewData["idSession"] = idSession;
            ViewData["cuotasIds"] = cuotasIds;
            TRCuotaTransferencia tr = (TRCuotaTransferencia)Session[idSession];
            model.SubFinanciacion = tr.SubFinanciacion;
            model.Cuotas = new List<Cuota>();
            Session[idSession] = model;
            model.Fecha = DateTime.Now.Date;

            this.eliminarValidacionesIgnorables("Destinatario", MetadataManager.IgnorablesDDL(new Financista()));
            this.eliminarValidacionesIgnorables("Sucursal", MetadataManager.IgnorablesDDL(new Sucursal()));
            this.eliminarValidacionesIgnorables("SubFinanciacion.Financista", MetadataManager.IgnorablesDDL(new Financista()));
            if (model.Importe != null) {
                this.eliminarValidacionesIgnorables("Importe.Moneda", MetadataManager.IgnorablesDDL(new Moneda()));
            }

            //Lo hago aca al principio para que si hay error la tr vuelva con medios de pago con los valores anteriores.
            //Si es externo esta vacio, porque no se ve la opcion pagos.
            model.Pago.AgregarCheques((IEnumerable<Cheque>)Session[idSession + SessionUtils.CHEQUES]);
            model.Pago.AgregarMovsBanco((IEnumerable<MovBanco>)Session[idSession + SessionUtils.MOV_BANCARIO]);
            model.Pago.AgregarEfectivos((IEnumerable<Efectivo>)Session[idSession + SessionUtils.EFECTIVO]);

            if (ModelState.IsValid) {
                try {
                    string[] ach = cuotasIds.Split(new Char[] { ',' });
                    List<Cuota> ll = new List<Cuota>();
                    foreach (string s in ach) {
                        if (!string.IsNullOrWhiteSpace(s)) {
                            Cuota c0 = new Cuota();
                            c0.Codigo = Int32.Parse(s);
                            int i = model.SubFinanciacion.CuotasPendientes().IndexOf(c0);
                            if (i < 0) {
                                ViewBag.ErrorCode = c0.Codigo.ToString();
                                ViewBag.ErrorMessage = "No se encontro una de las cuotas seleccionados. Operacion cancelada";
                                return View("Pasar", model);
                            }
                            model.Cuotas.Add(model.SubFinanciacion.CuotasPendientes()[i]);
                        }
                    }

                    if (model.Cuotas.Count == 0) {
                        ViewBag.ErrorCode = "100";
                        ViewBag.ErrorMessage = "No hay cuotas seleccionadas. Operacion cancelada";
                        return View("Pasar", model);
                    }
                    model.Ejecutar();
                    return RedirectToAction("ReciboTransfCuotas", CuotasController.CONTROLLER, new { id = model.NroRecibo });
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View("Pasar", model);
                }
            }
            return View("Pasar", model);

        }

        public ActionResult ReciboTransfCuotas(int id) {
            try {
                Usuario u = getUsuario();
                TRCuotaTransferencia tr = (TRCuotaTransferencia)Transaccion.ObtenerTransaccion(id,u);
                ViewData["idParametros"] = id;
                return View("ReciboTransfCuotas", tr.NroOperacion);
            } catch (UsuarioException exc) {
                ViewData["idParametros"] = 0;
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private XtraReport _generarReciboTransfCuotas(int id) {
            Usuario u = getUsuario();
            TRCuotaTransferencia tr = (TRCuotaTransferencia)Transaccion.ObtenerTransaccion(id,u);
            List<TRCuotaTransferencia> ll = new List<TRCuotaTransferencia>();
            ll.Add(tr);
            XtraReport rep = new DXReciboTransfCuotas();
            rep.DataSource = ll;
            return rep;
        }

        public ActionResult ReciboTransfCuotasPartial(int idParametros) {
            XtraReport rep = _generarReciboTransfCuotas(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reciboTransfCuotas");
        }

        public ActionResult ReciboTransfCuotasExport(int idParametros) {
            XtraReport rep = _generarReciboTransfCuotas(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        #endregion

        #region RefinanciarCuotas

        //Se reusan metodos de cobrar: CuotasVigentesVenta, GrillaVentasCobrarCliente, VentasCobrarCliente    

        private TRRefinanciacion iniRefinanciacion() {
            TRRefinanciacion tr = new TRRefinanciacion();
            tr.ViejasCuotas = new List<Cuota>();
            tr.NuevasCuotas = new List<Cuota>();

            tr.ClienteOp = new Cliente();
            tr.ClienteOp.Codigo = 0;
            tr.Venta = new Venta();
            tr.Venta.Codigo = 0; //aca deberia cargar el codigo la gridlookup
            tr.Fecha = DateTime.Now;
            Usuario usuario = getUsuario();
            tr.Sucursal = usuario.Sucursal;
            return tr;
        }

         [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Modificar() {
            //refinanciar
            TRRefinanciacion tr = iniRefinanciacion();
            string idSession = SessionUtils.generarIdVarSesion("refinanciar", getUserName()) + "|";
            Session[idSession] = tr;
            ViewData["idSession"] = idSession;
            return View("Modificar", tr);
        }

        //cuando se selecciona un cliente de la ddl de clientes
        public ActionResult VentasRefinanciarCliente(int idCliente, string idSession) {
            TRRefinanciacion tr = iniRefinanciacion();
            Session[idSession]=tr;
            ViewData["idSession"] = idSession;
            tr.ViejasCuotas= new List<Cuota>();
            tr.NuevasCuotas= new List<Cuota>();

            tr.ClienteOp = new Cliente();
            tr.ClienteOp.Codigo = idCliente;
            tr.Venta = new Venta();
            tr.Venta.Codigo = 0;
            return PartialView("_seleccionVentaModificar", tr);
        }

        //se invoca al seleccionar una venta de la gridlookup, por ajax
        public ActionResult DetallesModificacion(int idVenta, string idSession) {
            TRRefinanciacion tr = iniRefinanciacion();

            Session[idSession] = tr;
            ViewData["idSession"] = idSession;

            tr.Venta = new Venta();
            tr.Venta.Codigo = idVenta;
            Usuario u = getUsuario();
            tr.Venta.Consultar(u);
            tr.ClienteOp = tr.Venta.Cliente;

            if (tr.Venta.Financiacion != null && tr.Venta.Financiacion.CuotasVigentes != null) {
                tr.ViejasCuotas= tr.Venta.Financiacion.CuotasVigentes;

                //Uso una replica de las cuotas, para que los cambios no afecten las originales
                tr.NuevasCuotas =  new List<Cuota>();
                int i = 0;
                foreach(Cuota c in tr.ViejasCuotas){
                    if (!c.Finalizada) {
                        i++;
                        Cuota c1 = new Cuota(c);
                        c1.NumeroCuotaSet = i;
                        tr.NuevasCuotas.Add(c1);
                    }
                }
                tr.CantNuevasCuotas = tr.NuevasCuotas.Count;
                if (tr.NuevasCuotas.Count > 0) {
                    tr.MontoBase = tr.NuevasCuotas[0].Importe.Monto;
                    tr.FechaBase = DateTime.Now.Date.AddMonths(1);
                }
            } 

            return PartialView("_detalleModifCuotas", tr);
        }

        private List<String> validarCambio(int cant, double importe) {
            List<String> errors = new List<String>();
            if (importe <= 0) {
                errors.Add("El importe base debe ser mayor a 0");
            }

            if (cant <= 0) {
                errors.Add("La cantidad de cuotas debe ser mayor a 0");
            }
            return errors;
        }


        [HttpPost]
        public JsonResult generarRefinanciacion(int cantCuotas, double montoBase, string idSession, string fechaBase) {

            try {

                //validacion del controller
                List<String> errors = this.validarCambio(cantCuotas, montoBase);
                if (errors.Count > 0) {
                    return Json(new { Result = "ERROR", ErrorCode = "VALIDATION_ERROR", ErrorMessage = errors.ToArray() });
                }

                TRRefinanciacion tr = (TRRefinanciacion)Session[idSession];
                IFormatProvider formato = new CultureInfo("es-UY").DateTimeFormat;
                DateTime fecha;
                if (!DateTime.TryParse(fechaBase, formato, DateTimeStyles.None, out fecha)) {
                    fecha = DateTime.Now.Date;
                }
                Importe importeBase = new Importe(tr.Venta.Financiacion.MontoFinanciado.Moneda, montoBase);
                tr.NuevasCuotas = tr.Venta.Financiacion.generarRefinanciacion(cantCuotas, importeBase, fecha);

                return Json(new { Result = "OK" });
            } catch (UsuarioException exc) {
                List<String> errores1 = new List<string>();
                errores1.Add(exc.Message);
                return Json(new { Result = "ERROR", ErrorCode = exc.Codigo, ErrorMessage = errores1.ToArray() });
            }

        }

        public ActionResult grillaNuevasCuotas(string idSession) {
            TRRefinanciacion tr = (TRRefinanciacion)Session[idSession];
            return PartialView("_grillaNuevasCuotas", tr.NuevasCuotas);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult grillaNuevasCuota_Update(Cuota cuota, string idSession) {
         
            TRRefinanciacion tr = (TRRefinanciacion)Session[idSession];
         
            this.eliminarValidacionesIgnorables("Importe.Moneda", MetadataManager.IgnorablesDDL(new Moneda()));
            if (cuota.Importe.Monto <= 0) {
                ModelState.AddModelError("Importe.Monto", "El monto debe ser un valor positivo");
            }

            if (ModelState.IsValid) {
                try {
                    int pos = -1;
                    for (int i = 0; i < tr.NuevasCuotas.Count; i++) {
                        if (tr.NuevasCuotas[i].NumeroCuotaSet == cuota.NumeroCuotaSet) {
                            pos = i;
                            break;
                        }
                    }
                    if (pos >= 0) {
                        tr.NuevasCuotas.RemoveAt(pos);
                        tr.NuevasCuotas.Add(cuota);
                        tr.NuevasCuotas = tr.NuevasCuotas.OrderBy(x => x.NumeroCuotaSet).ToList<Cuota>();
                    } else {
                        ViewData["EditError"] = "ERROR: Cuota no encontrada";
                    }
                } catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            } else {
                ViewData["EditError"] = "Corrija los valores incorrectos";
            }
            return PartialView("_grillaNuevasCuotas", tr.NuevasCuotas);
        }



        //Al confirmar la refinanciacion
        [HttpPost]
        public ActionResult Modificar(TRRefinanciacion tr, string idSession) {
            ViewData["idSession"] = idSession;

            TRRefinanciacion tr0 = (TRRefinanciacion)Session[idSession];
            tr0.Sucursal = tr.Sucursal;
            tr0.Observaciones = tr.Observaciones;
            tr0.Fecha = DateTime.Now.Date;
            string usuario = getUserName();
            string IP = getIP();
            tr0.setearAuditoria(usuario, IP);

            ModelState.Remove("Venta.Sucursal");
            ModelState.Remove("Venta.Importe");
            ModelState.Remove("Venta.Vehiculo");
            ModelState.Remove("Venta.Cliente");

            this.eliminarValidacionesIgnorables("Sucursal", MetadataManager.IgnorablesDDL(new Sucursal()));
            this.eliminarValidacionesIgnorables("Venta", MetadataManager.IgnorablesDDL(new Venta()));
            this.eliminarValidacionesIgnorables("ClienteOp", MetadataManager.IgnorablesDDL(new Cliente()));

            if (ModelState.IsValid) {
                try {
                    tr0.Ejecutar();
                    return RedirectToAction("ReciboRefinanc", CuotasController.CONTROLLER, new { id = tr0.NroRecibo });
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View("Modificar", tr0);
                }
            }
            return View("Modificar", tr0);
        }

        public ActionResult ReciboRefinanc(int id) {
            try {
                Usuario u = getUsuario();
                TRRefinanciacion tr = (TRRefinanciacion)Transaccion.ObtenerTransaccion(id,u);
                ViewData["idParametros"] = id;
                return View("ReciboRefinanc", tr.Venta);
            } catch (UsuarioException exc) {
                ViewData["idParametros"] = 0;
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private XtraReport _generarReciboRefinanc(int id) {
            Usuario u = getUsuario();
            TRRefinanciacion tr = (TRRefinanciacion)Transaccion.ObtenerTransaccion(id,u);
            List<TRRefinanciacion> ll = new List<TRRefinanciacion>();
            ll.Add(tr);
            XtraReport rep = new DXReciboRefinanciacion();
            rep.DataSource = ll;
            return rep;
        }

        public ActionResult ReciboRefinancPartial(int idParametros) {
            XtraReport rep = _generarReciboRefinanc(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reciboRefinanc");
        }

        public ActionResult ReciboRefinancExport(int idParametros) {
            XtraReport rep = _generarReciboRefinanc(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        #endregion
    }
}
