using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DLL_Backend;
using AutomotoraWeb.Models;
using AutomotoraWeb.Utils;

namespace AutomotoraWeb.Controllers.Bank
{
    public class MovimientosController : BankController
    {
       public static string CONTROLLER = "Movimientos";

       protected override void OnActionExecuting(ActionExecutingContext filterContext) {
           base.OnActionExecuting(filterContext);
           ViewBag.NombreEntidad = "Movimiento Bancario";
           ViewBag.NombreEntidades = "Movimientos Bancarios";
           ViewBag.Cuentas = CuentaBancaria.CuentasBancarias;
       }

       public ActionResult Show(int? id) {
           MovimientosBancoModel model = new MovimientosBancoModel();
           if (id != null && id > 0) {
               model.Cuenta = new CuentaBancaria();
               model.Cuenta.Codigo = (id ?? 0);
           }
           string s = SessionUtils.generarIdVarSesion("MovimientosBanco", Session[SessionUtils.SESSION_USER].ToString());
           model.idParametros = s;
           Session[s] = model;
           ViewData["idParametros"] = model.idParametros;
           model.generarListado();
           return View(model);
       }

       [HttpPost]
       //se invoca desde el boton actualizar e imprimir. Devuelve la pagina completa
       public ActionResult Show(MovimientosBancoModel model) {
           Session[model.idParametros] = model; //filtros actualizados
           ViewData["idParametros"] = model.idParametros;
           this.eliminarValidacionesIgnorables("Cuenta", MetadataManager.IgnorablesDDL(model.Cuenta));
           model.generarListado();
           if (ModelState.IsValid) {
               model.generarListado();
           } else {
               model.Resultado = new List<MovBanco>();
           }
           return View(model);
       }

       //Se invoca desde paginacion, ordenacion etc, de grilla de cuotas. 
       public ActionResult GrillaMovimientos(string idParametros) {
           MovimientosBancoModel model = (MovimientosBancoModel)Session[idParametros];
           ViewData["idParametros"] = model;
           this.eliminarValidacionesIgnorables("Cuenta", MetadataManager.IgnorablesDDL(model.Cuenta));
           model.generarListado();
           if (ModelState.IsValid) {
               model.generarListado();
           } else {
               model.Resultado = new List<MovBanco>();
           }
           return PartialView("_listGrillaMovimientos", model);
       }

       public ActionResult Create(int id) {
           MovBanco mov = new MovBanco();
           mov.Cuenta = new CuentaBancaria();
           mov.Cuenta.Codigo = id;
           mov.Cuenta.Consultar();
           mov.ImporteMov = new Importe();
           mov.ImporteMov.Moneda = mov.Cuenta.Moneda;
           mov.FechaMov = DateTime.Now.Date;
           return View(mov);
       }

       [HttpPost]
       public ActionResult Create(MovBanco mov) {
           this.eliminarValidacionesIgnorables("ImporteMov.Moneda", MetadataManager.IgnorablesDDL(mov.ImporteMov.Moneda));
           this.eliminarValidacionesIgnorables("Cuenta", MetadataManager.IgnorablesDDL(mov.Cuenta));
           if (ModelState.IsValid) {
               try {
                   mov.Agregar();
                   return RedirectToAction("Show", new { id = mov.Cuenta.Codigo });
               } catch (UsuarioException exc) {
                   ViewBag.ErrorCode = exc.Codigo;
                   ViewBag.ErrorMessage = exc.Message;
                   return View(mov);
               }
           }
           return View(mov);
       }

       [HttpPost]
       public JsonResult Conciliar(int id) {
            MovBanco mov = new MovBanco();
            mov.Codigo = id;
            mov.Consultar();
            if (mov.ConciliadoMov) {
                return Json(new { Result = "ERROR", ErrorMessage = "Ya esta conciliado" });
            }
            mov.Conciliar(DateTime.Now.Date);
            return Json(new { Result = "OK"});
       }

       [HttpPost]
       public JsonResult Desconciliar(int id) {
           MovBanco mov = new MovBanco();
           mov.Codigo = id;
           mov.Consultar();
           if (!mov.ConciliadoMov) {
               return Json(new { Result = "ERROR", ErrorMessage = "No esta conciliado" });
           }
           mov.DesConciliar(); 
           return Json(new { Result = "OK" });
       }

    }
}
