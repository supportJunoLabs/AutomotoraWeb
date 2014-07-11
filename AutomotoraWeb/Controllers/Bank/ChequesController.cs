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

namespace AutomotoraWeb.Controllers.Bank {
    public class ChequesController : BankController {

        public static string CONTROLLER = "Cheques";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            Usuario usuario = (Usuario)(Session[SessionUtils.SESSION_USER]);
            if (usuario==null){
                filterContext.Result = new RedirectResult("/" + AuthenticationController.CONTROLLER + "/" + AuthenticationController.LOGIN);
                return;
            }

            ViewBag.Sucursales = Sucursal.Sucursales;
            ViewBag.Financistas = Financista.FinancistasTodos;
            ViewBag.Cuentas = CuentaBancaria.CuentasBancarias;
            if (usuario.MultiSucursal) {
                ViewBag.SucursalesTransaccion = Sucursal.Sucursales;
            } else {
                List<Sucursal> listSucursal = new List<Sucursal>();
                listSucursal.Add(usuario.Sucursal);
                ViewBag.SucursalesTransaccion = listSucursal;
            }
        }


        #region ListadoCheques

        //Se invoca desde el menu de financiaciones,en lugar de banco.
        public ActionResult ListChequesF() {
            return RedirectToAction("ListCheques");
        }


        //Se invoca desde la url del browser o desde el menu principal, o referencias externas. Devuelve la pagina completa
        public ActionResult ListCheques() {
            ListadoChequesModel model = new ListadoChequesModel();
            string s = SessionUtils.generarIdVarSesion("ListadoCheques", Session[SessionUtils.SESSION_USER].ToString());
            Session[s] = model;
            model.idParametros = s;
            ViewData["idParametros"] = model.idParametros;
            model.obtenerListado();
            return View(model);
        }

        [HttpPost]
        //Se invoca desde el boton actualizar o imprimir.
        public ActionResult ListCheques(ListadoChequesModel model) {
            Session[model.idParametros] = model; //filtros actualizados
            ViewData["idParametros"] = model.idParametros;
            //ViewBag.Financistas = Financista.Financistas(Financista.FIN_TIPO_LISTADO.TODOS);
            this.eliminarValidacionesIgnorables("Filtro.Financista", MetadataManager.IgnorablesDDL(model.Filtro.Financista));
            this.eliminarValidacionesIgnorables("Filtro.Sucursal", MetadataManager.IgnorablesDDL(model.Filtro.Sucursal));
            if (ModelState.IsValid) {
                if (model.Accion == ListadoChequesModel.ACCIONES.IMPRIMIR) {
                    return this.ReportCheques(model);
                }
                model.obtenerListado();
            }
            return View(model);
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de cuotas. Devuelve la partial del tab de cuotas
        public ActionResult ListGrillaCheques(string idParametros) {
            ListadoChequesModel model = (ListadoChequesModel)Session[idParametros];
            ViewData["idParametros"] = model.idParametros;
            model.obtenerListado();
            return PartialView("_listGrillaCheques", model.Resultado.Cheques);
        }

        public ActionResult ReportCheques(ListadoChequesModel model) {
            return View("ReportCheques", model);
        }

        private XtraReport _generarReporteCheques(string idParametros) {
            ListadoChequesModel model = (ListadoChequesModel)Session[idParametros];
            model.obtenerListado();
            List<ListadoCheques> ll = new List<ListadoCheques>();
            ll.Add(model.Resultado);
            XtraReport rep = new DXListadoCheques();
            rep.DataSource = ll;
            setParamsToReport(rep, model);
            return rep;
        }

        public ActionResult ReportChequesPartial(string idParametros) {
            XtraReport rep = _generarReporteCheques(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reportCheques");
        }

        public ActionResult ReportChequesExport(string idParametros) {
            XtraReport rep = _generarReporteCheques(idParametros);
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        #endregion


        #region ConsultaCheque

        private Cheque _consultarCheque(int? idCheque) {
            Cheque ch = new Cheque();
            ch.Codigo = idCheque ?? 0;
            if (idCheque != null && idCheque != 0) {
                ch.Consultar();
            }
            ViewData["idParametros"] = ch.Codigo;
            return ch;
        }

        public ActionResult ConsultaCheque(int? idCheque) {
            Cheque ch = _consultarCheque(idCheque);
            return View("ConsultaCheque", ch);
        }

        private void setParamsToReport(XtraReport report, ListadoChequesModel model) {
            Parameter param = new Parameter();
            param.Name = "detalleFiltros";
            param.Type = typeof(string);
            param.Value = model.detallesFiltro();
            param.Description = "Detalle Filtros";
            param.Visible = false;
            report.Parameters.Add(param);
          }

        #endregion

        #region DepositarCheque

        public ActionResult Depositar() {
            TRChequeDepositarDescontar model = new TRChequeDepositarDescontar();
            model.TipoDestino = TRChequeDepositarDescontar.TIPO_DESTINO.DEPOSITAR;
            return View(model);
        }

        //Se invoca desde paginacion, ordenacion etc, de grilla de cuotas. Devuelve la partial del tab de cuotas
        public ActionResult ChequesDepositablesGrilla() {
            return PartialView("_selectCheque", Cheque.ChequesDepositables());
        }


        [HttpPost]
        public ActionResult Depositar(TRChequeDepositarDescontar tr) {

            //Hacer las validaciones del ModelState manualmente porque los mensajes por defecto son muy feos:
            ModelState.Clear();
            if (tr.Cuenta == null || tr.Cuenta.Codigo <= 0) {
                ModelState.AddModelError("Cuenta.Codigo", "La Cuenta es requerida");
            }
            if (tr.Cheque == null || tr.Cheque.Codigo <= 0) {
                ModelState.AddModelError("Cheque.Codigo", "El Cheque es requerido");
            }
            if (string.IsNullOrWhiteSpace(tr.NumeroComprobante)) {
                ModelState.AddModelError("NumeroComprobante", "El Numero de Comprobante es requerido");
            }
            if (tr.Sucursal == null || tr.Sucursal.Codigo <= 0) {
                ModelState.AddModelError("Sucursal.Codigo", "La Sucursal es requerida");
            }


            if (ModelState.IsValid) {
                try {
                    tr.Ejecutar();
                    return RedirectToAction(BankController.INDEX, BankController.BCONTROLLER);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(tr);
                }
            }

            return View(tr);
        }

        #endregion

    }
}
