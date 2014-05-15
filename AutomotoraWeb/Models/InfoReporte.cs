using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomotoraWeb.Models {
    public class InfoReporte {
        public string Controlador { get; set; }
        public string AccionReporte { get; set; }
        public string AccionExportacion { get; set; }
        public bool TieneIdParametros { get; set; }

        //constructor
        public InfoReporte() {
            TieneIdParametros = false;
        }
    }
}