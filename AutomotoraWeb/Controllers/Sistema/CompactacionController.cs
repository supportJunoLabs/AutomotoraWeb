using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DLL_Backend;

namespace AutomotoraWeb.Controllers.Sistema
{
    public class CompactacionController : SistemaController
    {
        public static string CONTROLLER = "Compactacion";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            //ViewBag.NombreEntidad = "Usuario";
            //ViewBag.NombreEntidades = "Usuarios";
            //ViewBag.Sucursales = Sucursal.Sucursales;
        }

        public ActionResult Compactar() {
            return View();
        }

        [HttpPost]
        public JsonResult EjecutarCompactacion() {
            try {
                string userName = getUserName();
                string IP = getIP();
                Automotora.SoloInfoVigente(userName, IP);

                string s = "Proceso finalizado correctamente";
                return Json(new { Result = "OK", Mensaje = s });
            } catch (UsuarioException exc) {
                string s = "Se ha producido un ERROR: "+exc.Message;
                return Json(new { Result = "ERROR", s});
            }
        }

    }
}
