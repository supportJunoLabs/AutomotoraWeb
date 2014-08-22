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

namespace AutomotoraWeb.Controllers.Financing
{
    public class FinancistasController : FinancingController, IMaintenance {

        public static string CONTROLLER = "Financistas";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            ViewBag.NombreEntidad = "Financista";
            ViewBag.NombreEntidades = "Financistas";
            ViewBag.Financistas = Financista.FinancistasTodos;
        }

        #region Mantenimiento

        public ActionResult Show([ModelBinder(typeof(DevExpressEditorsBinder))] Financista fin) {
            return View(_listaElementos());
        }

        public ActionResult ListaGrilla() {
            return PartialView("_listGrilla", _listaElementos());
        }


        //--------------------------------------    REPORT    ----------------------------------------------
        public ActionResult Report() {
            return View();
        }

        public ActionResult ReportPartial() {
            DXReportFinancistas rep = new DXReportFinancistas();
            rep.DataSource = _listaElementos();
            ViewData["Report"] = rep;
            return PartialView("_reportList");
        }

        public ActionResult ReportExport() {
            DXReportFinancistas rep = new DXReportFinancistas();
            //setParamsToReport(rep);
            rep.DataSource = _listaElementos();
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

       //--------------------------------------------------------------------------------------------------

        public ActionResult Details(int id) {
            ViewBag.SoloLectura = true;
            return VistaElemento(id);
        }

        public ActionResult Create() {
            Financista fin = new Financista();
            return View(fin); //para que tenga los valores inicializados por defecto en el nuevo objeto.
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
                Financista fin = _obtenerElemento(id);
                return View(fin);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private Financista _obtenerElemento(int id) {
            Financista fin = new Financista();
            fin.Codigo = id;
            fin.Consultar();
            return fin;
        }

        private List<Financista> _listaElementos() {
            return Financista.FinancistasTodos;
        }

        //-----------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Create(Financista td) {

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
        public ActionResult Edit(Financista fin) {
            if (ModelState.IsValid) {
                try {
                    fin.ModificarDatos();
                    return RedirectToAction(BaseController.SHOW);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(fin);
                }
            }

            return View(fin);
        }



        [HttpPost]
        public ActionResult Delete(Financista fin) {
            ViewBag.SoloLectura = true;
            if (ModelState.IsValid) {
                try {
                    string userName = (string)HttpContext.Session.Contents[SessionUtils.SESSION_USER_NAME];
                    string IP = HttpContext.Request.UserHostAddress;
                    fin.Eliminar(userName, IP);
                    return RedirectToAction(BaseController.SHOW);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(fin);
                }
            }

            return View(fin);
        }
        //-----------------------------------------------------------------------------------------------------

        #endregion

        #region consultaSituacionFinancista

        //Se invoca desde la url del browser o desde el menu principal, o referencias externas. Devuelve la pagina completa
        public ActionResult ConsultaSituacion(int? id) {
            try {
                Financista f = new Financista();
                f.Codigo = id ?? 0;
                if (id != null && id != 0) {
                    f.Consultar();
                }
                ViewData["idParametros"] = f.Codigo;
                return View(f);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        [HttpPost]
        //se invoca desde el boton actualizar e imprimir. Devuelve la pagina completa
        public ActionResult ConsultaSituacion(Financista model, string btnSubmit) {
            try {
                if (model != null) {
                    ViewData["idParametros"] = model.Codigo;
                }
                if (btnSubmit == "Imprimir" && model != null && model.Codigo != 0) {
                    return this.ReportSitFinancista(model);
                }
                return View(model);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View(model);
            }
        }

        //Se invoca por json al actualizar la ddl de clientes, devuelve solo la partial de contenido.
        public ActionResult ConsultaSituacionPartial(int id) {
            Financista f = new Financista();
            f.Codigo = id;
            ViewData["idParametros"] = f.Codigo;
            SituacionFinancista sit = new SituacionFinancista();
            sit.generarSituacion(f);
            return PartialView("_situacionFinancista", sit);
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de cheques. Devuelve la partial del tab de cheques
        public ActionResult SitFinancistaChequesGrilla(int idParametros) {
            Financista f = new Financista();
            f.Codigo = idParametros;
            ViewData["idParametros"] = f.Codigo;
            SituacionFinancista sit = new SituacionFinancista();
            sit.generarSituacion(f);
            return PartialView("_sitFinancistaChequesGrilla", sit);
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de efectivo. Devuelve la partial del tab de efectivo
        public ActionResult SitFinancistaEfectivoGrilla(int idParametros) {
            Financista f = new Financista();
            f.Codigo = idParametros;
            ViewData["idParametros"] = f.Codigo;
            SituacionFinancista sit = new SituacionFinancista();
            sit.generarSituacion(f);
            return PartialView("_sitFinancistaEfectivoGrilla", sit);
        }

        public ActionResult ReportSitFinancista(Financista f) {
            return View("ReportSitFinancista", f);
        }

        private XtraReport _generarReport(int idParametros) {
            Financista f = new Financista();
            f.Codigo = idParametros;
            f.Consultar();
            SituacionFinancista model = new SituacionFinancista();
            model.generarSituacion(f);
            List<SituacionFinancista> ll = new List<SituacionFinancista>();
            ll.Add(model);
            XtraReport rep = new DXSituacionFinancista();
            rep.DataSource = ll;
            return rep;
        }

        public ActionResult ReportSitFinancistaPartial(int idParametros) {
            XtraReport rep = _generarReport(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reportSitFinancista");
        }

        public ActionResult ReportSitFinancistaExport(int idParametros) {
            XtraReport rep = _generarReport(idParametros);
            ViewData["idParametros"] = idParametros;
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        #endregion

        #region PagoFinancista

        public ActionResult ReciboPago(int id) {
            try {
                TRFinancistaPago tr = (TRFinancistaPago)Transaccion.ObtenerTransaccion(id);
                ViewData["idParametros"] = id;
                return View("ReciboPago", tr.Financista);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private XtraReport _generarReciboPago(int id) {
            TRFinancistaPago tr = (TRFinancistaPago)Transaccion.ObtenerTransaccion(id);
            List<TRFinancistaPago> ll = new List<TRFinancistaPago>();
            ll.Add(tr);
            XtraReport rep = new DXReciboFinancistaPago();
            rep.DataSource = ll;
            return rep;
        }

        public ActionResult ReciboPagoPartial(int idParametros) {
            XtraReport rep = _generarReciboPago(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reciboPago");
        }

        public ActionResult ReciboPagoExport(int idParametros) {
            XtraReport rep = _generarReciboPago(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        #endregion

    }
}
