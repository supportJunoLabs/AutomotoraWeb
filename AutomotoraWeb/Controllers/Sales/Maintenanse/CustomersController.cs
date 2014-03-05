using AutomotoraWeb.Controllers.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomotoraWeb.Controllers.Sales.Maintenanse {
    public class CustomersController : SalesController, IMaintenance {

        public static string CUSTOMERS = "customers";

        public ActionResult Show() {
            return View();
        }

        public ActionResult Details(int id) {
            return View();
        }

        public ActionResult Create() {
            return View();
        }

        public ActionResult Edit(int id) {
            return View();
        }

        public ActionResult Delete(int id) {
            return View();
        }


        [HttpPost]
        public ActionResult Create(FormCollection collection) {
            try {
                // TODO: Add insert logic here

                return RedirectToAction("Show");
            } catch {
                return View();
            }
        }



        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection) {
            try {
                // TODO: Add update logic here

                return RedirectToAction("Show");
            } catch {
                return View();
            }
        }


        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection) {
            try {
                // TODO: Add delete logic here

                return RedirectToAction("Show");
            } catch {
                return View();
            }
        }
    }
}
