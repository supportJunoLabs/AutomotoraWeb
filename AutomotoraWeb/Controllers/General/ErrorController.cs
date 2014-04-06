using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomotoraWeb.Controllers.General
{
    public class ErrorController : Controller
    {
        public static string ERROR = "error";
        public static string ERROR_403 = "error403";
        public static string ERROR_404 = "error404";
        public static string ERROR_500 = "error500";


        [HttpGet]
        public ActionResult Error403() {
            HttpContext.Response.StatusCode = 403;
            return View();
        }

        [HttpGet]
        public ActionResult Error404() {
            HttpContext.Response.StatusCode = 404;
            return View();
        }

        [HttpGet]
        public ActionResult Error500() {
            HttpContext.Response.StatusCode = 500;
            return View();
        }

    }
}
