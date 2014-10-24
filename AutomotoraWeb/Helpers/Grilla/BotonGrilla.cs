using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomotoraWeb.Helpers.Grilla {
    public class BotonGrilla {
        public string Accion { get; set; }
        public string Controlador { get; set; }
        public string Clase { get; set; }
        public string Tooltip { get; set; }
        public string Texto {get; set;}
        public TIPO Tipo { get; set; }
        public int Ancho { get; set; } //por defecto es cero, se ajusta automaticamente

        public enum TIPO { IMAGEN, TEXTO}

        //Constructor
        public BotonGrilla(string accion, string controlador, string clase, string tooltip) {
            Accion = accion;
            Controlador = controlador;
            Clase = clase;
            Tooltip = tooltip;
            Tipo=TIPO.IMAGEN;
        }

        //Constructor
        public BotonGrilla(string accion, string controlador, string clase, string tooltip, string texto, int ancho) {
            Accion = accion;
            Controlador = controlador;
            Clase = clase;
            Tooltip = tooltip;
            Tipo = TIPO.TEXTO;
            Texto = texto;
            Ancho = ancho;
        }

    }

    
}