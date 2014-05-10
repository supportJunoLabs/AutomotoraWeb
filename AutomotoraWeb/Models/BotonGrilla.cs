using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomotoraWeb.Models {
    public class BotonGrilla {
        public string Accion { get; set; }
        public string Controlador { get; set; }
        public string Clase { get; set; }
        public string Tooltip { get; set; }

        //Constructor
        public BotonGrilla(string accion, string controlador, string clase, string tooltip) {
            Accion = accion;
            Controlador = controlador;
            Clase = clase;
            Tooltip = tooltip;
        }

    }

    
}