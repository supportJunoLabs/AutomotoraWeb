﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutomotoraWeb.Models;
using DLL_Backend;
using AutomotoraWeb.Utils;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;

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


        //--------------------------METODOS PARA LISTADOS DE pedidos  -----------------------------

        #region Listados

        public ActionResult List() {

            ListadoPedidosModel model = new ListadoPedidosModel();
            string s = SessionUtils.generarIdVarSesion("ListadoPedidos", Session[SessionUtils.SESSION_USER].ToString());
            Session[s] = model;
            model.idParametros = s;
            ViewBag.SucursalesListado = Sucursal.Sucursales;
            ViewBag.Clientes = Cliente.Clientes();
            ViewBag.Vendedores = Vendedor.Vendedores(Vendedor.VEND_TIPO_LISTADO.TODOS);
            ViewData["idParametros"] = model.idParametros;
            model.Resultado = _listaElementos(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult List(ListadoPedidosModel model, string btnSubmit) {
            Session[model.idParametros] = model; //filtros actualizados
            ViewData["idParametros"] = model.idParametros;
            ViewBag.SucursalesListado = Sucursal.Sucursales;
            ViewBag.Clientes = Cliente.Clientes();
            ViewBag.Vendedores = Vendedor.Vendedores(Vendedor.VEND_TIPO_LISTADO.TODOS);
            this.eliminarValidacionesIgnorables("Filtro.Sucursal", MetadataManager.IgnorablesDDL(model.Filtro.Sucursal));
            this.eliminarValidacionesIgnorables("Filtro.Cliente", MetadataManager.IgnorablesDDL(model.Filtro.Cliente));
            this.eliminarValidacionesIgnorables("Filtro.Vendedor", MetadataManager.IgnorablesDDL(model.Filtro.Vendedor));
            if (ModelState.IsValid) {
                if (btnSubmit == "Imprimir") {
                    return this.Report(model);
                }
                model.Resultado = _listaElementos(model);
            }
            return View(model);
        }

        public ActionResult ReportGrilla(string idParametros) {
            ListadoPedidosModel model = (ListadoPedidosModel)Session[idParametros];
            model.Resultado = _listaElementos(model);
            ViewData["idParametros"] = model.idParametros;
            return PartialView("_reportGrilla", model);
        }

        private List<Pedido> _listaElementos(ListadoPedidosModel model) {
            model.AcomodarFiltro();
            return Pedido.Pedidos(model.Filtro);
        }

        #endregion

        //---------- METODOS PARA REPORTES DE LISTADOS DE Pedidos  -----------------------------

        #region Reportes
        public ActionResult Report(ListadoPedidosModel model) {
            return View("report", model);
        }

        public ActionResult ReportPartial(string idParametros) {
            ListadoPedidosModel model = null;
            XtraReport rep = new DXListadoPedidos();
            model = (ListadoPedidosModel)Session[idParametros];
            rep.DataSource = _listaElementos(model);
            setParamsToReport(rep, model); // lo hago despues porque listaElementos acomoda los filtros en model
            Session[idParametros] = model;
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reportList");
        }

        private void setParamsToReport(XtraReport report, ListadoPedidosModel model) {
            Parameter paramSystemName = new Parameter();
            paramSystemName.Name = "detalleFiltros";
            paramSystemName.Type = typeof(string);
            paramSystemName.Value = model.detallesFiltro();
            paramSystemName.Description = "Detalle Filtros";
            paramSystemName.Visible = false;
            report.Parameters.Add(paramSystemName);
        }

        public ActionResult ReportExport(string idParametros) {
            ListadoPedidosModel model = null;
            XtraReport rep = new DXListadoPedidos();
            model = (ListadoPedidosModel)Session[idParametros];
            setParamsToReport(rep, model);
            rep.DataSource = _listaElementos(model);
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);

        }
        #endregion



    }
}
