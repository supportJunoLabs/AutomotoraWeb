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
using AutomotoraWeb.Controllers.Sistema;
using AutomotoraWeb.Services;

namespace AutomotoraWeb.Controllers.Sales {
    public class PedidosController : SalesController {

        public static string CONTROLLER = "Pedidos";
        public const string RECIBIR = "Recibir";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            ViewBag.NombreEntidad = "Pedido";
            ViewBag.NombreEntidades = "Pedidos";
            ViewBag.Monedas = Moneda.Monedas;
            ViewBag.Clientes = Cliente.Clientes();
            ViewBag.Vendedores = Vendedor.Vendedores(Vendedor.VEND_TIPO_LISTADO.HABILITADOS);
            ViewBag.Departamentos = Departamento.Departamentos();
            ViewBag.TiposCombustible = TipoCombustible.TiposCombustible();

            Usuario usuario = getUsuario();
            if (usuario == null) {
                filterContext.Result = new RedirectResult("/" + AuthenticationController.CONTROLLER + "/" + AuthenticationController.LOGIN);
                return;
            }
            ViewBag.MultiSucursal = usuario.MultiSucursal;
            ViewBag.Sucursales = Sucursal.Sucursales;
        }


        //--------------------------METODOS PARA GESTION DE pedidos  -----------------------------
        #region Gestion

        //private bool PedidoConsultable(Pedido ped) {
        //    if (ped == null || ped.Codigo == 0) return true;
        //    Usuario usuario = getUsuario();
        //    if (!SecurityService.Instance.verInfoAntigua(usuario) && ped.Antiguo) {
        //        return false;
        //    }
        //    return true;
        //}

        public ActionResult Show() {
            Usuario u = getUsuario();
            return View(Pedido.Pedidos(Pedido.PED_TIPO_LISTADO.MODIFICABLES, u));
        }

        public ActionResult ListaGrilla() {
            Usuario u = getUsuario();
            return PartialView("_listGrilla", Pedido.Pedidos(Pedido.PED_TIPO_LISTADO.MODIFICABLES,u));
        }

        public ActionResult Details(int id) {
            ViewBag.SoloLectura = true;
            try {
                Pedido td = new Pedido();
                td.Codigo = id;
                Usuario u = getUsuario();
                td.Consultar(u);
                return View(td);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        public ActionResult SubconsultaDetalle(int id) {
            ViewBag.SoloLectura = true;
            Pedido td = new Pedido();
            td.Codigo = id;
            Usuario u = getUsuario();
            td.Consultar(u);
            return PartialView("_datosDetalle", td);
        }

        public ActionResult Create() {
            Pedido ped = new Pedido();
            ped.FechaPedido = DateTime.Now.Date;
            ped.Sucursal = (getUsuario()).Sucursal;
            return View(ped);
        }

        public ActionResult Edit(int id) {
            try {
                Pedido td = new Pedido();
                td.Codigo = id;
                Usuario u = getUsuario();
                td.Consultar(u);
                return View(td);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        public ActionResult Delete(int id) {
            ViewBag.SoloLectura = true;
            try {
                Pedido td = new Pedido();
                td.Codigo = id;
                Usuario u = getUsuario();
                td.Consultar(u);
                return View(td);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        public ActionResult VerSenia(int id) {
            try {
                ViewBag.SoloLectura = true;
                Pedido ped = new Pedido();
                ped.Codigo = id;
                Usuario u = getUsuario();
                ped.Consultar(u);
                if (ped.Seniado) {
                    Senia s = ped.ObtenerSenia();
                    if (s != null) {
                        return RedirectToAction(BaseController.DETAILS, SeniasController.CONTROLLER, new { id = s.Codigo });
                    }
                    return RedirectToAction(BaseController.DETAILS, new { id = id });
                }

                return RedirectToAction(BaseController.DETAILS, new { id = id });
            } catch {
                return RedirectToAction("Mensaje", SistemaController.CONTROLLER, new { id = SistemaController.MSJ_ERROR});
            }
        }

        public ActionResult Recibir(int id) {
            try {
                Pedido ped = new Pedido();
                ped.Codigo = id;
                Usuario u = getUsuario();
                ped.Consultar(u);
                ped.Vehiculo = new Vehiculo();
                ped.Vehiculo.Marca = ped.Marca;
                ped.Vehiculo.Modelo = ped.Modelo;
                ped.Vehiculo.Color = ped.Color;
                ped.Vehiculo.Costo = ped.Costo;
                ped.FechaRecibido = DateTime.Now.Date;
                ped.Vehiculo.FechaAdquirido = ped.FechaRecibido ?? DateTime.Now.Date;
                ped.Vehiculo.Sucursal = ped.Sucursal;

                return View(ped);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        [HttpPost]
        public ActionResult Recibir(Pedido ped) {
            this.eliminarValidacionesIgnorables("Vehiculo.Departamento", MetadataManager.IgnorablesDDL(new Departamento()));
            this.eliminarValidacionesIgnorables("Vehiculo.TipoCombustible", MetadataManager.IgnorablesDDL(new TipoCombustible()));
            this.eliminarValidacionesIgnorables("Vehiculo.Costo.Moneda", MetadataManager.IgnorablesDDL(new Moneda()));
            this.eliminarValidacionesIgnorables("Vehiculo.PrecioVenta.Moneda", MetadataManager.IgnorablesDDL(new Moneda()));
            this.eliminarValidacionesIgnorables("Vehiculo.Sucursal", MetadataManager.IgnorablesDDL(new Sucursal()));

            this.eliminarValidacionesIgnorables("Cliente",  MetadataManager.IgnorablesDDL(new Cliente()));
            this.eliminarValidacionesIgnorables("Vendedor", MetadataManager.IgnorablesDDL(new Vendedor()));
            this.eliminarValidacionesIgnorables("Costo.Moneda", MetadataManager.IgnorablesDDL(new Moneda()));
            this.eliminarValidacionesIgnorables("Sucursal", MetadataManager.IgnorablesDDL(new Sucursal()));


            ModelState.Remove("Sucursal"); //se toma del vehiculo
            ModelState.Remove("Cliente.Codigo"); //puede no estar reservado, no tiene cliente ni vendedor
            ModelState.Remove("Vendedor.Codigo");

            if (ModelState.IsValid) {
                try {
                    ped.FechaRecibido = DateTime.Now.Date; //ver despues si lo permito cambiar manualmente
                    ped.Vehiculo.FechaAdquirido = ped.FechaRecibido??DateTime.Now.Date;
                    ped.Vehiculo.Sucursal = ped.Sucursal;
                    Usuario u = new Usuario();
                    u.UserName = getUserName();
                    string IP = getIP();
                    ped.RecibirPedido(u, IP);
                    return RedirectToAction(BaseController.DETAILS, VehiculosController.CONTROLLER, new { id = ped.Vehiculo.Codigo });
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    _completarDatos(ped);
                    return View(ped);
                }
            }
            _completarDatos(ped);
            return View(ped);
        }

        private void _completarDatos(Pedido ped){
            Vehiculo v1 = new Vehiculo(ped.Vehiculo); //para no perder lo que ingreso el usuario
            Usuario u = getUsuario();
            ped.Consultar(u);  //para volver a trare los datos de las entidades asociadas (ej: cliente, vendedor, etc)
            ped.Vehiculo=v1;
        }

        //-----------------------------------------------------------------------------------------------------


        //private ActionResult VistaElemento(int id) {
        //    try {
        //        Pedido td = new Pedido();
        //        td.Codigo = id;
        //        td.Consultar();
        //        return View(td);
        //    } catch (UsuarioException exc) {
        //        ViewBag.ErrorCode = exc.Codigo;
        //        ViewBag.ErrorMessage = exc.Message;
        //        return View();
        //    }
        //}

      //private List<Pedido> _listaElementos() {
      //      return Pedido.Pedidos(Pedido.PED_TIPO_LISTADO.MODIFICABLES);
      //  }

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
                    string userName = getUserName();
                    string IP = getIP();
                    Usuario u = new Usuario();
                    u.UserName = userName;
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

            //this.eliminarValidacionesIgnorables(td);
            ViewBag.SoloLectura = true;
            //if (ModelState.IsValid) {
                try {
                    string userName = getUserName();
                    string IP = getIP();
                    td.Eliminar(userName, IP);
                    return RedirectToAction(BaseController.SHOW);
                } catch (UsuarioException exc) {
                    Usuario u = getUsuario();
                    td.Consultar(u); //para traer lo datos a mostrar
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    ModelState.Clear();
                    return View(td);
                }
            //}

            //return View(td);
        }

        private void eliminarValidacionesIgnorables(Pedido ped) {
            this.eliminarValidacionesIgnorables("Costo.Moneda", MetadataManager.IgnorablesDDL(new Moneda()));
            this.eliminarValidacionesIgnorables("Sucursal", MetadataManager.IgnorablesDDL(new Sucursal()));
            this.eliminarValidacionesIgnorables("Cliente", MetadataManager.IgnorablesDDL(new Cliente()));
            this.eliminarValidacionesIgnorables("Vendedor", MetadataManager.IgnorablesDDL(new Vendedor()));

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

         [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult List() {

            ListadoPedidosModel model = new ListadoPedidosModel();
            try{
            string s = SessionUtils.generarIdVarSesion("ListadoPedidos", getUserName());
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
        public ActionResult List(ListadoPedidosModel model, string btnSubmit) {
            try{
            Session[model.idParametros] = model; //filtros actualizados
            ViewData["idParametros"] = model.idParametros;
            ViewBag.SucursalesListado = Sucursal.Sucursales;
            ViewBag.ClientesListado = Cliente.Clientes();
            ViewBag.VendedoresListado = Vendedor.Vendedores(Vendedor.VEND_TIPO_LISTADO.TODOS);
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
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(model);
            }
        }

        public ActionResult ReportGrilla(string idParametros) {
            ListadoPedidosModel model = (ListadoPedidosModel)Session[idParametros];
            model.Resultado = _listaElementos(model);
            ViewData["idParametros"] = model.idParametros;
            return PartialView("_reportGrilla", model);
        }

        private List<Pedido> _listaElementos(ListadoPedidosModel model) {
            model.AcomodarFiltro();
            Usuario u = getUsuario();
            List<Pedido> lista= Pedido.Pedidos(model.Filtro, u);
            return lista;
        }

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

        public ActionResult VerVehiculo(int id) {
            return RedirectToAction(BaseController.DETAILS, VehiculosController.CONTROLLER, new { id = id });
        }



    }
}
