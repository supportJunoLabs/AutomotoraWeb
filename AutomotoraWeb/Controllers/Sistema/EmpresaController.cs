using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DLL_Backend;
using AutomotoraWeb.Services;
using System.Configuration;
using AutomotoraWeb.Controllers.General;
using AutomotoraWeb.Utils;



namespace AutomotoraWeb.Controllers.Sistema {
    public class EmpresaController : SistemaController {

        public static string CONTROLLER = "empresa";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            ViewBag.NombreEntidad = "Datos Empresa";
            ViewBag.NombreEntidades = "Empresas";
            
        }

        public ActionResult Edit() {
            return VistaElemento();
        }

        public ActionResult Details() {
            ViewBag.SoloLectura = true;
            return VistaElemento();
        }

        private ActionResult VistaElemento() {
            try {
                //por las dudas que haya habido cambios en la base, lo tomamos de la base para este mantenimiento
                Empresa emp = Automotora.GetEmpresaActiva(true);
                return View(emp);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        [HttpPost]
        public ActionResult Edit(Empresa empresa) {
            if (ModelState.IsValid) {
                try {
                    empresa.ModificarDatos();

                    //refrescar el nombre de la empresa a nivel de aplicacion por si hubo cambios desde el mto o por bd
                    HttpContext.Application["AppVar"]=empresa.NomEmpresa;

                    //NO se hace mas desde aca, ahora lo hace el backend en su propio singleton
                    //CompanyService.actualizarDatos(empresa); //para actualizar los datos del singleton.

                    return RedirectToAction(BaseController.DETAILS, new { id=empresa.Codigo});
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(empresa);
                }
            }

            return View(empresa);
        }

        //[HttpGet]
        //public ActionResult Cancelar() {
        //    Destino dest = BaseController.DestinoIndexUltimoModulo(Session[SessionUtils.ULTIMO_MODULO]);
        //    return RedirectToAction(dest.Accion, dest.Controlador);
        //}

    }
}
