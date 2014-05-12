using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutomotoraWeb.Models;
using DLL_Backend;

namespace AutomotoraWeb.Controllers.Sales
{
    public class PedidosController : SalesController
    {

        public static string CONTROLLER = "Pedidos";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            ViewBag.NombreEntidad = "Pedido";
            ViewBag.NombreEntidades = "Pedidos";
            
        }

        public ActionResult Index(){
            return View();
        }

        public ActionResult List() {
            ListadoPedidosModel model = new ListadoPedidosModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult List( ListadoPedidosModel lf ) {
            lf.Resultado = _listaElementos(lf.Filtro);
            return View(lf);
        }

        public ActionResult ReportGrilla(ListadoPedidosModel model ) {
            model.Resultado=_listaElementos(model.Filtro);
            return PartialView("_reportGrilla", model);
        }

        private List<Pedido> _listaElementos(PedidoFiltro pf){
            return Pedido.Pedidos(pf);
        }
    }
}
