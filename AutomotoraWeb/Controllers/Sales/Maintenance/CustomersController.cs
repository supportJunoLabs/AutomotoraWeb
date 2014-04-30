using AutomotoraWeb.Controllers.General;
using AutomotoraWeb.Helpers;
using AutomotoraWeb.Models;
using AutomotoraWeb.Services;
using DevExpress.Web.Mvc;
using DLL_Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomotoraWeb.Controllers.Sales.Maintenance {
    public class CustomersController : SalesController, IMaintenance {

        public static string CUSTOMERS = "customers";

        public ActionResult Show([ModelBinder(typeof(DevExpressEditorsBinder))] CustomerModel customer) {
            return View(SalesService.Instance.listCustomers());
        }

        public ActionResult listCustomers() {
            return PartialView("_listCustomers", SalesService.Instance.listCustomers());
        }

        public ActionResult ExportarExcel() {
            return GridViewExtension.ExportToXlsx(CreateExportGridViewSettings(), SalesService.Instance.listCustomers());
        }

        public ActionResult ExportarPDF() {
            return GridViewExtension.ExportToPdf(CreateExportGridViewSettings(), SalesService.Instance.listCustomers());
        }

        static GridViewSettings CreateExportGridViewSettings() {
            //Application.Contents[SessionUtils.APPLICATION_COMPANY_NAME];
            //Application.Contents[SessionUtils.APPLICATION_SYSTEM_NAME];

            GridViewSettings settings = new GridViewSettings();
            return settings;
        }

        public ActionResult Details(int id) {
            return getCustomer(id);
        }

        public ActionResult Create() {
            return View();
        }

        public ActionResult Edit(int id) {
            return getCustomer(id);
        }

        public ActionResult Delete(int id) {
            return getCustomer(id);
        }

        //-----------------------------------------------------------------------------------------------------

        private ActionResult getCustomer(int id) {
            try {
                ViewBag.listSelectListItemCustomerMaritalStatus = HtmlHelpers.getListSelectListItemCustomerMaritalStatus();

                CustomerModel CustomerModel = SalesService.Instance.getCustomer(id);
                return View(CustomerModel);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        //-----------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Create(CustomerModel customer) {

            if (ModelState.IsValid) {
                try {
                    SalesService.Instance.createCustomer(customer);
                    return RedirectToAction(BaseController.SHOW, CUSTOMERS);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(customer);
                }
            }

            return View(customer);
        }

        //-----------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Edit(CustomerModel customer) {
            if (ModelState.IsValid) {
                try {
                    SalesService.Instance.updateCustomer(customer);
                    return RedirectToAction(BaseController.SHOW, CUSTOMERS);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(customer);
                }
            }

            return View(customer);
        }

        //-----------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Delete(CustomerModel customer) {
            if (ModelState.IsValid) {
                try {
                    SalesService.Instance.deleteCustomer(customer);
                    return RedirectToAction(BaseController.SHOW, CUSTOMERS);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(customer);
                }
            }

            return View(customer);
        }

        //-----------------------------------------------------------------------------------------------------

        [HttpPost]
        public JsonResult nameAvailable(string name, int id) {
            CustomerModel customer = new CustomerModel() { Name = name, Id = id };
            bool result = SalesService.Instance.existCustomer(customer);
            return Json(result);
        }
    }
}
