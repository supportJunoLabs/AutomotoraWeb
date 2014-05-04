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

        public ContentResult NombreEntidad() {
            return new ContentResult { Content = "Cliente" };
        }

        public ContentResult NombreEntidades() {
            return new ContentResult { Content = "Clientes" };
        }
        
        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            ViewBag.EstadosCiviles = EstadoCivil.EstadosCiviles();
            ViewBag.TiposOtroDoc = Cliente.TiposOtrosDocumentos();
            ViewBag.NombreEntidad = "Clientes";
            base.OnActionExecuting(filterContext);
        }

        public ActionResult Show([ModelBinder(typeof(DevExpressEditorsBinder))] Cliente cliente) {
            return View(_listaElementos());
        }

        public ActionResult ListaGrilla() {
            return PartialView("_listGrilla", _listaElementos());
        }

        //----------reporte ficha cliente -----------------------------
        public ActionResult Report2(int id) {
            // Add a report to the view data. 
            Cliente cli = _obtenerElemento(id);
            //setParamsToReport(rep);
            DXReportFichaCliente rep = new DXReportFichaCliente();
            ViewData["Report"] = rep;
            return View(cli);
        }

        public ActionResult ReportPartial2(int id) {
            DXReportFichaCliente rep = new DXReportFichaCliente();
            //setParamsToReport(rep);
            Cliente cli = _obtenerElemento(id);
            List<Cliente> lista = new List<Cliente>();
            lista.Add(cli);
            rep.DataSource = lista;
            ViewData["Report"] = rep;
            return PartialView("_reportDetalle", cli);
        }

        public ActionResult ReportExport2(int id) {
            DXReportFichaCliente rep = new DXReportFichaCliente();
            //setParamsToReport(rep);
            Cliente cli = _obtenerElemento(id);
            List<Cliente> lista = new List<Cliente>();
            lista.Add(cli);
            rep.DataSource = lista;
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        //----------reporte listado de clientes  -----------------------------

        public ActionResult Report() {
            // Add a report to the view data. 
            DXReportClientes rep = new DXReportClientes();
            //setParamsToReport(rep);
            ViewData["Report"] = rep;
            return View();
        }

        public ActionResult ReportPartial() {
            DXReportClientes rep = new DXReportClientes();
            //setParamsToReport(rep);
            rep.DataSource = _listaElementos();
            ViewData["Report"] = rep;
            return PartialView("_reportList");
        }

        public ActionResult ReportExport() {
            DXReportClientes rep = new DXReportClientes();
            //setParamsToReport(rep);
            rep.DataSource = Cliente.Clientes();
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
                Cliente cliente = _obtenerElemento(id);
                return View(cliente);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private Cliente _obtenerElemento(int id) { //Trae los datos del elemento de la base de datos y los pone en un objeto.
            Cliente cliente = new Cliente();
            cliente.Codigo = id;
            cliente.Consultar();
            return cliente;
        }

        private List<Cliente> _listaElementos() {
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

            if (ModelState.IsValid) {

                try {
                    string userName = (string)HttpContext.Session.Contents[SessionUtils.SESSION_USER_NAME];
                    string IP = HttpContext.Request.UserHostAddress;
                    cliente.Eliminar(userName, IP);
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

        public JsonResult mostrarDatosConyuge(string codEcivil) {
            EstadoCivil ec = new EstadoCivil();
            ec.Codigo = codEcivil;
            bool resp = ec.RequiereDatosConyuge();
            return this.Json(new { mostrar = resp }, JsonRequestBehavior.AllowGet);
        }
    }
}
