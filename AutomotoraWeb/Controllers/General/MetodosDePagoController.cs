using AutomotoraWeb.Utils;
using DevExpress.Web.ASPxGridView;
using DLL_Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomotoraWeb.Controllers.General
{
    public class MetodosDePagoController : BaseController
    {

        #region Cheques

        public ActionResult grillaPagosCheque(string idSession) {
            return PartialView("_grillaPagosCheque", _listaPagosCheque(idSession));
        }

        private List<Cheque> _listaPagosCheque(string idSession) {

            List<Cheque> listPagosCheques = (List<Cheque>)(Session[idSession + SessionUtils.CHEQUES]);
            ViewBag.Monedas = Moneda.Monedas;
            return listPagosCheques;

        }

        public ActionResult EditModesPartial() {
            List<Cheque> listCheque = new List<Cheque>();
            return PartialView("EditModesPartial", listCheque);
        }

        public ActionResult grillaPagosCheque_CustomActionRouteValues(GridViewEditingMode editMode) {
            //GridViewEditingDemosHelper.EditMode = editMode;
            List<Cheque> listCheque = new List<Cheque>();
            return PartialView("EditModesPartial", listCheque);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult grillaPagosCheque_AddNewRowRouteValues(Cheque cheque, string idSession) {

            eliminarValidacionesIgnorablesCheque(cheque);

            List<Cheque> listCheque = _listaPagosCheque(idSession);

            if (ModelState.IsValid) {
                try {
                    int maxIdLinea = listCheque.Count > 0 ? listCheque.Max(c => c.IdLinea) : 0;
                    cheque.IdLinea = maxIdLinea + 1;
                    listCheque.Add(cheque);
                } catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            } else {
                ViewData["EditError"] = "Please, correct all errors.";
            }

            return PartialView("_grillaPagosCheque", listCheque);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult grillaPagosCheque_UpdateRowRouteValues(Cheque cheque, string idSession) {

            eliminarValidacionesIgnorablesCheque(cheque);

            List<Cheque> listCheque = _listaPagosCheque(idSession);

            if (ModelState.IsValid) {
                try {
                    
                    Moneda monedaElejida =
                        (from m in Moneda.Monedas
                         where (m.Codigo == cheque.Importe.Moneda.Codigo)
                         select m).First<Moneda>();

                    Cheque chequeEditado =
                        (from c in listCheque
                         where (c.IdLinea == cheque.IdLinea)
                         select c).First<Cheque>();

                    chequeEditado.Banco = cheque.Banco;
                    chequeEditado.Cuenta  = cheque.Cuenta;
                    chequeEditado.Librador = cheque.Librador;
                    chequeEditado.NumeroCheque = cheque.NumeroCheque;
                    chequeEditado.FechaValor = cheque.FechaValor;
                    chequeEditado.FechaVencimiento = cheque.FechaVencimiento;
                    chequeEditado.Observaciones = cheque.Observaciones;
                    chequeEditado.Banco = cheque.Banco;
                    chequeEditado.Importe.Monto = cheque.Importe.Monto;
                    chequeEditado.Importe.Moneda = monedaElejida;
                } catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            } else
                ViewData["EditError"] = "Please, correct all errors.";

            return PartialView("_grillaPagosCheque", listCheque);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult grillaPagosCheque_DeleteRowRouteValues(int IdLinea, string idSession) {

            List<Cheque> listCheque = _listaPagosCheque(idSession);

            if (IdLinea >= 0) {
                try {
                    Cheque chequeEliminado =
                        (from c in listCheque
                         where (c.IdLinea == IdLinea)
                         select c).First<Cheque>();
                    listCheque.Remove(chequeEliminado);
                } catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_grillaPagosCheque", listCheque);
        }

        private void eliminarValidacionesIgnorablesCheque(Cheque cheque) {
            this.eliminarValidacionesIgnorables("Importe.Moneda", MetadataManager.IgnorablesDDL(cheque.Importe.Moneda));
        }

        #endregion

        public override string getParentControllerName() {
            return "BaseController";
        }
    }
}
