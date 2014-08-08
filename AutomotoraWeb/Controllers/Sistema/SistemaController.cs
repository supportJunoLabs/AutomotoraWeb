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

        protected override void setearUltimoModulo() {
            //para este controller que no es ningun modulo, dejo el original.
        }

        public const int MSJ_CAMBIO_CLAVE_OK = 1;

        public ActionResult Mensaje(int id) {
            Mensaje msj = new Mensaje{Titulo="", Contenido=""};

            switch (id) { 
                case 1:
                    msj.Titulo = "Cambio de Clave";
                    msj.Contenido = "Ha cambiado su clave de acceso exitosamente";
                    break;
            }

            return View(msj);
        }
    }
}
