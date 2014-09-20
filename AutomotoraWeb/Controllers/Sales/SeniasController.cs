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
    public class SeniasController : SalesController {

        public static string CONTROLLER = "Senias";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            ViewBag.NombreEntidad = "Seña";
            ViewBag.NombreEntidades = "Señas";
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

        #region Consultar


        public ActionResult Index() {
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

        #endregion

        //--------------------------METODOS PARA LISTADOS DE senias  -----------------------------

        #region Listados

        public ActionResult List() {

            ListadoSeniasModel model = new ListadoSeniasModel();
            try {
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
            try {
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
                //tr.Consultar();  solo usa elcodigo en la vista
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

        #region Seniar

        public void iniSenia(SeniaModel tr, string idSession) {
            //Session[idSession] = tr;
            Session[idSession + SessionUtils.CHEQUES] = new List<Cheque>();
            Session[idSession + SessionUtils.EFECTIVO] = new List<Efectivo>();
            Session[idSession + SessionUtils.MOV_BANCARIO] = new List<MovBanco>();
            Session[idSession + SessionUtils.EFECTIVO_PROMESA] = new List<Efectivo>();
            Session[idSession + SessionUtils.CHEQUES_PROMESA] = new List<SeniaPromesa_ChequeVale>();
            Session[idSession + SessionUtils.VALES_PROMESA] = new List<SeniaPromesa_ChequeVale>();
            tr.Precondicion = new PrecondicionesOperacion();
        }

        private void iniSeniado(SeniaModel tr, string idSession, bool esPostback) {
            if (tr.PedidoVehiculo == 1) {
                tr.Senia.Pedido = null;
                tr.Senia.Vehiculo.Consultar();
            } else {
                tr.Senia.Vehiculo = null;
                tr.Senia.Pedido.Consultar();
            }
            tr.asignarPrecondicion(esPostback);
            if (!esPostback) {
                tr.Senia.Promesa.Financiacion = new Financiacion();
                tr.Senia.Promesa.Financiacion.MontoFinanciado = new Importe(Moneda.MonedaDefault, 0);
                tr.Senia.Promesa.Financiacion.Tasa = 0;
                tr.Senia.Promesa.Financiacion.CantCuotas = 0;
            }
            
            tr.Senia.Fecha = DateTime.Now;
            Usuario usuario = (Usuario)(Session[SessionUtils.SESSION_USER]);
            tr.Senia.Sucursal = usuario.Sucursal;

            tr.Senia.Pago.AgregarCheques((IEnumerable<Cheque>)Session[idSession + SessionUtils.CHEQUES]);
            tr.Senia.Pago.AgregarMovsBanco((IEnumerable<MovBanco>)Session[idSession + SessionUtils.MOV_BANCARIO]);
            tr.Senia.Pago.AgregarEfectivos((IEnumerable<Efectivo>)Session[idSession + SessionUtils.EFECTIVO]);

            tr.Senia.Promesa.agregarEfectivos((List<Efectivo>)Session[idSession + SessionUtils.EFECTIVO_PROMESA]);
            tr.Senia.Promesa.agregarCheques((List<SeniaPromesa_ChequeVale>)Session[idSession + SessionUtils.CHEQUES_PROMESA]);
            tr.Senia.Promesa.agregarVales((List<SeniaPromesa_ChequeVale>)Session[idSession + SessionUtils.VALES_PROMESA]);
        }

        public ActionResult PedidosSeniablesGrilla() {
            //Se invoca desde paginacion, ordenacion etc, de grilla de cuotas. Devuelve la partial del tab de cuotas
            GridLookUpModel gmodel = new GridLookUpModel { Opciones = Pedido.Pedidos(Pedido.PED_TIPO_LISTADO.SENIABLES) };
            return PartialView("_selectPedido", gmodel);
        }

        //FALTA OPCION PARA SENIAR PEDIDO Y VER DONDE IRIAN LOS LINKS EN GESTION DE PEDIDOS O CONSULTA PEDIDO.

        public ActionResult Seniar(int? id) {
            SeniaModel tr = new SeniaModel();
            string idSession = SessionUtils.generarIdVarSesion("acv", Session[SessionUtils.SESSION_USER].ToString()) + "|";
            ViewData["idSession"] = idSession;
            iniSenia(tr, idSession);

            if (id != null && id > 0) {
                tr.PedidoVehiculo = 1;
                tr.Senia.Vehiculo = new Vehiculo();
                tr.Senia.Vehiculo.Codigo = id ?? 0;
                tr.Senia.Pedido = null;
                iniSeniado(tr, idSession, false);
            }
            return View("Seniar", tr);
        }

        public ActionResult SeniarPedido(int? id) {
            SeniaModel tr = new SeniaModel();
            string idSession = SessionUtils.generarIdVarSesion("acv", Session[SessionUtils.SESSION_USER].ToString()) + "|";
            ViewData["idSession"] = idSession;
            iniSenia(tr, idSession);

            if (id != null && id > 0) {
                tr.PedidoVehiculo = 2;
                tr.Senia.Pedido = new Pedido();
                tr.Senia.Pedido.Codigo = id ?? 0;
                tr.Senia.Vehiculo = null;
                iniSeniado(tr, idSession, false);
            }
            return View("Seniar", tr);
        }

        public ActionResult DetalleSeniaPedido(int idPedido, string idSession) {
            ViewData["idSession"] = idSession;

            SeniaModel tr = new SeniaModel();
            tr.PedidoVehiculo = 2;
            tr.Senia.Pedido = new Pedido();
            tr.Senia.Pedido.Codigo = idPedido;
            tr.Senia.Vehiculo = null;
            iniSeniado(tr, idSession, false);
            return PartialView("_detallesSenia", tr);
        }

        public ActionResult DetalleSeniaVehiculo(int idVehiculo, string idSession) {
            ViewData["idSession"] = idSession;

            SeniaModel tr = new SeniaModel();
            tr.PedidoVehiculo = 1;
            tr.Senia.Vehiculo = new Vehiculo();
            tr.Senia.Vehiculo.Codigo = idVehiculo;
            tr.Senia.Pedido = null;
            iniSeniado(tr, idSession, false);
            return PartialView("_detallesSenia", tr);
        }

        [HttpPost]
        public ActionResult Seniar(SeniaModel model, string idSession) {

            ViewData["idSession"] = idSession;
            iniSeniado(model, idSession, true);//Lo hago aca al principio para que si hay error la tr vuelva con medios de pago con los valores anteriores.

            this.eliminarValidacionesIgnorables("Senia.Cliente", MetadataManager.IgnorablesDDL(model.Senia.Cliente));
            this.eliminarValidacionesIgnorables("Senia.Sucursal", MetadataManager.IgnorablesDDL(model.Senia.Sucursal));
            this.eliminarValidacionesIgnorables("Senia.Vendedor", MetadataManager.IgnorablesDDL(model.Senia.Vendedor));
            if (model.PedidoVehiculo == 1) {
                this.eliminarValidacionesIgnorables("Senia.Vehiculo", MetadataManager.IgnorablesDDL(model.Senia.Vehiculo));
                this.eliminarValidacionesIgnorables("Senia.Pedido", MetadataManager.IgnorablesDDL(new Pedido()));
                ModelState.Remove("Senia.Pedido.Codigo");
                model.Senia.Pedido = null;
            } else {
                this.eliminarValidacionesIgnorables("Senia.Vehiculo", MetadataManager.IgnorablesDDL(new Vehiculo()));
                this.eliminarValidacionesIgnorables("Senia.Pedido", MetadataManager.IgnorablesDDL(model.Senia.Vehiculo));
                ModelState.Remove("Senia.Vehiculo.Codigo");
                model.Senia.Vehiculo = null;
            }
            this.eliminarValidacionesIgnorables("Senia.Importe.Moneda", MetadataManager.IgnorablesDDL(model.Senia.Importe.Moneda));
            this.eliminarValidacionesIgnorables("Senia.PrecioVenta.Moneda", MetadataManager.IgnorablesDDL(model.Senia.PrecioVenta.Moneda));
            this.eliminarValidacionesIgnorables("Senia.Promesa.Financiacion.MontoFinanciado.Moneda", MetadataManager.IgnorablesDDL(new Moneda()));

            if (model.Senia.Promesa.Financiacion.MontoFinanciado.Monto != 0) {
                if (model.Senia.Promesa.Financiacion.MontoFinanciado.Monto < 0) {
                    ModelState.AddModelError("Senia.Promesa.Financiacion.MontoFinanciado.Monto", "El valor financiado no puede ser negativo");
                }
                if (model.Senia.Promesa.Financiacion.Tasa < 0) {
                    ModelState.AddModelError("Senia.Promesa.Financiacion.Tasa", "La tasa no puede ser negativa");
                }
                if (model.Senia.Promesa.Financiacion.CantCuotas < 0) {
                    ModelState.AddModelError("Senia.Promesa.Financiacion.CantCuotas", "La Cantidad de cuotas no puede ser negativa");
                }
            }

            if (ModelState.IsValid) {
                try {
                    string usuario = (string)HttpContext.Session.Contents[SessionUtils.SESSION_USER_NAME];
                    string IP = HttpContext.Request.UserHostAddress;
                    model.Senia.setearAuditoria(usuario, IP);
                    model.Senia.Ejecutar();
                    return RedirectToAction("Details", SeniasController.CONTROLLER, new { id = model.Senia.Codigo });
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View("Seniar", model);
                }
            }
            return View("Seniar", model);

        }


        #endregion

        #region PromesaEfectivo

        public ActionResult grillaPromesaEfectivo(string idSession) {
            return PartialView("_grillaPromesaEfectivo", Session[idSession + SessionUtils.EFECTIVO_PROMESA]);
        }

        private void _validarEfectivoPromesa(Efectivo ef) {

            this.eliminarValidacionesIgnorables("Importe.Moneda", MetadataManager.IgnorablesDDL(ef.Importe.Moneda));
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

        [HttpPost, ValidateInput(false)]
        public ActionResult grillaPromesaEfectivo_Add(Efectivo efectivo, string idSession) {
            var lista = (List<Efectivo>)Session[idSession + SessionUtils.EFECTIVO_PROMESA];
            _validarEfectivoPromesa(efectivo);
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

            return PartialView("_grillaPromesaEfectivo", lista);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult grillaPromesaEfectivo_Update(Efectivo efectivo, string idSession) {

            //pruebaError();
            var lista = (List<Efectivo>)Session[idSession + SessionUtils.EFECTIVO_PROMESA];

            _validarEfectivoPromesa(efectivo);
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

            return PartialView("_grillaPromesaEfectivo", lista);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult grillaPromesaEfectivo_Delete(int IdLinea, string idSession) {

            var lista = (List<Efectivo>)Session[idSession + SessionUtils.EFECTIVO_PROMESA];
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
            return PartialView("_grillaPromesaEfectivo", lista);
        }

    
        #endregion

        #region PromesaCheques

        public ActionResult grillaPromesaCheques(string idSession) {
            return PartialView("_grillaPromesaCheques", Session[idSession + SessionUtils.CHEQUES_PROMESA]);
        }

        private void _validarChequesValesPromesa(SeniaPromesa_ChequeVale cv) {

            this.eliminarValidacionesIgnorables("Importe.Moneda", MetadataManager.IgnorablesDDL(cv.Importe.Moneda));
            ModelState.Remove("Importe.ImporteEnMonedaDefault.Monto");

            //Sacar la validacion de moneda no nula porque da mensaje feo, hacerla manualmente
            ModelState.Remove("Importe.Moneda.Codigo");
            if (cv.Importe.Moneda == null || cv.Importe.Moneda.Codigo <= 0) {
                ModelState.AddModelError("Importe.Moneda.Codigo", "La moneda es requerida");
            }

            //validar el importe
            if (cv.Importe.Monto <= 0) {
                ModelState.AddModelError("Importe.Monto", "El monto debe ser un valor positivo");
            }
            if (cv.Dias < 0) {
                ModelState.AddModelError("Dias", "Los dias para el vencimiento no pueden ser negativos");
            }
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult grillaPromesaCheques_Add(SeniaPromesa_ChequeVale cv, string idSession) {
            var lista = (List<SeniaPromesa_ChequeVale>)Session[idSession + SessionUtils.CHEQUES_PROMESA];
            _validarChequesValesPromesa(cv);
            if (ModelState.IsValid) {
                try {
                    int maxIdLinea = lista.Count > 0 ? lista.Max(c => c.IdLinea) : 0;
                    cv.IdLinea = maxIdLinea + 1;
                    cv.tipo = SeniaPromesa_ChequeVale.CHEQUE_VALE.CHEQUE;
                    lista.Add(cv);
                } catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            } else {
                ViewData["EditError"] = "Corrija los valores incorrectos";
            }

            return PartialView("_grillaPromesaCheques", lista);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult grillaPromesaCheques_Update(SeniaPromesa_ChequeVale cv, string idSession) {

            var lista = (List<SeniaPromesa_ChequeVale>)Session[idSession + SessionUtils.CHEQUES_PROMESA];

            _validarChequesValesPromesa(cv);
            if (ModelState.IsValid) {
                try {
                    SeniaPromesa_ChequeVale cvEditado =
                        (from c in lista
                         where (c.IdLinea == cv.IdLinea)
                         select c).First<SeniaPromesa_ChequeVale>();
                    cvEditado.Importe.Monto = cv.Importe.Monto;
                    cvEditado.Importe.Moneda = cv.Importe.Moneda;
                    cvEditado.Importe.Moneda.Consultar();
                    cvEditado.Dias = cv.Dias;
                } catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            } else {
                ViewData["EditError"] = "Corrija los valores incorrectos";
            }

            return PartialView("_grillaPromesaCheques", lista);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult grillaPromesaCheques_Delete(int IdLinea, string idSession) {

            var lista = (List<SeniaPromesa_ChequeVale>)Session[idSession + SessionUtils.CHEQUES_PROMESA];
            if (IdLinea >= 0) {
                try {
                    SeniaPromesa_ChequeVale cvEliminado =
                        (from c in lista
                         where (c.IdLinea == IdLinea)
                         select c).First<SeniaPromesa_ChequeVale>();
                    lista.Remove(cvEliminado);
                } catch (Exception e) {
                    ViewData["DeleteError"] = e.Message;
                }
            }
            //ViewData["DeleteError"] = "Testing error message style";
            return PartialView("_grillaPromesaCheques", lista);
        }



        #endregion

        #region PromesaVales

        public ActionResult grillaPromesaVales(string idSession) {
            return PartialView("_grillaPromesaVales", Session[idSession + SessionUtils.VALES_PROMESA]);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult grillaPromesaVales_Add(SeniaPromesa_ChequeVale cv, string idSession) {
            var lista = (List<SeniaPromesa_ChequeVale>)Session[idSession + SessionUtils.VALES_PROMESA];
            _validarChequesValesPromesa(cv);
            if (ModelState.IsValid) {
                try {
                    int maxIdLinea = lista.Count > 0 ? lista.Max(c => c.IdLinea) : 0;
                    cv.IdLinea = maxIdLinea + 1;
                    cv.tipo = SeniaPromesa_ChequeVale.CHEQUE_VALE.VALE;
                    lista.Add(cv);
                } catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            } else {
                ViewData["EditError"] = "Corrija los valores incorrectos";
            }

            return PartialView("_grillaPromesaVales", lista);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult grillaPromesaVales_Update(SeniaPromesa_ChequeVale cv, string idSession) {

            var lista = (List<SeniaPromesa_ChequeVale>)Session[idSession + SessionUtils.VALES_PROMESA];

            _validarChequesValesPromesa(cv);
            if (ModelState.IsValid) {
                try {
                    SeniaPromesa_ChequeVale cvEditado =
                        (from c in lista
                         where (c.IdLinea == cv.IdLinea)
                         select c).First<SeniaPromesa_ChequeVale>();
                    cvEditado.Importe.Monto = cv.Importe.Monto;
                    cvEditado.Importe.Moneda = cv.Importe.Moneda;
                    cvEditado.Importe.Moneda.Consultar();
                    cvEditado.Dias = cv.Dias;
                } catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            } else {
                ViewData["EditError"] = "Corrija los valores incorrectos";
            }

            return PartialView("_grillaPromesaVales", lista);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult grillaPromesaVales_Delete(int IdLinea, string idSession) {

            var lista = (List<SeniaPromesa_ChequeVale>)Session[idSession + SessionUtils.VALES_PROMESA];
            if (IdLinea >= 0) {
                try {
                    SeniaPromesa_ChequeVale cvEliminado =
                        (from c in lista
                         where (c.IdLinea == IdLinea)
                         select c).First<SeniaPromesa_ChequeVale>();
                    lista.Remove(cvEliminado);
                } catch (Exception e) {
                    ViewData["DeleteError"] = e.Message;
                }
            }
            return PartialView("_grillaPromesaVales", lista);
        }

        #endregion
    }
}
