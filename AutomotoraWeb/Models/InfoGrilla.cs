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

        public List<string> HiddenColumns {get; set;}
        public Dictionary<string, int> TrunkColumns {get; set;}
        public List<string> VisibleColumns {get; set;} //Si es nulo, o esta vacia, muestra todas las columnas de tipo simple del objeto
                                                //Si ha columnas, muestra esta en el orden en que estan aqui.

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

        public void AddTrunkColumns(string name, int length) {
            if (TrunkColumns == null) {
                TrunkColumns = new Dictionary<string, int>();
            }
            TrunkColumns.Add(name, length);
        }
    }
}