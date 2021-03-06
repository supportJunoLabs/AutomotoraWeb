﻿using AutomotoraWeb.Controllers.General;
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
using System.Globalization;
using System.Collections;
using System.IO;
using System.Drawing;
using System.Web.Script.Serialization;

namespace AutomotoraWeb.Controllers.Sales {
    public class VehiculosController : SalesController, IMaintenance {

        public static string CONTROLLER = "vehiculos";
        public const string PHOTO_FOLDER = "~/Content/Images/autos/";
        public const string DETAILS_GASTO = "detailGasto";
        public const string CREATE_GASTO = "createGasto";
        public const string EDIT_GASTO = "editGasto";
        public const string DELETE_GASTO = "deleteGasto";
        public const string CREATE_DOC = "createDoc";
        public const string DETAILS_DOC = "detailDoc";
        public const string EDIT_DOC = "editDoc";
        public const string DELETE_DOC = "deleteDoc";
        public const string ADD_PHOTO = "addPhoto";

        public const string OK = "OK";
        public const string ERROR = "ERROR";
        public const string VALIDATION_ERROR = "VALIDATION_ERROR";

        private const int MAX_ANCHO_FOTO = 250;

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext); //esto tiene que ser lo primero, si no, falla si se llama directamente la consulta del usuario si nadie esta logueado
            ViewBag.NombreEntidades = "Vehiculos";
            ViewBag.NombreEntidad = "Vehiculo";
            Usuario usuario = getUsuario();
            if (usuario == null) {
                filterContext.Result = new RedirectResult("/" + AuthenticationController.CONTROLLER + "/" + AuthenticationController.LOGIN);
                return;
            }
            ViewBag.MultiSucursal = usuario.MultiSucursal;
            ViewBag.Sucursales = Sucursal.Sucursales;
            ViewBag.Departamentos = Departamento.Departamentos();
            ViewBag.TiposCombustible = DLL_Backend.TipoCombustible.TiposCombustible();
            ViewBag.Monedas = Moneda.Monedas;
            ViewBag.EstadosDocumento = EstadoDocumento.EstadosDocumento();
            ViewBag.TiposDocumento = TipoDocumento.TiposDocumento();
        }

        public ActionResult Show() {
            return View(_listaElementos());
        }

        public ActionResult ListaGrilla() {
            return PartialView("_listGrilla", _listaElementos());
        }

        //private bool VehiculoConsultable(Vehiculo v) {
        //    if (v == null || v.Codigo == 0) return true;
        //    Usuario usuario = getUsuario();
        //    if (!SecurityService.Instance.verInfoAntigua(usuario) && v.Antiguo) {
        //        return false;
        //    }
        //    return true;
        //}

        private Vehiculo iniVehiculo(int id) {
            Vehiculo vehiculo = new Vehiculo();
            vehiculo.Codigo = id;
            Usuario u = getUsuario();
            vehiculo.Consultar(u);
            _addResumeGastosToViewBag(vehiculo);
            ViewBag.ShortedListFotoAuto = shortListFotoAuto(vehiculo.Fotos);
            return vehiculo;
        }


        public ActionResult Details(int id) {
            ViewData["idParametros"] = id.ToString();
            ViewBag.SoloLectura = true;
            try {
                Vehiculo v = iniVehiculo(id);
                return View(v);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        public ActionResult Create() {
            Vehiculo v = new Vehiculo();
            try {
                Usuario usuario = getUsuario();
                v.Sucursal = usuario.Sucursal;
                return View(v);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(v);
            }
        }

        public ActionResult Edit(int id) {
            try {
                Vehiculo vehiculo=iniVehiculo(id);
                ViewData["idParametros"] = id.ToString();
                Usuario usuario = getUsuario();
                try {
                    vehiculo.Modificable(usuario);
                } catch (UsuarioException ex) {
                    //No es modificable por este usuario
                    ViewBag.SoloLectura = true;
                    ViewBag.ErrorCode = ex.Codigo;
                    ViewBag.ErrorMessage = ex.Message + " Se despliega en modo consulta";
                    return View("details", vehiculo);
                }
                return View("edit", vehiculo);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View("details", id);
            }
        }

        public ActionResult Delete(int id) {
            try {
                ViewData["idParametros"] = id.ToString();
                ViewBag.SoloLectura = true;
                Vehiculo vehiculo = iniVehiculo(id);

                Usuario usuario = getUsuario();
                try {
                    vehiculo.Eliminable(usuario);
                } catch (UsuarioException ex) {
                    //No es eliminable por este usuario
                    ViewBag.SoloLectura = true;
                    ViewBag.ErrorCode = ex.Codigo;
                    ViewBag.ErrorMessage = ex.Message + " Se despliega en modo consulta";
                    return View("details", vehiculo);
                }
                return View(vehiculo);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View("details", id);
            }
        }

        //-----------------------------------------------------------------------------------------------------

        //private ActionResult VistaElemento(string nomvista, Vehiculo vehiculo) {
        //    try {
        //        _addResumeGastosToViewBag(vehiculo);
        //        ViewBag.ShortedListFotoAuto = shortListFotoAuto(vehiculo.Fotos);
        //        return View(nomvista, vehiculo);
        //    } catch (UsuarioException exc) {
        //        ViewBag.ErrorCode = exc.Codigo;
        //        ViewBag.ErrorMessage = exc.Message;
        //        return View();
        //    }
        //}

        //private ActionResult VistaElemento(int id) {
           
        //}

        //private Vehiculo _obtenerElemento(int id) { //Trae los datos del elemento de la base de datos y los pone en un objeto.
           
        //    return vehiculo;
        //}

        private void _addResumeGastosToViewBag(Vehiculo vehiculo) {
            ViewBag.initialCost = vehiculo.Costo.ImporteEnMonedaDefault;
            ViewBag.totalGastos = vehiculo.TotalGastos;
            Importe actualCost = vehiculo.Costo.ImporteEnMonedaDefault;
            actualCost.Monto = actualCost.Monto + vehiculo.TotalGastos.Monto;
            ViewBag.actualCost = actualCost;
        }

        private List<Vehiculo> _listaElementos() {
            Usuario u = getUsuario();
            List<Vehiculo> list = Vehiculo.Vehiculos(Vehiculo.VHC_TIPO_LISTADO.EN_STOCK, u);
            list.Reverse();
            return list;
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
            this.eliminarValidacionesIgnorables("Departamento", MetadataManager.IgnorablesDDL(new Departamento()));
            this.eliminarValidacionesIgnorables("TipoCombustible", MetadataManager.IgnorablesDDL(new TipoCombustible()));
            this.eliminarValidacionesIgnorables("Costo.Moneda", MetadataManager.IgnorablesDDL(new Moneda()));
            this.eliminarValidacionesIgnorables("PrecioVenta.Moneda", MetadataManager.IgnorablesDDL(new Moneda()));
            this.eliminarValidacionesIgnorables("Sucursal", MetadataManager.IgnorablesDDL(new Sucursal()));
        }

        //-----------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Edit(Vehiculo vehiculo) {
            ViewBag.SoloLectura = false;

            this.eliminarValidacionesIgnorables(vehiculo);

            if (ModelState.IsValid) {
                try {
                    vehiculo.ModificarDatos();
                    return RedirectToAction(BaseController.SHOW);
                } catch (UsuarioException exc) {
                    Usuario u = getUsuario();
                    vehiculo.Consultar(u); //para completar datos asociados
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    _addResumeGastosToViewBag(vehiculo);
                    ViewBag.ShortedListFotoAuto = shortListFotoAuto(vehiculo.Fotos);
                    return View(vehiculo);
                }
            }

            return View(vehiculo);
        }



        [HttpPost]
        public ActionResult Delete(Vehiculo vehiculo) {
            ViewBag.SoloLectura = true;

            //this.eliminarValidacionesIgnorables(vehiculo); 

            //if (ModelState.IsValid) {

                try {
                    string userName = getUserName();
                    string IP = getIP();
                    vehiculo.Eliminar(userName, IP);
                    return RedirectToAction(BaseController.SHOW);
                } catch (UsuarioException exc) {
                    Usuario u = getUsuario();
                    vehiculo.Consultar(u); //para completar datos asociados
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    _addResumeGastosToViewBag(vehiculo);
                    ViewBag.ShortedListFotoAuto = shortListFotoAuto(vehiculo.Fotos);
                    return View(vehiculo);
                }
            //}
            //return View(vehiculo);

        }

        //--------------------------METODOS PARA LISTADOS DE VEHICULO -----------------------------

        #region Listados

         [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult List() {
            ListadoVehiculosModel model = new ListadoVehiculosModel();
            try {
                string s = SessionUtils.generarIdVarSesion("ListadoVehiculos", getUserName());
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
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult List(ListadoVehiculosModel model, string btnSubmit) {
            try {
                Session[model.idParametros] = model; //filtros actualizados
                ViewData["idParametros"] = model.idParametros;
                ViewBag.SucursalesListado = Sucursal.Sucursales;
                ViewBag.TiposComubstiblesListado = TipoCombustible.TiposCombustible();
                this.eliminarValidacionesIgnorables("Filtro.Sucursal", MetadataManager.IgnorablesDDL(new Sucursal()));
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
            ListadoVehiculosModel model = (ListadoVehiculosModel)Session[idParametros];
            model.Resultado = _listaElementos(model);
            ViewData["idParametros"] = model.idParametros;
            return PartialView("_reportGrilla", model);
        }

        private List<Vehiculo> _listaElementos(ListadoVehiculosModel model) {
            model.AcomodarFiltro();
            Usuario u = getUsuario();
            List<Vehiculo>lista= Vehiculo.Vehiculos(model.Filtro, u);
            return lista;
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
            Usuario u = getUsuario();
            vhc.Consultar(u);
            ViewData["idParametros"] = idVehiculo;
            return View(vhc);
        }

        public ActionResult ReportPartial2(int idParametros) {
            DXReportFichaVehiculo rep = new DXReportFichaVehiculo();
            Vehiculo vehiculo = new Vehiculo();
            vehiculo.Codigo = idParametros;
            Usuario u = getUsuario();
            vehiculo.Consultar(u);

            List<Vehiculo> lista = new List<Vehiculo>();
            lista.Add(vehiculo);
            rep.DataSource = lista;
            ViewData["Report"] = rep;
            ViewData["idParametros"] = vehiculo.Codigo;
            return PartialView("_reportDetalle");
        }

        public ActionResult ReportExport2(int idParametros) {
            DXReportFichaVehiculo rep = new DXReportFichaVehiculo();
            Vehiculo vehiculo = new Vehiculo();
            vehiculo.Codigo = idParametros;
            Usuario u = getUsuario();
            vehiculo.Consultar(u);
            List<Vehiculo> lista = new List<Vehiculo>();
            lista.Add(vehiculo);
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
            Usuario u = getUsuario();
            vehiculo.Consultar(u);
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
            Usuario u = getUsuario();
            vehiculo.Consultar(u);
            gasto.Vehiculo = vehiculo;

            //no es necesario este paso, porque lo hace el backend
            //gasto.ImporteGasto.Moneda.Consultar();
            //gasto.Cotizacion = gasto.ImporteGasto.Moneda.Cotizacion;

            //validacion del controller
            List<String> errors = this.validateAtributesGastos(gasto);
            if (errors.Count > 0) {
                return Json(new { Result = "ERROR", ErrorCode = "VALIDATION_ERROR", ErrorMessage = errors.ToArray() });
            }

            //envio al backend
            try {
                gasto.Agregar();
                vehiculo.Consultar(u);
                return createJsonResultGastosOK(vehiculo);
            } catch (UsuarioException exc) {
                List<String> errores1 = new List<string>();
                errores1.Add(exc.Message);
                return Json(new { Result = "ERROR", ErrorCode = exc.Codigo, ErrorMessage = errores1.ToArray() });
            }

        }

        [HttpPost]
        public ActionResult editGasto(Gasto gasto) {

            Vehiculo vehiculo = gasto.Vehiculo;
            Usuario u = getUsuario();
            vehiculo.Consultar(u);
            gasto.Vehiculo = vehiculo;

            //En la modificacion la cotizacion la ingresa el usuario, no se sobreescribe
            //gasto.ImporteGasto.Moneda.Consultar();
            //gasto.Cotizacion = gasto.ImporteGasto.Moneda.Cotizacion;

            //validacion del controller
            List<String> errors = this.validateAtributesGastos(gasto);
            if (errors.Count > 0) {
                return Json(new { Result = "ERROR", ErrorCode = "VALIDATION_ERROR", ErrorMessage = errors.ToArray() });
            }

            //envio al backend
            try {
                gasto.ModificarDatos();
                vehiculo.Consultar(u);
                return createJsonResultGastosOK(vehiculo);
            } catch (UsuarioException exc) {
                List<String> errores1 = new List<string>();
                errores1.Add(exc.Message);
                return Json(new { Result = "ERROR", ErrorCode = exc.Codigo, ErrorMessage = errores1.ToArray() });
            }
        }

        [HttpPost]
        public ActionResult deleteGasto(Gasto gasto) {
            try {
                string userName = getUserName();
                string IP = getIP();
                gasto.Eliminar(userName, IP);

                Vehiculo vehiculo = gasto.Vehiculo;
                Usuario u = getUsuario();
                vehiculo.Consultar(u);

                return createJsonResultGastosOK(vehiculo);
            } catch (UsuarioException exc) {
                List<String> errores1 = new List<string>();
                errores1.Add(exc.Message);
                return Json(new { Result = "ERROR", ErrorCode = exc.Codigo, ErrorMessage = errores1.ToArray() });
            }
        }

        private JsonResult createJsonResultGastosOK(Vehiculo vehiculo) {
            Importe initialCost = vehiculo.Costo.ImporteEnMonedaDefault;
            Importe totalGastos = vehiculo.TotalGastos;
            Importe actualCost = vehiculo.Costo.ImporteEnMonedaDefault;
            actualCost.Monto = actualCost.Monto + vehiculo.TotalGastos.Monto;

            return Json(new {
                Result = "OK",
                InitialCostMoneda = initialCost.Moneda.Simbolo,
                InitialCostMonto = initialCost.Monto.ToString("N", CultureInfo.InvariantCulture),
                TotalGastosMoneda = totalGastos.Moneda.Simbolo,
                TotalGastosMonto = totalGastos.Monto.ToString("N", CultureInfo.InvariantCulture),
                ActualCostMoneda = actualCost.Moneda.Simbolo,
                ActualCostMonto = actualCost.Monto.ToString("N", CultureInfo.InvariantCulture)
            });
        }

        //-------------------------------

        private Gasto _obtenerGasto(int id) { //Trae los datos del elemento de la base de datos y los pone en un objeto.
            Gasto gasto = new Gasto();
            gasto.Codigo = id;
            gasto.Consultar();
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
            Usuario u = getUsuario();
            vehiculo.Consultar(u);
            return vehiculo.DetalleGastos;
        }

        #endregion

        //---------------------------------------------------
        //---------------------------------------------------

        #region Documentos

        public ActionResult createDoc(int idVehiculo) {
            Vehiculo vehiculo = new Vehiculo();
            vehiculo.Codigo = idVehiculo;
            Usuario u = getUsuario();
            vehiculo.Consultar(u);
            DocAuto docAuto = new DocAuto();
            docAuto.Fecha = DateTime.Now;
            docAuto.Vehiculo = vehiculo;
            return PartialView("_popupDocumentacion", docAuto);
        }


        public ActionResult detailDoc(int id) {
            DocAuto docAuto = this._obtenerDocumentacion(id);
            ViewBag.SoloLectura = true;
            return PartialView("_popupDocumentacion", docAuto);
        }

        public ActionResult editDoc(int id) {
            DocAuto docAuto = this._obtenerDocumentacion(id);

            return PartialView("_popupDocumentacion", docAuto);
        }

        public ActionResult deleteDoc(int id) {
            DocAuto docAuto = this._obtenerDocumentacion(id);
            ViewBag.SoloLectura = true;
            return PartialView("_popupDocumentacion", docAuto);
        }

        public ActionResult listDocumentos(int idParametros) {
            ViewData["idParametros"] = idParametros;
            return PartialView("_listDocumentos", _listaDocumentos(idParametros));
        }

        [HttpPost]
        public JsonResult createDoc(DocAuto docAuto) {

            Vehiculo vehiculo = docAuto.Vehiculo;
            Usuario u = getUsuario();
            vehiculo.Consultar(u);
            docAuto.Vehiculo = vehiculo;

            //validacion del controller
            List<String> errors = this.validateAtributesDocumentacion(docAuto);
            if (errors.Count > 0) {
                return Json(new { Result = "ERROR", ErrorCode = "VALIDATION_ERROR", ErrorMessage = errors.ToArray() });
            }
            //envio al backend
            try {
                docAuto.Agregar();
                vehiculo.Consultar(u);
                return Json(new { Result = "OK" });
            } catch (UsuarioException exc) {
                List<String> errores1 = new List<string>();
                errores1.Add(exc.Message);
                return Json(new { Result = "ERROR", ErrorCode = exc.Codigo, ErrorMessage = errores1.ToArray() });
            }
        }

        [HttpPost]
        public ActionResult editDoc(DocAuto docAuto) {

            Vehiculo vehiculo = docAuto.Vehiculo;
            Usuario u = getUsuario();
            vehiculo.Consultar(u);
            docAuto.Vehiculo = vehiculo;

            //validacion del controller
            List<String> errors = this.validateAtributesDocumentacion(docAuto);
            if (errors.Count > 0) {
                return Json(new { Result = "ERROR", ErrorCode = "VALIDATION_ERROR", ErrorMessage = errors.ToArray() });
            }

            try {
                docAuto.ModificarDatos();
                return Json(new { Result = "OK" });
            } catch (UsuarioException exc) {
                List<String> errores1 = new List<string>();
                errores1.Add(exc.Message);
                return Json(new { Result = "ERROR", ErrorCode = exc.Codigo, ErrorMessage = errores1.ToArray() });
            }

        }

        [HttpPost]
        public ActionResult deleteDoc(DocAuto docAuto) {
            try {
                string userName = getUserName();
                string IP = getIP();
                docAuto.Eliminar(userName, IP);

                Vehiculo vehiculo = docAuto.Vehiculo;
                Usuario u = getUsuario();
                vehiculo.Consultar(u);

                return Json(new { Result = "OK" });
            } catch (UsuarioException exc) {
                List<String> errores1 = new List<string>();
                errores1.Add(exc.Message);
                return Json(new { Result = "ERROR", ErrorCode = exc.Codigo, ErrorMessage = errores1.ToArray() });
            }
        }

        private DocAuto _obtenerDocumentacion(int id) { //Trae los datos del elemento de la base de datos y los pone en un objeto.
            DocAuto docAuto = new DocAuto();
            docAuto.Codigo = id;
            docAuto.Consultar();
            return docAuto;
        }

        private List<DocAuto> _listaDocumentos(int id) {
            Vehiculo vehiculo = new Vehiculo();
            vehiculo.Codigo = id;
            Usuario u = getUsuario();
            vehiculo.Consultar(u);
            return vehiculo.Documentacion;
        }

        private List<String> validateAtributesDocumentacion(DocAuto docAuto) {

            List<String> errors = new List<String>();

            if ((docAuto.Fecha == null) || (docAuto.Fecha.Ticks == 0)) {
                errors.Add("El campo Fecha es obligatorio");
            }

            if (docAuto.Estado == null) {
                errors.Add("El Estado es requerido");
            }

            if (docAuto.Fecha == null) {
                errors.Add("La fecha es requerida");
            }

            if (string.IsNullOrWhiteSpace(docAuto.Poseedor)) {
                errors.Add("Poseedor es requerido");
            }

            if ((docAuto.Poseedor != null) && (docAuto.Poseedor.Length > 80)) {
                errors.Add("Poseedor: Largo maximo 40 caracteres");
            }

            if ((docAuto.Observaciones != null) && (docAuto.Observaciones.Length > 80)) {
                errors.Add("El campo Observaciones debe tener como maximo 80 caracteres");
            }

            return errors;
        }

        #endregion

        //---------------------------------------------------
        //---------------------------------------------------

        #region Fotos

        public JsonResult addPhoto() {
            return Json(new { Result = "OK" }); // TODO
        }

        [HttpPost]
        public JsonResult savePhotoOrder(Vehiculo vehiculo) {
            try {
                Vehiculo vehiculoOriginal = new Vehiculo();
                vehiculoOriginal.Codigo = vehiculo.Codigo;
                Usuario u = getUsuario();
                vehiculoOriginal.Consultar(u);
                foreach (FotoAuto fotoAuto in vehiculo.Fotos) {
                    var foto = from f in vehiculoOriginal.Fotos where (fotoAuto.Codigo == f.Codigo) select f;
                    if (foto != null) {
                        FotoAuto fa = foto.First<FotoAuto>();
                        fa.Orden = fotoAuto.Orden;
                        fa.ModificarDatos();
                    }
                }

                return Json(new { Result = "OK" });
            } catch (UsuarioException exc) {
                return Json(new { Result = "ERROR", ErrorCode = exc.Codigo, ErrorMessage = exc.Message });
            }

        }

        [HttpPost]
        public JsonResult removePhoto(int codePhoto) {
            try {
                string userName = getUserName();
                string IP = getIP();
                FotoAuto fotoAuto = new FotoAuto();
                fotoAuto.Codigo = codePhoto;
                fotoAuto.Eliminar(userName, IP);
                return Json(new { Result = "OK" });
            } catch (UsuarioException exc) {
                return Json(new { Result = "ERROR", ErrorCode = exc.Codigo, ErrorMessage = exc.Message });
            }

        }

        private List<FotoAuto> shortListFotoAuto(List<FotoAuto> listFotoAuto) {

            ArrayList arrayListFotoAuto = new ArrayList();
            foreach (FotoAuto fa in listFotoAuto) {
                arrayListFotoAuto.Add(fa);
            }

            // esta variable sirve para hacer los cambios
            FotoAuto auxiliar;

            // las iteraciones van a ser cada vez más pequeñas, 
            // porque los números más grandes ya van siendo ordenados al final
            for (int i = arrayListFotoAuto.Count - 1; i > 0; i--) {
                //las iteraciones son siempre desde el principio hasta el límite puesto arriba
                for (int j = 0; j < i; j++) {
                    //si el número es más grande que el próximo, se cambian de lugar
                    if (((FotoAuto)(arrayListFotoAuto[j])).Orden > ((FotoAuto)(arrayListFotoAuto[j + 1])).Orden) {
                        auxiliar = ((FotoAuto)(arrayListFotoAuto[j]));
                        arrayListFotoAuto[j] = (FotoAuto)(arrayListFotoAuto[j + 1]);
                        arrayListFotoAuto[j + 1] = auxiliar;
                    }
                }
            }

            List<FotoAuto> shortedListFotoAuto = new List<FotoAuto>();
            foreach (FotoAuto fa in arrayListFotoAuto) {
                shortedListFotoAuto.Add(fa);
            }

            return shortedListFotoAuto;
        }



        //---------------------------------------------------------------------

        public string getFileName(Vehiculo vehiculo) {
            string fileName = "";

            int newNumber = 0;
            foreach (FotoAuto fa in vehiculo.Fotos) {
                string aux = fa.Archivo.Split('-')[2];
                string aux2 = aux.Split('.')[0];
                int number = int.Parse(aux2);
                if (number > newNumber) {
                    newNumber = number;
                }
            }

            newNumber = newNumber + 1;

            string zeros = "000";
            if (newNumber > 99) {
                zeros = "0";
            } else if (newNumber > 9) {
                zeros = "00";
            }

            fileName = vehiculo.Ficha.Replace('/', '-') + "-" + zeros + newNumber;

            return fileName;
        }

        [HttpPost]
        public JsonResult Upload(FormCollection col) {
            string fileName = null;
            //string fileRandomName = "";

            if (Request.Files.Count > 0) {

                int idVehiculo = int.Parse(col["idVehiculo"]);
                Vehiculo vehiculo = new Vehiculo();
                vehiculo.Codigo = idVehiculo;
                Usuario u = getUsuario();
                vehiculo.Consultar(u);
                int order = vehiculo.Fotos.Count;

                HttpPostedFileBase file = Request.Files[0]; //Uploaded file

                string extension = getExtensionFile(file.FileName);
                fileName = getFileName(vehiculo) + extension;

                //---------------------------------------------------
                //Use the following properties to get file's name, size and MIMEType
                int fileSize = file.ContentLength;
                string mimeType = file.ContentType;
                Stream fileContent = file.InputStream;
                //To save file, use SaveAs method
                file.SaveAs(Server.MapPath(PHOTO_FOLDER) + fileName);

                Bitmap image1 = (Bitmap)Image.FromFile(Server.MapPath(PHOTO_FOLDER) + fileName, true);
                Bitmap image2 = (Bitmap)ImageUtils.CambiarTamanio(image1, MAX_ANCHO_FOTO, 0, 0);
                image2.Save(Server.MapPath(PHOTO_FOLDER) + fileName);

                //---------------------------------------------------

                FotoAuto fotoAuto = new FotoAuto();
                fotoAuto.Vehiculo = vehiculo;
                fotoAuto.Orden = order;
                fotoAuto.Archivo = fileName;
                string nomUsuario = getUserName();
                string origen = getIP();
                fotoAuto.setearAuditoria(nomUsuario, origen);
                fotoAuto.Agregar();

                //---------------------------------------------------

                return Json(new { code = fotoAuto.Codigo, order = order, fileName = fileName });

            } else {
                return Json(new { fileName = fileName });
            }

        }


        //-----------------------------------------------------------------------------------------------------

        private string getExtensionFile(string fileName) {
            //se corrige porque no andaba bien cuando hay un punto como parte del nombre del archivo, ademas del que separa la extension. 
            //se debe tomar el ultimo punto
            string extension = "";
            if (fileName.Contains('.')) {
                int pos = fileName.LastIndexOf('.');
                extension = fileName.Substring(pos);
            }
            return extension.ToLower();
        }

        //---------------------------------------------------------------------

        #endregion


        #region InformacionAsociada

        public ActionResult VerSenia(int id) {
            Vehiculo v = new Vehiculo();
            v.Codigo = id;
            Senia s = v.ObtenerSenia();
            return RedirectToAction(BaseController.DETAILS, SeniasController.CONTROLLER, new { id = s.Codigo });
        }

        public ActionResult VerVenta(int id) {
            Vehiculo v = new Vehiculo();
            v.Codigo = id;
            Venta s = v.ObtenerVenta();
            return RedirectToAction(BaseController.DETAILS, VentasController.CONTROLLER, new { id = s.Codigo });
        }

        public ActionResult VerPermutaOrigen(int id) {
            return RedirectToAction(BaseController.DETAILS, VentasController.CONTROLLER, new { id = id });
        }

        public ActionResult VerPedidoOrigen(int id) {
            return RedirectToAction(BaseController.DETAILS, PedidosController.CONTROLLER, new { id = id });
        }

        //public ActionResult VerAcvsVigentes(int id) {
        //    return RedirectToAction("ListActivosVehiculo", AcvsController.CONTROLLER, new { id = id });
        //}

        #endregion


        //public ActionResult GridLookupVehiculo() {
        //    ViewBag.ListVehiculos = Vehiculo.Vehiculos(Vehiculo.VHC_TIPO_LISTADO.EN_STOCK);
        //    return PartialView("_gridLookupVehiculo");
        //}

        //#region SeleccionDeVehiculo

        ////Se invoca desde paginacion, ordenacion etc, de grilla de cuotas. Devuelve la partial del tab de cuotas
        //public ActionResult VehiculosVendiblesGrilla(GridLookUpModel model) {
        //    model.Opciones = Vehiculo.Vehiculos(Vehiculo.VHC_TIPO_LISTADO.VENDIBLES);
        //    return PartialView("_selectVehiculo", model);
        //}

        //[HttpPost]
        //public JsonResult details(int codigo) {
        //    try {
        //        Vehiculo vehiculo = new Vehiculo();
        //        vehiculo.Codigo = codigo;
        //        vehiculo.Consultar();

        //        return Json(new {
        //            Result = "OK",
        //            Vehiculo = new {
        //                Ficha = vehiculo.Ficha,
        //                Sucursal = vehiculo.Sucursal.Nombre,
        //                Marca = vehiculo.Marca,
        //                Modelo = vehiculo.Modelo,
        //                Anio = vehiculo.Anio,
        //                Departamento = vehiculo.Departamento.Nombre,
        //                Color = vehiculo.Color,
        //                Matricula = vehiculo.Matricula,
        //                Padron = vehiculo.Padron,
        //                Motor = vehiculo.Motor,
        //                Chasis = vehiculo.Chasis,
        //                Propietario = vehiculo.Propietario,
        //                PrecioLista = vehiculo.PrecioVenta.ImporteTexto,
        //                Estado = vehiculo.Status,
        //                Observaciones = vehiculo.Observaciones
        //            }
        //        }
        //        );
        //    } catch (UsuarioException exc) {
        //        return Json(new { Result = "ERROR", ErrorCode = exc.Codigo, ErrorMessage = exc.Message });
        //    }
        //}

        //#endregion
    }
}
