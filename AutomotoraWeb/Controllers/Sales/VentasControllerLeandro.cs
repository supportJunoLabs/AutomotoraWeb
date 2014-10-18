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

namespace AutomotoraWeb.Controllers.Sales
{
    public class VentasControllerLeandro : SalesController
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

            Usuario usuario = getUsuario();
            if (usuario == null) {
                filterContext.Result = new RedirectResult("/" + AuthenticationController.CONTROLLER + "/" + AuthenticationController.LOGIN);
                return;
            }
            ViewBag.MultiSucursal = usuario.MultiSucursal;
        }

        public ActionResult VentaVehiculo(int? id) {
            
            Venta venta = new Venta();
            venta.Vehiculo = new Vehiculo();
            if (id != null && id!=0) {
                venta.Vehiculo.Codigo = id ?? 0;
                Usuario u = getUsuario();
                venta.Vehiculo.Consultar(u);
            }
            //venta.Cliente = new Cliente();
            //venta.Vendedor = new Vendedor();
            //venta.Sucursal = new Sucursal();
            //venta.Permuta = new Vehiculo();
            //venta.Pago.AgregarEfectivos(new List<Efectivo>());
            //venta.Financiacion = new Financiacion();
            //venta.Senia = new Senia();

            prepararSession(venta); 

            return View(venta);
        }

        //public ActionResult VentaVehiculoX(int id) {
        //    Venta venta = new Venta();
        //    venta.Vehiculo = new Vehiculo();
        //    venta.Vehiculo.Codigo = id;
        //    venta.Vehiculo.Consultar();

        //    venta.Cliente = new Cliente();
        //    venta.Vendedor = new Vendedor();
        //    venta.Sucursal = new Sucursal();
        //    venta.Permuta = new Vehiculo();
        //    venta.Financiacion = new Financiacion();
        //    venta.Senia = venta.Vehiculo.ObtenerSenia();

        //    prepararSession(venta); 

        //    return View("ventaVehiculo", venta);
        //}

        [HttpPost]
        public JsonResult finalizarVenta(string idSession, int codigoVehiculo, int condVentaCodigoCliente, int condVentaCodigoVendedor, 
                                         DateTime condVentaFecha, int condVentaCodigoMoneda, double condVentaMonto, int condVentaCodigoSucursal,
                                         bool condVentaVehiculoEntregado, DateTime condVentaFechaEntrega, String condVentaObservaciones, Financiacion financiacion, bool existePermuta, Vehiculo permuta) {

            List<string> listError = new List<string>();

            // Chequeos generales
            if (codigoVehiculo == 0) {
                listError.Add("Debe seleccionar un Vehículo para la venta");                       
            }
            if (condVentaCodigoCliente == 0) {
                listError.Add("Debe seleccionar un Cliente para la venta");  
            } 
            if (condVentaCodigoVendedor == 0) {
                listError.Add("Debe seleccionar un Vendedor para la venta");  
            }
            if (condVentaMonto == 0) {
                listError.Add("El monto de la venta debe ser mayor a 0");  
            }

            // Chequeo de campos obligatorios en Permuta
            if (existePermuta && permuta.Sucursal.Codigo == 0) {
                listError.Add("[Permuta] Debe seleccionar una Sucursal para el vehículo en Permuta"); 
            }
            if (existePermuta && (permuta.Marca == null) || (permuta.Marca == "")) {
                listError.Add("[Permuta] Debe seleccionar una Marca para el vehículo en Permuta");
            }
            if (existePermuta && (permuta.Modelo == null) || (permuta.Modelo == "")) {
                listError.Add("[Permuta] Debe seleccionar una Modelo para el vehículo en Permuta");
            }
            if (existePermuta && permuta.Anio == 0) {
                listError.Add("[Permuta] Debe seleccionar una Año para el vehículo en Permuta");
            }
            if (existePermuta && (permuta.Matricula == null) || (permuta.Modelo == "")) {
                listError.Add("[Permuta] Debe seleccionar una Matricula para el vehículo en Permuta");
            }

            if (listError.Count > 0) {
                return Json(new { Result = "ERROR", ErrorCode = "", ErrorMessage = listError }); 
            }

            // Se procesa la venta
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

                string nomUsuario = getUserName();
                string origen = getIP();
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

                venta.Financiacion = financiacion;
                if (existePermuta) {
                    venta.Permuta = permuta;
                }

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

                return Json(new { Result = "OK", CodigoVenta = venta.Codigo });
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
            string idSession = SessionUtils.generarIdVarSesion("VentaVehiculo", getUserName()) + "|";
            prepararSession(venta, idSession);
        }

        private void prepararSession(Venta venta, string idSession) {
           
            ViewBag.Controller = CONTROLLER;
            ViewBag.SoloLectura = false;

            Session[idSession + SessionUtils.VENTA] = venta;
            ViewData["idSession"] = idSession;
            ViewData["idParametros"] = venta.Codigo; // Para grillas que necesitan id de la venta

            List<Vale> listVale = null;
            List<Cuota> listCuota = new List<Cuota>();

            if (venta.Vehiculo.Codigo != 0) {
                //List<Vehiculo> listVehiculo = new List<Vehiculo>();
                //listVehiculo.Add(venta.Vehiculo);
                //ViewBag.VehiculosVendibles = listVehiculo;

                PrecondicionesVenta precondicionesVenta = venta.Vehiculo.ObtenerPrecondicionesVenta();
                
                // VALES
                listVale = precondicionesVenta.Vales;

                // FINANCIACION
                if (precondicionesVenta.Financiacion != null) {
                    precondicionesVenta.Financiacion.generarCuotasVenta(DateTime.Now);
                    listCuota = precondicionesVenta.Financiacion.CuotasOriginales;
                    venta.Financiacion = venta.Vehiculo.ObtenerPrecondicionesVenta().Financiacion;
                }

                // EFECTIVO
                List<Efectivo> listEfectivo = precondicionesVenta.efectivoOperacion;
                venta.Pago.AgregarEfectivos(listEfectivo);

                // CHEQUE
                List<Cheque> listCheque = precondicionesVenta.Cheques;
                venta.Pago.AgregarCheques(listCheque);

                venta.Permuta = venta.Vehiculo.ObtenerPrecondicionesVenta().Permuta;
            } else {
                listVale = new List<Vale>();
                listCuota = new List<Cuota>();
                //ViewBag.VehiculosVendibles = Vehiculo.Vehiculos(Vehiculo.VHC_TIPO_LISTADO.VENDIBLES);
            }

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
            Usuario u = getUsuario();
            v.Consultar(u);

            ViewBag.VehiculoSeniado = v.TieneSeniaActiva().ToString();
            
            return PartialView("_seleccionDeVehiculosDetalle", v);
        }

        public ActionResult CondicionesDeVentaVehiculo(int id) {

            Vehiculo vehiculo = new Vehiculo();
            vehiculo.Codigo = id;
            Usuario u = getUsuario();
            vehiculo.Consultar(u);

            PrecondicionesVenta cond = vehiculo.ObtenerPrecondicionesVenta();

            List<Cliente> clientes = new List<Cliente>();
            clientes.Add(cond.Cliente);

            List<Vendedor> vendedores = new List<Vendedor>();
            vendedores.Add(cond.Vendedor);

            ViewBag.Clientes = clientes;
            ViewBag.VendedoresHabilitados = vendedores;
            ViewBag.Sucursales = Sucursal.Sucursales;

            Venta venta = new Venta();

            venta.Importe = cond.PrecioVenta;
            venta.Cliente = cond.Cliente;
            venta.Vendedor = cond.Vendedor;
            venta.Sucursal = vehiculo.Sucursal;

            venta.FechaEntrega = DateTime.Now;

            return PartialView("_seleccionDeCondicionesDeVenta", venta);
        }


        public ActionResult ResetCondicionesDeVenta() {

            ViewBag.Sucursales = Sucursal.Sucursales;

            Venta venta = new Venta();
            venta.Sucursal = new Sucursal();

            return PartialView("_seleccionDeCondicionesDeVenta", venta);
        }

        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------

        public ActionResult SeniaVehiculo(int id) {

            Vehiculo vehiculo = new Vehiculo();
            vehiculo.Codigo = id;

            Senia senia = vehiculo.ObtenerSenia();
            ViewBag.SoloLectura = true;
            ViewBag.Multisucursal = false;
            ViewBag.Sucursales = Sucursal.Sucursales;

            SeniaModel seniaModel = new SeniaModel();
            seniaModel.Senia = senia;

            ViewBag.LinkConsulta = true;

            return PartialView("~/views/senias/_condicionesSenia.cshtml", seniaModel);
        }

        //------------------------------------------------------------------------------------------

        public ActionResult ACVsVehiculo(int id) {

            Vehiculo vehiculo = new Vehiculo();
            vehiculo.Codigo = id;


            Usuario u = getUsuario();
            vehiculo.Consultar(u);

            List<ACuentaVenta> listACuentaVenta = vehiculo.obtenerACVsNoanulados();

            ViewBag.SoloLectura = true;
            ViewBag.Multisucursal = false;
            ViewBag.Sucursales = Sucursal.Sucursales;

            return PartialView("~/views/Acvs/_grillaAcvNoAnulados.cshtml", listACuentaVenta);
        }

        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------

        public ActionResult DetallesPagoSeniaACVs(int id) {

            Vehiculo vehiculo = new Vehiculo();
            vehiculo.Codigo = id;
            Usuario u = getUsuario();
            vehiculo.Consultar(u);
            PagoTransaccion pago = vehiculo.pagosTransaccionesPreventa();

            return PartialView("_pagosSeniaACVs", pago);
        }

        //------------------------------------------------------------------------------------------

        public ActionResult ResetDetallesPagoSeniaACVs() {

            PagoTransaccion pago = new PagoTransaccion();

            return PartialView("_pagosSeniaACVs", pago);
        }

        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------

        public ActionResult MetodosDePago(int id, string idSession) {

            Venta venta = new Venta();
            venta.Vehiculo = new Vehiculo();
            venta.Vehiculo.Codigo = id;
            Usuario u = getUsuario();
            venta.Vehiculo.Consultar(u);
            prepararSession(venta, idSession);

            return PartialView("_seleccionDeMetodosDePago", venta);
        }


        public ActionResult ResetMetodosDePago(string idSession) {

            Venta venta = new Venta();
            venta.Vehiculo = new Vehiculo();
            venta.Vehiculo.Codigo = 0;
            Usuario u = getUsuario();
            venta.Vehiculo.Consultar(u);
            prepararSession(venta, idSession);

            return PartialView("_seleccionDeMetodosDePago", venta);
        }

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
        public ActionResult grillaPagosVale_AddNewRowRouteValues(Vale vale, string idSession) {

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
        public ActionResult grillaPagosVale_UpdateRowRouteValues(Vale vale, string idSession) {

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
        public ActionResult grillaPagosVale_DeleteRowRouteValues(int IdLinea, string idSession) {

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

        //===============================================================================================================

        #region Cuotas


        public ActionResult grillaPagosCuota(string idSession) {
            return PartialView("_grillaPagosCuota", _listaPagosCuota(idSession));
        }

        private List<Cuota> _listaPagosCuota(string idSession) {
            List<Cuota> listPagosCuota = (List<Cuota>)(Session[idSession + SessionUtils.CUOTAS]);
            return listPagosCuota;
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult grillaPagosCuota_UpdateRowRouteValues(Cuota cuota, string idSession) {

            List<Cuota> listCuota = new List<Cuota>();

            //_validarCheque(cheque);
            if (ModelState.IsValid) {
                try {
                    IEnumerable<Cuota> pagosCuota = (IEnumerable<Cuota>)(Session[idSession + SessionUtils.CUOTAS]);
                    List<Cuota> listPagosCuota = pagosCuota.ToList();
                    listPagosCuota.Remove(listPagosCuota.First(x => x.Codigo == cuota.Codigo));
                    listPagosCuota.Add(cuota);
                    var sortedProducts = listPagosCuota.OrderBy(x => x.Codigo);

                    Session[idSession + SessionUtils.CUOTAS] = sortedProducts;

                } catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            } else {
                ViewData["EditError"] = "Corrija los valores incorrectos";
            }

            return PartialView("_grillaPagosCheque", listCuota);
        }

        [HttpPost]
        public JsonResult cambiarFinanciacion(int cantCuotas, double tasa, int codigoMonedaImporte, double montoImporte, string idSession) {

            Moneda moneda = new Moneda();
            moneda.Codigo = codigoMonedaImporte;
            moneda.Consultar();

            Importe importe = new Importe(moneda, montoImporte);

            Financiacion financiacion = new Financiacion(importe, cantCuotas, tasa);

            //validacion del controller
            List<String> errors = this.validateAtributesFinanciacion(financiacion);
            if (errors.Count > 0) {
                return Json(new { Result = "ERROR", ErrorCode = "VALIDATION_ERROR", ErrorMessage = errors.ToArray() });
            }

            //envio al backend
            try {
                financiacion.generarCuotasVenta(DateTime.Now);

                Session[idSession + SessionUtils.CUOTAS] = financiacion.CuotasOriginales;

                return Json(new { Result = "OK" });
            } catch (UsuarioException exc) {
                List<String> errores1 = new List<string>();
                errores1.Add(exc.Message);
                return Json(new { Result = "ERROR", ErrorCode = exc.Codigo, ErrorMessage = errores1.ToArray() });
            }

        }

        //-------------------------------

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

        //-------------------------------

        #endregion
        //===============================================================================================================

        //------------------------------------------------------------------------------------------
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
            Usuario u = getUsuario();
            td.Consultar(u);
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

                Financiacion financiacion = new Financiacion(importe, cantCuotas, tasa, "");
                financiacion.generarCuotasVenta(DateTime.Now);
                List<Cuota> listCuota = new List<Cuota>();
                foreach (Cuota cuota in financiacion.CuotasOriginales) {
                    listCuota.Add(cuota);
                }
                Session[idSession + SessionUtils.CUOTAS] = listCuota;
                Session[idSession + SessionUtils.FINANCIACION] = financiacion;

                return Json(new { Result = "OK"  });
            } catch (UsuarioException exc) {
                List<String> errores1 = new List<string>();
                errores1.Add(exc.Message);
                return Json(new { Result = "ERROR", ErrorCode = exc.Codigo, ErrorMessage = errores1.ToArray() });
            }

        }


        private List<String> validateAtributesFinanciacion(int cantCuotas, double tasa, Importe importe) {

            List<String> errors = new List<String>();

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
            string s = SessionUtils.generarIdVarSesion("ListadoVentas", getUserName());
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
            Usuario u = getUsuario();
            return Venta.Ventas(model.Filtro, u);
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
                Usuario u = getUsuario();
                TRVentaAnulacion tr = (TRVentaAnulacion)Transaccion.ObtenerTransaccion(id, u);
                ViewData["idParametros"] = id;
                return View("ReciboAnulacion");
            } catch (UsuarioException exc) {
                ViewData["idParametros"] = 0;
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private XtraReport _generarReciboAnulacion(int id) {
            Usuario u = getUsuario();
            TRVentaAnulacion tr = (TRVentaAnulacion)Transaccion.ObtenerTransaccion(id,u);
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
                Usuario u = getUsuario();
                Entrega tr = (Entrega)Transaccion.ObtenerTransaccion(id, u);
                ViewData["idParametros"] = id;
                return View("ReciboEntrega");
            } catch (UsuarioException exc) {
                ViewData["idParametros"] = 0;
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private XtraReport _generarReciboEntrega(int id) {

            Usuario u = getUsuario();
            Entrega tr = (Entrega)Transaccion.ObtenerTransaccion(id,u);
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
