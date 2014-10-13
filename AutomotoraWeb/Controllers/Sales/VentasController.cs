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
using AutomotoraWeb.Controllers.General;
using System.Globalization;

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
            ViewBag.VehiculoSeniado = Boolean.Parse("false");
            ViewBag.Sucursales = Sucursal.Sucursales;
            ViewBag.Departamentos = Departamento.Departamentos();
            ViewBag.TiposCombustible = TipoCombustible.TiposCombustible();

            Usuario usuario = (Usuario)(Session[SessionUtils.SESSION_USER]);
            if (usuario == null) {
                filterContext.Result = new RedirectResult("/" + AuthenticationController.CONTROLLER + "/" + AuthenticationController.LOGIN);
                return;
            }
            ViewBag.MultiSucursal = usuario.MultiSucursal;

        }

        #region VentaVehiculo
        public ActionResult VehiculosVendiblesGrilla(GridLookUpModel model) {
            model.Opciones = Vehiculo.Vehiculos(Vehiculo.VHC_TIPO_LISTADO.VENDIBLES);
            return PartialView("_selectVehiculoVender", model);
        }

        public ActionResult VentaVehiculo(int? id) {
            string idSession = SessionUtils.generarIdVarSesion("ventaVehiculo", Session[SessionUtils.SESSION_USER].ToString()) + "|";
            ViewData["idSession"] = idSession;
            int idVehiculo = id ?? 0;
            Venta v = inicializarVenta(idVehiculo, idSession);
            return View("VentaVehiculo", v);
        }

        private Venta inicializarVenta(int idVehiculo, string idSession){
            Venta venta = new Venta();
            venta.Vehiculo = new Vehiculo();
            venta.Entregado = true;
            venta.FechaEntrega = DateTime.Now.Date;
            Usuario usuario = (Usuario)(Session[SessionUtils.SESSION_USER]);
            venta.Sucursal = usuario.Sucursal;
            if (idVehiculo!= 0) {
                venta.Vehiculo.Codigo = idVehiculo;
                venta.Vehiculo.Consultar();
                PrecondicionesVenta cond =   venta.Vehiculo.ObtenerPrecondicionesVenta(DateTime.Now.Date);
                venta.inicializarSegunPrecondiciones(cond);
            }
            if (venta.Permuta != null) {
                venta.Permuta.Sucursal = usuario.Sucursal;
            }
            Session[idSession + SessionUtils.CHEQUES] = venta.Pago.Cheques;
            Session[idSession + SessionUtils.EFECTIVO] = venta.Pago.Efectivos;
            Session[idSession + SessionUtils.MOV_BANCARIO] = venta.Pago.PagosBanco;
            Session[idSession + SessionUtils.VALES] = venta.ValesOriginales;
            if (venta.Financiacion != null) {
                Session[idSession + SessionUtils.CUOTAS] = venta.Financiacion.CuotasOriginales;
            } else {
                Session[idSession + SessionUtils.CUOTAS] = new List<Cuota>();
            }
            return venta;
        }

        public ActionResult VehiculoVentaSelected(int idVehiculo, string idSession) {
            ViewData["idSession"] = idSession;
            Venta v = inicializarVenta(idVehiculo, idSession);
            return PartialView("_detallesVenta", v);
        }

        [HttpPost]
        public ActionResult VentaVehiculo(Venta venta, string idSession, int hayPermuta) {
            ViewData["idSession"] = idSession;

            //Asocio los que esta en sesion ahora, por si hay error, que vuelva la venta con los datos
            venta.Pago.AgregarEfectivos((IEnumerable<Efectivo>)Session[idSession + SessionUtils.EFECTIVO]); 
            venta.Pago.AgregarCheques((IEnumerable<Cheque>)Session[idSession + SessionUtils.CHEQUES]);
            venta.Pago.AgregarMovsBanco((IEnumerable<MovBanco>)Session[idSession + SessionUtils.MOV_BANCARIO]);
            foreach (Vale vale in (IEnumerable<Vale>)Session[idSession + SessionUtils.VALES]) {
                venta.AgregarValeOriginal(vale);
            }
            var lcuotas = (IEnumerable<Cuota>)Session[idSession + SessionUtils.CUOTAS];
            foreach (Cuota c in lcuotas) {
                venta.Financiacion.AgregarCuotaVenta(c);    
            }
            if (hayPermuta == 0) {
                venta.Permuta = null;
            }
            
            //Antes de validar nada, asegurarse que vino vehiculo seleccionado:
            if (venta.Vehiculo == null || venta.Vehiculo.Codigo <= 0) {
                ModelState.Clear();
                ModelState.AddModelError("Vehiculo.Codigo", "Debe seleccionar un vehiculo");
                return View("VentaVehiculo", venta);
            }

            this.eliminarValidacionesIgnorables("Importe.Moneda", MetadataManager.IgnorablesDDL(new Moneda()));
            this.eliminarValidacionesIgnorables("Vehiculo", MetadataManager.IgnorablesDDL(new Vehiculo()));
            this.eliminarValidacionesIgnorables("Cliente", MetadataManager.IgnorablesDDL(new Cliente()));
            this.eliminarValidacionesIgnorables("Vendedor", MetadataManager.IgnorablesDDL(new Vendedor()));
            this.eliminarValidacionesIgnorables("Sucursal", MetadataManager.IgnorablesDDL(new Sucursal()));
            this.eliminarValidacionesIgnorables("Financiacion.MontoFinanciado.Moneda", MetadataManager.IgnorablesDDL(new Moneda()));

            if (hayPermuta == 0) {
                //Si no hay permuta, ignorar todas las validaciones de la misma
                GeneralUtils.ModelStateRemoveAllStarting(ModelState, "Permuta");
            } else {
                ModelState.Remove("Permuta.Codigo");
                var l = ModelState.Keys.Where(x=>x.StartsWith("Permuta")).ToList();
                foreach (string s in l) {
                    foreach (var err in ModelState[s].Errors) {
                        string snew = "Permuta: " + err.ErrorMessage;
                        ModelState.Remove(s);
                        ModelState.AddModelError(s, snew);
                    }       
                }
                this.eliminarValidacionesIgnorables("Permuta.Costo.Moneda", MetadataManager.IgnorablesDDL(new Moneda()));
                this.eliminarValidacionesIgnorables("Permuta.Sucursal", MetadataManager.IgnorablesDDL(new Sucursal()));
                this.eliminarValidacionesIgnorables("Permuta.PrecioVenta.Moneda", MetadataManager.IgnorablesDDL(new Moneda()));
            }


            GeneralUtils.ModelStateRemoveAllStarting(ModelState, "Senia");

            ModelState.Remove("Vendedor.Codigo");
            if (venta.Vendedor == null || venta.Vendedor.Codigo <= 0) {
                ModelState.AddModelError("Vendedor.Codigo", "Debe especificar el vendedor");
            }

            ModelState.Remove("Cliente.Codigo");
            if (venta.Cliente == null || venta.Cliente.Codigo <= 0) {
                ModelState.AddModelError("Cliente.Codigo", "Debe especificar el cliente");
            }

            if (venta.Financiacion.MontoFinanciado.Monto == 0) {
                venta.Financiacion = null; //no hay financiacion
                GeneralUtils.ModelStateRemoveAllStarting(ModelState, "Financiacion");
            } else {
                GeneralUtils.ModelStateRemoveAllStarting(ModelState, "Financiacion");
                if (venta.Financiacion.CantCuotas <= 0) {
                    ModelState.AddModelError("Financiacion.CantCuotas", "Cantidad de cuotas financiacion no es valida");
                }
                if (venta.Financiacion.Tasa < 0) {
                    ModelState.AddModelError("Financiacion.Tasa", "Tasa de financicion no es valida");
                }
            }

            if (ModelState.IsValid) {
                try {
                    venta.Ejecutar();
                    return RedirectToAction("Details", VentasController.CONTROLLER, new { id = venta.Codigo });
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View("VentaVehiculo", venta);
                }
            }

            return View("VentaVehiculo", venta);
        
        }

        #endregion

      
        #region Vales

        public ActionResult grillaPagosVale(string idSession) {
            return PartialView("_grillaPagosVale", _listaPagosVale(idSession));
        }

        private List<Vale> _listaPagosVale(string idSession) {
            List<Vale> listPagosVale = (List<Vale>)(Session[idSession + SessionUtils.VALES]);
            return listPagosVale;
        }

        private void _validarVale(Vale vale) {

            this.eliminarValidacionesIgnorables("Importe.Moneda", MetadataManager.IgnorablesDDL(new Moneda()));

            //Sacar la validacion de moneda no nula porque da mensaje feo, hacerla manualmente
            ModelState.Remove("Importe.Moneda.Codigo");
            if (vale.Importe.Moneda == null || vale.Importe.Moneda.Codigo <= 0) {
                ModelState.AddModelError("Importe.Moneda.Codigo", "La moneda es requerida");
            }

            //validar el importe
            if (vale.Importe.Monto <= 0) {
                ModelState.AddModelError("Importe.Monto", "El monto debe ser un valor positivo");
            }

        }

        [HttpPost, ValidateInput(false)]
        public ActionResult grillaPagosVale_Add(Vale vale, string idSession) {

            List<Vale> listVale = _listaPagosVale(idSession);

            _validarVale(vale);
            if (ModelState.IsValid) {
                try {
                    int maxIdLinea = listVale.Count > 0 ? listVale.Max(c => c.IdLinea) : 0;
                    vale.IdLinea = maxIdLinea + 1;
                    listVale.Add(vale);
                } catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            } else {
                ViewData["EditError"] = "Corrija los valores incorrectos";
            }

            return PartialView("_grillaPagosVale", listVale);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult grillaPagosVale_Update(Vale vale, string idSession) {

            List<Vale> listVale = _listaPagosVale(idSession);

            _validarVale(vale);
            if (ModelState.IsValid) {
                try {

                    Moneda monedaElejida =
                        (from m in Moneda.Monedas
                         where (m.Codigo == vale.Importe.Moneda.Codigo)
                         select m).First<Moneda>();

                    Vale valeEditado =
                        (from c in listVale
                         where (c.IdLinea == vale.IdLinea)
                         select c).First<Vale>();

                    valeEditado.Vencimiento = vale.Vencimiento;
                    valeEditado.Importe.Monto = vale.Importe.Monto;
                    valeEditado.Importe.Moneda = monedaElejida;
                    valeEditado.Observaciones = vale.Observaciones;

                } catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            } else {
                ViewData["EditError"] = "Corrija los valores incorrectos";
            }

            return PartialView("_grillaPagosVale", listVale);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult grillaPagosVale_Delete(int IdLinea, string idSession) {

            List<Vale> listVale = _listaPagosVale(idSession);

            if (IdLinea >= 0) {
                try {
                    Vale ValeEliminado =
                        (from c in listVale
                         where (c.IdLinea == IdLinea)
                         select c).First<Vale>();
                    listVale.Remove(ValeEliminado);
                } catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_grillaPagosVale", listVale);
        }

        #endregion

        ////===============================================================================================================

        #region Cuotas


        public ActionResult grillaPagosCuota(string idSession) {
            return PartialView("_grillaPagosCuota", _listaPagosCuota(idSession));
        }

        private List<Cuota> _listaPagosCuota(string idSession) {
            List<Cuota> listPagosCuota = (List<Cuota>)(Session[idSession + SessionUtils.CUOTAS]);
            return listPagosCuota;
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult grillaPagosCuota_Update(Cuota cuota, string idSession) {

            List<Cuota> listCuota = (List<Cuota>)(Session[idSession + SessionUtils.CUOTAS]);

            this.eliminarValidacionesIgnorables("Importe.Moneda", MetadataManager.IgnorablesDDL(new Moneda()));
            if (cuota.Importe.Monto <= 0) {
                ModelState.AddModelError("Importe.Monto", "El monto debe ser un valor positivo");
            }

            if (ModelState.IsValid) {
                try {
                    //IEnumerable<Cuota> pagosCuota = (IEnumerable<Cuota>)(Session[idSession + SessionUtils.CUOTAS]);
                    //List<Cuota> listPagosCuota = pagosCuota.ToList();
                    int pos=-1;
                    for (int i=0; i<listCuota.Count;  i++){
                        if (listCuota[i].Numero==cuota.Numero){
                            pos=i;
                            break;
                        }
                    }
                    if (pos >= 0) {
                        listCuota.RemoveAt(pos);
                        listCuota.Add(cuota);
                        listCuota = listCuota.OrderBy(x => x.Numero).ToList<Cuota>();
                        Session[idSession + SessionUtils.CUOTAS] = listCuota;
                    } else {
                        ViewData["EditError"] = "ERROR: Cuota no encontrada";
                    }
                } catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            } else {
                ViewData["EditError"] = "Corrija los valores incorrectos";
            }

            return PartialView("_grillaPagosCuota", listCuota);
        }


        [HttpPost]
        public JsonResult cambiarFinanciacion(int cantCuotas, double tasa, int codigoMonedaImporte, double montoImporte, string idSession, string fechaVenta) {

            Moneda moneda = new Moneda();
            moneda.Codigo = codigoMonedaImporte;
            moneda.Consultar();
            Importe importe = new Importe(moneda, montoImporte);
            Financiacion financiacion = new Financiacion(importe, cantCuotas, tasa);

            IFormatProvider formato = new CultureInfo("es-UY").DateTimeFormat;
            DateTime fecha;
            if (!DateTime.TryParse(fechaVenta, formato, DateTimeStyles.None, out fecha)) {
                fecha = DateTime.Now.Date;
            }

            //validacion del controller
            List<String> errors = this.validateAtributesFinanciacion(financiacion);
            if (errors.Count > 0) {
                return Json(new { Result = "ERROR", ErrorCode = "VALIDATION_ERROR", ErrorMessage = errors.ToArray() });
            }

            //envio al backend
            try {
                financiacion.generarCuotasVenta(fecha);

                Session[idSession + SessionUtils.CUOTAS] = financiacion.CuotasOriginales;

                return Json(new { Result = "OK" });
            } catch (UsuarioException exc) {
                List<String> errores1 = new List<string>();
                errores1.Add(exc.Message);
                return Json(new { Result = "ERROR", ErrorCode = exc.Codigo, ErrorMessage = errores1.ToArray() });
            }

        }

        ////-------------------------------

        private List<String> validateAtributesFinanciacion(Financiacion financiacion) {

            List<String> errors = new List<String>();

            if (financiacion.MontoFinanciado.Monto <= 0) {
                errors.Add("El monto financiado debe ser mayor a 0");
            }

            if (financiacion.CantCuotas <= 0) {
                errors.Add("La cantidad de cuotas debe ser mayor a 0");
            }

            return errors;
        }

       #endregion


        //===============================================================================================================

        #region Consulta

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

        #endregion

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
            this.eliminarValidacionesIgnorables("Filtro.Sucursal", MetadataManager.IgnorablesDDL(new Sucursal()));
            this.eliminarValidacionesIgnorables("Filtro.Cliente", MetadataManager.IgnorablesDDL(new Cliente()));
            this.eliminarValidacionesIgnorables("Filtro.Vendedor", MetadataManager.IgnorablesDDL(new Vendedor()));
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
