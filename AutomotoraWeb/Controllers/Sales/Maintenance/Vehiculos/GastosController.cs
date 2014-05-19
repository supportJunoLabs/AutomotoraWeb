using AutomotoraWeb.Controllers.General;
using AutomotoraWeb.Utils;
using DevExpress.Web.Mvc;
using DLL_Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomotoraWeb.Controllers.Sales.Maintenance.Vehiculos
{
    public class GastosController : SalesController
    {

        public static string CONTROLLER = "gastos";

        public ActionResult Show([ModelBinder(typeof(DevExpressEditorsBinder))] Gasto gasto) {
            return View(listGastos(gasto));
        }

        public ActionResult listGastos(Gasto gasto) {
            return PartialView("_listGastos", _listaGastos());
        }

        private List<Gasto> _listaGastos() {
            int code = (int)Session[SessionUtils.CODIGO_VEHICULO];
            Vehiculo vehiculo = new Vehiculo();
            vehiculo.Codigo = code;
            vehiculo.Consultar();
            return vehiculo.DetalleGastos;
        }

    }
}
