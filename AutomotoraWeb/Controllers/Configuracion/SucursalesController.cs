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

namespace AutomotoraWeb.Controllers.Configuracion
{
    public class SucursalesController : ConfiguracionController, IMaintenance {

        public static string CONTROLLER = "sucursales";


        public ActionResult Show([ModelBinder(typeof(DevExpressEditorsBinder))] Sucursal Sucursal) {
            return View(_listaSucursales());
        }

        public ActionResult listSucursales() {
            return PartialView("_listSucursales", _listaSucursales());
        }


        //--------------------------------------    REPORT    ----------------------------------------------
        //public ActionResult Report() {
        //    // Add a report to the view data. 
        //    DXReportSucursales rep = new DXReportSucursales();
        //    setParamsToReport(rep);
        //    ViewData["Report"] = rep;
        //    return View();
        //}

        //public ActionResult ReportPartial() {
        //    DXReportSucursales rep = new DXReportSucursales();
        //    setParamsToReport(rep);
        //    rep.DataSource = _listaSucursales();
        //    ViewData["Report"] = rep;
        //    return PartialView("_reportList");
        //}

        //public ActionResult ReportExport() {
        //    return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(new DXReportSucursales());
        //}

        //private void setParamsToReport(XtraReport report) {
        //    Parameter paramSystemName = new Parameter();
        //    paramSystemName.Name = "SystemName";
        //    paramSystemName.Type = typeof(string);
        //    paramSystemName.Value = (string)(HttpContext.Application.Contents[SessionUtils.APPLICATION_SYSTEM_NAME]);
        //    paramSystemName.Description = "Nombre de la empresa";
        //    paramSystemName.Visible = false;
        //    report.Parameters.Add(paramSystemName);

        //    Parameter paramCompanyName = new Parameter();
        //    paramCompanyName.Name = "CompanyName";
        //    paramCompanyName.Type = typeof(string);
        //    paramCompanyName.Value = (string)(HttpContext.Application.Contents[SessionUtils.APPLICATION_COMPANY_NAME]);
        //    paramCompanyName.Description = "Nombre de la compania";
        //    paramCompanyName.Visible = false;
        //    report.Parameters.Add(paramCompanyName);
        //}

        //--------------------------------------------------------------------------------------------------

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
                Sucursal sucursal = _getSucursal(id);
                return View(sucursal);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private Sucursal _getSucursal(int id) {
            Sucursal sucursal = new Sucursal();
            sucursal.Codigo = id;
            sucursal.Consultar();
            return sucursal;
        }

        private List<Sucursal> _listaSucursales() {
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
