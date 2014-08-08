using System;
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
    public class AcvsController : SalesController
    {

        public static string CONTROLLER = "Acvs";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            ViewBag.NombreEntidad = "A cuenta de venta futura";
            ViewBag.NombreEntidades = "A cuenta de venta futura";
            
        }

        public ActionResult Details(int id) {
            ViewBag.SoloLectura = true;
            return VistaElemento(id);
        }

        private ActionResult VistaElemento(int id) {
            try {
                ACuentaVenta td = _obtenerElemento(id);
                return View(td);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private ACuentaVenta _obtenerElemento(int id) {
            ACuentaVenta td = new ACuentaVenta();
            td.Codigo = id;
            td.Consultar();
            return td;
        }


        #region Listados

        public ActionResult List() {

            ListadoAcvsModel model = new ListadoAcvsModel();
            try{
            string s = SessionUtils.generarIdVarSesion("ListadoACVs", Session[SessionUtils.SESSION_USER].ToString());
            Session[s] = model;
            model.idParametros = s;
            ViewBag.SucursalesListado = Sucursal.Sucursales;
            ViewBag.ClientesListado = Cliente.Clientes();
            ViewBag.VendedoresListado = Vendedor.Vendedores(Vendedor.VEND_TIPO_LISTADO.TODOS);
            ViewData["idParametros"] = model.idParametros;
            model.Resultado = _listaElementos(model);
            return View(model);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(model);
            }
        }

        public ActionResult ListActivosVehiculo(int id) {
            ListadoAcvsModel model = new ListadoAcvsModel();
            try{
            string s = SessionUtils.generarIdVarSesion("ListadoACVs", Session[SessionUtils.SESSION_USER].ToString());
            Session[s] = model;
            model.idParametros = s;
            ViewBag.SucursalesListado = Sucursal.Sucursales;
            ViewBag.ClientesListado = Cliente.Clientes();
            ViewBag.VendedoresListado = Vendedor.Vendedores(Vendedor.VEND_TIPO_LISTADO.TODOS);
            ViewData["idParametros"] = model.idParametros;
            model.AcomodarFiltroActivosVehiculo(id);
            model.Resultado = _listaElementos(model);
            return View("List", model);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(model);
            }
        }


        [HttpPost]
        public ActionResult List(ListadoAcvsModel model, string btnSubmit) {
            try{
            Session[model.idParametros] = model; //filtros actualizados
            ViewData["idParametros"] = model.idParametros;
            ViewBag.SucursalesListado = Sucursal.Sucursales;
            ViewBag.ClientesListado = Cliente.Clientes();
            ViewBag.VendedoresListado = Vendedor.Vendedores(Vendedor.VEND_TIPO_LISTADO.TODOS);
            this.eliminarValidacionesIgnorables("Filtro.Sucursal", MetadataManager.IgnorablesDDL(model.Filtro.Sucursal));
            this.eliminarValidacionesIgnorables("Filtro.Cliente", MetadataManager.IgnorablesDDL(model.Filtro.Cliente));
            this.eliminarValidacionesIgnorables("Filtro.Vendedor", MetadataManager.IgnorablesDDL(model.Filtro.Vendedor));
            
            //Revisar el codigo de vehiculo ingresado manualmente:
            if (string.IsNullOrEmpty(model.CodigoVhc)) {
                model.Filtro.Vehiculo = null;
            } else {
                if (model.CodigoVhc.Trim() == "0") {
                    model.Filtro.Vehiculo = null;
                } else {
                    try {
                        model.Filtro.Vehiculo = new Vehiculo();
                        model.Filtro.Vehiculo.Codigo = Int32.Parse(model.CodigoVhc);
                    } catch (FormatException) {
                        this.ModelState.AddModelError("CodigoVhc", "El codigo de vehiculo debe ser un numero entero positivo");
                    }
                }
            }
            if (ModelState.IsValid) {
                if (btnSubmit == "Imprimir") {
                    return this.Report(model);
                }
                model.Resultado = _listaElementos(model);
            }
            return View(model);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(model);
            }
        }

        public ActionResult ReportGrilla(string idParametros) {
            ListadoAcvsModel model = (ListadoAcvsModel)Session[idParametros];
            model.Resultado = _listaElementos(model);
            ViewData["idParametros"] = model.idParametros;
            return PartialView("_reportGrilla", model);
        }

        private List<ACuentaVenta> _listaElementos(ListadoAcvsModel model) {
            model.AcomodarFiltro();
            return ACuentaVenta.ACuentaVentas(model.Filtro);
        }

        #endregion

        #region Reportes
        public ActionResult Report(ListadoAcvsModel model) {
            return View("report", model);
        }

        private XtraReport _generarReporte(string idParametros) {
            ListadoAcvsModel model = (ListadoAcvsModel)Session[idParametros];
            model.obtenerListado();
            XtraReport rep = new DXListadoAcvs(); //Falta cambiar por el reporte verdadero
            rep.DataSource = model.Resultado;
            setParamsToReport(rep, model);
            return rep;
        }

        public ActionResult ReportPartial(string idParametros) {
            XtraReport rep = _generarReporte(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reportList");
        }

        private void setParamsToReport(XtraReport report, ListadoAcvsModel model) {
            Parameter paramSystemName = new Parameter();
            paramSystemName.Name = "detalleFiltros";
            paramSystemName.Type = typeof(string);
            paramSystemName.Value = model.detallesFiltro();
            paramSystemName.Description = "Detalle Filtros";
            paramSystemName.Visible = false;
            report.Parameters.Add(paramSystemName);
        }

        public ActionResult ReportExport(string idParametros) {
            XtraReport rep = _generarReporte(idParametros);
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);

        }
        #endregion



    }
}
