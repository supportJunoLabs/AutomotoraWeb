using AutomotoraWeb.Controllers.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomotoraWeb.Controllers.General {
    public interface IMaintenance {
        
        ActionResult Show();

        ActionResult Details(int id);

        ActionResult Create();

        ActionResult Edit(int id);

        ActionResult Delete(int id);

    }
}
