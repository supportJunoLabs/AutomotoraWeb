using AutomotoraWeb.Controllers.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutomotoraWeb.Models;

namespace AutomotoraWeb.Controllers.Sistema {
    public class SistemaController : BaseController {

        public static string BCONTROLLER = "sistema";
        public static string CONTROLLER = "sistema";
        public static string INDEX = "index";  

        public ActionResult Index() {
            return View();
        }

        public override string getParentControllerName() {
            return BCONTROLLER;
        }

        //protected override void setearUltimoModulo() {
        //    //para este controller que no es ningun modulo, dejo el original.
        //}

        public const int MSJ_CAMBIO_CLAVE_OK = 1;
        public const int MSJ_ERROR = 2;
        public const int MSJ_RESET_CLAVE_OK = 3;
        public const int MSJ_DESBLOQUEAR_USUARIO_OK = 4;

        public ActionResult Mensaje(int id) {
            Mensaje msj = new Mensaje{Titulo="", Contenido=""};

            switch (id) {
                case MSJ_CAMBIO_CLAVE_OK:
                    msj.Titulo = "Cambio de Clave";
                    msj.Contenido = "Ha cambiado su clave de acceso exitosamente";
                    break;
                case MSJ_ERROR:
                    msj.Titulo = "Error";
                    msj.Contenido = "Se ha producido un error";
                    break;
                case MSJ_RESET_CLAVE_OK:
                    msj.Titulo = "Reset Clave Usuario";
                    msj.Contenido = "Se ha reseteado la clave de este usuario exitosamente";
                    break;
                case MSJ_DESBLOQUEAR_USUARIO_OK:
                    msj.Titulo = "Desbloquear Usuario";
                    msj.Contenido = "Se ha desbloqueado este usuario exitosamente";
                    break;
            }

            return View(msj);
        }
    }
}
