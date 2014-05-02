using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DLL_Backend;
using AutomotoraWeb.Services;
using System.Configuration;

namespace AutomotoraWeb.Controllers.Sistema
{
    public class EmpresaController : SistemaController
    {

        public static string CONTROLLER = "empresa";

        public ActionResult Edit() {
            return getEmpresa();
        }

        private ActionResult getEmpresa() {
            try {
                //por las dudas que haya habido cambios en la base, lo tomamos de la base para este mantenimiento
                //Empresa emp = CompanyService.Empresa();

                Empresa emp = new Empresa();
                emp.Codigo = CompanyService.CodigoEmpresaActiva();
                emp.Consultar();
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
                    CompanyService.actualizarDatos(empresa); //para actualizar los datos del singleton.

                    //TODO:  aca cambiar por la redireccion al index del ultimo modulo utilizado antes de acceder a esta opcion.
                    return RedirectToAction(SistemaController.INDEX, SistemaController.BCONTROLLER);
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
