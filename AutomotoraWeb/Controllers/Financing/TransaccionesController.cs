using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DLL_Backend;

namespace AutomotoraWeb.Controllers.Financing
{
    public class TransaccionesController : FinancingController
    {

        public static string BCONTROLLER = "Financing";
        public static string CONTROLLER = "Transacciones";

        public override string getParentControllerName() {
            return BCONTROLLER;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
        }

        public ActionResult ConsultaTransaccion(int id)
        {
            Recibo recibo = new Recibo();
            recibo.Numero = id;
            recibo.Consultar();

            return Redirect("/"+recibo.DestinoConsulta());
        }

    }
}
