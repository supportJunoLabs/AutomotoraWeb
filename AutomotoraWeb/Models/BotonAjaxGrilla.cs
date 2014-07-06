using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomotoraWeb.Models {
    public class BotonAjaxGrilla : BotonGrilla {

        //Constructor
        public BotonAjaxGrilla(string accion, string controlador, string clase, string tooltip) : base(accion, controlador, clase, tooltip) {
        }

        public BotonAjaxGrilla(string accion, string controlador, string clase, string tooltip, string texto, int ancho)
            : base(accion, controlador, clase, tooltip, texto, ancho) {
        }
    }
}