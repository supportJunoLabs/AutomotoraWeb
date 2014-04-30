using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DLL_Backend;
using AutomotoraWeb.Services;

namespace AutomotoraWeb.Controllers.Configuracion
{
    public class EmpresaController : ConfiguracionController
    {

        public static string CONTROLLER = "empresa";

        public ActionResult Edit() {
            return getEmpresa();
        }

        private ActionResult getEmpresa() {
            try {
                Empresa emp = CompanyService.Empresa();
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
                    CompanyService.actualizarDatos(empresa);

                    //TODO:  aca cambiar por la redireccion al index del ultimo modulo utilizado antes de acceder a esta opcion.
                    return RedirectToAction(ConfiguracionController.INDEX, ConfiguracionController.CONFIG);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(empresa);
                }
            }

            return View(empresa);
        }

     }
}
