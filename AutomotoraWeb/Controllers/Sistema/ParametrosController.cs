using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DLL_Backend;
using AutomotoraWeb.Utils;
using AutomotoraWeb.Models;

namespace AutomotoraWeb.Controllers.Sistema
{
    public class ParametrosController : SistemaController  {

        public static string CONTROLLER = "Parametros";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
        }

        private List<ParametroModel> ListaParametros() {
            List<ParametroModel> lista = new List<ParametroModel>();
            foreach (var p in Automotora.ParametrosModificables()) {
                ParametroModel par = new ParametroModel {
                    Codigo = p.Codigo, Descripcion = p.Descripcion, Valor = p.Valor};
                lista.Add(par);
            }
            return lista;
        }

        public ActionResult Edit() {
            return View(ListaParametros());
        }

        public ActionResult grillaParametros() {
            return PartialView("_grillaParametros", ListaParametros());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult grillaParametros_UpdateRow(ParametroModel model) {
            try {
                string nomUsuario = (string)HttpContext.Session.Contents[SessionUtils.SESSION_USER_NAME];
                string origen = HttpContext.Request.UserHostAddress;
                Parametro par = new Parametro();
                par.Codigo = model.Codigo;
                par.Valor = model.Valor;
                par.setearAuditoria(nomUsuario, origen);
                Automotora.ParametroModificarValor(par);
            } catch (UsuarioException e){
                ModelState.AddModelError("Valor", e.Message);
                ViewData["EditError"] = e.Message;
            } catch (Exception e) {
                ViewData["EditError"] = e.Message;
            }
            return PartialView("_grillaParametros", ListaParametros());
        }
    }
}
