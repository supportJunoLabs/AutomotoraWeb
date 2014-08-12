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
    public class VentasController : SalesController
    {

        public static string CONTROLLER = "Ventas";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            ViewBag.NombreEntidad = "Venta";
            ViewBag.NombreEntidades = "Ventas";
            
        }

        public ActionResult Index(){
            return View();
        }


        public ActionResult Details(int id) {
            ViewBag.SoloLectura = true;
            return VistaElemento(id);
        }

        public ActionResult VentaVehiculo() {
            
            Venta venta = new Venta();
            venta.Vehiculo = new Vehiculo();
            venta.Cliente = new Cliente();
            venta.Vendedor = new Vendedor();
            venta.Sucursal = new Sucursal();
            venta.Pago.agregarEfectivos(new List<Importe>());

            ViewData["idParametros"] = "0"; // Para grillas que necesitan id de la venta

            ViewBag.Sucursales = Sucursal.Sucursales;
            ViewBag.Controller = CONTROLLER;

            string idSession = SessionUtils.generarIdVarSesion("VentaVehiculo", Session[SessionUtils.SESSION_USER].ToString()) + "|";
            Session[idSession] = venta;
            ViewData["idSession"] = idSession;

            return View(venta);
        }

        public ActionResult VentaVehiculoX(int id) {
            Venta venta = new Venta();
            venta.Vehiculo = new Vehiculo();
            venta.Vehiculo.Codigo = id;
            venta.Vehiculo.Consultar();
            venta.Consultar();
            venta.Cliente = new Cliente();
            venta.Vendedor = new Vendedor();
            venta.Sucursal = new Sucursal();

            ViewData["idParametros"] = venta.Codigo.ToString(); // Para grillas que necesitan id de la venta

            ViewBag.Sucursales = Sucursal.Sucursales;
            ViewBag.Controller = CONTROLLER;

            string idSession = SessionUtils.generarIdVarSesion("VentaVehiculoX", Session[SessionUtils.SESSION_USER].ToString()) + "|";
            Session[idSession] = venta;
            ViewData["idSession"] = idSession;

            return View("ventaVehiculo", venta);
        }

        private ActionResult VistaElemento(int id) {
            try {
                Venta td = _obtenerElemento(id);
                return View(td);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private Venta _obtenerElemento(int id) {
            Venta td = new Venta();
            td.Codigo = id;
            td.Consultar();
            return td;
        }

        //--------------------------METODOS Pagos Efectivo  -----------------------------

        #region PagosEfectivo

        [HttpPost]
        public JsonResult addPagoEfectivo(Importe importe, String idSession) {

            try {
                Venta venta = (Venta)(Session[idSession]);
                importe.Moneda.Consultar();
                venta.Pago.Efectivo.Add(importe);
                
                return createJsonResultPagoEfectivo(venta.Pago.Efectivo);
            } catch (UsuarioException exc) {
                List<String> errores1 = new List<string>();
                errores1.Add(exc.Message);
                return Json(new { Result = "ERROR", ErrorCode = exc.Codigo, ErrorMessage = errores1.ToArray() });
            }

        }

        private JsonResult createJsonResultPagoEfectivo(List<Importe> listImporte) { // Paso la lista de importes por si hay que devolver el total
            return Json(new {
                Result = "OK",
            });
        }

        public ActionResult grillaPagosEfectivo(string idSession, int idParametros) {
            return PartialView("_grillaPagosEfectivo", _listaPagosEfectivo(idSession));
        }

        private List<Importe> _listaPagosEfectivo(string idSession) {
            Venta venta = (Venta)(Session[idSession]);
            return venta.Pago.Efectivo;
        }

        #endregion

        //--------------------------METODOS PARA LISTADOS DE Ventas  -----------------------------

        #region Listados

        public ActionResult List() {

            ListadoVentasModel model = new ListadoVentasModel();
            string s = SessionUtils.generarIdVarSesion("ListadoVentas", Session[SessionUtils.SESSION_USER].ToString());
            Session[s] = model;
            model.idParametros = s;
            ViewBag.SucursalesListado = Sucursal.Sucursales;
            ViewBag.ClientesListado = Cliente.Clientes();
            ViewBag.VendedoresListado = Vendedor.Vendedores(Vendedor.VEND_TIPO_LISTADO.TODOS);
            ViewBag.TiposComubstiblesListado = TipoCombustible.TiposCombustible();
            ViewData["idParametros"] = model.idParametros;
            model.Resultado = _listaElementos(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult List(ListadoVentasModel model, string btnSubmit) {
            Session[model.idParametros] = model; //filtros actualizados
            ViewData["idParametros"] = model.idParametros;
            ViewBag.SucursalesListado = Sucursal.Sucursales;
            ViewBag.ClientesListado = Cliente.Clientes();
            ViewBag.VendedoresListado = Vendedor.Vendedores(Vendedor.VEND_TIPO_LISTADO.TODOS);
            ViewBag.TiposComubstiblesListado = TipoCombustible.TiposCombustible();
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
            ListadoVentasModel model = (ListadoVentasModel)Session[idParametros];
            model.Resultado = _listaElementos(model);
            ViewData["idParametros"] = model.idParametros;
            return PartialView("_reportGrilla", model);
        }

        private List<Venta> _listaElementos(ListadoVentasModel model) {
            model.AcomodarFiltro();
            return Venta.Ventas(model.Filtro);
        }

        #endregion
        //---------- METODOS PARA REPORTES DE LISTADOS DE Ventas  -----------------------------

        #region Reportes
        public ActionResult Report(ListadoVentasModel model) {
            return View("report", model);
        }

        public ActionResult ReportPartial(string idParametros) {
            ListadoVentasModel model = null;
            XtraReport rep = new DXListadoVentas();
            model = (ListadoVentasModel)Session[idParametros];
            rep.DataSource = _listaElementos(model);
            setParamsToReport(rep, model); // lo hago despues porque listaElementos acomoda los filtros en model
            Session[idParametros] = model;
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reportList");
        }

        private void setParamsToReport(XtraReport report, ListadoVentasModel model) {
            Parameter paramSystemName = new Parameter();
            paramSystemName.Name = "detalleFiltros";
            paramSystemName.Type = typeof(string);
            paramSystemName.Value = model.detallesFiltro();
            paramSystemName.Description = "Detalle Filtros";
            paramSystemName.Visible = false;
            report.Parameters.Add(paramSystemName);
        }

        public ActionResult ReportExport(string idParametros) {
            ListadoVentasModel model = null;
            XtraReport rep = new DXListadoVentas();
            model = (ListadoVentasModel)Session[idParametros];
            setParamsToReport(rep, model);
            rep.DataSource = _listaElementos(model);
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);

        }
        #endregion


    }
}
