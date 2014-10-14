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
using AutomotoraWeb.Controllers.General;

namespace AutomotoraWeb.Controllers.Sales {
    public class AcvsController : SalesController {

        public static string CONTROLLER = "Acvs";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            ViewBag.NombreEntidad = "Anticipo de Venta";
            ViewBag.NombreEntidades = "Anticipos de Ventas";
            ViewBag.Sucursales = Sucursal.Sucursales;
            ViewBag.Clientes = Cliente.Clientes();
            ViewBag.VendedoresHabilitados = Vendedor.Vendedores(Vendedor.VEND_TIPO_LISTADO.HABILITADOS);
            Usuario usuario = (Usuario)(Session[SessionUtils.SESSION_USER]);
            if (usuario == null) {
                filterContext.Result = new RedirectResult("/" + AuthenticationController.CONTROLLER + "/" + AuthenticationController.LOGIN);
                return;
            }
            ViewBag.MultiSucursal = usuario.MultiSucursal;
        }

        #region Consulta

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

        #endregion

        #region Listados

         [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult List() {

            ListadoAcvsModel model = new ListadoAcvsModel();
            try {
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

        //public ActionResult ListActivosVehiculo(int id) {
        //    ListadoAcvsModel model = new ListadoAcvsModel();
        //    try {
        //        string s = SessionUtils.generarIdVarSesion("ListadoACVs", Session[SessionUtils.SESSION_USER].ToString());
        //        Session[s] = model;
        //        model.idParametros = s;
        //        ViewBag.SucursalesListado = Sucursal.Sucursales;
        //        ViewBag.ClientesListado = Cliente.Clientes();
        //        ViewBag.VendedoresListado = Vendedor.Vendedores(Vendedor.VEND_TIPO_LISTADO.TODOS);
        //        ViewData["idParametros"] = model.idParametros;
        //        model.AcomodarFiltroActivosVehiculo(id);
        //        model.Resultado = _listaElementos(model);
        //        return View("List", model);
        //    } catch (UsuarioException exc) {
        //        ViewBag.ErrorCode = exc.Codigo;
        //        ViewBag.ErrorMessage = exc.Message;
        //        return View(model);
        //    }
        //}


        [HttpPost]
        public ActionResult List(ListadoAcvsModel model, string btnSubmit) {
            try {
                Session[model.idParametros] = model; //filtros actualizados
                ViewData["idParametros"] = model.idParametros;
                ViewBag.SucursalesListado = Sucursal.Sucursales;
                ViewBag.ClientesListado = Cliente.Clientes();
                ViewBag.VendedoresListado = Vendedor.Vendedores(Vendedor.VEND_TIPO_LISTADO.TODOS);
                this.eliminarValidacionesIgnorables("Filtro.Sucursal", MetadataManager.IgnorablesDDL(new Sucursal()));
                this.eliminarValidacionesIgnorables("Filtro.Cliente", MetadataManager.IgnorablesDDL(new Cliente()));
                this.eliminarValidacionesIgnorables("Filtro.Vendedor", MetadataManager.IgnorablesDDL(new Vendedor()));

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

        #region Crear

        public ActionResult VehiculosAnticipablesGrilla(GridLookUpModel model) {
            model.Opciones = Vehiculo.Vehiculos(Vehiculo.VHC_TIPO_LISTADO.VENDIBLES);
            return PartialView("_selectVehiculoACV", model);
        }

         [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult ACVenta(int? id) {
            ACuentaVenta tr = new ACuentaVenta();

            string idSession = SessionUtils.generarIdVarSesion("acv", Session[SessionUtils.SESSION_USER].ToString()) + "|";
            //Session[idSession] = tr;
            ViewData["idSession"] = idSession;
            Session[idSession + SessionUtils.CHEQUES] = tr.Pago.Cheques;
            Session[idSession + SessionUtils.EFECTIVO] = tr.Pago.Efectivos;
            Session[idSession + SessionUtils.MOV_BANCARIO] = tr.Pago.PagosBanco;

            tr.Fecha = DateTime.Now;
            Usuario usuario = (Usuario)(Session[SessionUtils.SESSION_USER]);
            tr.Sucursal = usuario.Sucursal;

            if (id != null && id > 0) {
                tr.Vehiculo = new Vehiculo();
                tr.Vehiculo.Codigo = id ?? 0;
                tr.Vehiculo.Consultar();

                PrecondicionesOperacion cond = tr.Vehiculo.ObtenerPrecondicionesACV();
                tr.Cliente = cond.Cliente;
                tr.Vendedor = cond.Vendedor;
            }

            return View("ACVenta", tr);
        }

        //se invoca al seleccionar una venta de la gridlookup, por ajax
        public ActionResult DetalleACV(int idVehiculo, string idSession) {

            ACuentaVenta tr = new ACuentaVenta();
            tr.Vehiculo = new Vehiculo();
            tr.Vehiculo.Codigo = idVehiculo;
            tr.Vehiculo.Consultar();

            tr.Fecha = DateTime.Now;
            Usuario usuario = (Usuario)(Session[SessionUtils.SESSION_USER]);
            tr.Sucursal = usuario.Sucursal;

            PrecondicionesOperacion cond = tr.Vehiculo.ObtenerPrecondicionesACV();
            tr.Cliente = cond.Cliente;
            tr.Vendedor = cond.Vendedor;

            //Session[idSession + SessionUtils.CHEQUES] = tr.Pago.Cheques;
            //Session[idSession + SessionUtils.EFECTIVO] = tr.Pago.Efectivos;
            //Session[idSession + SessionUtils.MOV_BANCARIO] = tr.Pago.PagosBanco;

            return PartialView("_detalleAcv", tr);
        }

        [HttpPost]
        public ActionResult ACVenta(ACuentaVenta model, string idSession) {

            ViewData["idSession"] = idSession;
            ACuentaVenta tr = (ACuentaVenta)Session[idSession];
            Session[idSession] = model;
            model.Fecha = DateTime.Now.Date;

            this.eliminarValidacionesIgnorables("Cliente", MetadataManager.IgnorablesDDL(new Cliente()));
            this.eliminarValidacionesIgnorables("Sucursal", MetadataManager.IgnorablesDDL(new Sucursal()));
            this.eliminarValidacionesIgnorables("Vendedor", MetadataManager.IgnorablesDDL(new Vendedor()));
            this.eliminarValidacionesIgnorables("Vehiculo", MetadataManager.IgnorablesDDL(new Vehiculo()));
            this.eliminarValidacionesIgnorables("Importe.Moneda", MetadataManager.IgnorablesDDL(new Moneda()));

            //Lo hago aca al principio para que si hay error la tr vuelva con medios de pago con los valores anteriores.
            //Si es externo esta vacio, porque no se ve la opcion pagos.
            model.Pago.AgregarCheques((IEnumerable<Cheque>)Session[idSession + SessionUtils.CHEQUES]);
            model.Pago.AgregarMovsBanco((IEnumerable<MovBanco>)Session[idSession + SessionUtils.MOV_BANCARIO]);
            model.Pago.AgregarEfectivos((IEnumerable<Efectivo>)Session[idSession + SessionUtils.EFECTIVO]);

            if (ModelState.IsValid) {
                try {
                    model.Ejecutar();
                    return RedirectToAction("Recibo", AcvsController.CONTROLLER, new { id = model.Codigo });
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View("ACVenta", model);
                }
            }
            return View("ACVenta", model);

        }

        public ActionResult Recibo(int id) {
            ACuentaVenta acv = new ACuentaVenta();
            acv.Codigo = id;
            ViewData["idParametros"] = id;
            return View("Recibo", acv);
        }

        private XtraReport _generarRecibo(int id) {
            ACuentaVenta acv = new ACuentaVenta();
            acv.Codigo = id;
            acv.Consultar();
            List<ACuentaVenta> ll = new List<ACuentaVenta>();
            ll.Add(acv);
            XtraReport rep = new DXReciboACV();
            rep.DataSource = ll;
            return rep;
        }

        public ActionResult ReciboPartial(int idParametros) {
            XtraReport rep = _generarRecibo(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_recibo");
        }

        public ActionResult ReciboExport(int idParametros) {
            XtraReport rep = _generarRecibo(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);

        }

        #endregion

        #region Anular()

        public ActionResult Anular(int? id) {
            ViewBag.SoloLectura = true;
            TRACuentaVentaAnulacion tr = new TRACuentaVentaAnulacion();
            tr.Acv = new ACuentaVenta();
            if (id != null) {
                tr.Acv.Codigo = id ?? 0;
                tr.Acv.Consultar();
            }
            tr.Fecha = DateTime.Now;
            Usuario usuario = (Usuario)(Session[SessionUtils.SESSION_USER]);
            tr.Sucursal = usuario.Sucursal;

            return View("Anular", tr);
        }

        public ActionResult GrillaAcvsAnulables() {
            GridLookUpModel gm = new GridLookUpModel();
            gm.Opciones = ACuentaVenta.ACuentaVentas(ACuentaVenta.ACV_TIPO_LISTADO.ANULABLES);
            return PartialView("_seleccionAcvAnularGridLookup", gm);
        }

        public ActionResult DetalleAcvAnular(int idAcv) {
            ViewBag.SoloLectura = true;
            TRACuentaVentaAnulacion tr = new TRACuentaVentaAnulacion();
            tr.Acv = new ACuentaVenta();
            tr.Acv.Codigo = idAcv;
            tr.Acv.Consultar();
            tr.Fecha = DateTime.Now;
            Usuario usuario = (Usuario)(Session[SessionUtils.SESSION_USER]);
            tr.Sucursal = usuario.Sucursal;
            return PartialView("_consultaAcv", tr.Acv);
        }

        [HttpPost]
        public ActionResult Anular(TRACuentaVentaAnulacion model) {
            ViewBag.SoloLectura = true;
            model.Fecha = DateTime.Now.Date;

            //this.eliminarValidacionesIgnorables("Cliente", MetadataManager.IgnorablesDDL(new Cliente));
            //this.eliminarValidacionesIgnorables("Sucursal", MetadataManager.IgnorablesDDL(new Sucursal));
            //this.eliminarValidacionesIgnorables("Vendedor", MetadataManager.IgnorablesDDL(new Vendedor));
            //this.eliminarValidacionesIgnorables("Vehiculo", MetadataManager.IgnorablesDDL(new Vehiculo));
            //this.eliminarValidacionesIgnorables("Importe.Moneda", MetadataManager.IgnorablesDDL(new Moneda));

            //if (ModelState.IsValid) { -- solo necesito el codigo
            ModelState.Clear();
            if (model.Acv != null && model.Acv.Codigo > 0) {
                try {
                    model.Ejecutar();
                    return RedirectToAction("ReciboAnulacion", AcvsController.CONTROLLER, new { id = model.Acv.Codigo });
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View("Anular", model);
                }
            } else {
                ModelState.AddModelError("Acv.Codigo", "Debe especificar la operacion a anular");
            }
            return View("Anular", model);

        }

        public ActionResult ReciboAnulacion(int id) {
            TRACuentaVentaAnulacion tr = new TRACuentaVentaAnulacion();
            tr.Acv = new ACuentaVenta();
            tr.Acv.Codigo = id;
            ViewData["idParametros"] = id;
            return View("ReciboAnulacion", tr.Acv);
        }

        private XtraReport _generarReciboAnulacion(int id) {
            TRACuentaVentaAnulacion tr = new TRACuentaVentaAnulacion();
            tr.Acv = new ACuentaVenta();
            tr.Acv.Codigo = id;
            tr.Consultar();
            List<TRACuentaVentaAnulacion> ll = new List<TRACuentaVentaAnulacion>();
            ll.Add(tr);
            XtraReport rep = new DXReciboACVAnulacion();
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

    }
}
