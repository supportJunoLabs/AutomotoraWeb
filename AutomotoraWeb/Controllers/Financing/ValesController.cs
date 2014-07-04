using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DLL_Backend;

namespace AutomotoraWeb.Controllers.Financing
{
    public class ValesController : FinancingController
    {
        public static string CONTROLLER = "Vales";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            //ViewBag.Clientes = Cliente.Clientes();
            //ViewBag.Financistas = Financista.Financistas(Financista.FIN_TIPO_LISTADO.TODOS);
        }
        #region ConsultaVales

        private Vale _consultarVale(string idVale) {
            Vale v = new Vale();
            if (idVale != null && idVale.Trim() != "") {
                v.Codigo = idVale;
                v.Consultar();
            }
            ViewData["idParametros"] = v.Codigo;
            return v;
        }

        public ActionResult ConsultaVale(string idVale) {
            Vale v = _consultarVale(idVale);
            return View("ConsultaVale", v);
        }

        //public ActionResult ConsultaValeCliente(int? idCliente) { 

        //}

        #endregion
    }
}
