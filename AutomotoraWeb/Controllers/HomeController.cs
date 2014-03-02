using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomotoraWeb.Controllers
{
    public class HomeController : Controller
    {
        public static string HOME = "home";
        public static string INDEX = "index";

        public ActionResult Index()
        {
            return View();
        }

    }
}
