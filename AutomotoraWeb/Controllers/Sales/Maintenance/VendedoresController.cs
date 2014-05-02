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

namespace AutomotoraWeb.Controllers.Sales.Maintenance {
    public class VendedoresController : SalesController, IMaintenance {

        public static string CONTROLLER = "vendedores";

        public ActionResult Show([ModelBinder(typeof(DevExpressEditorsBinder))] Vendedor vendedor) {
            return View(_listaVendedores());
        }

        public ActionResult listVendedores() {
            return PartialView("_listVendedores", _listaVendedores());
        }

    
        //--------------------------------------    REPORT    ----------------------------------------------
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
            rep.DataSource = _listaVendedores();
            ViewData["Report"] = rep;
            return PartialView("_reportList");
        }

        public ActionResult ReportExport() {
            DXReportVendedores rep = new DXReportVendedores();
            setParamsToReport(rep);
            rep.DataSource = _listaVendedores();
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
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
                Vendedor vendedor = _getVendedor(id);
                return View(vendedor);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            } 
        }

        private Vendedor _getVendedor(int id) {
            Vendedor vendedor = new Vendedor();
            vendedor.Codigo = id;
            vendedor.Consultar();
            return vendedor;
        }

        private List<Vendedor> _listaVendedores() {
            return Vendedor.Vendedores(Vendedor.VEND_TIPO_LISTADO.TODOS);
        }

        //-----------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Create(Vendedor vendedor) {

            if (ModelState.IsValid) {
                try {
                    vendedor.Agregar();
                    return RedirectToAction(BaseController.SHOW);
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
                    vendedor.ModificarDatos();
                    return RedirectToAction(BaseController.SHOW);
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
                    vendedor.Eliminar(userName, IP);
                    return RedirectToAction(BaseController.SHOW);
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
