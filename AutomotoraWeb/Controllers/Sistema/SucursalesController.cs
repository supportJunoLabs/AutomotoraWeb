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

namespace AutomotoraWeb.Controllers.Sistema
{
    public class SucursalesController : SistemaController, IMaintenance {

        public static string CONTROLLER = "sucursales";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            ViewBag.NombreEntidad = "Sucursal";
            ViewBag.NombreEntidades = "Sucursales";
            
        }

        public ActionResult Show([ModelBinder(typeof(DevExpressEditorsBinder))] Sucursal Sucursal) {
            return View(_listaElementos());
        }

        public ActionResult ListaGrilla() {
            return PartialView("_listGrilla", _listaElementos());
        }


        //--------------------------------------    REPORT    ----------------------------------------------
        public ActionResult Report() {
            // Add a report to the view data. 
            //DXReportSucursales rep = new DXReportSucursales();
            ////setParamsToReport(rep);
            //ViewData["Report"] = rep;
            return View();
        }

        public ActionResult ReportPartial() {
            DXReportSucursales rep = new DXReportSucursales();
            //setParamsToReport(rep);
            rep.DataSource = _listaElementos();
            ViewData["Report"] = rep;
            return PartialView("_reportList");
        }

        public ActionResult ReportExport() {
            DXReportSucursales rep = new DXReportSucursales();
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
                Sucursal sucursal = _obtenerElemento(id);
                return View(sucursal);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private Sucursal _obtenerElemento(int id) {
            Sucursal sucursal = new Sucursal();
            sucursal.Codigo = id;
            sucursal.Consultar();
            return sucursal;
        }

        private List<Sucursal> _listaElementos() {
            return Sucursal.Sucursales();
        }

        //-----------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Create(Sucursal sucursal) {

            if (ModelState.IsValid) {
                try {
                    sucursal.Agregar();
                    return RedirectToAction(BaseController.SHOW);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(sucursal);
                }
            }

            return View(sucursal);
        }

        //-----------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Edit(Sucursal sucursal) {
            if (ModelState.IsValid) {
                try {
                    sucursal.ModificarDatos();
                    return RedirectToAction(BaseController.SHOW);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(sucursal);
                }
            }

            return View(sucursal);
        }



        [HttpPost]
        public ActionResult Delete(Sucursal sucursal) {
            ViewBag.SoloLectura = true;
            if (ModelState.IsValid) {
                try {
                    string userName = (string)HttpContext.Session.Contents[SessionUtils.SESSION_USER_NAME];
                    string IP = HttpContext.Request.UserHostAddress;
                    sucursal.Eliminar(userName, IP);
                    return RedirectToAction(BaseController.SHOW);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(sucursal);
                }
            }

            return View(sucursal);
        }
        //-----------------------------------------------------------------------------------------------------
    }
}
