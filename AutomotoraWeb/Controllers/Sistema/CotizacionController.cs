using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DLL_Backend;
using AutomotoraWeb.Utils;

namespace AutomotoraWeb.Controllers.Sistema
{
    public class CotizacionController : SistemaController  {

        public static string CONTROLLER = "Cotizacion";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
        }

        public ActionResult Edit() {
            return View(Moneda.MonedasCotizacion());
        }

        public ActionResult grillaCotizaciones() {
            return PartialView("_grillaCotizaciones", Moneda.MonedasCotizacion());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult grillaCotizaciones_UpdateRow(Moneda model) {
            if (ModelState.IsValid) {
                try {
                    string nomUsuario = getUserName();
                    string origen = getIP();
                    model.setearAuditoria(nomUsuario, origen);
                    model.ModificarCotizacion();
                } catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            } else {
                ViewData["EditError"] = "Corrija los valores incorrectos";
            }
            //throw new Exception("excepcion de prueba");
            return PartialView("_grillaCotizaciones", Moneda.MonedasCotizacion());
        }

    }
}
