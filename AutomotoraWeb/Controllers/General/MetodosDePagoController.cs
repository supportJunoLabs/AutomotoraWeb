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
            return listPagosCheques;

        }

        private void _validarCheque(Cheque cheque){
            this.eliminarValidacionesIgnorables("Importe.Moneda", MetadataManager.IgnorablesDDL(new Moneda()));

            //Sacar la validacion de moneda no nula porque da mensaje feo, hacerla manualmente
            ModelState.Remove("Importe.Moneda.Codigo");
            if (cheque.Importe.Moneda == null || cheque.Importe.Moneda.Codigo <= 0) {
                ModelState.AddModelError("Importe.Moneda.Codigo", "La moneda es requerida");
            }

            //validar el importe
            if (cheque.Importe.Monto <= 0) {
                ModelState.AddModelError("Importe.Monto", "El monto debe ser un valor positivo");
            }

            if (cheque.FechaVencimiento < cheque.FechaValor) {
                ModelState.AddModelError("FechaVencimiento", "El vencimiento no puede ser anterior a la emision");
            }

        }

        [HttpPost, ValidateInput(false)]
        public ActionResult grillaPagosCheque_AddNewRowRouteValues(Cheque cheque, string idSession) {

            List<Cheque> listCheque = _listaPagosCheque(idSession);

            _validarCheque(cheque);
            if (ModelState.IsValid) {
                try {
                    cheque.Importe.Moneda.Consultar();
                    int maxIdLinea = listCheque.Count > 0 ? listCheque.Max(c => c.IdLinea) : 0;
                    cheque.IdLinea = maxIdLinea + 1;
                    listCheque.Add(cheque);
                } catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            } else {
                ViewData["EditError"] = "Corrija los valores incorrectos";
            }

            return PartialView("_grillaPagosCheque", listCheque);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult grillaPagosCheque_UpdateRowRouteValues(Cheque cheque, string idSession) {

            List<Cheque> listCheque = _listaPagosCheque(idSession);

            _validarCheque(cheque);
            if (ModelState.IsValid) {
                try {

                    cheque.Importe.Moneda.Consultar();
                    Cheque chequeEditado =
                        (from c in listCheque
                         where (c.IdLinea == cheque.IdLinea)
                         select c).First<Cheque>();

                    chequeEditado.Banco = cheque.Banco;
                    chequeEditado.Cuenta = cheque.Cuenta;
                    chequeEditado.Librador = cheque.Librador;
                    chequeEditado.NumeroCheque = cheque.NumeroCheque;
                    chequeEditado.FechaValor = cheque.FechaValor;
                    chequeEditado.FechaVencimiento = cheque.FechaVencimiento;
                    chequeEditado.Observaciones = cheque.Observaciones;
                    chequeEditado.Banco = cheque.Banco;
                    chequeEditado.Importe = cheque.Importe;

                } catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            } else {
                ViewData["EditError"] = "Corrija los valores incorrectos";
            }

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
            
        }

        #endregion

        //===============================================================================================================

        #region Efectivo

        public ActionResult grillaPagosEfectivo(string idSession) {
            return PartialView("_grillaPagosEfectivo", _listaPagosEfectivo(idSession));
        }

        private List<Efectivo> _listaPagosEfectivo(string idSession) {

            List<Efectivo> listPagosEfectivo = (List<Efectivo>)(Session[idSession + SessionUtils.EFECTIVO]);
            return listPagosEfectivo;

        }

        private void _validarEfectivo(Efectivo efectivo){

            this.eliminarValidacionesIgnorables("Importe.Moneda", MetadataManager.IgnorablesDDL(new Moneda()));

            ModelState.Remove("Importe.ImporteEnMonedaDefault.Monto");

            //Sacar la validacion de moneda no nula porque da mensaje feo, hacerla manualmente
            ModelState.Remove("Importe.Moneda.Codigo");
            if (efectivo.Importe.Moneda == null || efectivo.Importe.Moneda.Codigo <= 0) {
                ModelState.AddModelError("Importe.Moneda.Codigo", "La moneda es requerida");
            }

            //validar el importe
            if (efectivo.Importe.Monto <= 0 ) {
                ModelState.AddModelError("Importe.Monto", "El monto debe ser un valor positivo");
            }
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult grillaPagosEfectivo_AddNewRowRouteValues(Efectivo efectivo, string idSession) {

            List<Efectivo> listEfectivo = _listaPagosEfectivo(idSession);

            _validarEfectivo(efectivo);
            if (ModelState.IsValid) {
                try {
                    efectivo.Importe.Moneda.Consultar();
                    int maxIdLinea = listEfectivo.Count > 0 ? listEfectivo.Max(c => c.IdLinea) : 0;
                    efectivo.IdLinea = maxIdLinea + 1;
                    listEfectivo.Add(efectivo);
                } catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            } else {
                ViewData["EditError"] = "Corrija los valores incorrectos";
            }

            return PartialView("_grillaPagosEfectivo", listEfectivo);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult grillaPagosEfectivo_UpdateRowRouteValues(Efectivo efectivo, string idSession) {

            //pruebaError();
            List<Efectivo> listEfectivo = _listaPagosEfectivo(idSession);

            _validarEfectivo(efectivo);
            if (ModelState.IsValid) {
                try {

                    efectivo.Importe.Moneda.Consultar();
                    Efectivo efectivoEditado =
                        (from c in listEfectivo
                         where (c.IdLinea == efectivo.IdLinea)
                         select c).First<Efectivo>();
                    efectivoEditado.Importe = efectivo.Importe;
                } catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            } else {
                ViewData["EditError"] = "Corrija los valores incorrectos";
            }

            return PartialView("_grillaPagosEfectivo", listEfectivo);
        }

        private void pruebaError() {
            throw new Exception("Error de prueba a proposito para probar excepciones");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult grillaPagosEfectivo_DeleteRowRouteValues(int IdLinea, string idSession) {

            List<Efectivo> listEfectivo = _listaPagosEfectivo(idSession);

            if (IdLinea >= 0) {
                try {
                    Efectivo efectivoEliminado =
                        (from c in listEfectivo
                         where (c.IdLinea == IdLinea)
                         select c).First<Efectivo>();
                    listEfectivo.Remove(efectivoEliminado);
                    
                } catch (Exception e) {
                    ViewData["DeleteError"] = e.Message;
                }
            }
            //ViewData["DeleteError"] = "Testing error message style";
            return PartialView("_grillaPagosEfectivo", listEfectivo);
        }



        #endregion

        //===============================================================================================================

        #region Movimientos Bancarios

        public ActionResult grillaPagosMovBanco(string idSession) {
            return PartialView("_grillaPagosMovBanco", _listaPagosMovBanco(idSession));
        }

        private List<MovBanco> _listaPagosMovBanco(string idSession) {

            List<MovBanco> listPagosMovBanco = (List<MovBanco>)(Session[idSession + SessionUtils.MOV_BANCARIO]);
            return listPagosMovBanco;

        }


        private void _validarMovBanco(MovBanco movBanco){
            if (movBanco.Cuenta != null) {
                this.eliminarValidacionesIgnorables("Cuenta", MetadataManager.IgnorablesDDL(new CuentaBancaria()));
            }

            //Sacar la validacion de moneda no nula porque da mensaje feo, hacerla manualmente
            ModelState.Remove("Cuenta.Codigo");
            if (movBanco.Cuenta== null || movBanco.Cuenta.Codigo <= 0) {
                ModelState.AddModelError("Cuenta.Codigo", "La cuenta es requerida");
            }

            //Eliminar la validacion de la moneda, se tomara a partir de la moneda de la cuenta elegida
            ModelState.Remove("ImporteMov.Moneda");

            //validar el importe
            if (movBanco.ImporteMov.Monto <= 0) {
                ModelState.AddModelError("ImporteMov.Monto", "El monto debe ser un valor positivo");
            }

        }

        [HttpPost, ValidateInput(false)]
        public ActionResult grillaPagosMovBanco_AddNewRowRouteValues(MovBanco movBanco, string idSession) {
            List<MovBanco> listMovBanco = _listaPagosMovBanco(idSession);

            _validarMovBanco(movBanco);
            if (ModelState.IsValid) {
                try {
                    int maxIdLinea = listMovBanco.Count > 0 ? listMovBanco.Max(c => c.IdLinea) : 0;
                    movBanco.IdLinea = maxIdLinea + 1;

                    movBanco.Cuenta.Consultar();
                    movBanco.ImporteMov.Moneda = movBanco.Cuenta.Moneda;

                    listMovBanco.Add(movBanco);
                } catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            } else {
                ViewData["EditError"] = "Corrija los valores incorrectos";
            }

            return PartialView("_grillaPagosMovBanco", listMovBanco);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult grillaPagosMovBanco_UpdateRowRouteValues(MovBanco movBanco, string idSession) {

            List<MovBanco> listMovBanco = _listaPagosMovBanco(idSession);

            _validarMovBanco(movBanco);
            if (ModelState.IsValid) {
                try {

                    movBanco.Cuenta.Consultar();
                    MovBanco movBancoEditado =
                        (from c in listMovBanco
                         where (c.IdLinea == movBanco.IdLinea)
                         select c).First<MovBanco>();

                    movBancoEditado.Cuenta = movBanco.Cuenta;
                    movBancoEditado.ImporteMov.Moneda = movBanco.Cuenta.Moneda;
                    movBancoEditado.ImporteMov.Monto = movBanco.ImporteMov.Monto;
                    movBancoEditado.FechaMov = movBanco.FechaMov;
                    movBancoEditado.ConceptoMov = movBanco.ConceptoMov;
                    movBancoEditado.DescripcionMov = movBanco.DescripcionMov;

                } catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            } else {
                ViewData["EditError"] = "Corrija los valores incorrectos";
            }

            return PartialView("_grillaPagosMovBanco", listMovBanco);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult grillaPagosMovBanco_DeleteRowRouteValues(int IdLinea, string idSession) {

            List<MovBanco> listMovBanco = _listaPagosMovBanco(idSession);

            if (IdLinea >= 0) {
                try {
                    MovBanco MovBancoEliminado =
                        (from c in listMovBanco
                         where (c.IdLinea == IdLinea)
                         select c).First<MovBanco>();
                    listMovBanco.Remove(MovBancoEliminado);
                } catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_grillaPagosMovBanco", listMovBanco);
        }


        #endregion

        //===============================================================================================================

     


        #region ChequesEmitidos

        public ActionResult grillaPagosChequesEmitidos(string idSession) {
            return PartialView("_grillaPagosChequesEmitidos", _listaPagosChequesEmitidos(idSession));
        }

        private List<ChequeEmitido> _listaPagosChequesEmitidos(string idSession) {

            List<ChequeEmitido> listPagosCheques = (List<ChequeEmitido>)(Session[idSession + SessionUtils.CHEQUES_EMITIDOS]);
            return listPagosCheques;

        }

        private void _validarChequeEmitido(ChequeEmitido cheque) {
            this.eliminarValidacionesIgnorables("Cuenta", MetadataManager.IgnorablesDDL(cheque.Cuenta));

            ModelState.Remove("Importe.Moneda");//porque se va a tomar la moneda de la cuenta elegida

            //Sacar la validacion de cuenta no nula porque da mensaje feo, hacerla manualmente
            ModelState.Remove("Cuenta.Codigo");
            if (cheque.Cuenta == null || cheque.Cuenta.Codigo <= 0) {
                ModelState.AddModelError("Cuenta.Codigo", "La cuenta bancaria es requerida");
            }

            //validar el importe
            if (cheque.Importe.Monto <= 0) {
                ModelState.AddModelError("Importe.Monto", "El monto debe ser un valor positivo");
            }

            if (cheque.FechaVencimiento < cheque.FechaEmision) {
                ModelState.AddModelError("FechaVencimiento", "El vencimiento no puede ser anterior a la emision");
            }

        }

        [HttpPost, ValidateInput(false)]
        public ActionResult grillaPagosChequesEmi_Add(ChequeEmitido cheque, string idSession) {

            List<ChequeEmitido> listCheque = _listaPagosChequesEmitidos(idSession);

            _validarChequeEmitido(cheque);
            if (ModelState.IsValid) {
                try {
                    cheque.Cuenta.Consultar();
                    cheque.Importe.Moneda = cheque.Cuenta.Moneda;
                    int maxIdLinea = listCheque.Count > 0 ? listCheque.Max(c => c.IdLinea) : 0;
                    cheque.IdLinea = maxIdLinea + 1;
                    listCheque.Add(cheque);
                } catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            } else {
                ViewData["EditError"] = "Corrija los valores incorrectos";
            }

            return PartialView("_grillaPagosChequesEmitidos", listCheque);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult grillaPagosChequesEmi_Update(ChequeEmitido cheque, string idSession) {

            List<ChequeEmitido> listCheque = _listaPagosChequesEmitidos(idSession);

            _validarChequeEmitido(cheque);
            if (ModelState.IsValid) {
                try {

                    cheque.Cuenta.Consultar();

                    ChequeEmitido chequeEditado =
                       (from c in listCheque
                        where (c.IdLinea == cheque.IdLinea)
                        select c).First<ChequeEmitido>();

                    chequeEditado.Cuenta = cheque.Cuenta;
                    chequeEditado.Importe.Moneda = cheque.Cuenta.Moneda;
                    chequeEditado.Importe.Monto = cheque.Importe.Monto;
                    chequeEditado.FechaEmision = cheque.FechaEmision;
                    chequeEditado.FechaVencimiento = cheque.FechaVencimiento;
                    chequeEditado.Numero = cheque.Numero;
                    chequeEditado.Observaciones = cheque.Observaciones;
                } catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            } else {
                ViewData["EditError"] = "Corrija los valores incorrectos";
            }

            return PartialView("_grillaPagosChequesEmitidos", listCheque);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult grillaPagosChequesEmi_Delete(int IdLinea, string idSession) {

            List<ChequeEmitido> listCheque = _listaPagosChequesEmitidos(idSession);

            if (IdLinea >= 0) {
                try {
                    ChequeEmitido chequeEliminado =
                        (from c in listCheque
                         where (c.IdLinea == IdLinea)
                         select c).First<ChequeEmitido>();
                    listCheque.Remove(chequeEliminado);
                } catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_grillaPagosChequesEmitidos", listCheque);
        }

        #endregion

        public override string getParentControllerName() {
            return "BaseController";
        }

        
    }
}
