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

namespace AutomotoraWeb.Controllers.Sistema {
    public class PerfilesController : SistemaController, IMaintenance {

        public static string CONTROLLER = "Perfiles";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            ViewBag.NombreEntidad = "Perfil";
            ViewBag.NombreEntidades = "Perfiles";
        }

        public ActionResult Show() {
            return View(_listaElementos());
        }

        public ActionResult ListaGrilla() {
            return PartialView("_listGrilla", _listaElementos());
        }


        //--------------------------------------    REPORT    ----------------------------------------------
        public ActionResult Report() {
            return View();
        }

        public ActionResult ReportPartial() {
            XtraReport rep = new DXReportPerfiles();
            rep.DataSource = _listaElementos();
            ViewData["Report"] = rep;
            return PartialView("_reportList");
        }

        public ActionResult ReportExport() {
            XtraReport rep = new DXReportPerfiles();
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
            if (TempData["MensajeCreacion"] != null) {
                ViewBag.MensajeCreacion = TempData["MensajeCreacion"].ToString();
            }
            return VistaElemento(id);
        }

        public ActionResult Delete(int id) {
            ViewBag.SoloLectura = true;
            return VistaElemento(id);
        }

        //-----------------------------------------------------------------------------------------------------


        private ActionResult VistaElemento(int id) {
            try {
                PerfilModel td = _obtenerElemento(id);
                return View(td);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private PerfilModel _obtenerElemento(int id) {
            Perfil td = new Perfil();
            td.Codigo = id;
            td.Consultar();
            PerfilModel um = new PerfilModel();
            um.Perfil = td;
            return um;
        }

        private List<Perfil> _listaElementos() {
            return Perfil.Perfiles();
        }

        //-----------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Create(PerfilModel td) {

            if (ModelState.IsValid) {
                try {
                    string userName = (string)HttpContext.Session.Contents[SessionUtils.SESSION_USER_NAME];
                    string IP = HttpContext.Request.UserHostAddress;
                    td.Perfil.setearAuditoria(userName, IP);
                    td.Perfil.Agregar();

                    return RedirectToAction("Details", new { id = td.Perfil.Codigo });
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
        public ActionResult Edit(PerfilModel td) {
            if (ModelState.IsValid) {
                try {
                    string userName = (string)HttpContext.Session.Contents[SessionUtils.SESSION_USER_NAME];
                    string IP = HttpContext.Request.UserHostAddress;

                    td.Perfil.Usuarios = new List<Usuario>();
                    if (!string.IsNullOrWhiteSpace(td.UsuariosTexto)) {
                        string[] ss = td.UsuariosTexto.Split('|');
                        foreach (string s in ss) {
                            Usuario p = new Usuario();
                            p.Codigo = Int32.Parse(s);
                            td.Perfil.Usuarios.Add(p);
                        }
                    }
                    td.Perfil.setearAuditoria(userName, IP);
                    td.Perfil.ModificarDatos();
                    return RedirectToAction(BaseController.DETAILS, new { id = td.Perfil.Codigo });
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(td);
                }
            }

            return View(td);
        }



        [HttpPost]
        public ActionResult Delete(PerfilModel td) {
            ViewBag.SoloLectura = true;
            if (ModelState.IsValid) {
                try {
                    string userName = (string)HttpContext.Session.Contents[SessionUtils.SESSION_USER_NAME];
                    string IP = HttpContext.Request.UserHostAddress;
                    td.Perfil.setearAuditoria(userName, IP);
                    td.Perfil.Eliminar(userName, IP);
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
