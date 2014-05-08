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

        public ContentResult NombreEntidad() {
            return new ContentResult { Content = "Datos Empresa" };
        }

        public ContentResult NombreEntidades() {
            return new ContentResult { Content = "Empresas" };
        }

        public ActionResult Edit() {
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

                    //aca se cambia por la redireccion al index del ultimo modulo utilizado antes de acceder a esta opcion.
                    //return RedirectToAction(SistemaController.INDEX, SistemaController.BCONTROLLER);
                    return IndexUltimoModulo();
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(empresa);
                }
            }

            return View(empresa);
        }

        [HttpGet]
        public ActionResult Cancelar() {
            Destino dest = BaseController.DestinoIndexUltimoModulo(Session[SessionUtils.ULTIMO_MODULO]);
            return RedirectToAction(dest.Accion, dest.Controlador);
        }

    }
}
