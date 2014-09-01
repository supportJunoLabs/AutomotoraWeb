using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DLL_Backend;

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
                    model.ModificarCotizacion();
                } catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            } else {
                ViewData["EditError"] = "Please, correct all errors.";
            }

            return PartialView("_grillaCotizaciones", Moneda.MonedasCotizacion());
        }

    }
}
