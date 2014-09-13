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
using DevExpress.Web.ASPxGridView;

namespace AutomotoraWeb.Controllers.Sales
{
    public class VentasController : SalesController
    {

        public static string CONTROLLER = "Ventas";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            ViewBag.NombreEntidad = "Venta";
            ViewBag.NombreEntidades = "Ventas";
            ViewBag.Clientes = Cliente.Clientes();
            ViewBag.VendedoresHabilitados = Vendedor.Vendedores(Vendedor.VEND_TIPO_LISTADO.HABILITADOS);
            
        }

        public ActionResult Index(){
            return View();
        }

        public ActionResult VentaVehiculo() {
            
            Venta venta = new Venta();
            venta.Vehiculo = new Vehiculo();
            venta.Cliente = new Cliente();
            venta.Vendedor = new Vendedor();
            venta.Sucursal = new Sucursal();
            venta.Pago.AgregarEfectivos(new List<Efectivo>());

            prepararSession(venta); 

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

            prepararSession(venta); 

            return View("ventaVehiculo", venta);
        }

        [HttpPost]
        public JsonResult finalizarVenta(string idSession, int codigoVehiculo, int condVentaCodigoCliente, int condVentaCodigoVendedor, 
                                         DateTime condVentaFecha, int condVentaCodigoMoneda, double condVentaMonto, int condVentaCodigoSucursal, 
                                         bool condVentaVehiculoEntregado, DateTime condVentaFechaEntrega, String condVentaObservaciones) {

            try {
                Venta venta = (Venta)(Session[idSession + SessionUtils.VENTA]);

                IEnumerable<Cheque> listPagosCheque = (IEnumerable<Cheque>)(Session[idSession + SessionUtils.CHEQUES]);
                IEnumerable<Efectivo> listPagosEfectivo = (IEnumerable<Efectivo>)(Session[idSession + SessionUtils.EFECTIVO]);
                IEnumerable<MovBanco> listPagosBanco = (IEnumerable<MovBanco>)(Session[idSession + SessionUtils.MOV_BANCARIO]);
                IEnumerable<Vale> listPagosVale = (IEnumerable<Vale>)(Session[idSession + SessionUtils.VALES]);
                IEnumerable<Cuota> listPagosCuota = (IEnumerable<Cuota>)(Session[idSession + SessionUtils.CUOTAS]);

                venta.Vehiculo = new Vehiculo();
                venta.Vehiculo.Codigo = codigoVehiculo;

                venta.Sucursal = new Sucursal();
                venta.Sucursal.Codigo = condVentaCodigoSucursal;

                venta.Cliente = new Cliente();
                venta.Cliente.Codigo = condVentaCodigoCliente;

                venta.Vendedor = new Vendedor();
                venta.Vendedor.Codigo = condVentaCodigoVendedor;

                string nomUsuario = Session[SessionUtils.SESSION_USER_NAME].ToString();
                string origen = HttpContext.Request.UserHostAddress;
                venta.setearAuditoria(nomUsuario, origen);

                venta.Fecha = condVentaFecha;
                Moneda moneda = new Moneda();
                moneda.Codigo = condVentaCodigoMoneda;
                Importe importe = new Importe(moneda, condVentaMonto);
                venta.Importe = importe;
                venta.Entregado = condVentaVehiculoEntregado;
                if (!condVentaVehiculoEntregado) {
                    venta.FechaEntrega = condVentaFechaEntrega;
                }
                venta.Observaciones = condVentaObservaciones;

                //--------------------------------------------------

                //foreach (Cheque cheque in listPagosCheque) {
                //    venta.Pago.agregarCheque(cheque);
                //}
                //foreach (Efectivo efectivo in listPagosEfectivo) {
                //    venta.Pago.AgregarEfectivo(efectivo);
                //}
                //foreach (MovBanco movBanco in listPagosBanco) {
                //    venta.Pago.AgregarMovBanco(movBanco);
                //}

                foreach (Vale vale in listPagosVale) {
                    venta.AgregarValeOriginal(vale);
                }
                foreach (Cuota cuota in listPagosCuota) {
                    venta.Financiacion.AgregarCuotaVenta(cuota);
                }

                // TODO: Falta verificar que se cubra total a pagar

                venta.Ejecutar();

                return Json(new { Result = "OK" });
            } catch (UsuarioException exc) {
                List<String> errores = new List<string>();
                errores.Add(exc.Message);
                return Json(new { Result = "ERROR", ErrorCode = exc.Codigo, ErrorMessage = errores.ToArray() });
            } catch (Exception exc) {
                List<String> errores = new List<string>();
                errores.Add(exc.Message);
                return Json(new { Result = "ERROR", ErrorCode = "0", ErrorMessage = errores.ToArray() });
            }
        }

        private void prepararSession(Venta venta) {
            ViewBag.Sucursales = Sucursal.Sucursales;
            ViewBag.Controller = CONTROLLER;

            string idSession = SessionUtils.generarIdVarSesion("VentaVehiculo", Session[SessionUtils.SESSION_USER].ToString()) + "|";
            Session[idSession + SessionUtils.VENTA] = venta;
            ViewData["idSession"] = idSession;
            ViewData["idParametros"] = venta.Codigo; // Para grillas que necesitan id de la venta

            List<Vale> listVale = null;

            if (venta.Vehiculo.Codigo != 0) {
                PrecondicionesVenta precondicionesVenta = venta.Vehiculo.ObtenerPrecondicionesVenta();
                listVale = precondicionesVenta.Vales;
            } else {
                listVale = new List<Vale>();
            }

            List<Cuota> listCuota = new List<Cuota>();

            Session[idSession + SessionUtils.CHEQUES] = venta.Pago.Cheques;
            Session[idSession + SessionUtils.EFECTIVO] = venta.Pago.Efectivos;
            Session[idSession + SessionUtils.MOV_BANCARIO] = venta.Pago.PagosBanco;
            Session[idSession + SessionUtils.VALES] = listVale;
            Session[idSession + SessionUtils.CUOTAS] = listCuota;

            ViewBag.Vales = listVale;
            ViewBag.Cuotas = listCuota;
        }


        //----------------- Seleccion vehiculo ----------------------------------------------------
        public ActionResult VehiculoElegido(int id) {
            Vehiculo v = new Vehiculo();
            v.Codigo = id;
            v.Consultar();
            return PartialView("_seleccionDeVehiculosDetalle", v);
        }

        //--------------------- Consulta venta -----------------------------------------------------

        public ActionResult Details(int id) {
            ViewBag.SoloLectura = true;
            return VistaElemento(id);
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

        //----------------------------------------------------------------------------------------

        
        [HttpPost]
        public JsonResult cambiarFinanciacion(int cantCuotas, double tasa, Importe importe, string idSession) {
            //List<Cuota> listCuota = (List<Cuota>)(Session[idSession + SessionUtils.CUOTAS]);

            //validacion del controller
            List<String> errors = this.validateAtributesFinanciacion(cantCuotas, tasa, importe);
            if (errors.Count > 0) {
                return Json(new { Result = "ERROR", ErrorCode = "VALIDATION_ERROR", ErrorMessage = errors.ToArray() });
            }

            //envio al backend
            try {
                //listCuota.Add(cuota);

                Financiacion f = new Financiacion(importe, cantCuotas, tasa, "");
                f.generarCuotasVenta(DateTime.Now);
                List<Cuota> listCuota = new List<Cuota>();
                foreach (Cuota cuota in f.CuotasOriginales) {
                    listCuota.Add(cuota);
                }
                Session[idSession + SessionUtils.CUOTAS] = listCuota;

                return Json(new { Result = "OK"  });
            } catch (UsuarioException exc) {
                List<String> errores1 = new List<string>();
                errores1.Add(exc.Message);
                return Json(new { Result = "ERROR", ErrorCode = exc.Codigo, ErrorMessage = errores1.ToArray() });
            }

        }


        private List<String> validateAtributesFinanciacion(int cantCuotas, double tasa, Importe importe) {

            List<String> errors = new List<String>();

            if (tasa == null) {
                errors.Add("El campo Tasa es obligatorio");
            }

            if (cantCuotas <= 0) {
                errors.Add("El campo Número es obligatorio, y debe ser mayor a 0");
            }

            if ((importe == null) || (importe.Monto <= 0)) {
                errors.Add("El campo Monto es obligatorio, y debe ser mayor a 0");
            }

            if ((importe == null) || (importe.Moneda == null) || (importe.Moneda.Codigo == 0)) {
                errors.Add("El campo Moneda es obligatorio");
            }

            return errors;
        }

        //----------------------------------------------------------------------------------------

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

        #region AnulacionVenta

        //----------------- Trasnsaccion Anulacion  FALTA -----------------------

        //----------------- Recibo Anulacion -----------------------
        
        public ActionResult ReciboAnulacion(int id) {
            try {
                ViewData["idParametros"] = id;
                return View("ReciboAnulacion");
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private XtraReport _generarReciboAnulacion(int id) {
            TRVentaAnulacion tr = (TRVentaAnulacion)Transaccion.ObtenerTransaccion(id);
            List<TRVentaAnulacion> ll = new List<TRVentaAnulacion>();
            ll.Add(tr);
            XtraReport rep = new DXReciboVentaAnulacion();
            rep.DataSource = ll;
            return rep;
        }

        public ActionResult ReciboAnulacionPartial(int idParametros) {
            XtraReport rep = _generarReciboAnulacion(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reciboAnulacion");
        }

        public ActionResult ReciboAnulacionExport(int idParametros) {
            XtraReport rep = _generarReciboAnulacion(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }



        #endregion


        #region EntregaVehiculo

        //----------------- Trasnsaccion Entrega FALTA  -----------------------

        //----------------- Recibo Entrega  -----------------------

        public ActionResult ReciboEntrega(int id) {
            try {
                ViewData["idParametros"] = id;
                return View("ReciboEntrega");
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private XtraReport _generarReciboEntrega(int id) {
            Entrega tr = (Entrega)Transaccion.ObtenerTransaccion(id);
            List<Entrega> ll = new List<Entrega>();
            ll.Add(tr);
            XtraReport rep = new DXReciboVentaEntrega();
            rep.DataSource = ll;
            return rep;
        }

        public ActionResult ReciboEntregaPartial(int idParametros) {
            XtraReport rep = _generarReciboEntrega(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reciboEntrega");
        }

        public ActionResult ReciboEntregaExport(int idParametros) {
            XtraReport rep = _generarReciboEntrega(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }



        #endregion

    }
}
