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

        public ActionResult grillaPagosCheque(string idSession, int idParametros) {
            return PartialView("_grillaPagosCheque", _listaPagosCheque(idSession));
        }

        private List<Cheque> _listaPagosCheque(string idSession) {
            //Venta venta = (Venta)(Session[idSession]);

            List<Cheque> listCheque = new List<Cheque>();
            Cheque cheque = new Cheque();
            cheque.Banco = "Banco1";
            cheque.Codigo = 1;
            Importe importe = new Importe();
            importe.Monto = 1;
            cheque.Importe = importe;
            listCheque.Add(cheque);

            //venta.Pago.agregarCheque(cheque);

            //return venta.Pago.Cheques;

            ViewBag.Monedas = Moneda.Monedas;

            return listCheque;
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
        public ActionResult grillaPagosCheque_AddNewRowRouteValues(Cheque cheque) {

            eliminarValidacionesIgnorablesCheque(cheque);

            List<Cheque> listCheque = new List<Cheque>();

            if (ModelState.IsValid) {
                try {
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
        public ActionResult grillaPagosCheque_UpdateRowRouteValues(Cheque cheque) {

            eliminarValidacionesIgnorablesCheque(cheque);

            List<Cheque> listCheque = new List<Cheque>();

            if (ModelState.IsValid) {
                try {
                    listCheque.Add(cheque);
                } catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            } else
                ViewData["EditError"] = "Please, correct all errors.";

            return PartialView("_grillaPagosCheque", listCheque);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult grillaPagosCheque_DeleteRowRouteValues(int codigo) {

            List<Cheque> listCheque = new List<Cheque>();

            if (codigo >= 0) {
                try {
                    // TODO: Delete
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
