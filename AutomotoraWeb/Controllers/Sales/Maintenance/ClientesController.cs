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
    public class ClientesController : SalesController, IMaintenance {

        public static string CONTROLLER = "clientes";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            ViewBag.EstadosCiviles = EstadoCivil.EstadosCiviles();
            ViewBag.TiposOtroDoc = Cliente.TiposOtrosDocumentos();
            base.OnActionExecuting(filterContext);
        }

        public ActionResult Show([ModelBinder(typeof(DevExpressEditorsBinder))] Cliente cliente) {
            return View(_listaClientes());
        }

        public ActionResult listClientes() {
            return PartialView("_listClientes", _listaClientes());
        }


        //--------------------------------------    REPORT    ----------------------------------------------
        //public ActionResult Report() {
        //    // Add a report to the view data. 
        //    DXReportClientes rep = new DXReportClientes();
        //    setParamsToReport(rep);
        //    ViewData["Report"] = rep;
        //    return View();
        //}

        //public ActionResult ReportPartial() {
        //    DXReportClientes rep = new DXReportClientes();
        //    setParamsToReport(rep);
        //    rep.DataSource = _listaClientes();
        //    ViewData["Report"] = rep;
        //    return PartialView("_reportList");
        //}

        //public ActionResult ReportExport() {
        //    return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(new DXReportClientes());
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
            ViewBag.SoloLectura = true;
            return getCliente(id);
        }

        public ActionResult Create() {

            return View();
        }

        public ActionResult Edit(int id) {
            return getCliente(id);
        }

        public ActionResult Delete(int id) {
            ViewBag.SoloLectura = true;
            return getCliente(id);
        }

        //-----------------------------------------------------------------------------------------------------


        private ActionResult getCliente(int id) {
            try {
                Cliente cliente = _getCliente(id);
                return View(cliente);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private Cliente _getCliente(int id) {
            Cliente cliente = new Cliente();
            cliente.Codigo = id;
            cliente.Consultar();
            return cliente;
        }

        private List<Cliente> _listaClientes() {
            return Cliente.Clientes();
        }

        //-----------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Create(Cliente cliente) {

            if (ModelState.IsValid) {
                try {
                    if (!cliente.Ecivil.RequiereDatosConyuge()) {
                        cliente.Conyuge = null;
                    }
                    cliente.Agregar();
                    return RedirectToAction(BaseController.SHOW);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(cliente);
                }
            }

            return View(cliente);
        }

        //-----------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Edit(Cliente cliente) {
            if (ModelState.IsValid) {
                try {
                    if (!cliente.Ecivil.RequiereDatosConyuge()) {
                        cliente.Conyuge = null;
                    }
                    cliente.ModificarDatos();
                    return RedirectToAction(BaseController.SHOW);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(cliente);
                }
            }

            return View(cliente);
        }



        [HttpPost]
        public ActionResult Delete(Cliente cliente) {
            ViewBag.SoloLectura = true;

            //if (ModelState.IsValid) { //no se validan los campos porque no estan fisicamente los textboxes para cargar el cliente.
            
            try {
                string userName = (string)HttpContext.Session.Contents[SessionUtils.SESSION_USER_NAME];
                string IP = HttpContext.Request.UserHostAddress;
                cliente.Eliminar(userName, IP);
                return RedirectToAction(BaseController.SHOW);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(cliente);
                //  }
            }

            return View(cliente);
        }

        //-----------------------------------------------------------------------------------------------------

        public JsonResult mostrarDatosConyuge(string codEcivil) {
            EstadoCivil ec = new EstadoCivil();
            ec.Codigo = codEcivil;
            bool resp = ec.RequiereDatosConyuge();
            return this.Json(new { mostrar = resp }, JsonRequestBehavior.AllowGet);
        }
    }
}
