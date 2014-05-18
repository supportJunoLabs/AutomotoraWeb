﻿using System;
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

        public List<string> HiddenColumns {get; set;} //columnas que no hay que mostrar
        
        public Dictionary<string, int> TrunkColumns {get; set;} //columnas que hay que truncar (o viene en visibleColumns)

        //public Dictionary<string, int> AnchosColumns { get; set; }

        public List<ColumnaGrilla> VisibleColumns { get; set; }
            //Si es nulo, o esta vacia, muestra todas las columnas de tipo simple del objeto
            //Si hay columnas, muestra esta en el orden en que estan aqui.
        
        //public List<string> VisibleColumns {get; set;} 

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

        public bool anchosDefinidos() {
            return totalAnchos() > 0;
        }

        private int totalAnchos() {
            if (VisibleColumns == null) {
                return 0;
            }
            int tot = 0;
            foreach(ColumnaGrilla cg in VisibleColumns){
                tot += cg.Ancho;
            }
            return tot;
        }

        public void AddVisibleColumn(string campo, string titulo = null, int ancho = 0, int largoMax = 0, ColumnaGrilla.ALINEACIONES alin = ColumnaGrilla.ALINEACIONES.DEFAULT) {
            ColumnaGrilla cg = new ColumnaGrilla {
                Campo = campo, Titulo = titulo,
                Ancho = ancho, LargoMax = largoMax,
                Alineacion = alin};
            VisibleColumns.Add(cg);
        }

        public void AddVisibleColumn(ColumnaGrilla cg) {
            VisibleColumns.Add(cg);

        }
    }

    public class ColumnaGrilla {

        public enum ALINEACIONES {DEFAULT, IZQUIERDA, DERECHA, CENTRO, JUSTIFICADO}
        
        public string Campo {get; set;}
        public string Titulo {get; set;}
        public int Ancho {get; set;}
        public int LargoMax { get; set; }
        public ALINEACIONES Alineacion { get; set;}

        public ColumnaGrilla(){
            Alineacion=ALINEACIONES.DEFAULT;
            Ancho=0;
            LargoMax =0;
        }

        public override bool Equals(object obj) {
            if (obj == null) {
                return false;
            }
            if (!(obj is ColumnaGrilla)){ 
                return false;
            }
            ColumnaGrilla other = (ColumnaGrilla)obj;
            if (string.IsNullOrWhiteSpace(this.Campo) || string.IsNullOrWhiteSpace(other.Campo)){
                return false;
            }
            return (this.Campo.Equals(other.Campo));
        }
    }

}