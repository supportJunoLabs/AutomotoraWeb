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

        public ActionResult Show([ModelBinder(typeof(DevExpressEditorsBinder))] Sucursal sucursal) {
            List<Sucursal> listSucursal = SalesService.Instance.listSucursales();
            return View(listSucursal);
        }

        public ActionResult listSucursales() {
            List<Sucursal> listSucursal = SalesService.Instance.listSucursales();
            return PartialView("_listSucursales", listSucursal);
        }

        public ActionResult ExportarExcel() {
            return GridViewExtension.ExportToXlsx(CreateExportGridViewSettings(), SalesService.Instance.listSucursales());
        }

        public ActionResult ExportarPDF() {
            return GridViewExtension.ExportToPdf(CreateExportGridViewSettings(), SalesService.Instance.listSucursales());
        }

        static GridViewSettings CreateExportGridViewSettings() {
            //Application.Contents[SessionUtils.APPLICATION_COMPANY_NAME];
            //Application.Contents[SessionUtils.APPLICATION_SYSTEM_NAME];

            GridViewSettings settings = new GridViewSettings();
            settings.Name = "SGA-Vendedores";
            settings.CallbackRouteValues = new { Controller = "sellers", Action = "listSellers" };
            settings.Width = Unit.Percentage(100);
            settings.Columns.Add("Id").Visible = false; ;
            settings.Columns.Add("Name");
            settings.Columns.Add("Address");
            settings.Columns.Add("Telephone");
            settings.Columns.Add("IngressDate");
            settings.Columns.Add("Observations");
            settings.Columns.Add("Enabled");
            settings.SettingsExport.PageHeader.Left = "SGA - Kilometro Cero";
            settings.SettingsExport.PageHeader.Right = "Vendedores";
            settings.SettingsExport.PageFooter.Left = DateTime.Now.ToString();
            settings.SettingsExport.PageFooter.Right = "Usuario: " + "mariel";
            settings.SettingsExport.RenderBrick = (sender, e) => { e.BrickStyle.BorderWidth = 0; };
            return settings;
        }

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
                    string nomUsuario = (string)HttpContext.Session.Contents[SessionUtils.SESSION_USER_NAME];
                    string origen = HttpContext.Request.UserHostAddress;
                    SalesService.Instance.deleteSucursal(sucursal, nomUsuario, origen);
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
