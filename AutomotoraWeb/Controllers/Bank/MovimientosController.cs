using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DLL_Backend;
using AutomotoraWeb.Models;
using AutomotoraWeb.Utils;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using AutomotoraWeb.Controllers.General;
using AutomotoraWeb.Services;

namespace AutomotoraWeb.Controllers.Bank {
    public class MovimientosController : BankController {
        public static string CONTROLLER = "Movimientos";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            ViewBag.NombreEntidad = "Movimiento Bancario";
            ViewBag.NombreEntidades = "Movimientos Bancarios";
            ViewBag.Cuentas = CuentaBancaria.CuentasBancarias;
        }

        #region Mantenimiento

        //private void _generarListado(MovimientosBancoModel model) {
        //    Usuario usuario = getUsuario();
        //    bool verInfoAntigua = SecurityService.Instance.verInfoAntigua(usuario);
        //    model.generarListado(verInfoAntigua);
        //}

         [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Show(int? id) {
            MovimientosBancoModel model = new MovimientosBancoModel();
            try {
                if (id != null && id > 0) {
                    model.Cuenta = new CuentaBancaria();
                    model.Cuenta.Codigo = (id ?? 0);
                    model.Cuenta.Consultar();
                }
                string s = SessionUtils.generarIdVarSesion("MovimientosBanco", getUserName());
                model.idParametros = s;
                Session[s] = model;
                ViewData["idParametros"] = model.idParametros;
                Usuario u = getUsuario();
                model.GenerarListado(u);
                //ViewData["Movimientos"] = model.Resultado;

                return View(model);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(model);
            }
        }

        [HttpPost]
        //se invoca desde el boton actualizar e imprimir. Devuelve la pagina completa
        public ActionResult Show(MovimientosBancoModel model) {
            try {
                Session[model.idParametros] = model; //filtros actualizados
                ViewData["idParametros"] = model.idParametros;
                this.eliminarValidacionesIgnorables("Cuenta", MetadataManager.IgnorablesDDL(new CuentaBancaria()));
                if (ModelState.IsValid) {
                    Usuario u = getUsuario();
                    model.GenerarListado(u);
                } else {
                    model.Resultado = new List<MovBanco>();
                }
                return View(model);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de cuotas. 
        public ActionResult GrillaMovimientos(string idParametros) {
            MovimientosBancoModel model = (MovimientosBancoModel)Session[idParametros];
            ViewData["idParametros"] = model;
            this.eliminarValidacionesIgnorables("Cuenta", MetadataManager.IgnorablesDDL(new CuentaBancaria()));
            if (ModelState.IsValid) {
                Usuario u = getUsuario();
                model.GenerarListado(u);
            } else {
                model.Resultado = new List<MovBanco>();
            }
            return PartialView("_listGrillaMovimientos", model);
        }

        public ActionResult Create(int id) {
            MovBanco mov = new MovBanco();
            try {
                mov.Cuenta = new CuentaBancaria();
                mov.Cuenta.Codigo = id;
                mov.Cuenta.Consultar();
                mov.ImporteMov = new Importe();
                mov.ImporteMov.Moneda = mov.Cuenta.Moneda;
                mov.FechaMov = DateTime.Now.Date;
                return View(mov);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(mov);
            }
        }

        [HttpPost]
        public ActionResult Create(MovBanco mov) {
            return agregarMovimiento(mov);
        }

        public ActionResult Details(int id) {
            ViewBag.SoloLectura = true;
            return VistaElemento(id);
        }

        public ActionResult Edit(int id) {
            return VistaElemento(id);
        }

        [HttpPost]
        public ActionResult Edit(MovBanco td) {
            if (ModelState.IsValid) {
                try {
                    //td.ModificarDatos();
                    return RedirectToAction(BaseController.SHOW);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(td);
                }
            }

            return View(td);
        }
        
        public ActionResult Delete(int id) {
            ViewBag.SoloLectura = true;
            return VistaElemento(id);
        }

        [HttpPost]
        public ActionResult Delete(MovBanco td) {
            ViewBag.SoloLectura = true;
            if (ModelState.IsValid) {
                try {
                    string userName = getUserName();
                    string IP = getIP();
                    //td.Eliminar(userName, IP);
                    return RedirectToAction(BaseController.SHOW);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(td);
                }
            }

            return View(td);
        }

        private ActionResult VistaElemento(int id) {
            try {
                MovBanco td = new MovBanco();
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

        //Se invoca desde crear y extornar
        private ActionResult agregarMovimiento(MovBanco mov) {
            this.eliminarValidacionesIgnorables("ImporteMov.Moneda", MetadataManager.IgnorablesDDL(new Moneda()));
            this.eliminarValidacionesIgnorables("Cuenta", MetadataManager.IgnorablesDDL(new CuentaBancaria()));
            if (ModelState.IsValid) {
                try {
                    mov.Cuenta.Consultar(); //lo necesito para mostrar los datos de la cuenta si vuelvo porque hubo error
                    mov.ImporteMov.Moneda = mov.Cuenta.Moneda;
                    mov.Agregar();
                    return RedirectToAction("Show", new { id = mov.Cuenta.Codigo });
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(mov);
                }
            }
            return View(mov);
        }

        [HttpPost]
        public JsonResult Conciliar(int id) {
            try {
                MovBanco mov = new MovBanco();
                mov.Codigo = id;
                Usuario u = getUsuario();
                mov.Consultar(u);
                mov.Conciliar(DateTime.Now.Date);
                return Json(new { Result = "OK" });
            } catch (UsuarioException ex) {
                return Json(new { Result = "ERROR", ErrorMessage = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Desconciliar(int id) {
            try {
                MovBanco mov = new MovBanco();
                mov.Codigo = id;
                Usuario u = getUsuario();
                mov.Consultar(u);
                mov.DesConciliar();
                return Json(new { Result = "OK" });
            } catch (UsuarioException ex) {
                return Json(new { Result = "ERROR", ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult ExtornarMov(int id) {
            MovBanco mov = new MovBanco();
            try {
                mov.Codigo = id;
                Usuario u = getUsuario();
                mov.Consultar(u);
                MovBanco mov1 = mov.MovimientoInverso(DateTime.Now.Date, "Extorno: " + mov.ConceptoMov);
                return View("Create", mov1);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View("Create", mov);
            }
        }

        [HttpPost]
        public ActionResult ExtornarMov(MovBanco mov) {
            return agregarMovimiento(mov);
        }

        #endregion



        #region Listados-EstadoDeCuenta

        //private void _generarListado(EstadoCuentaModel model) {
        //    Usuario usuario = getUsuario();
        //    bool verInfoAntigua = SecurityService.Instance.verInfoAntigua(usuario);
        //    if (!verInfoAntigua) {
        //        DateTime minfecha = DateTime.Now.Date.AddDays(Automotora.DiasInfoAntigua() * (-1));
        //        if (model.EstadoCuenta.Desde < minfecha) {
        //            model.EstadoCuenta.Desde = minfecha;
        //        }
        //        if (model.EstadoCuenta.Hasta < minfecha) {
        //            model.EstadoCuenta.Hasta = minfecha;
        //        }
        //    }
        //    model.EstadoCuenta.generarListado();
        //}

        //Se invoca desde la url del browser o desde el menu principal, o referencias externas. Devuelve la pagina completa
         [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult List(int? id) {
            EstadoCuentaModel model = new EstadoCuentaModel();
            try {
                if (id != null && id > 0) {
                    model.EstadoCuenta.Cuenta = new CuentaBancaria();
                    model.EstadoCuenta.Cuenta.Codigo = (id ?? 0);
                    Usuario u = getUsuario();
                    model.EstadoCuenta.generarListado(u);
                }
                string s = SessionUtils.generarIdVarSesion("ListadoMovsBanco", getUserName());
                Session[s] = model;
                model.idParametros = s;
                ViewData["idParametros"] = model.idParametros;
                return View(model);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(model);
            }
        }

        [HttpPost]
        //Se invoca desde el boton actualizar o imprimir.
        public ActionResult List(EstadoCuentaModel model) {
            try {
                Session[model.idParametros] = model; //filtros actualizados
                ViewData["idParametros"] = model.idParametros;
                this.eliminarValidacionesIgnorables("EstadoCuenta.Cuenta", MetadataManager.IgnorablesDDL(new CuentaBancaria()));
                if (ModelState.IsValid) {
                    if (model.Accion == EstadoCuentaModel.ACCIONES.IMPRIMIR) {
                        return this.Report(model);
                    }
                    Usuario u = getUsuario();
                    model.EstadoCuenta.generarListado(u);
                }
                return View(model);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(model);
            }
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de cuotas. Devuelve la partial del tab de cuotas
        public ActionResult ListGrillaMovimientosRep(string idParametros) {
            EstadoCuentaModel model = (EstadoCuentaModel)Session[idParametros];
            ViewData["idParametros"] = model.idParametros;
            Usuario u = getUsuario();
            model.EstadoCuenta.generarListado(u);
            return PartialView("_listGrillaMovimientosRep", model);
        }

        public ActionResult Report(EstadoCuentaModel model) {
            return View("Report", model);
        }


        private XtraReport _generarReporte(string idParametros) {
            EstadoCuentaModel model = (EstadoCuentaModel)Session[idParametros];
            Usuario u = getUsuario();
            model.EstadoCuenta.generarListado(u);
            List<EstadoCuenta> ll = new List<EstadoCuenta>();
            ll.Add(model.EstadoCuenta);
            XtraReport rep = new DXEstadoCuenta();
            rep.DataSource = ll;
            //setParamsToReport(rep, model);
            return rep;
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

        #region Mantenimiento

         [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Saldos() {
            BancoSaldoModel model = new BancoSaldoModel();
            try {
                string s = SessionUtils.generarIdVarSesion("SaldosBanco", getUserName());
                model.idParametros = s;
                Session[s] = model;
                ViewData["idParametros"] = model.idParametros;
                model.generarListado();
                return View(model);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(model);
            }
        }

        [HttpPost]
        //se invoca desde el boton actualizar e imprimir. Devuelve la pagina completa
        public ActionResult Saldos(BancoSaldoModel model) {
            try {
                Session[model.idParametros] = model; //filtros actualizados
                ViewData["idParametros"] = model.idParametros;
                if (model.Accion == BancoSaldoModel.ACCIONES.IMPRIMIR) {
                    return this.ReportSaldos(model);
                }
                model.generarListado();
                return View(model);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(model);
            }
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de cuotas. 
        public ActionResult GrillaSaldos(string idParametros) {
            BancoSaldoModel model = (BancoSaldoModel)Session[idParametros];
            ViewData["idParametros"] = model;
            model.generarListado();
            return PartialView("_saldosGrilla", model.Saldos);
        }

        public ActionResult ReportSaldos(BancoSaldoModel model) {
            return View("ReportSaldos", model);
        }


        private XtraReport _generarReporteSaldos(string idParametros) {
            BancoSaldoModel model = (BancoSaldoModel)Session[idParametros];
            model.generarListado();
            List<BancoSaldoModel> ll = new List<BancoSaldoModel>();
            ll.Add(model);
            XtraReport rep = new DXSaldosBanco();
            rep.DataSource = ll;
            //setParamsToReport(rep, model);
            return rep;
        }

        public ActionResult ReportPartialSaldos(string idParametros) {
            XtraReport rep = _generarReporteSaldos(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reportSaldos");
        }

        public ActionResult ReportExportSaldos(string idParametros) {
            XtraReport rep = _generarReporteSaldos(idParametros);
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }


        #endregion

    }
}
