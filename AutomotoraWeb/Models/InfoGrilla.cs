using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomotoraWeb.Models {
    public class InfoGrilla {
        public string NameGrid{get; set;}
        public string Controller {get; set;}
        public string ActionCallbackRoute{get; set;}
        public string KeyFieldName {get; set;}
        public Type TypeOfModel {get; set;}

        private bool accionesAlComienzo =false;
        public bool AccionesAlComienzo{
            get { return accionesAlComienzo; }
            set { accionesAlComienzo = value; }
        }
        public int registrosPorPagina { get; set; }

        private bool dobleClick = false;
        public bool DobleClick {
            get { return dobleClick; }
            set { dobleClick = value; }
        }
        public string ControladorDobleClick{get; set;}
        public string AccionDobleClick {get; set;}

        public List<string> HiddenColumns {get; set;}
        public Dictionary<string, int> TrunkColumns {get; set;}
        public Dictionary<string, int> AnchosColumns { get; set; }
        public List<string> VisibleColumns {get; set;} //Si es nulo, o esta vacia, muestra todas las columnas de tipo simple del objeto
                                                //Si hay columnas, muestra esta en el orden en que estan aqui.

        public List<BotonGrilla> Botones {get; set;} //Si viene en null o vacio,van los cuatro botones estandar

        public void AddButton (BotonGrilla b){
            if (Botones==null){
                Botones = new List<BotonGrilla>();
            }
            Botones.Add(b);
        }

        public void  AddHiddenColumn(string name){
            if(HiddenColumns==null){
                HiddenColumns=new List<string>();
            }
            HiddenColumns.Add(name);
        }

        public void AddVisibleColumn(string name) {
            if (VisibleColumns == null) {
                VisibleColumns = new List<string>();
            }
            VisibleColumns.Add(name);
        }

        public void AddTrunkColumn(string name, int length) {
            if (TrunkColumns == null) {
                TrunkColumns = new Dictionary<string, int>();
            }
            TrunkColumns.Add(name, length);
        }

        public void AddAnchoColumn(string name, int width) {
            if (AnchosColumns == null) {
                AnchosColumns = new Dictionary<string, int>();
            }
            AnchosColumns.Add(name, width);
        }
    }
}