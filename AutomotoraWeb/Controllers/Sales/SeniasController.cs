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
    public class SeniasController : SalesController
    {

        public static string CONTROLLER = "Senias";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            ViewBag.NombreEntidad = "Seña";
            ViewBag.NombreEntidades = "Señas";
            
        }

        public ActionResult Index(){
            return View();
        }

        public ActionResult Details(int id) {
            ViewBag.SoloLectura = true;
            return VistaElemento(id);
        }

        private ActionResult VistaElemento(int id) {
            try {
                Senia td = _obtenerElemento(id);
                if (td.EsSeniaPedido()) {
                    ViewBag.NombreEntidad = "Seña Pedido";
                } else {
                    ViewBag.NombreEntidad = "Seña Vehículo";
                }

                return View(td);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private Senia _obtenerElemento(int id) {
            Senia td = new Senia();
            td.Codigo = id;
            td.Consultar();
            return td;
        }

        //--------------------------METODOS PARA LISTADOS DE senias  -----------------------------

        #region Listados

        public ActionResult List() {

            ListadoSeniasModel model = new ListadoSeniasModel();
            try{
            string s = SessionUtils.generarIdVarSesion("ListadoSenias", Session[SessionUtils.SESSION_USER].ToString());
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

        [HttpPost]
        public ActionResult List(ListadoSeniasModel model, string btnSubmit) {
            try{
            Session[model.idParametros] = model; //filtros actualizados
            ViewData["idParametros"] = model.idParametros;
            ViewBag.SucursalesListado = Sucursal.Sucursales;
            ViewBag.ClientesListado = Cliente.Clientes();
            ViewBag.VendedoresListado = Vendedor.Vendedores(Vendedor.VEND_TIPO_LISTADO.TODOS);
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
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(model);
            }
        }

        public ActionResult ReportGrilla(string idParametros) {
            ListadoSeniasModel model = (ListadoSeniasModel)Session[idParametros];
            model.Resultado = _listaElementos(model);
            ViewData["idParametros"] = model.idParametros;
            return PartialView("_reportGrilla", model);
        }

        private List<Senia> _listaElementos(ListadoSeniasModel model) {
            model.AcomodarFiltro();
            return Senia.Senias(model.Filtro);
        }

        #endregion
        //---------- METODOS PARA REPORTES DE LISTADOS DE Senias  -----------------------------

        #region Reportes
        public ActionResult Report(ListadoSeniasModel model) {
            return View("report", model);
        }

        public ActionResult ReportPartial(string idParametros) {
            ListadoSeniasModel model = null;
            XtraReport rep = new DXListadoSenias();
            model = (ListadoSeniasModel)Session[idParametros];
            rep.DataSource = _listaElementos(model);
            setParamsToReport(rep, model); // lo hago despues porque listaElementos acomoda los filtros en model
            Session[idParametros] = model;
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reportList");
        }

        private void setParamsToReport(XtraReport report, ListadoSeniasModel model) {
            Parameter paramSystemName = new Parameter();
            paramSystemName.Name = "detalleFiltros";
            paramSystemName.Type = typeof(string);
            paramSystemName.Value = model.detallesFiltro();
            paramSystemName.Description = "Detalle Filtros";
            paramSystemName.Visible = false;
            report.Parameters.Add(paramSystemName);
        }

        public ActionResult ReportExport(string idParametros) {
            ListadoSeniasModel model = null;
            XtraReport rep = new DXListadoSenias();
            model = (ListadoSeniasModel)Session[idParametros];
            setParamsToReport(rep, model);
            rep.DataSource = _listaElementos(model);
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);

        }
        #endregion


        #region DevolucionSenia

        public ActionResult ReciboDev(int id) {
            try {
                TRSeniaDevolucion tr = new TRSeniaDevolucion();
                tr.Senia = new Senia();
                tr.Senia.Codigo = id;
                tr.Consultar();
                    
                ViewData["idParametros"] = id;
                return View("ReciboDev", tr.Senia);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private XtraReport _generarReciboDev(int id) {
            TRSeniaDevolucion tr = new TRSeniaDevolucion();
            tr.Senia = new Senia();
            tr.Senia.Codigo = id;
            tr.Consultar();
            List<TRSeniaDevolucion> ll = new List<TRSeniaDevolucion>();
            ll.Add(tr);
            XtraReport rep = new DXReciboSeniaDevolucion();
            rep.DataSource = ll;
            return rep;
        }

        public ActionResult ReciboDevPartial(int idParametros) {
            XtraReport rep = _generarReciboDev(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reciboDev");
        }

        public ActionResult ReciboDevExport(int idParametros) {
            XtraReport rep = _generarReciboDev(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        #endregion


    }
}
