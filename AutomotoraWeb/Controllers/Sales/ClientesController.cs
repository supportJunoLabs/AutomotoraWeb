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

namespace AutomotoraWeb.Controllers.Sales {
    public class ClientesController : SalesController, IMaintenance {

        public static string CONTROLLER = "clientes";

      
        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            ViewBag.EstadosCiviles = EstadoCivil.EstadosCiviles();
            ViewBag.TiposOtroDoc = Cliente.TiposOtrosDocumentos();
            ViewBag.NombreEntidad = "Cliente";
            ViewBag.NombreEntidades = "Clientes";
            ViewBag.Clientes = Cliente.Clientes();
            ViewBag.Financistas = Financista.FinancistasTodos;
            
        }

        public ActionResult Show([ModelBinder(typeof(DevExpressEditorsBinder))] Cliente cliente) {
            return View(_listaElementos());
        }

        public ActionResult ListaGrilla() {
            return PartialView("_listGrilla", _listaElementos());
        }

        //----------reporte ficha cliente -----------------------------
        public ActionResult Report2(int idCliente) {
            Cliente cli = new Cliente();
            cli.Codigo = idCliente;
            // Add a report to the view data. 
            //Cliente cli = _obtenerElemento(id);
            //setParamsToReport(rep);
            //DXReportFichaCliente rep = new DXReportFichaCliente();
            //ViewData["Report"] = rep;
            ViewData["idParametros"] = idCliente;
            return View(cli);
        }

        public ActionResult ReportPartial2(int idParametros) {
            DXReportFichaCliente rep = new DXReportFichaCliente();
            //setParamsToReport(rep);
            Cliente cli = _obtenerElemento(idParametros);
            List<Cliente> lista = new List<Cliente>();
            lista.Add(cli);
            rep.DataSource = lista;
            ViewData["Report"] = rep;
            ViewData["idParametros"] = cli.Codigo;
            return PartialView("_reportDetalle");
        }

        public ActionResult ReportExport2(int idParametros) {
            DXReportFichaCliente rep = new DXReportFichaCliente();
            //setParamsToReport(rep);
            Cliente cli = _obtenerElemento(idParametros);
            List<Cliente> lista = new List<Cliente>();
            lista.Add(cli);
            rep.DataSource = lista;
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        //----------reporte listado de clientes  -----------------------------

        public ActionResult Report() {
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
            ViewData["idParametros"] = id;
            return VistaElemento(id);
        }

        public ActionResult GrillaVentasCliente(int idParametros) {
            Cliente c = new Cliente();
            c.Codigo = idParametros;
            ViewData["idParametros"] = idParametros;
            return PartialView("_ventasCliente", c);
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

        //-----------------------------------------------------------------------------------------------------

        //#region SeleccionDeCliente

        //[HttpPost]
        //public JsonResult details(int codigo) {
        //    try {
        //        Cliente cliente = new Cliente();
        //        cliente.Codigo = codigo;
        //        cliente.Consultar();

        //        return Json(new {
        //            Result = "OK",
        //            Cliente = new {
        //                Cedula = cliente.Cedula,
        //                Nombre = cliente.Nombre,
        //                Ciudad = cliente.Ciudad,
        //                Pais = cliente.Pais,
        //                Telefono = cliente.Telefono
        //            }
        //        }
        //        );
        //    } catch (UsuarioException exc) {
        //        return Json(new { Result = "ERROR", ErrorCode = exc.Codigo, ErrorMessage = exc.Message });
        //    }
        //}

        //#endregion

        #region ConsultaSituacionCliente

        private Cliente _consultarCliente(int? idCliente) {
            Cliente c = new Cliente();
            c.Codigo = idCliente ?? 0;
            if (idCliente != null && idCliente != 0) {
                c.Consultar();
            }
            ViewData["idParametros"] = c.Codigo;
            return c;
        }


        //Se invoca desde la url del browser o desde el menu principal, o referencias externas. Devuelve la pagina completa
        public ActionResult SitCliente(int? id) {
            try {
                Cliente c = _consultarCliente(id);
                return View(c);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }


        [HttpPost]
        //se invoca desde el boton actualizar e imprimir. Devuelve la pagina completa
        public ActionResult SitCliente(Cliente model, string btnSubmit) {
            try {
                int? idCliente = model.Codigo;
                Cliente c = _consultarCliente(idCliente);
                if (btnSubmit == "Imprimir" && idCliente != null && idCliente != 0) {
                    return this.ReportSitCliente(c);
                }
                return View(c);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(model);
            }
        }

        //Se invoca por json al actualizar la ddl de clientes, devuelve solo la partial de contenido.
        public ActionResult SitClientePartial(int? idCliente) {
            Cliente c = _consultarCliente(idCliente);
            SituacionCliente sit = new SituacionCliente();
            sit.generarSituacion(c);
            return PartialView("_sitCliente", sit);
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de cuotas. Devuelve la partial del tab de cuotas
        public ActionResult GrillaCuotasCliente(int? idParametros) {
            Cliente c = _consultarCliente(idParametros);
            SituacionCliente sit = new SituacionCliente();
            sit.generarSituacion(c);
            return PartialView("_listGrillaSitClienteCuotas", sit);
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de vales. Devuelve la partial del tab de vales
        public ActionResult GrillaValesCliente(int? idParametros) {
            Cliente c = _consultarCliente(idParametros);
            SituacionCliente sit = new SituacionCliente();
            sit.generarSituacion(c);
            return PartialView("_listGrillaSitClienteVales", sit);
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de cheques. Devuelve la partial del tab de cheques
        public ActionResult GrillaChequesCliente(int? idParametros) {
            Cliente c = _consultarCliente(idParametros);
            SituacionCliente sit = new SituacionCliente();
            sit.generarSituacion(c);
            return PartialView("_listGrillaSitClienteCheques", sit);
        }

        public ActionResult ReportSitCliente(Cliente c) {
            return View("ReportSitCliente", c);
        }

        private SituacionCliente _obtenerDatos(Cliente c) {
            SituacionCliente model = new SituacionCliente();
            model.Cliente = c;
            c.Consultar();
            return model;
        }

        public ActionResult ReportSitClientePartial(int idParametros) {
            Cliente c = _consultarCliente(idParametros);
            SituacionCliente model = new SituacionCliente();
            model.generarSituacion(c);
            List<SituacionCliente> ll = new List<SituacionCliente>();
            ll.Add(model);
            XtraReport rep = new DXSituacionCliente();
            rep.DataSource = ll;
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reportSitCliente");
        }

        public ActionResult ReportSitClienteExport(int idParametros) {
            Cliente c = _consultarCliente(idParametros);
            SituacionCliente model = new SituacionCliente();
            model.generarSituacion(c);

            List<SituacionCliente> ll = new List<SituacionCliente>();
            ll.Add(model);

            XtraReport rep = new DXSituacionCliente();
            rep.DataSource = ll;
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        #endregion

    }
}
