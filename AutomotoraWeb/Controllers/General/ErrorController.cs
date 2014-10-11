using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomotoraWeb.Controllers.General
{
    public class ErrorController : Controller
    {
        [HttpGet]
        public ActionResult Index() {
            HttpContext.Response.StatusCode = 500;
            return View();
        }

        [HttpGet]
        public ActionResult Error() {
            HttpContext.Response.StatusCode = 500;
            return View();
        }

        public ActionResult ErrorDX() {

            var server = HttpContext.Server;
            Exception exception = server.GetLastError();

            HttpContext.Response.StatusCode = 500;
            return View();
        }

        [HttpGet]
        public ActionResult Error404() {
            HttpContext.Response.StatusCode = 404;
            return View();
        }

        [HttpGet]
        public ActionResult Error403(string id) {
            HttpContext.Response.StatusCode = 403;
            ViewBag.PermisoFaltante = id;
            return View();
        }
    }
}
