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

namespace AutomotoraWeb.Controllers.Sales {
    public class VentasController : SalesController {

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

         [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult VentaVehiculo(int? id) {
            string idSession = SessionUtils.generarIdVarSesion("ventaVehiculo", Session[SessionUtils.SESSION_USER].ToString()) + "|";
            ViewData["idSession"] = idSession;
            int idVehiculo = id ?? 0;
            Venta v = inicializarVenta(idVehiculo, idSession);
            return View("VentaVehiculo", v);
        }

        private Venta inicializarVenta(int idVehiculo, string idSession) {
            Venta venta = new Venta();
            venta.Vehiculo = new Vehiculo();
            venta.Entregado = true;
            venta.FechaEntrega = DateTime.Now.Date;
            Usuario usuario = (Usuario)(Session[SessionUtils.SESSION_USER]);
            venta.Sucursal = usuario.Sucursal;
            if (idVehiculo != 0) {
                venta.Vehiculo.Codigo = idVehiculo;
                venta.Vehiculo.Consultar();
                PrecondicionesVenta cond = venta.Vehiculo.ObtenerPrecondicionesVenta(DateTime.Now.Date);
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
                var l = ModelState.Keys.Where(x => x.StartsWith("Permuta")).ToList();
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
                    int pos = -1;
                    for (int i = 0; i < listCuota.Count; i++) {
                        if (listCuota[i].Numero == cuota.Numero) {
                            pos = i;
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

         [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
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


        private string _sacarComasFinal(string scheques) {
            if (string.IsNullOrWhiteSpace(scheques)){
                return "";
            }
            bool seguir = true;
            while (seguir) {
                scheques = scheques.Trim();
                if (scheques.Length > 0 && scheques[scheques.Length - 1] == ',') {
                    scheques = scheques.Substring(0, scheques.Length - 1);
                } else {
                    seguir = false;
                }
            }
            return scheques;
        }

        private VentaAnulacionModel _iniDevolucion(int id, string idSession) {
            VentaAnulacionModel model = new VentaAnulacionModel();
            TRVentaAnulacion tr = new TRVentaAnulacion();
            model.VentaDev = tr;
            tr.Venta = new Venta();
            tr.Venta.Codigo = id;
            tr.Fecha = DateTime.Now.Date;

            if (id > 0) {
                tr.Venta.Consultar();
                Session[idSession + SessionUtils.EFECTIVO_DEVOLUCION] = tr.Venta.PagoAnulacionSugerida().Efectivos;
                Session[idSession + SessionUtils.CHEQUES_DEVOLUCION] = tr.Venta.PagoAnulacionSugerida().Cheques;
                Session[idSession + SessionUtils.CHEQUES_EMITIDOS] = new List<ChequeEmitido>();
                string s = "";
                foreach (Cheque ch in (List<Cheque>)Session[idSession + SessionUtils.CHEQUES_DEVOLUCION]) {
                    s += ch.Codigo.ToString() + ",";
                }
                s = _sacarComasFinal(s);
                ViewData["chequesDevolverIds"] = s;

                tr.Pago.AgregarEfectivos((IEnumerable<Efectivo>)Session[idSession + SessionUtils.EFECTIVO_DEVOLUCION]);
                tr.Pago.AgregarCheques((IEnumerable<Cheque>)Session[idSession + SessionUtils.CHEQUES_DEVOLUCION]);
                tr.Sucursal = tr.Venta.Sucursal;
                tr.Importe = tr.Venta.TotalEfectivoMonedaDefault + tr.Venta.TotalChequesMonedaDefault + tr.Venta.TotalBancoMonedaDefault;
            } else {
                Session[idSession + SessionUtils.EFECTIVO_DEVOLUCION] = new List<Efectivo>();
                Session[idSession + SessionUtils.CHEQUES_DEVOLUCION] = new List<Cheque>();
                Session[idSession + SessionUtils.CHEQUES_EMITIDOS] = new List<ChequeEmitido>();
                ViewData["chequesDevolverIds"] = "";
                Usuario usuario = (Usuario)(Session[SessionUtils.SESSION_USER]);
                tr.Sucursal = usuario.Sucursal;
            }
            return model;
        }

        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Anular(int? id) {
            string idSession = SessionUtils.generarIdVarSesion("devsenia", Session[SessionUtils.SESSION_USER].ToString()) + "|";
            ViewData["idSession"] = idSession;
            ViewBag.SoloLectura = true;

            int idVenta = id ?? 0;
            VentaAnulacionModel model = _iniDevolucion(idVenta, idSession);
            return View("Anular", model);
        }

        public ActionResult VentasAnulablesGrilla(GridLookUpModel model) {
            model.Opciones = Venta.Ventas(Venta.TIPO_LISTADO_VENTAS.ANULABLES);
            return PartialView("_selectVentaAnular", model);
        }

        public ActionResult VentaAnularSelected(int idVenta, string idSession) {
            ViewData["idSession"] = idSession;
            ViewBag.SoloLectura = true;
            VentaAnulacionModel model = _iniDevolucion(idVenta, idSession);
            return PartialView("_ventaAnulacion", model);
        }


        [HttpPost]
        public ActionResult Anular(VentaAnulacionModel model, string idSession, string chequesDevolverIds) {
            ViewData["idSession"]=idSession;
            ViewData["chequesDevolverIds"] = chequesDevolverIds;
            ViewBag.SoloLectura = true;

            if (model.VentaDev==null || model.VentaDev.Venta == null || model.VentaDev.Venta.Codigo <= 0) {
                ViewBag.ErrorCode = "USR";
                ViewBag.ErrorMessage = "No hay venta seleccionada";
                return View("VentaVehiculo", model);
            }
            try {

                TRVentaAnulacion tr = model.VentaDev;
                tr.Venta.Consultar();//en caso de error, la venta vuelve con sus datos.
                tr.Pago.Reset();
                tr.Pago.AgregarEfectivos((IEnumerable<Efectivo>)Session[idSession + SessionUtils.EFECTIVO_DEVOLUCION]);
                tr.Pago.AgregarChequesEmitidos((IEnumerable<ChequeEmitido>)Session[idSession + SessionUtils.CHEQUES_EMITIDOS]);

                chequesDevolverIds = _sacarComasFinal(chequesDevolverIds);
                ViewData["chequesDevolverIds"] = chequesDevolverIds;
                //cheques devolver
                if (!string.IsNullOrWhiteSpace(chequesDevolverIds)) {
                    string[] ach =chequesDevolverIds.Split(new Char[] { ',' });
                    List<Cheque> lista = (List<Cheque>)Session[idSession + SessionUtils.CHEQUES_DEVOLUCION];
                    foreach (string s in ach) {
                        Cheque ch = new Cheque { Codigo = Int32.Parse(s) };
                        int i = lista.IndexOf(ch);
                        tr.Pago.AgregarCheque(lista[i]);
                    }
                }

                GeneralUtils.ModelStateRemoveAllStarting(ModelState, "Venta");
                this.eliminarValidacionesIgnorables("Importe.Moneda", MetadataManager.IgnorablesDDL(new Moneda()));
                this.eliminarValidacionesIgnorables("Sucursal", MetadataManager.IgnorablesDDL(new Sucursal()));

                string usuario = (string)HttpContext.Session.Contents[SessionUtils.SESSION_USER_NAME];
                string IP = HttpContext.Request.UserHostAddress;
                model.VentaDev.setearAuditoria(usuario, IP);

                if (ModelState.IsValid) {
                    tr.Ejecutar();
                    return RedirectToAction("ReciboAnulacion", VentasController.CONTROLLER, new { id = tr.NroRecibo });
                }
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View("Anular", model);
            }
            return View("Anular", model);

        }

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

        #region DevolucionEfectivo

        public ActionResult grillaDevolucionEfectivo(string idSession) {
            return PartialView("_grillaDevolucionEfectivo", Session[idSession + SessionUtils.EFECTIVO_DEVOLUCION]);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult grillaDevolucionEfectivo_Add(Efectivo efectivo, string idSession) {
            var lista = (List<Efectivo>)Session[idSession + SessionUtils.EFECTIVO_DEVOLUCION];
            _validarEfectivoGrilla(efectivo);
            if (ModelState.IsValid) {
                try {
                    int maxIdLinea = lista.Count > 0 ? lista.Max(c => c.IdLinea) : 0;
                    efectivo.IdLinea = maxIdLinea + 1;
                    lista.Add(efectivo);
                } catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            } else {
                ViewData["EditError"] = "Corrija los valores incorrectos";
            }

            return PartialView("_grillaDevolucionEfectivo", lista);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult grillaDevolucionEfectivo_Update(Efectivo efectivo, string idSession) {

            //pruebaError();
            var lista = (List<Efectivo>)Session[idSession + SessionUtils.EFECTIVO_DEVOLUCION];

            _validarEfectivoGrilla(efectivo);
            if (ModelState.IsValid) {
                try {
                    Efectivo efectivoEditado =
                        (from c in lista
                         where (c.IdLinea == efectivo.IdLinea)
                         select c).First<Efectivo>();
                    efectivoEditado.Importe.Monto = efectivo.Importe.Monto;
                    efectivoEditado.Importe.Moneda = efectivo.Importe.Moneda;
                    efectivoEditado.Importe.Moneda.Consultar();
                } catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            } else {
                ViewData["EditError"] = "Corrija los valores incorrectos";
            }

            return PartialView("_grillaDevolucionEfectivo", lista);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult grillaDevolucionEfectivo_Delete(int IdLinea, string idSession) {

            var lista = (List<Efectivo>)Session[idSession + SessionUtils.EFECTIVO_DEVOLUCION];
            if (IdLinea >= 0) {
                try {
                    Efectivo efectivoEliminado =
                        (from c in lista
                         where (c.IdLinea == IdLinea)
                         select c).First<Efectivo>();
                    lista.Remove(efectivoEliminado);
                } catch (Exception e) {
                    ViewData["DeleteError"] = e.Message;
                }
            }
            //ViewData["DeleteError"] = "Testing error message style";
            return PartialView("_grillaDevolucionEfectivo", lista);
        }

        private void _validarEfectivoGrilla(Efectivo ef) {

            this.eliminarValidacionesIgnorables("Importe.Moneda", MetadataManager.IgnorablesDDL(new Moneda()));
            ModelState.Remove("Importe.ImporteEnMonedaDefault.Monto");

            //Sacar la validacion de moneda no nula porque da mensaje feo, hacerla manualmente
            ModelState.Remove("Importe.Moneda.Codigo");
            if (ef.Importe.Moneda == null || ef.Importe.Moneda.Codigo <= 0) {
                ModelState.AddModelError("Importe.Moneda.Codigo", "La moneda es requerida");
            }

            //validar el importe
            if (ef.Importe.Monto <= 0) {
                ModelState.AddModelError("Importe.Monto", "El monto debe ser un valor positivo");
            }


        }


        #endregion

        #region DevolucionCheques

        public ActionResult GrillaChequesDevolucion(string idSession) {
            return PartialView("_grillaChequesDevolucion", Session[idSession + SessionUtils.CHEQUES_DEVOLUCION]);
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
