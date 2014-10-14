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

namespace AutomotoraWeb.Controllers.Bank {
    public class ChEmitidosController : BankController {

        public static string CONTROLLER = "ChEmitidos";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            ViewBag.Monedas = Moneda.Monedas;
            ViewBag.Cuentas = CuentaBancaria.CuentasBancarias;
            ViewBag.NombreEntidad = "Cheque Emitido";
            ViewBag.NombreEntidades = "Cheques Emitidos";
            base.OnActionExecuting(filterContext);
        }

        #region mantenimiento

        public ActionResult Show(int? id) {
            CuentaBancaria c = new CuentaBancaria();
            try {

                if ((id ?? 0) > 0) {
                    c.Codigo = id ?? 0;
                    c.Consultar();
                } else {
                    c.Codigo = 0;
                    c.Moneda = new Moneda();
                    c.Moneda.Codigo = 0;
                }
                return View(c);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(c);
            }
        }

        //Se invoca desde la grilla
        public ActionResult ListaGrilla(int? idParametros) {
            if ((idParametros ?? 0) <= 0) {
                CuentaBancaria c = new CuentaBancaria();
                c.Codigo = 0;
                c.Moneda = new Moneda();
                c.Moneda.Codigo = 0;
                return PartialView("_listaGrilla", c);
            }
            CuentaBancaria cuenta = new CuentaBancaria();
            cuenta.Codigo = idParametros ?? 0;
            cuenta.Consultar();
            ViewData["idParametros"] = idParametros;
            return PartialView("_listGrilla", cuenta);
        }

        //Se invoca por json al actualizar la ddl de cuentas
        public ActionResult CuentaSelected(int? idCuenta) {
            CuentaBancaria c = new CuentaBancaria();
            c.Codigo = idCuenta ?? 0;
            if (c.Codigo > 0) {
                c.Consultar();
            } else {
                c.Codigo = 0;
                c.Moneda = new Moneda();
                c.Moneda.Codigo = 0;
            }
            ViewData["idParametros"] = c.Codigo;
            if (idCuenta != null && idCuenta != 0) {
                return PartialView("_listGrilla", c);
            }
            return PartialView("_listaGrilla", c);
        }

        //Se invoca desde la url del browser o desde el menu principal, o referencias externas. Devuelve la pagina completa
        [HttpPost]
        public ActionResult Show(CuentaBancaria model) {
            try {
                ModelState.Clear();
                if (model == null || model.Codigo <= 0) {
                    model.Codigo = 0;
                    model.Moneda = new Moneda();
                    model.Moneda.Codigo = 0;
                } else {
                    model.Consultar();
                }
                ViewData["idParametros"] = model.Codigo;
                return View(model);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(model);
            }
        }

        public ActionResult Create(int? id) {
            ChequeEmitido ch = new ChequeEmitido();
            try {
                if ((id ?? 0) > 0) {
                    ch.Cuenta = new CuentaBancaria();
                    ch.Cuenta.Codigo = id ?? 0;
                }
                ch.FechaEmision = DateTime.Now;
                ch.FechaVencimiento = DateTime.Now;
                return View(ch);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(ch);
            }

        }

        public ActionResult Details(int id) {
            ViewBag.SoloLectura = true;
            return VistaElemento(id);
        }

        public ActionResult Edit(int id) {
            return VistaElemento(id);
        }

        public ActionResult Delete(int id) {
            ViewBag.SoloLectura = true;
            return VistaElemento(id);
        }

        private ActionResult VistaElemento(int id) {
            try {
                ChequeEmitido td = new ChequeEmitido();
                td.Codigo = id;
                td.Consultar();
                return View(td);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        [HttpPost]
        public ActionResult Create(ChequeEmitido ch) {
            this.eliminarValidacionesIgnorables("Importe.Moneda", MetadataManager.IgnorablesDDL(new Moneda()));
            this.eliminarValidacionesIgnorables("Cuenta", MetadataManager.IgnorablesDDL(new CuentaBancaria()));
            if (ModelState.IsValid) {
                try {
                    ch.Agregar();
                    return RedirectToAction(BaseController.SHOW, new { id = ch.Cuenta.Codigo });
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(ch);
                }
            }
            return View(ch);
        }

        [HttpPost]
        public ActionResult Edit(ChequeEmitido ch) {
            this.eliminarValidacionesIgnorables("Importe.Moneda", MetadataManager.IgnorablesDDL(new Moneda()));
            this.eliminarValidacionesIgnorables("Cuenta", MetadataManager.IgnorablesDDL(new CuentaBancaria()));

            if (ModelState.IsValid) {
                try {
                    ch.ModificarDatos();
                    return RedirectToAction(BaseController.SHOW, new { id = ch.Cuenta.Codigo });
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(ch);
                }
            }
            return View(ch);
        }

        [HttpPost]
        public ActionResult Delete(ChequeEmitido ch) {
            ViewBag.SoloLectura = true;
            this.eliminarValidacionesIgnorables("Importe.Moneda", MetadataManager.IgnorablesDDL(new Moneda()));
            this.eliminarValidacionesIgnorables("Cuenta", MetadataManager.IgnorablesDDL(new CuentaBancaria()));

            if (ModelState.IsValid) {
                try {
                    string userName = (string)HttpContext.Session.Contents[SessionUtils.SESSION_USER_NAME];
                    string IP = HttpContext.Request.UserHostAddress;
                    ch.Eliminar(userName, IP);
                    return RedirectToAction(BaseController.SHOW, new { id = ch.Cuenta.Codigo });
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(ch);
                }
            }
            return View(ch);
        }

        #endregion

        #region listadoyreporte

        //Se invoca desde la url del browser o desde el menu principal, o referencias externas. Devuelve la pagina completa
         [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult List() {
            try {
                ListadoChequesEmitidosModel model = new ListadoChequesEmitidosModel();
                string s = SessionUtils.generarIdVarSesion("ListadoChequesEmitidos", Session[SessionUtils.SESSION_USER].ToString());
                Session[s] = model;
                model.idParametros = s;
                ViewData["idParametros"] = model.idParametros;
                model.obtenerListado();
                return View(model);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        [HttpPost]
        //Se invoca desde el boton actualizar o imprimir.
        public ActionResult List(ListadoChequesEmitidosModel model) {
            try{
            Session[model.idParametros] = model; //filtros actualizados
            ViewData["idParametros"] = model.idParametros;
            //ViewBag.Financistas = Financista.Financistas(Financista.FIN_TIPO_LISTADO.TODOS);
            this.eliminarValidacionesIgnorables("Filtro.Cuenta", MetadataManager.IgnorablesDDL(new CuentaBancaria()));
            this.eliminarValidacionesIgnorables("Filtro.Moneda", MetadataManager.IgnorablesDDL(new Moneda()));
            if (ModelState.IsValid) {
                if (model.Accion == ListadoChequesEmitidosModel.ACCIONES.IMPRIMIR) {
                    return this.Report(model);
                }
                model.obtenerListado();
            }
            return View(model);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de cuotas. Devuelve la partial del tab de cuotas
        public ActionResult ListadoGrilla(string idParametros) {
            ListadoChequesEmitidosModel model = (ListadoChequesEmitidosModel)Session[idParametros];
            ViewData["idParametros"] = model.idParametros;
            model.obtenerListado();
            return PartialView("_listCheques", model.Resultado);
        }

        public ActionResult Report(ListadoChequesEmitidosModel model) {
            return View("Report", model);
        }

        private XtraReport _generarReporte(string idParametros) {
            ListadoChequesEmitidosModel model = (ListadoChequesEmitidosModel)Session[idParametros];
            model.obtenerListado();
            List<ListadoChequesEmitidosModel> ll = new List<ListadoChequesEmitidosModel>();
            ll.Add(model);
            XtraReport rep = new DXListadoChequesEmitidos();
            rep.DataSource = ll;
            setParamsToReport(rep, model);
            return rep;
        }
        private void setParamsToReport(XtraReport report, ListadoChequesEmitidosModel model) {
            Parameter param = new Parameter();
            param.Name = "detalleFiltros";
            param.Type = typeof(string);
            param.Value = model.detallesFiltro();
            param.Description = "Detalle Filtros";
            param.Visible = false;
            report.Parameters.Add(param);
        }

        public ActionResult ReportPartial(string idParametros) {
            XtraReport rep = _generarReporte(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_report");
        }

        public ActionResult ReportExport(string idParametros) {
            XtraReport rep = _generarReporte(idParametros);
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        #endregion

        #region debitarVencidos
        public ActionResult Debitar() {
            return View();
        }

        public ActionResult GrillaDebitables() {
            return PartialView("_chequesDebitar", ChequeEmitido.ChequesDebitables());
        }

        [HttpPost]
        public ActionResult EjecutarDebitar() {
            try {
                string userName = (string)HttpContext.Session.Contents[SessionUtils.SESSION_USER_NAME];
                string IP = HttpContext.Request.UserHostAddress;
                int cant = ChequeEmitido.DebitarVencidos(userName, IP);
                ViewBag.ErrorCode = "0";
                if (cant == 0) {
                    ViewBag.ErrorMessage = "No se encontraron cheques emitidos pendientes vencidos.";
                } else if (cant == 1) {
                    ViewBag.ErrorMessage = "Ejecución exitosa. Se generó 1 débito.";
                } else {
                    ViewBag.ErrorMessage = "Ejecución exitosa. Se generaron " + cant + " débitos.";
                }
                return View("debitar");
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View("debitar");
            }
        }

        #endregion
    }
}
