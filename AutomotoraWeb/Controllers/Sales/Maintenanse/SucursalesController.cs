using AutomotoraWeb.Controllers.General;
using AutomotoraWeb.Services;
using AutomotoraWeb.Utils;
using DevExpress.Web.Mvc;
using DLL_Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace AutomotoraWeb.Controllers.Sales.Maintenanse
{
    public class SucursalesController : SalesController, IMaintenance {

        public static string SUCURSALES = "sucursales";
        public static string CONTROLLER = "sucursales";
        public static string CONCRETE_LIST = "listSucursales";

        public ActionResult Show([ModelBinder(typeof(DevExpressEditorsBinder))] Sucursal sucursal) {
            List<Sucursal> listSucursal = SalesService.Instance.listSucursales();
            return View(listSucursal);
        }

        public ActionResult listSucursales() {
            List<Sucursal> listSucursal = SalesService.Instance.listSucursales();
            return PartialView("_listSucursales", listSucursal);
        }

        //-----------------------------------------------------------------------------------------------------

        public ActionResult ExportarExcel() {
            string companyName = ViewBag.companyName;
            string systemName = ViewBag.systemName;
            string userName = (string)HttpContext.Session.Contents[SessionUtils.SESSION_USER_NAME];
            return GridViewExtension.ExportToXlsx(CreateExportGridViewSettings(userName, companyName, systemName), SalesService.Instance.listVendedores());
        }

        public ActionResult ExportarPDF() {
            string companyName = ViewBag.companyName;
            string systemName = ViewBag.systemName;
            string userName = (string)HttpContext.Session.Contents[SessionUtils.SESSION_USER_NAME];
            return GridViewExtension.ExportToPdf(CreateExportGridViewSettings(userName, companyName, systemName), SalesService.Instance.listVendedores());
        }

        static GridViewSettings CreateExportGridViewSettings(string userName, string companyName, string systemName) {

            GridViewSettings settings = new GridViewSettings();
            settings.Name = "Sucursales";
            settings.CallbackRouteValues = new { Controller = CONTROLLER, Action = CONCRETE_LIST };
            settings.Width = Unit.Percentage(100);
            settings.Columns.Add("Codigo").Visible = false; ;
            settings.Columns.Add("Nombre");
            settings.Columns.Add("Direccion");
            settings.Columns.Add("Ciudad");
            settings.Columns.Add("CodigoPostal");
            settings.Columns.Add("Email");
            settings.Columns.Add("Fax");
            settings.Columns.Add("Observaciones");
            settings.SettingsExport.PageHeader.Left = systemName + " - " + companyName;
            settings.SettingsExport.PageHeader.Right = "Sucursales";
            settings.SettingsExport.PageFooter.Left = DateTime.Now.ToString();
            settings.SettingsExport.PageFooter.Right = "Usuario: " + userName;
            settings.SettingsExport.RenderBrick = (sender, e) => { e.BrickStyle.BorderWidth = 0; };
            return settings;
        }

        //--------------------------------------------------------------------------------------------------
        //--------------------------------------    REPORT    ----------------------------------------------
        //--------------------------------------------------------------------------------------------------

        public ActionResult Report() {
            // Add a report to the view data. 
            ViewData["Report"] = new DXReportVendedores();  // TODO: cambiar a reporte de sucursales

            return View();
        }

        public ActionResult ReportPartial() {
            ViewData["Report"] = new DXReportVendedores(); // TODO: cambiar a reporte de sucursales
            return PartialView("_reportList");
        }

        public ActionResult ReportExport() {
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(new DXReportVendedores()); // TODO: cambiar a reporte de sucursales
        }

        //--------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------

        public ActionResult Details(int id) {
            return getSucursal(id);
        }

        public ActionResult Create() {
            return View();
        }

        public ActionResult Edit(int id) {
            return getSucursal(id);
        }

        public ActionResult Delete(int id) {
            return getSucursal(id);
        }

        //-----------------------------------------------------------------------------------------------------

        private ActionResult getSucursal(int id) {
            try {
                Sucursal sucursal = SalesService.Instance.getSucursal(id);
                return View(sucursal);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Create(Sucursal sucursal) {

            if (ModelState.IsValid) {
                try {
                    SalesService.Instance.createSucursal(sucursal);
                    return RedirectToAction(BaseController.SHOW, SUCURSALES);
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
                    SalesService.Instance.updateSucursal(sucursal);
                    return RedirectToAction(BaseController.SHOW, SUCURSALES);
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
        public ActionResult Delete(Sucursal sucursal) {
            if (ModelState.IsValid) {
                try {
                    string userName = (string)HttpContext.Session.Contents[SessionUtils.SESSION_USER_NAME];
                    string IP = HttpContext.Request.UserHostAddress;
                    SalesService.Instance.deleteSucursal(sucursal, userName, IP);
                    return RedirectToAction(BaseController.SHOW, SUCURSALES);
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
