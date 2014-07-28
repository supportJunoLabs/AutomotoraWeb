using AutomotoraWeb.Controllers.General;
using AutomotoraWeb.Utils;
using DevExpress.Web.Mvc;
using DLL_Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomotoraWeb.Controllers.Sales
{
    public class GastosController : SalesController
    {

        public static string CONTROLLER = "gastos";

        public ActionResult listGastos(int id) {
            ViewData["idParametros"] = id;
            return PartialView("_listGastos", _listaGastos(id));
        }

        private List<Gasto> _listaGastos(int id) {
            Vehiculo vehiculo = new Vehiculo();
            vehiculo.Codigo = id;
            vehiculo.Consultar();
            return vehiculo.DetalleGastos;
        }

    }
}
