using AutomotoraWeb.Controllers.General;
using AutomotoraWeb.Models;
using AutomotoraWeb.Services;
using AutomotoraWeb.Utils;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DLL_Backend;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;

namespace AutomotoraWeb.Controllers.Sales.Maintenance {
    public class VehiculosController : SalesController, IMaintenance {

        public static string CONTROLLER = "vehiculos";
        public const string PHOTO_FOLDER = "~/Content/Images/vehiculos/";
        public const string DETAIL_GASTO = "detailGasto";
        public const string CREATE_GASTO = "createGasto";
        public const string EDIT_GASTO = "editGasto";
        public const string DELETE_GASTO = "deleteGasto";

        public const string OK = "OK";
        public const string ERROR = "ERROR";
        public const string VALIDATION_ERROR = "VALIDATION_ERROR";
        

        //No usarlo mas porque da lios de permisos al invocar esta accion si no tiene permisos full en vehiculos (ej: usuario de solo consulta a traves de listados)
        //ademas, ya no es mas el estandar.
        //public ContentResult NombreEntidad() {
        //    return new ContentResult { Content = "Vehiculo" };
        //}

        //public ContentResult NombreEntidades() {
        //    return new ContentResult { Content = "Vehiculos" };
        //}

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext); //esto tiene que ser lo primero, si no, falla si se llama directamente la consulta del usuario si nadie esta logueado
            ViewBag.NombreEntidades = "Vehiculos";
            ViewBag.NombreEntidad = "Vehiculo";
            Usuario usuario = (Usuario)(Session[SessionUtils.SESSION_USER]);
            // Verificamos que seguimos en una session (sino se redirige al login)
            if (usuario != null) {
                if (usuario.MultiSucursal) {
                    ViewBag.Sucursales = Sucursal.Sucursales;
                } else {
                    List<Sucursal> listSucursal = new List<Sucursal>();
                    listSucursal.Add(usuario.Sucursal);
                    ViewBag.Sucursales = listSucursal;
                }
                ViewBag.Departamentos = Departamento.Departamentos();
                ViewBag.TiposCombustible = DLL_Backend.TipoCombustible.TiposCombustible();
                ViewBag.Monedas = Moneda.Monedas;
            } else { 
                filterContext.Result = new RedirectResult("/" + AuthenticationController.CONTROLLER + "/" + AuthenticationController.LOGIN);
            }
        }


        //public ActionResult Show([ModelBinder(typeof(DevExpressEditorsBinder))] Vehiculo vehiculo) {
        public ActionResult Show() {
            return View(_listaElementos());
        }

        public ActionResult ListaGrilla() {
            return PartialView("_listGrilla", _listaElementos());
        }

        public ActionResult Details(int id) {
            Session[SessionUtils.CODIGO_VEHICULO] = id;
            ViewData["idParametros"] = id.ToString();
            ViewBag.SoloLectura = true;
            return VistaElemento(id);
        }

        public ActionResult Create() {

            return View();
        }

        public ActionResult Edit(int id) {
            return VistaElemento(id);
        }

        public ActionResult Delete(int id) {
            ViewBag.SoloLectura = true;
            return VistaElemento(id);
        }

        //-----------------------------------------------------------------------------------------------------


        private ActionResult VistaElemento(int id) {
            try {
                Vehiculo vehiculo = _obtenerElemento(id);
                return View(vehiculo);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private Vehiculo _obtenerElemento(int id) { //Trae los datos del elemento de la base de datos y los pone en un objeto.
            Vehiculo vehiculo = new Vehiculo();
            vehiculo.Codigo = id;
            vehiculo.Consultar();
            return vehiculo;
        }

        private List<Vehiculo> _listaElementos() {
            return Vehiculo.Vehiculos(Vehiculo.VHC_TIPO_LISTADO.TODOS);
        }

        //-----------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Create(Vehiculo vehiculo) {

            this.eliminarValidacionesIgnorables(vehiculo);

            if (ModelState.IsValid) {
                try {
                    vehiculo.Agregar();
                    return RedirectToAction(BaseController.SHOW);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(vehiculo);
                }
            }

            return View(vehiculo);
        }

        private void eliminarValidacionesIgnorables(Vehiculo vehiculo) {
            this.eliminarValidacionesIgnorables("Departamento", MetadataManager.IgnorablesDDL(vehiculo.Departamento));
            this.eliminarValidacionesIgnorables("TipoCombustible", MetadataManager.IgnorablesDDL(vehiculo.TipoCombustible));
            this.eliminarValidacionesIgnorables("Costo.Moneda", MetadataManager.IgnorablesDDL(vehiculo.Costo.Moneda));
            this.eliminarValidacionesIgnorables("PrecioVenta.Moneda", MetadataManager.IgnorablesDDL(vehiculo.PrecioVenta.Moneda));
            this.eliminarValidacionesIgnorables("Sucursal", MetadataManager.IgnorablesDDL(vehiculo.Sucursal));
        }

        //-----------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Edit(Vehiculo vehiculo) {

            this.eliminarValidacionesIgnorables(vehiculo);

            if (ModelState.IsValid) {
                try {
                    vehiculo.ModificarDatos();
                    return RedirectToAction(BaseController.SHOW);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(vehiculo);
                }
            }

            return View(vehiculo);
        }



        [HttpPost]
        public ActionResult Delete(Vehiculo vehiculo) {
            ViewBag.SoloLectura = true;

            this.eliminarValidacionesIgnorables(vehiculo); //aca tambien se necesita porque  se llama al model.isvalid

            if (ModelState.IsValid) {

                try {
                    string userName = (string)HttpContext.Session.Contents[SessionUtils.SESSION_USER_NAME];
                    string IP = HttpContext.Request.UserHostAddress;
                    vehiculo.Eliminar(userName, IP);
                    return RedirectToAction(BaseController.SHOW);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(vehiculo);
                }
            }
            return View(vehiculo);

        }

        //--------------------------METODOS PARA LISTADOS DE VEHICULO -----------------------------

        #region Listados
        public ActionResult List() {
            ListadoVehiculosModel model = new ListadoVehiculosModel();
            string s = SessionUtils.generarIdVarSesion("ListadoVehiculos", Session[SessionUtils.SESSION_USER].ToString());
            Session[s] = model;
            model.idParametros = s;
            model.Formato = ListadoVehiculosModel.FORMATO_LISTADO.ABREVIADO;
            model.Filtro.Tipo = Vehiculo.VHC_TIPO_LISTADO.LIBRES;
            model.Filtro.Categoria = VehiculoFiltro.VHC_CATEGORIA_LISTADO.TODOS;
            ViewBag.SucursalesListado = Sucursal.Sucursales;
            ViewBag.TiposComubstiblesListado = TipoCombustible.TiposCombustible();
            ViewData["idParametros"] = model.idParametros;
            model.Resultado = _listaElementos(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult List(ListadoVehiculosModel model, string btnSubmit) {
            Session[model.idParametros] = model; //filtros actualizados
            ViewData["idParametros"] = model.idParametros;
            ViewBag.SucursalesListado = Sucursal.Sucursales;
            ViewBag.TiposComubstiblesListado = TipoCombustible.TiposCombustible();
            this.eliminarValidacionesIgnorables("Filtro.Sucursal", MetadataManager.IgnorablesDDL(model.Filtro.Sucursal));
            if (ModelState.IsValid) {
                if (btnSubmit == "Imprimir") {
                    return this.Report(model);
                }
                model.Resultado = _listaElementos(model);
            }
            return View(model);
        }

        public ActionResult ReportGrilla(string idParametros) {
            ListadoVehiculosModel model = (ListadoVehiculosModel)Session[idParametros];
            model.Resultado = _listaElementos(model);
            ViewData["idParametros"] = model.idParametros;
            return PartialView("_reportGrilla", model);
        }

        private List<Vehiculo> _listaElementos(ListadoVehiculosModel model) {
            model.AcomodarFiltro();
            return Vehiculo.Vehiculos(model.Filtro);
        }

        #endregion


        //---------- METODOS PARA REPORTES DE LISTADOS DE VEHICULOS  -----------------------------

        #region Reportes
        public ActionResult Report(ListadoVehiculosModel model) {
            return View("report", model);
        }

        public ActionResult ReportPartial(string idParametros) {
            ListadoVehiculosModel model = null;
            XtraReport rep = new DXListadoVehiculosAbreviado();
            model = (ListadoVehiculosModel)Session[idParametros];
            if (model.Formato == ListadoVehiculosModel.FORMATO_LISTADO.COMPLETO) {
                rep = new DXListadoVehiculosCompleto();
            }
            rep.DataSource = _listaElementos(model);
            setParamsToReport(rep, model); // lo hago despues porque listaElementos acomoda los filtros en model
            Session[idParametros] = model;
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reportList");
        }

        private void setParamsToReport(XtraReport report, ListadoVehiculosModel model) {
            Parameter paramSystemName = new Parameter();
            paramSystemName.Name = "detalleFiltros";
            paramSystemName.Type = typeof(string);
            paramSystemName.Value = model.detallesFiltro();
            paramSystemName.Description = "Detalle Filtros";
            paramSystemName.Visible = false;
            report.Parameters.Add(paramSystemName);
        }

        public ActionResult ReportExport(string idParametros) {
            ListadoVehiculosModel model = null;
            XtraReport rep = new DXListadoVehiculosAbreviado();
            model = (ListadoVehiculosModel)Session[idParametros];
            if (model.Formato == ListadoVehiculosModel.FORMATO_LISTADO.COMPLETO) {
                rep = new DXListadoVehiculosCompleto();
            }
            setParamsToReport(rep, model);
            rep.DataSource = _listaElementos(model);
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);

        }
        #endregion


        //---------- METODOS PARA REPORTES DE FICHA DE VEHICULO  -----------------------------

        #region ReporteFicha

        public ActionResult Report2(int idVehiculo) {
            Vehiculo vhc = new Vehiculo();
            vhc.Codigo = idVehiculo;
            ViewData["idParametros"] = idVehiculo;
            return View(vhc);
        }

        public ActionResult ReportPartial2(int idParametros) {
            DXReportFichaVehiculo rep = new DXReportFichaVehiculo();
            Vehiculo vhc = _obtenerElemento(idParametros);
            List<Vehiculo> lista = new List<Vehiculo>();
            lista.Add(vhc);
            rep.DataSource = lista;
            ViewData["Report"] = rep;
            ViewData["idParametros"] = vhc.Codigo;
            return PartialView("_reportDetalle");
        }

        public ActionResult ReportExport2(int idParametros) {
            DXReportFichaVehiculo rep = new DXReportFichaVehiculo();
            Vehiculo vhc = _obtenerElemento(idParametros);
            List<Vehiculo> lista = new List<Vehiculo>();
            lista.Add(vhc);
            rep.DataSource = lista;
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        #endregion


        //---------------------------------------------------------------------

        #region Gastos

        public ActionResult listGastos(int idParametros) {
            ViewData["idParametros"] = idParametros;
            return PartialView("_listGastos", _listaGastos(idParametros));
        }

        public ActionResult detailGasto(int id) {
            Gasto gasto = this._obtenerGasto(id);
            ViewBag.SoloLectura = true;
            return PartialView("_popupGastos", gasto);
        }

        public ActionResult createGasto(int idVehiculo) {
            Vehiculo vehiculo = new Vehiculo();
            vehiculo.Codigo = idVehiculo;
            vehiculo.Consultar();
            Gasto gasto = new Gasto();
            gasto.Fecha = DateTime.Now;
            gasto.Vehiculo = vehiculo;
            return PartialView("_popupGastos", gasto);
        }

        public ActionResult editGasto(int id) {
            Gasto gasto = this._obtenerGasto(id);
            return PartialView("_popupGastos", gasto);
        }

        public ActionResult deleteGasto(int id) {
            Gasto gasto = this._obtenerGasto(id);
            ViewBag.SoloLectura = true;
            return PartialView("_popupGastos", gasto);
        }

        //-------------------------------


        [HttpPost]
        public JsonResult createGasto(Gasto gasto) {

            Vehiculo vehiculo = gasto.Vehiculo;
            vehiculo.Consultar();
            gasto.Vehiculo = vehiculo;
            gasto.ImporteGasto.Moneda.Consultar();
            gasto.Cotizacion = gasto.ImporteGasto.Moneda.Cotizacion;

            List<String> errors = this.validateAtributesGastos(gasto);

            if (errors.Count == 0) {
                try {
                    gasto.Agregar();
                    return Json(new { Result = "OK" });
                } catch (UsuarioException exc) {
                    return Json(new { Result = "ERROR", ErrorCode = exc.Codigo, ErrorMessage = exc.Message });
                }
            }

            return Json(new { Result = "ERROR", ErrorCode = "VALIDATION_ERROR", ErrorMessage = errors.ToArray() });
        }

        [HttpPost]
        public ActionResult editGasto(Gasto gasto) {

            if (ModelState.IsValid) {
                try {
                    gasto.ModificarDatos();
                    return Json(new { Result = "OK" });
                } catch (UsuarioException exc) {
                    return Json(new { Result = "ERROR", ErrorCode = exc.Codigo, ErrorMessage = exc.Message });
                }
            }

            return Json(new { Result = "ERROR", ErrorCode = "VALIDATION_ERROR", ErrorMessage = "Uno de los campos ingresados no es válido" });
        }

        [HttpPost]
        public ActionResult deleteGasto(Gasto gasto) {
            try {
                gasto.Agregar();
                return Json(new { Result = "OK" });
            } catch (UsuarioException exc) {
                return Json(new { Result = "ERROR", ErrorCode = exc.Codigo, ErrorMessage = exc.Message });
            }
        }

        //-------------------------------

        private Gasto _obtenerGasto(int id) { //Trae los datos del elemento de la base de datos y los pone en un objeto.
            Gasto gasto = new Gasto();
            gasto.Codigo = id;
            //gasto.Consultar();
            return gasto;
        }

        //-------------------------------

        private List<String> validateAtributesGastos(Gasto gasto) {

            List<String> errors = new List<String>();

            if ((gasto.Fecha == null) || (gasto.Fecha.Ticks == 0)) {
                errors.Add("El campo Fecha es obligatorio");
            }
            
            if ((gasto.ImporteGasto == null) || (gasto.ImporteGasto.Monto <= 0)) {
                errors.Add("El campo Importe es obligatorio, y debe ser mayor a 0"); 
            }
            
            if ((gasto.Descripcion == null) || (gasto.Descripcion == "")) {
                errors.Add("El campo Descripcion es obligatorio");
            }
            
            if ((gasto.Descripcion != null) && (gasto.Descripcion.Length > 80)) {
                errors.Add("El campo Descripcion debe tener como maximo 80 caracteres");
            }
            
            if ((gasto.Observaciones != null) && (gasto.Observaciones.Length > 80)) {
                errors.Add("El campo Observaciones debe tener como maximo 80 caracteres");
            } 

            return errors;
        }

        //-------------------------------

        private List<Gasto> _listaGastos(int id) {
                Vehiculo vehiculo = new Vehiculo();
                vehiculo.Codigo = id;
                vehiculo.Consultar();
                return vehiculo.DetalleGastos;
        }

        #endregion

        //---------------------------------------------------------------------
    }
}
