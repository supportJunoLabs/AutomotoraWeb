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
    public class UsuariosController : SistemaController, IMaintenance {

        public static string CONTROLLER = "Usuarios";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            ViewBag.NombreEntidad = "Usuario";
            ViewBag.NombreEntidades = "Usuarios";
            ViewBag.Sucursales = Sucursal.Sucursales;

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
            XtraReport rep = new DXReportUsuarios();
            rep.DataSource = _listaElementos();
            ViewData["Report"] = rep;
            return PartialView("_reportList");
        }

        public ActionResult ReportExport() {
            XtraReport rep = new DXReportUsuarios();
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
                UsuarioModel td = _obtenerElemento(id);
                return View(td);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private UsuarioModel _obtenerElemento(int id) {
            Usuario td = new Usuario();
            td.Codigo = id;
            td.Consultar(Usuario.MODO_CONSULTA.AVANZADO);
            UsuarioModel um = new UsuarioModel();
            um.Usuario = td;
            return um;
        }

        private List<Usuario> _listaElementos() {
            return Usuario.Usuarios();
        }

        //-----------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Create(UsuarioModel td) {
            this.eliminarValidacionesIgnorables("Usuario.Sucursal", MetadataManager.IgnorablesDDL(td.Usuario.Sucursal));

            if (ModelState.IsValid) {
                try {
                    string userName = (string)HttpContext.Session.Contents[SessionUtils.SESSION_USER_NAME];
                    string IP = HttpContext.Request.UserHostAddress;
                    Usuario u = new Usuario();
                    u.UserName = userName;
                    td.Usuario.setearAuditoria(userName, IP);

                    string clave0 = td.Usuario.Agregar(u);

                    TempData["MensajeCreacion"] = "Recuerde asignarle perfiles. La clave inicial del usuario es: " + clave0;
                    return RedirectToAction("Edit", new { id = td.Usuario.Codigo });
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
        public ActionResult Edit(UsuarioModel td) {
            this.eliminarValidacionesIgnorables("Usuario.Sucursal", MetadataManager.IgnorablesDDL(td.Usuario.Sucursal));
            if (ModelState.IsValid) {
                try {
                    string userName = (string)HttpContext.Session.Contents[SessionUtils.SESSION_USER_NAME];
                    string IP = HttpContext.Request.UserHostAddress;

                    Usuario u = new Usuario();
                    u.UserName = userName;

                    td.Usuario.Perfiles = new List<Perfil>();
                    if (!string.IsNullOrWhiteSpace(td.PerfilesTexto)) {
                        string[] ss = td.PerfilesTexto.Split('|');
                        foreach (string s in ss) {
                            Perfil p = new Perfil();
                            p.Codigo = Int32.Parse(s);
                            td.Usuario.Perfiles.Add(p);
                        }
                    }
                    td.Usuario.setearAuditoria(userName, IP);
                    td.Usuario.ModificarDatos(u);
                    return RedirectToAction(BaseController.DETAILS, new { id = td.Usuario.Codigo });
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(td);
                }
            }

            return View(td);
        }



        [HttpPost]
        public ActionResult Delete(UsuarioModel td) {
            ViewBag.SoloLectura = true;
            this.eliminarValidacionesIgnorables("Usuario.Sucursal", MetadataManager.IgnorablesDDL(td.Usuario.Sucursal));
            if (ModelState.IsValid) {
                try {
                    string userName = (string)HttpContext.Session.Contents[SessionUtils.SESSION_USER_NAME];
                    string IP = HttpContext.Request.UserHostAddress;
                    td.Usuario.setearAuditoria(userName, IP);
                    td.Usuario.Eliminar(userName, IP);
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

        #region resetPassword

        public ActionResult ResetPassword() {
            ResetPasswordModel m = new ResetPasswordModel();
            m.UsuarioActual=(string)HttpContext.Session.Contents[SessionUtils.SESSION_USER_NAME];
            return View(m);
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordModel model) {
            string IP = HttpContext.Request.UserHostAddress;

            if (ModelState.IsValid) {
                try {
                    Usuario u = new Usuario();
                    u.UserName = model.UsuarioReset;
                    u.Consultar(Usuario.MODO_CONSULTA.BASICO);

                    Usuario a = new Usuario();
                    a.UserName = model.UsuarioActual;
                    a.Consultar(Usuario.MODO_CONSULTA.BASICO);
                    a.Clave = model.ClaveUsuarioActual;

                    u.ResetearClave(IP, a, model.ClaveUsuarioReset, model.VerificacionClaveUsuarioReset);

                    return RedirectToAction("Mensaje", SistemaController.CONTROLLER, new { id = SistemaController.MSJ_RESET_CLAVE_OK });

                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(model);
                }
            }
            return View(model);
        }

        #endregion
    }
}
