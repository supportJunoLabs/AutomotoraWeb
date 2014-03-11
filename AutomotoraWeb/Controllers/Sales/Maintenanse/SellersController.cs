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
        public ActionResult Create(SellerModel seller) {

            if (ModelState.IsValid) {
                try {
                    SalesService.Instance.createSeller(seller);
                    return RedirectToAction(BaseController.SHOW, SELLERS);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(seller);
                } catch (Exception exc) {
                    ViewBag.ErrorCode = ERROR_CODE_SYSTEM_ERROR;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(seller);
                }
            }

            return View(seller);
        }

        //-----------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Edit(SellerModel seller) {
            if (ModelState.IsValid) {
                try {
                    SalesService.Instance.updateSeller(seller);
                    return RedirectToAction(BaseController.SHOW, SELLERS);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(seller);
                } catch (Exception exc) {
                    ViewBag.ErrorCode = ERROR_CODE_SYSTEM_ERROR;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(seller);
                }
            }

            return View(seller);
        }

        //-----------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Delete(SellerModel seller) {
            if (ModelState.IsValid) {
                try {
                    SalesService.Instance.deleteSeller(seller);
                    return RedirectToAction(BaseController.SHOW, SELLERS);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(seller);
                } catch (Exception exc) {
                    ViewBag.ErrorCode = ERROR_CODE_SYSTEM_ERROR;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(seller);
                }
            }

            return View(seller);
        }

        //-----------------------------------------------------------------------------------------------------

        [HttpPost]
        public JsonResult nameAvailable(string name, int id) {
            SellerModel seller = new SellerModel() {Name = name, Id = id};
            bool result = SalesService.Instance.existSeller(seller);
            return Json(result);
        }

    }
}
