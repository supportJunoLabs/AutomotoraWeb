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

namespace AutomotoraWeb.Controllers.Bank
{
    public class CuentasBancariasController : BankController, IMaintenance {

        public static string CONTROLLER = "CuentasBancarias";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            ViewBag.Monedas = Moneda.Monedas;
            ViewBag.NombreEntidad = "Cuenta Bancaria";
            ViewBag.NombreEntidades = "Cuentas Bancarias";
            base.OnActionExecuting(filterContext);
        }

        public ActionResult Show([ModelBinder(typeof(DevExpressEditorsBinder))] CuentaBancaria td) {
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
            DXReportCuentasBancarias rep = new DXReportCuentasBancarias();
            rep.DataSource = _listaElementos();
            ViewData["Report"] = rep;
            return PartialView("_reportList");
        }

        public ActionResult ReportExport() {
            DXReportCuentasBancarias rep = new DXReportCuentasBancarias();
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
            return View();
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
                CuentaBancaria td = _obtenerElemento(id);
                return View(td);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private CuentaBancaria _obtenerElemento(int id) {
            CuentaBancaria td = new CuentaBancaria();
            td.Codigo = id;
            td.Consultar();
            return td;
        }

        private List<CuentaBancaria> _listaElementos() {
            return CuentaBancaria.CuentasBancarias;
        }

        //-----------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Create(CuentaBancaria td) {

            this.eliminarValidacionesIgnorables("Moneda", MetadataManager.IgnorablesDDL(new Moneda()));

            if (ModelState.IsValid) {
                try {
                    td.Moneda.Consultar();
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
        public ActionResult Edit(CuentaBancaria td) {

            this.eliminarValidacionesIgnorables("Moneda", MetadataManager.IgnorablesDDL(new Moneda()));

            if (ModelState.IsValid) {
                try {
                    td.Moneda.Consultar();
                    td.ModificarDatos();
                    return RedirectToAction(BaseController.SHOW);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(td);
                }
            }

            return View(td);
        }



        [HttpPost]
        public ActionResult Delete(CuentaBancaria td) {
            ViewBag.SoloLectura = true;
            this.eliminarValidacionesIgnorables("Moneda", MetadataManager.IgnorablesDDL(new Moneda()));

            if (ModelState.IsValid) {
                try {
                    string userName = getUserName();
                    string IP = getIP();
                    td.Moneda.Consultar();
                    td.Eliminar(userName, IP);
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
    }
}
