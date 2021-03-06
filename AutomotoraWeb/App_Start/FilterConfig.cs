﻿using AutomotoraWeb.Filters;
using System.Web;
using System.Web.Mvc;

namespace AutomotoraWeb {
    public class FilterConfig {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AutorizacionActionFilterAttribute());
            filters.Add(new GeneralModelActionFilterAttribute());
        }
    }
}