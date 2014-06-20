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

namespace AutomotoraWeb.Controllers.Sales
{
    public class PedidosController : SalesController
    {

        public static string CONTROLLER = "Pedidos";
        public const string RECIBIR = "Recibir";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            ViewBag.NombreEntidad = "Pedido";
            ViewBag.NombreEntidades = "Pedidos";
            ViewBag.Monedas = Moneda.Monedas;
            ViewBag.Clientes = Cliente.Clientes();
            ViewBag.Vendedores = Vendedor.Vendedores(Vendedor.VEND_TIPO_LISTADO.HABILITADOS);
            Usuario usuario = (Usuario)(Session[SessionUtils.SESSION_USER]);
            if (usuario== null){
                filterContext.Result = new RedirectResult("/" + AuthenticationController.CONTROLLER + "/" + AuthenticationController.LOGIN);
                return;
            }
            if (usuario.MultiSucursal) {
                ViewBag.Sucursales = Sucursal.Sucursales;
            } else {
                List<Sucursal> listSucursal = new List<Sucursal>();
                listSucursal.Add(usuario.Sucursal);
                ViewBag.Sucursales = listSucursal;
            }
        }


        //--------------------------METODOS PARA GESTION DE pedidos  -----------------------------
        #region Gestion

        public ActionResult Show() {
            return View(_listaElementos());
        }

        public ActionResult ListaGrilla() {
            return PartialView("_listGrilla", _listaElementos());
        }

        public ActionResult Details(int id) {
            ViewBag.SoloLectura = true;
            return VistaElemento(id);
        }

        public ActionResult Create() {
            Pedido ped = new Pedido();
            //ped.Cliente = new Cliente();
            //ped.Vendedor = new Vendedor();
            ped.FechaPedido = DateTime.Now.Date;
            return View(ped);
        }

        public ActionResult Edit(int id) {
            return VistaElemento(id);
        }

        public ActionResult Delete(int id) {
            ViewBag.SoloLectura = true;
            return VistaElemento(id);
        }

        public ActionResult VerSenia(int id) {
            ViewBag.SoloLectura = true;
            Pedido ped = new Pedido();
            ped.Codigo = id;
            ped.Consultar();
            if (ped.Seniado) {
                Senia s = ped.ObtenerSenia();
                if (s != null) {
                    return RedirectToAction(BaseController.DETAILS, SeniasController.CONTROLLER, new { id = s.Codigo });
                }
                return RedirectToAction(BaseController.DETAILS, new { id = id });
            }

            return RedirectToAction(BaseController.DETAILS, new { id = id });
        }

        public ActionResult Recibir(int id) {
            ViewBag.SoloLectura = true;
            return VistaElemento(id);
        }

        //-----------------------------------------------------------------------------------------------------


        private ActionResult VistaElemento(int id) {
            try {
                Pedido td = _obtenerElemento(id);
                //if (td.Cliente == null) td.Cliente = new Cliente();
                //if (td.Vendedor == null) td.Vendedor = new Vendedor();
                return View(td);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private Pedido _obtenerElemento(int id) {
            Pedido td = new Pedido();
            td.Codigo = id;
            td.Consultar();
            return td;
        }

        private List<Pedido> _listaElementos() {
            return Pedido.Pedidos(Pedido.PED_TIPO_LISTADO.MODIFICABLES);
        }

        //-----------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Create(Pedido td) {

            this.eliminarValidacionesIgnorables(td);
            
            if (ModelState.IsValid) {
                try {
                    td.Agregar();
                    return RedirectToAction(BaseController.SHOW);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(td);
                }
            }

            return View(td);
        }

        //-----------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Edit(Pedido td) {

            this.eliminarValidacionesIgnorables(td);

            if (ModelState.IsValid) {
                try {
                    string userName = (string)HttpContext.Session.Contents[SessionUtils.SESSION_USER_NAME];
                    string IP = HttpContext.Request.UserHostAddress;
                    Usuario u = new Usuario();
                    u.Username = userName;
                    td.ModificarDatos(u, IP);
                    return RedirectToAction(BaseController.SHOW);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(td);
                }
            }

            return View(td);
        }



        [HttpPost]
        public ActionResult Delete(Pedido td) {

            this.eliminarValidacionesIgnorables(td);
            ViewBag.SoloLectura = true;
            if (ModelState.IsValid) {
                try {
                    string userName = (string)HttpContext.Session.Contents[SessionUtils.SESSION_USER_NAME];
                    string IP = HttpContext.Request.UserHostAddress;
                    td.Eliminar(userName, IP);
                    return RedirectToAction(BaseController.SHOW);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(td);
                }
            }

            return View(td);
        }

        private void eliminarValidacionesIgnorables(Pedido ped) {
            this.eliminarValidacionesIgnorables("Costo.Moneda", MetadataManager.IgnorablesDDL(ped.Costo.Moneda));
            this.eliminarValidacionesIgnorables("Sucursal", MetadataManager.IgnorablesDDL(ped.Sucursal));
            this.eliminarValidacionesIgnorables("Cliente", MetadataManager.IgnorablesDDL(ped.Cliente));
            this.eliminarValidacionesIgnorables("Vendedor", MetadataManager.IgnorablesDDL(ped.Vendedor));

            if (!ped.Reservado) {
                //Lo tengo que sacar porque las ddls siempre el ponen un valor aunque no corresponda
                ped.Cliente = null;
                ped.Vendedor = null;
                ped.FechaReserva = null;
            }
        }

        
        #endregion




        //--------------------------METODOS PARA LISTADOS DE pedidos  -----------------------------

        #region Listados

        public ActionResult List() {

            ListadoPedidosModel model = new ListadoPedidosModel();
            string s = SessionUtils.generarIdVarSesion("ListadoPedidos", Session[SessionUtils.SESSION_USER].ToString());
            Session[s] = model;
            model.idParametros = s;
            ViewBag.SucursalesListado = Sucursal.Sucursales;
            ViewBag.ClientesListado = Cliente.Clientes();
            ViewBag.VendedoresListado = Vendedor.Vendedores(Vendedor.VEND_TIPO_LISTADO.TODOS);
            ViewData["idParametros"] = model.idParametros;
            model.Resultado = _listaElementos(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult List(ListadoPedidosModel model, string btnSubmit) {
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
        }

        public ActionResult ReportGrilla(string idParametros) {
            ListadoPedidosModel model = (ListadoPedidosModel)Session[idParametros];
            model.Resultado = _listaElementos(model);
            ViewData["idParametros"] = model.idParametros;
            return PartialView("_reportGrilla", model);
        }

        private List<Pedido> _listaElementos(ListadoPedidosModel model) {
            model.AcomodarFiltro();
            return Pedido.Pedidos(model.Filtro);
        }

        #endregion

        //---------- METODOS PARA REPORTES DE LISTADOS DE Pedidos  -----------------------------

        #region Reportes
        public ActionResult Report(ListadoPedidosModel model) {
            return View("report", model);
        }

        public ActionResult ReportPartial(string idParametros) {
            ListadoPedidosModel model = null;
            XtraReport rep = new DXListadoPedidos();
            model = (ListadoPedidosModel)Session[idParametros];
            rep.DataSource = _listaElementos(model);
            setParamsToReport(rep, model); // lo hago despues porque listaElementos acomoda los filtros en model
            Session[idParametros] = model;
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reportList");
        }

        private void setParamsToReport(XtraReport report, ListadoPedidosModel model) {
            Parameter paramSystemName = new Parameter();
            paramSystemName.Name = "detalleFiltros";
            paramSystemName.Type = typeof(string);
            paramSystemName.Value = model.detallesFiltro();
            paramSystemName.Description = "Detalle Filtros";
            paramSystemName.Visible = false;
            report.Parameters.Add(paramSystemName);
        }

        public ActionResult ReportExport(string idParametros) {
            ListadoPedidosModel model = null;
            XtraReport rep = new DXListadoPedidos();
            model = (ListadoPedidosModel)Session[idParametros];
            setParamsToReport(rep, model);
            rep.DataSource = _listaElementos(model);
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);

        }
        #endregion



    }
}
