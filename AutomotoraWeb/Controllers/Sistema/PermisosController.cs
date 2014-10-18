using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DLL_Backend;
using AutomotoraWeb.Models;
using AutomotoraWeb.Utils;

namespace AutomotoraWeb.Controllers.Sistema {
    public class PermisosController : SistemaController {
        public static string CONTROLLER = "Permisos";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            ViewBag.Perfiles = Perfil.Perfiles();
        }

         [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Edit(int? id) {
            string s = SessionUtils.generarIdVarSesion("Permisos", getUserName());
            PermisosModel model = new PermisosModel();
            model.Perfil = new Perfil();
            if (id != null) {
                model.Perfil.Codigo = id ?? 0;
            }
            model.ListaOpciones = OpcionMenuEstructura.ObtenerEstructura(model.Perfil);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(PermisosModel model) {
            try {

                //por si no vino perfil
                if (model.Perfil == null || model.Perfil.Codigo <= 0) {
                    model.ListaOpciones = OpcionMenuEstructura.ObtenerEstructura(model.Perfil);
                    return View(model);
                }

                string userName = getUserName();
                string IP = getIP();
                List<OpcionMenu> todas = OpcionMenu.opcionesHabilitables();

                List<OpcionMenu> lom = new List<OpcionMenu>();
                string[] ach = model.OpcionesHabilitadasTexto.Split(new Char[] { ',' });
                foreach (string s in ach) {
                    if (!string.IsNullOrWhiteSpace(s)) {
                        OpcionMenu om = new OpcionMenu();
                        om.Codigo = Int32.Parse(s);
                        if (todas.Contains(om)){  //para no tener en cuenta los nodos raiz agregados para agrupar nivel superior en el arbol.
                            om.setearAuditoria(userName, IP);
                            lom.Add(om);
                        }
                    }
                }
                model.Perfil.Consultar();
                OpcionMenu.GuardarOpcionesPerfil(model.Perfil, lom);
                return RedirectToAction("details", "Perfiles", new { id=model.Perfil.Codigo});

            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                model.ListaOpciones = OpcionMenuEstructura.ObtenerEstructura(model.Perfil); //para recuperar los permisos antes del cambio
                return View(model);
            }
        }

        public ActionResult ArbolPermisosPartial() {
            PermisosModel model = new PermisosModel();
            model.Perfil = new Perfil();
            model.Perfil.Codigo = 1; //da igual cualquier perfil porque despues le pasa por arriba con lo manual (cambiar despues)
            model.ListaOpciones = OpcionMenuEstructura.ObtenerEstructura(model.Perfil);
            return PartialView("_arbolPermisos", model);
        }

        //Se invoca por json al actualizar la ddl de clientes, devuelve solo la partial de contenido.
        public ActionResult JsonArbolPermisosPartial(int? idPerfil) {
            PermisosModel model = new PermisosModel();
            model.Perfil = new Perfil();
            model.Perfil.Codigo = idPerfil??0;
            model.ListaOpciones = OpcionMenuEstructura.ObtenerEstructura(model.Perfil);
            return PartialView("_arbolPermisos", model);
        }

    }
}
