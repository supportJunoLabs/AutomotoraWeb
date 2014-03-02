using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomotoraWeb.Controllers {
    public class FinancingController : Controller {

        public static string FINANCING = "financing";
        public static string INDEX = "index";

        //
        // GET: /Financing/

        public ActionResult Index() {
            return View();
        }

        //
        // GET: /Financing/Details/5

        public ActionResult Details(int id) {
            return View();
        }

        //
        // GET: /Financing/Create

        public ActionResult Create() {
            return View();
        }

        //
        // POST: /Financing/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection) {
            try {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            } catch {
                return View();
            }
        }

        //
        // GET: /Financing/Edit/5

        public ActionResult Edit(int id) {
            return View();
        }

        //
        // POST: /Financing/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection) {
            try {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            } catch {
                return View();
            }
        }

        //
        // GET: /Financing/Delete/5

        public ActionResult Delete(int id) {
            return View();
        }

        //
        // POST: /Financing/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection) {
            try {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            } catch {
                return View();
            }
        }
    }
}
