using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DLL_Backend;
using AutomotoraWeb.Models;
using AutomotoraWeb.Utils;

namespace AutomotoraWeb.Controllers.Bank {
    public class ChequesController : BankController {

        public static string CONTROLLER = "Cheques";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            ViewBag.Sucursales = Sucursal.Sucursales;
            ViewBag.Financistas = Financista.Financistas(Financista.FIN_TIPO_LISTADO.TODOS);
        }


        #region ListadoCheques

        //Se invoca desde la url del browser o desde el menu principal, o referencias externas. Devuelve la pagina completa
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


        #endregion

    }
}
