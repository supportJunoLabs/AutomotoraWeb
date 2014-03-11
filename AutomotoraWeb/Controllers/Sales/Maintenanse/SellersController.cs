using AutomotoraWeb.Controllers.General;
using AutomotoraWeb.Models;
using AutomotoraWeb.Services;
using AutomotoraWeb.Utils;
using DLL_Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomotoraWeb.Controllers.Sales.Maintenanse {
    public class SellersController : SalesController, IMaintenance {

        public static string SELLERS = "sellers";

        public ActionResult Show() {
            return View();
        }

        public ActionResult Details(int id) {
            return getSeller(id);
        }

        public ActionResult Create() {
            return View();
        }

        public ActionResult Edit(int id) {
            return getSeller(id);
        }

        public ActionResult Delete(int id) {
            return getSeller(id);
        }

        //-----------------------------------------------------------------------------------------------------

        private ActionResult getSeller(int id) {
            try {
                SellerModel sellerModel = SalesService.Instance.getSeller(id);
                return View(sellerModel);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            } catch (Exception exc) {
                ViewBag.ErrorCode = ERROR_CODE_SYSTEM_ERROR;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        //-----------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Create(Vendedor vendedor) {

            if (ModelState.IsValid) {
                try {
                    vendedor.setearAuditoria(SessionUtils.SESSION_USER_NAME, Request.UserHostAddress);
                    vendedor.Agregar();
                    return RedirectToAction(BaseController.SHOW, SELLERS);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(vendedor);
                } catch (Exception exc) {
                    ViewBag.ErrorCode = ERROR_CODE_SYSTEM_ERROR;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(vendedor);
                }
            }

            return View(vendedor);
        }

        //-----------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Edit(Vendedor vendedor) {
            if (ModelState.IsValid) {
                try {
                    vendedor.setearAuditoria(SessionUtils.SESSION_USER_NAME, Request.UserHostAddress);
                    vendedor.ModificarDatos();
                    return RedirectToAction(BaseController.SHOW, SELLERS);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(vendedor);
                } catch (Exception exc) {
                    ViewBag.ErrorCode = ERROR_CODE_SYSTEM_ERROR;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(vendedor);
                }
            }

            return View(vendedor);
        }

        //-----------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Delete(Vendedor vendedor) {
            if (ModelState.IsValid) {
                try {
                    vendedor.setearAuditoria(SessionUtils.SESSION_USER_NAME, Request.UserHostAddress);
                    vendedor.Eliminar();
                    return RedirectToAction(BaseController.SHOW, SELLERS);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(vendedor);
                } catch (Exception exc) {
                    ViewBag.ErrorCode = ERROR_CODE_SYSTEM_ERROR;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(vendedor);
                }
            }

            return View(vendedor);
        }

    }
}
