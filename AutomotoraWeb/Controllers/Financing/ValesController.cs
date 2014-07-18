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

            ViewBag.Cuentas = CuentaBancaria.CuentasBancarias;
            ViewBag.Sucursales = Sucursal.Sucursales;
            if (usuario.MultiSucursal) {
                ViewBag.SucursalesTransaccion = Sucursal.Sucursales;
            } else {
                List<Sucursal> listSucursal = new List<Sucursal>();
                listSucursal.Add(usuario.Sucursal);
                ViewBag.SucursalesTransaccion = listSucursal;
            }
        }

        #region ConsultaVales

        private Vale _consultarVale(string idVale) {
            Vale v = new Vale();
            if (idVale != null && idVale.Trim() != "") {
                v.Codigo = idVale;
                v.Consultar();
            }
            ViewData["idParametros"] = v.Codigo;
            return v;
        }

        public ActionResult ConsultaVale(string idVale) {
            Vale v = _consultarVale(idVale);
            return View("ConsultaVale", v);
        }

        //public ActionResult ConsultaValeCliente(int? idCliente) { 

        //}

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
    }
}
