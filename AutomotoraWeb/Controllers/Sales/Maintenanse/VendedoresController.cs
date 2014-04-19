using AutomotoraWeb.Controllers.General;
using AutomotoraWeb.Models;
using AutomotoraWeb.Services;
using AutomotoraWeb.Utils;
using DevExpress.Web.Mvc;
using DevExpress.XtraPrinting;
using DLL_Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace AutomotoraWeb.Controllers.Sales.Maintenanse {
    public class VendedoresController : SalesController, IMaintenance {

        public static string CONTROLLER = "vendedores";
        public static string CONCRETE_LIST = "listVendedores";
        public static string KEY_FIELD_NAME = "codigo";

        public ActionResult Show([ModelBinder(typeof(DevExpressEditorsBinder))] Vendedor vendedor) {
            return View(SalesService.Instance.listVendedores());
        }

        public ActionResult listVendedores() {
            return PartialView("_listVendedores", SalesService.Instance.listVendedores());
        }

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
            settings.Name = "SGA-Vendedores";
            settings.CallbackRouteValues = new { Controller = CONTROLLER, Action = CONCRETE_LIST };
            settings.Width = Unit.Percentage(100);
            settings.Columns.Add("Codigo").Visible = false; ;
            settings.Columns.Add("Nombre");
            settings.Columns.Add("Direccion");
            settings.Columns.Add("Telefono");
            settings.Columns.Add("FechaIngreso");
            settings.Columns.Add("Observaciones");
            settings.Columns.Add("Habilitado");
            settings.SettingsExport.PageHeader.Left = systemName + " - " + companyName;
            settings.SettingsExport.PageHeader.Right = "Vendedores";
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
            ViewData["Report"] = new DXReportVendedores(); //new DXWebApplication1.Reports.XtraReport1();

            return View();
        }

        public ActionResult ReportPartial() {
            ViewData["Report"] = new DXReportVendedores();
            return PartialView("_reportList");
        }

        public ActionResult ReportExport() {
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(new DXReportVendedores());
        }

        //--------------------------------------------------------------------------------------------------

        public ActionResult Details(int id) {
            return getVendedor(id);
        }

        public ActionResult Create() {
            return View();
        }

        public ActionResult Edit(int id) {
            return getVendedor(id);
        }

        public ActionResult Delete(int id) {
            return getVendedor(id);
        }

        //-----------------------------------------------------------------------------------------------------

        private ActionResult getVendedor(int id) {
            try {
                Vendedor vendedor = SalesService.Instance.getVendedor(id);
                return View(vendedor);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            } 
        }

        //-----------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Create(Vendedor vendedor) {

            if (ModelState.IsValid) {
                try {
                    SalesService.Instance.createVendedor(vendedor);
                    return RedirectToAction(BaseController.SHOW, CONTROLLER);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(vendedor);
                } 
            }

            return View(vendedor);
        }

        //-----------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Edit(Vendedor vendedor) {
            if (ModelState.IsValid) {
                try {
                    SalesService.Instance.updateVendedor(vendedor);
                    return RedirectToAction(BaseController.SHOW, CONTROLLER);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(vendedor);
                } 
            }

            return View(vendedor);
        }

        

        [HttpPost]
        public ActionResult Delete(Vendedor vendedor) {
            if (ModelState.IsValid) {
                try {
                    string userName = (string)HttpContext.Session.Contents[SessionUtils.SESSION_USER_NAME];
                    string IP = HttpContext.Request.UserHostAddress;
                    SalesService.Instance.deleteVendedor(vendedor, userName, IP);
                    return RedirectToAction(BaseController.SHOW, CONTROLLER);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(vendedor);
                }
            }

            return View(vendedor);
        }

        //-----------------------------------------------------------------------------------------------------

    }
}
