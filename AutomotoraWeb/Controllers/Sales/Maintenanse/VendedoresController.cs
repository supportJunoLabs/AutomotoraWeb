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
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using System.IO;
using System.Text;

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

        //--------------------------------------------------------------------------------------------------
        //--------------------------------------    REPORT    ----------------------------------------------
        //--------------------------------------------------------------------------------------------------

        public ActionResult Report() {
            // Add a report to the view data. 
            DXReportVendedores rep = new DXReportVendedores();
            setParamsToReport(rep);
            ViewData["Report"] = rep;
            return View();
        }

        public ActionResult ReportPartial() {
            DXReportVendedores rep = new DXReportVendedores();
            setParamsToReport(rep);
            rep.DataSource = Vendedor.Vendedores(Vendedor.VEND_TIPO_LISTADO.TODOS);
            ViewData["Report"] = rep;
            return PartialView("_reportList");
        }

        public ActionResult ReportExport() {
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(new DXReportVendedores());
        }

        private void setParamsToReport(XtraReport report) {
            Parameter paramSystemName = new Parameter();
            paramSystemName.Name = "SystemName";
            paramSystemName.Type = typeof(string);
            paramSystemName.Value = (string)(HttpContext.Application.Contents[SessionUtils.APPLICATION_SYSTEM_NAME]);
            paramSystemName.Description = "Nombre de la empresa";
            paramSystemName.Visible = false;
            report.Parameters.Add(paramSystemName);

            Parameter paramCompanyName = new Parameter();
            paramCompanyName.Name = "CompanyName";
            paramCompanyName.Type = typeof(string);
            paramCompanyName.Value = (string)(HttpContext.Application.Contents[SessionUtils.APPLICATION_COMPANY_NAME]);
            paramCompanyName.Description = "Nombre de la compania";
            paramCompanyName.Visible = false;
            report.Parameters.Add(paramCompanyName);
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

        public void SaveFileStream(Stream stream, string destPath) {
            using (var fileStream = new FileStream(destPath, FileMode.Create, FileAccess.Write)) {
                stream.CopyTo(fileStream);
            }
        }

        [HttpPost]
        public JsonResult Upload() {

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            SaveFileStream(this.HttpContext.Request.InputStream, baseDirectory + "Content/Images/tmp/prueba.png");
            return Json(new { nombreArchivo = "prueba.png" });
        }

        //-----------------------------------------------------------------------------------------------------
    }
}
