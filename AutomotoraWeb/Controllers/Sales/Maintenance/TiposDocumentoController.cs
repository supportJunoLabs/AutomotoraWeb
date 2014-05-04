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


        public ActionResult Show([ModelBinder(typeof(DevExpressEditorsBinder))] TipoDocumento td) {
            return View(_listaTiposDocumento());
        }

        public ActionResult listTiposDocumento() {
            return PartialView("_listTiposDocumento", _listaTiposDocumento());
        }


        //--------------------------------------    REPORT    ----------------------------------------------
        public ActionResult Report() {
            // Add a report to the view data. 
            DXReportTiposDocumento rep = new DXReportTiposDocumento();
            ViewData["Report"] = rep;
            return View();
        }

        public ActionResult ReportPartial() {
            DXReportTiposDocumento rep = new DXReportTiposDocumento();
            rep.DataSource = _listaTiposDocumento();
            ViewData["Report"] = rep;
            return PartialView("_reportList");
        }

        public ActionResult ReportExport() {
            DXReportTiposDocumento rep = new DXReportTiposDocumento();
            //setParamsToReport(rep);
            rep.DataSource = _listaTiposDocumento();
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

       //--------------------------------------------------------------------------------------------------

        public ActionResult Details(int id) {
            ViewBag.SoloLectura = true;
            return getTipoDocumento(id);
        }

        public ActionResult Create() {
            return View();
        }

        public ActionResult Edit(int id) {
            return getTipoDocumento(id);
        }

        public ActionResult Delete(int id) {
            ViewBag.SoloLectura = true;
            return getTipoDocumento(id);
        }

        //-----------------------------------------------------------------------------------------------------


        private ActionResult getTipoDocumento(int id) {
            try {
                TipoDocumento td = _getTipoDocumento(id);
                return View(td);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private TipoDocumento _getTipoDocumento(int id) {
            TipoDocumento td = new TipoDocumento();
            td.Codigo = id;
            td.Consultar();
            return td;
        }

        private List<TipoDocumento> _listaTiposDocumento() {
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
