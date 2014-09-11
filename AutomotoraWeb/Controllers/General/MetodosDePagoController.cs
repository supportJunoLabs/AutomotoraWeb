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

        //public ActionResult EditModesPartial() {
        //    List<Cheque> listCheque = new List<Cheque>();
        //    return PartialView("EditModesPartial", listCheque);
        //}

        //public ActionResult grillaPagosCheque_CustomActionRouteValues(GridViewEditingMode editMode) {
        //    //GridViewEditingDemosHelper.EditMode = editMode;
        //    List<Cheque> listCheque = new List<Cheque>();
        //    return PartialView("EditModesPartial", listCheque);
        //}

        private void _validarCheque(Cheque cheque){
            eliminarValidacionesIgnorablesCheque(cheque);

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

                    Moneda monedaElejida =
                        (from m in Moneda.Monedas
                         where (m.Codigo == cheque.Importe.Moneda.Codigo)
                         select m).First<Moneda>();

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
                    chequeEditado.Importe.Monto = cheque.Importe.Monto;
                    chequeEditado.Importe.Moneda = monedaElejida;
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
            this.eliminarValidacionesIgnorables("Importe.Moneda", MetadataManager.IgnorablesDDL(cheque.Importe.Moneda));
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

        //public ActionResult EditModesPartial() {
        //    List<Efectivo> listEfectivo = new List<Efectivo>();
        //    return PartialView("EditModesPartial", listEfectivo);
        //}

        //public ActionResult grillaPagosEfectivo_CustomActionRouteValues(GridViewEditingMode editMode) {
        //    //GridViewEditingDemosHelper.EditMode = editMode;
        //    List<Efectivo> listEfectivo = new List<Efectivo>();
        //    return PartialView("EditModesPartial", listEfectivo);
        //}

        private void _validarEfectivo(Efectivo efectivo){
            
            eliminarValidacionesIgnorablesEfectivo(efectivo);

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
                    Moneda monedaElejida =
                        (from m in Moneda.Monedas
                         where (m.Codigo == efectivo.Importe.Moneda.Codigo)
                         select m).First<Moneda>();

                    Efectivo efectivoEditado =
                        (from c in listEfectivo
                         where (c.IdLinea == efectivo.IdLinea)
                         select c).First<Efectivo>();

                    efectivoEditado.Importe.Monto = efectivo.Importe.Monto;
                    efectivoEditado.Importe.Moneda = monedaElejida;
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

        private void eliminarValidacionesIgnorablesEfectivo(Efectivo efectivo) {
            this.eliminarValidacionesIgnorables("Importe.Moneda", MetadataManager.IgnorablesDDL(efectivo.Importe.Moneda));
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

        //public ActionResult EditModesPartial() {
        //    List<MovBanco> listMovBanco = new List<MovBanco>();
        //    return PartialView("EditModesPartial", listMovBanco);
        //}

        //public ActionResult grillaPagosMovBanco_CustomActionRouteValues(GridViewEditingMode editMode) {
        //    //GridViewEditingDemosHelper.EditMode = editMode;
        //    List<MovBanco> listMovBanco = new List<MovBanco>();
        //    return PartialView("EditModesPartial", listMovBanco);
        //}

        private void _validarMovBanco(MovBanco movBanco){
            eliminarValidacionesIgnorablesMovBanco(movBanco);

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

                    MovBanco movBancoEditado =
                        (from c in listMovBanco
                         where (c.IdLinea == movBanco.IdLinea)
                         select c).First<MovBanco>();

                    movBancoEditado.Cuenta = movBanco.Cuenta;
                    movBancoEditado.Cuenta.Consultar();
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

        private void eliminarValidacionesIgnorablesMovBanco(MovBanco movBanco) {
            if (movBanco.Cuenta != null) {
                this.eliminarValidacionesIgnorables("Cuenta", MetadataManager.IgnorablesDDL(movBanco.Cuenta));
            }

            //this.eliminarValidacionesIgnorables("ImporteMov.Moneda", MetadataManager.IgnorablesDDL(movBanco.ImporteMov.Moneda));
            //this.eliminarValidacionesIgnorables("TipoMov", MetadataManager.IgnorablesDDL(movBanco.TipoMov));
        }

        #endregion

        //===============================================================================================================

        #region Vales

        public ActionResult grillaPagosVale(string idSession) {
            return PartialView("_grillaPagosVale", _listaPagosVale(idSession));
        }

        private List<Vale> _listaPagosVale(string idSession) {
            List<Vale> listPagosVale = (List<Vale>)(Session[idSession + SessionUtils.VALES]);
            return listPagosVale;
        }

        //public ActionResult EditModesPartial() {
        //    List<Vale> listVale = new List<Vale>();
        //    return PartialView("EditModesPartial", listVale);
        //}


        private void _validarVale(Vale vale) {
            eliminarValidacionesIgnorablesVale(vale);

            //Sacar la validacion de moneda no nula porque da mensaje feo, hacerla manualmente
            ModelState.Remove("Importe.Moneda.Codigo");
            if (vale.Importe.Moneda == null || vale.Importe.Moneda.Codigo <= 0) {
                ModelState.AddModelError("Importe.Moneda.Codigo", "La moneda es requerida");
            }

            //validar el importe
            if (vale.Importe.Monto <= 0) {
                ModelState.AddModelError("Importe.Monto", "El monto debe ser un valor positivo");
            }

        }

        //public ActionResult grillaPagosVale_CustomActionRouteValues(GridViewEditingMode editMode) {
        //    //GridViewEditingDemosHelper.EditMode = editMode;
        //    List<Vale> listVale = new List<Vale>();
        //    return PartialView("EditModesPartial", listVale);
        //}

        [HttpPost, ValidateInput(false)]
        public ActionResult grillaPagosVale_AddNewRowRouteValues(Vale vale, string idSession) {

            List<Vale> listVale = _listaPagosVale(idSession);

            _validarVale(vale);
            if (ModelState.IsValid) {
                try {
                    int maxIdLinea = listVale.Count > 0 ? listVale.Max(c => c.IdLinea) : 0;
                    vale.IdLinea = maxIdLinea + 1;
                    listVale.Add(vale);
                } catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            } else {
                ViewData["EditError"] = "Corrija los valores incorrectos";
            }

            return PartialView("_grillaPagosVale", listVale);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult grillaPagosVale_UpdateRowRouteValues(Vale vale, string idSession) {

            List<Vale> listVale = _listaPagosVale(idSession);

            _validarVale(vale);
            if (ModelState.IsValid) {
                try {

                    Moneda monedaElejida =
                        (from m in Moneda.Monedas
                         where (m.Codigo == vale.Importe.Moneda.Codigo)
                         select m).First<Moneda>();

                    Vale valeEditado =
                        (from c in listVale
                         where (c.IdLinea == vale.IdLinea)
                         select c).First<Vale>();

                    valeEditado.Vencimiento = vale.Vencimiento;
                    valeEditado.Importe.Monto = vale.Importe.Monto;
                    valeEditado.Importe.Moneda = monedaElejida;
                    valeEditado.Observaciones = vale.Observaciones;

                } catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            } else {
                ViewData["EditError"] = "Corrija los valores incorrectos";
            }

            return PartialView("_grillaPagosVale", listVale);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult grillaPagosVale_DeleteRowRouteValues(int IdLinea, string idSession) {

            List<Vale> listVale = _listaPagosVale(idSession);

            if (IdLinea >= 0) {
                try {
                    Vale ValeEliminado =
                        (from c in listVale
                         where (c.IdLinea == IdLinea)
                         select c).First<Vale>();
                    listVale.Remove(ValeEliminado);
                } catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_grillaPagosVale", listVale);
        }

        private void eliminarValidacionesIgnorablesVale(Vale vale) {
            this.eliminarValidacionesIgnorables("Importe.Moneda", MetadataManager.IgnorablesDDL(vale.Importe.Moneda));
        }

        #endregion

        //===============================================================================================================

        #region Vales


        public ActionResult grillaPagosCuota(string idSession) {
            return PartialView("_grillaPagosCuota", _listaPagosCuota(idSession));
        }

        private List<Cuota> _listaPagosCuota(string idSession) {
            List<Cuota> listPagosCuota = (List<Cuota>)(Session[idSession + SessionUtils.CUOTAS]);
            return listPagosCuota;
        }


        [HttpPost]
        public JsonResult cambiarFinanciacion(int cantCuotas, double tasa, int codigoMonedaImporte, double montoImporte, string idSession) {

            Moneda moneda = new Moneda();
            moneda.Codigo = codigoMonedaImporte;
            moneda.Consultar();

            Importe importe = new Importe(moneda, montoImporte);

            Financiacion financiacion = new Financiacion(importe, cantCuotas, tasa);

            //validacion del controller
            List<String> errors = this.validateAtributesFinanciacion(financiacion);
            if (errors.Count > 0) {
                return Json(new { Result = "ERROR", ErrorCode = "VALIDATION_ERROR", ErrorMessage = errors.ToArray() });
            }

            //envio al backend
            try {
                financiacion.generarCuotasVenta(DateTime.Now);

                Session[idSession + SessionUtils.CUOTAS] = financiacion.CuotasOriginales;

                return Json(new { Result = "OK" });
            } catch (UsuarioException exc) {
                List<String> errores1 = new List<string>();
                errores1.Add(exc.Message);
                return Json(new { Result = "ERROR", ErrorCode = exc.Codigo, ErrorMessage = errores1.ToArray() });
            }

        }

        //-------------------------------

        private List<String> validateAtributesFinanciacion(Financiacion financiacion) {

            List<String> errors = new List<String>();

            if (financiacion.MontoFinanciado.Monto <= 0) {
                errors.Add("El monto financiado debe ser mayor a 0");
            }

            if (financiacion.CantCuotas <= 0) {
                errors.Add("La cantidad de cuotas debe ser mayor a 0");
            }

            return errors;
        }

        //-------------------------------

        #endregion
        //===============================================================================================================

        public override string getParentControllerName() {
            return "BaseController";
        }
    }
}
