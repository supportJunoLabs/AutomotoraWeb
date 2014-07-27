using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DLL_Backend;

namespace AutomotoraWeb.Models {
    public class ConsultaVentaModel {
        public Cliente Cliente { get; set; }
        public Venta Venta { get; set; }

    }
}