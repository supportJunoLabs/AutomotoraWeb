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

namespace AutomotoraWeb.Controllers.Financing
{
    public class CuotasController : FinancingController
    {

        public static string CONTROLLER = "Cuotas";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            ViewBag.NombreEntidad = "Cuota";
            ViewBag.NombreEntidades = "Cuotas";
            Usuario usuario = (Usuario)(Session[SessionUtils.SESSION_USER]);
            if (usuario == null) {
                filterContext.Result = new RedirectResult("/" + AuthenticationController.CONTROLLER + "/" + AuthenticationController.LOGIN);
                return;
            }
            ViewBag.MultiSucursal = usuario.MultiSucursal;
            ViewBag.Sucursales = Sucursal.Sucursales;
        }
        
        #region CobrarCuota

        private void prepararSessionCobranza(Transaccion tr, string sop) {
            string idSession = SessionUtils.generarIdVarSesion(sop, Session[SessionUtils.SESSION_USER].ToString()) + "|";
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

            prepararSessionCobranza(tr, "CobroCuota");
            return tr;
        }

        public ActionResult Cobrar() {
            //cobrar una cuota
            TRCuotaCobro tr = iniCobro();
            tr.Tipo = TRCuotaCobro.TIPO.CUOTA;
            tr.cantCuotas = 1;
            return View("Cobrar", tr);
        }

        public ActionResult CobrarAC() {
            //cobrar a cuenta de cuota
            TRCuotaCobro tr = iniCobro();
            tr.Tipo = TRCuotaCobro.TIPO.PARCIAL;
            tr.cantCuotas = 1;
            return View("Cobrar", tr);
        }

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
            //ViewData["idParametros"] = tr.ClienteOp.Codigo;  //Lo necesita la gridlookup para pedir los datos cuando se despliega cada vez
            //ViewData["idSession"] = idSession;
            return PartialView("_seleccionVenta", tr);
        }

        // se invoca desde la propia gridlookup cada vez que se va a desplegar
        public ActionResult GrillaVentasCobrarCliente(string idSession) {
            TRCuotaCobro tr = (TRCuotaCobro)Session[idSession];
            GridLookUpModel gmodel = new GridLookUpModel();
            gmodel.Opciones = tr.ClienteOp.VentasCuotasPendientes();
            //ViewData["idParametros"] = c.Codigo;
            return PartialView("_seleccionVentaGridLookup", gmodel);
        }

        //se invoca desde la grilla de cuotas para paginar
        public ActionResult CuotasVigentesVenta(string idSession) {
            TRCuotaCobro tr = (TRCuotaCobro)Session[idSession];
            //ViewData["idSession"] = idSession;
            //ViewData["idParametros"] = tr.ClienteOp.Codigo;
            return PartialView("_grillacuotasVigentes", tr.Venta.Financiacion.CuotasVigentes);
        }

        //se invoca al seleccionar una venta de la gridlookup, por ajax
        public ActionResult DetallesCobro(int idVenta, string idSession) {
            TRCuotaCobro tr = (TRCuotaCobro)Session[idSession];
            //ViewData["idSession"] = idSession;
            //ViewData["idParametros"] = tr.ClienteOp.Codigo;
            tr.Venta = new Venta();
            tr.Venta.Codigo = idVenta;
            tr.Venta.Consultar();

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
                if (!cantok || cant<=0 || cant>cantMax){
                    List<String> errores1 = new List<string>();
                    errores1.Add("No es una cantidad de cuotas valida");
                    return Json(new { Result = "ERROR", ErrorCode = "100", ErrorMessage = errores1.ToArray() }, JsonRequestBehavior.AllowGet);
                }
                CuotaCobroSugerido sug = tr.Venta.Financiacion.CobroSugerido(DateTime.Now.Date, cant);
                var ret= Json(new { Result = "OK", Intereses=sug.Intereses.ImporteTexto,  
                    ImporteCalculado=sug.CobroSugerido.ImporteTexto,  Importe=sug.CobroSugerido.Monto }, JsonRequestBehavior.AllowGet);
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
            tr.Fecha =DateTime.Now.Date;

            Session[idSession] = tr;

            CuotaCobroSugerido sug = tr.Venta.Financiacion.ProximaCuota().CobroSugerido();
            tr.Importe.Moneda = sug.CobroSugerido.Moneda;

            ModelState.Remove("Venta.Sucursal");
            ModelState.Remove("Venta.Importe");
            ModelState.Remove("Importe.Moneda");

            this.eliminarValidacionesIgnorables("Sucursal", MetadataManager.IgnorablesDDL(tr.Sucursal));
            this.eliminarValidacionesIgnorables("Importe.Moneda", MetadataManager.IgnorablesDDL(tr.Importe.Moneda));
            this.eliminarValidacionesIgnorables("Venta", MetadataManager.IgnorablesDDL(tr.Venta));
            this.eliminarValidacionesIgnorables("ClienteOp", MetadataManager.IgnorablesDDL(tr.ClienteOp));

            if (ModelState.IsValid) {
                try {
                    tr.Pago.AgregarCheques((IEnumerable<Cheque>)Session[idSession + SessionUtils.CHEQUES]);
                    tr.Pago.AgregarMovsBanco((IEnumerable<MovBanco>)Session[idSession + SessionUtils.MOV_BANCARIO]);
                    tr.Pago.AgregarEfectivos((IEnumerable<Efectivo>)Session[idSession + SessionUtils.EFECTIVO]);

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

        public ActionResult ReciboCuota(int id) {
            try {
                TRCuotaCobro tr = (TRCuotaCobro)Transaccion.ObtenerTransaccion(id);
                ViewData["idParametros"] = id;
                return View("ReciboCuota", tr);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private XtraReport _generarReciboCuota(int id) {
            TRCuotaCobro tr = (TRCuotaCobro)Transaccion.ObtenerTransaccion(id);
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

        public ActionResult ReciboTransfCuotas(int id) {
            try {
                TRCuotaTransferencia tr = (TRCuotaTransferencia)Transaccion.ObtenerTransaccion(id);
                ViewData["idParametros"] = id;
                return View("ReciboTransfCuotas", tr.NroOperacion);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private XtraReport _generarReciboTransfCuotas(int id) {
            TRCuotaTransferencia tr = (TRCuotaTransferencia)Transaccion.ObtenerTransaccion(id);
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

        public ActionResult ReciboRefinanc(int id) {
            try {
                TRRefinanciacion tr = (TRRefinanciacion)Transaccion.ObtenerTransaccion(id);
                ViewData["idParametros"] = id;
                return View("ReciboRefinanc", tr.Venta);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private XtraReport _generarReciboRefinanc(int id) {
            TRRefinanciacion tr = (TRRefinanciacion)Transaccion.ObtenerTransaccion(id);
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
