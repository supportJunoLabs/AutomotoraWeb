using DevExpress.Web.ASPxGridView;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomotoraWeb.Models {

    #region ColumnaGrilla


    public class ColumnaGrilla {

        public enum ALINEACIONES {DEFAULT, IZQUIERDA, DERECHA, CENTRO, JUSTIFICADO}
        
        public string Campo {get; set;}
        public string Titulo {get; set;}
        public int Ancho {get; set;}
        public int LargoMax { get; set; }
        public ALINEACIONES Alineacion { get; set;}
        
        public bool Hipervinculo { get; set; }
        public string Haccion { get; set; }
        public string Hcontrolador { get; set; }
        public string HCampoParametro { get; set; }

        //Se usan para marcar comportamiento especial en la grilla cuando son atributos que no se obtienen por metadata
        //porque son atributos derivados.
        //Se podria hacer por reflection recursivo, pero asi es mas facil, quizas se pueda mejorar mas adelante.
        public bool EsEntero { get; set; }
        public bool EsFecha { get; set; }
        public bool EsBoolean { get; set; }
        public bool EsImporte { get; set; }

        public string Formato { get; set; }

        public bool TotalSuma { get; set; }

        public bool MostrarSoloEdicion { get;set;}
        public int EdicionColSpan { get; set; }

        //----------------------------------------------------

        public MVCxGridViewColumnType ColumnType { get; set; }
        public object ComboBoxPropertiesDataSource  { get; set; }
        public String ComboBoxPropertiesTextField  { get; set; }
        public String ComboBoxPropertiesValueField  { get; set; }
        public Type ComboBoxPropertiesValueType  { get; set; }

        //----------------------------------------------------


        public ColumnaGrilla(){
            Alineacion=ALINEACIONES.DEFAULT;
            Ancho=0;
            LargoMax =0;
            Hipervinculo = false;
            EsBoolean = false;
            EsEntero = false;
            EsFecha = false;
            EsImporte = false;
            TotalSuma = false;
            MostrarSoloEdicion = false;
            EdicionColSpan =0 ;
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

    #endregion

}