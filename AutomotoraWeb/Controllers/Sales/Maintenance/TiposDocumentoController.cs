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

namespace AutomotoraWeb.Controllers.Sales.Maintenance
{
    public class TiposDocumentoController : SalesController, IMaintenance {

        public static string CONTROLLER = "TiposDocumento";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            ViewBag.NombreEntidad = "Tipo Documento";
            ViewBag.NombreEntidades = "TiposDocumento";
           
        }

        //public ContentResult NombreEntidades() {
        //    return new ContentResult { Content = "Tipos Documento" };
        //}

        //public ContentResult NombreEntidad() {
        //    return new ContentResult { Content = "Tipo Documento" };
        //}

        public ActionResult Show([ModelBinder(typeof(DevExpressEditorsBinder))] TipoDocumento td) {
            return View(_listaElementos());
        }

        public ActionResult ListaGrilla() {
            return PartialView("_listGrilla", _listaElementos());
        }


        //--------------------------------------    REPORT    ----------------------------------------------
        public ActionResult Report() {
            // Add a report to the view data. 
            //DXReportTiposDocumento rep = new DXReportTiposDocumento();
            //ViewData["Report"] = rep;
            return View();
        }

        public ActionResult ReportPartial() {
            DXReportTiposDocumento rep = new DXReportTiposDocumento();
            rep.DataSource = _listaElementos();
            ViewData["Report"] = rep;
            return PartialView("_reportList");
        }

        public ActionResult ReportExport() {
            DXReportTiposDocumento rep = new DXReportTiposDocumento();
            //setParamsToReport(rep);
            rep.DataSource = _listaElementos();
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
                TipoDocumento td = _obtenerElemento(id);
                return View(td);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private TipoDocumento _obtenerElemento(int id) {
            TipoDocumento td = new TipoDocumento();
            td.Codigo = id;
            td.Consultar();
            return td;
        }

        private List<TipoDocumento> _listaElementos() {
            return TipoDocumento.TiposDocumento();
        }

        //-----------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Create(TipoDocumento td) {

            if (ModelState.IsValid) {
                try {
                    td.Agregar();
                    return RedirectToAction(BaseController.SHOW);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(td);
                }
            }

            return View(td);
        }

        //-----------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Edit(TipoDocumento td) {
            if (ModelState.IsValid) {
                try {
                    td.ModificarDatos();
                    return RedirectToAction(BaseController.SHOW);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(td);
                }
            }

            return View(td);
        }



        [HttpPost]
        public ActionResult Delete(TipoDocumento td) {
            ViewBag.SoloLectura = true;
            if (ModelState.IsValid) {
                try {
                    string userName = (string)HttpContext.Session.Contents[SessionUtils.SESSION_USER_NAME];
                    string IP = HttpContext.Request.UserHostAddress;
                    td.Eliminar(userName, IP);
                    return RedirectToAction(BaseController.SHOW);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(td);
                }
            }

            return View(td);
        }
        //-----------------------------------------------------------------------------------------------------
    }
}
