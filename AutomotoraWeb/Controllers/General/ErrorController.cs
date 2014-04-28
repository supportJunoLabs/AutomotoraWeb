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
        public ActionResult Error404() {
            HttpContext.Response.StatusCode = 404;
            return View();
        }

        [HttpGet]
        public ActionResult Error403() {
            HttpContext.Response.StatusCode = 403;
            return View();
        }
    }
}
