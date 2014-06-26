using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DLL_Backend;

namespace AutomotoraWeb.Controllers.Bank {
    public class ChequesListController : BankController {

        public static string CONTROLLER = "ChequesList";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            //ViewBag.Clientes = Cliente.Clientes();
        }


   
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
