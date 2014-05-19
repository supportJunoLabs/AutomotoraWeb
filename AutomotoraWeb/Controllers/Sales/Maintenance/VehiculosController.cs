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

        public ContentResult NombreEntidad() {
            return new ContentResult { Content = "Vehiculo" };
        }

        public ContentResult NombreEntidades() {
            return new ContentResult { Content = "Vehiculos" };
        }

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


        public ActionResult Show([ModelBinder(typeof(DevExpressEditorsBinder))] Vehiculo vehiculo) {
            return View(_listaElementos());
        }

        public ActionResult ListaGrilla() {
            return PartialView("_listGrilla", _listaElementos());
        }

        public ActionResult Details(int id) {
            Session[SessionUtils.CODIGO_VEHICULO] = id;
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

    }
}
