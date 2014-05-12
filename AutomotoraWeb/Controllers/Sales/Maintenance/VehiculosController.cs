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

        public ContentResult NombreEntidad() {
            return new ContentResult { Content = "Vehiculo" };
        }

        public ContentResult NombreEntidades() {
            return new ContentResult { Content = "Vehiculos" };
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            ViewBag.NombreEntidad = "Vehiculos";
            Usuario usuario = (Usuario)(Session[SessionUtils.SESSION_USER]);
            if (usuario.MultiSucursal){
                ViewBag.Sucursales = Sucursal.Sucursales();
            }
            else{
                List<Sucursal> listSucursal = new List<Sucursal>();
                listSucursal.Add(usuario.Sucursal);
                ViewBag.Sucursales =  listSucursal;
            }

            ViewBag.Departamentos = Departamento.Departamentos();
            ViewBag.TiposCombustible = DLL_Backend.TipoCombustible.TiposCombustible();
            ViewBag.Monedas = Moneda.Monedas;

            base.OnActionExecuting(filterContext);
        }


        public ActionResult Show([ModelBinder(typeof(DevExpressEditorsBinder))] Vehiculo vehiculo) {
            return View(_listaElementos());
        }

        public ActionResult ListaGrilla() {
            return PartialView("_listGrilla", _listaElementos());
        }

        //----------reporte ficha vehiculo -----------------------------
        public ActionResult Report2(int id) {
            // Add a report to the view data. 
            Vehiculo cli = _obtenerElemento(id);
            //setParamsToReport(rep);
            DXReportFichaVehiculo rep = new DXReportFichaVehiculo();
            ViewData["Report"] = rep;
            return View(cli);
        }

        public ActionResult ReportPartial2(int id) {
            DXReportFichaVehiculo rep = new DXReportFichaVehiculo();
            //setParamsToReport(rep);
            Vehiculo cli = _obtenerElemento(id);
            List<Vehiculo> lista = new List<Vehiculo>();
            lista.Add(cli);
            rep.DataSource = lista;
            ViewData["Report"] = rep;
            return PartialView("_reportDetalle", cli);
        }

        public ActionResult ReportExport2(int id) {
            DXReportFichaVehiculo rep = new DXReportFichaVehiculo();
            //setParamsToReport(rep);
            Vehiculo cli = _obtenerElemento(id);
            List<Vehiculo> lista = new List<Vehiculo>();
            lista.Add(cli);
            rep.DataSource = lista;
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        //----------reporte listado de vehiculos  -----------------------------

        public ActionResult Report() {
            // Add a report to the view data. 
            DXReportVehiculos rep = new DXReportVehiculos();
            //setParamsToReport(rep);
            ViewData["Report"] = rep;
            return View();
        }

        public ActionResult ReportPartial() {
            DXReportVehiculos rep = new DXReportVehiculos();
            //setParamsToReport(rep);
            rep.DataSource = _listaElementos();
            ViewData["Report"] = rep;
            return PartialView("_reportList");
        }

        public ActionResult ReportExport() {
            DXReportVehiculos rep = new DXReportVehiculos();
            //setParamsToReport(rep);
            rep.DataSource = Vehiculo.Vehiculos(Vehiculo.VHC_TIPO_LISTADO.TODOS);
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        //--------------------------------------------------------------------------------------------------

        public ActionResult Details(int id) {
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
            this.eliminarValidacionesIgnorables("Costo.Moneda", MetadataManager.IgnorablesDDL(vehiculo.Costo));
            this.eliminarValidacionesIgnorables("PrecioVenta.Moneda", MetadataManager.IgnorablesDDL(vehiculo.PrecioVenta));
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

        //-----------------------------------------------------------------------------------------------------

    }
}
