using DevExpress.Web.ASPxGridView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomotoraWeb.Models {
    public class InfoGrilla {

        public enum TIPO_CONTROL { GRILLA, GRIDLOOKUP}

        public string NameGrid { get; set; }
        public string Controller { get; set; }
        public string ActionCallbackRoute { get; set; }
        public string KeyFieldName { get; set; }
        public Type TypeOfModel { get; set; }

        private TIPO_CONTROL _tipoControl = TIPO_CONTROL.GRILLA;
        public TIPO_CONTROL TipoControl {
            get { return _tipoControl; }
            set { _tipoControl = value; }
        }

        private bool accionesAlComienzo = false;
        public bool AccionesAlComienzo{
            get { return accionesAlComienzo; }
            set { accionesAlComienzo = value; }
        }

        public int registrosPorPagina { get; set; }

        private bool registrosPorPaginaVisible = true;
        public bool RegistrosPorPaginaVisible {
            get { return registrosPorPaginaVisible; }
            set { registrosPorPaginaVisible = value; }
        }
        
        private bool dobleClick = false;
        public bool DobleClick {
            get { return dobleClick; }
            set { dobleClick = value; }
        }
        public string ControladorDobleClick { get; set; }
        public string AccionDobleClick { get; set; }

        //para endless mode
        private bool _endlessMode=false;
        public bool EndlessMode {
            get { return _endlessMode; }
            set { _endlessMode = value; }
        }

        //para multiseleccion con checkbox
        private bool _checksSeleccion = false;
        public bool ChecksSeleccion {
            get { return _checksSeleccion; }
            set { _checksSeleccion = value; }
        }
        public string AccionSeleccion { get; set; }

        // atributos para lookupgrid
        public string FocusedRowChangedAccion { get; set; }
        public string FormatoSeleccionLookup { get; set; }
        public int AnchoSeleccion { get; set; }

        public Action<object, ASPxGridViewTableRowEventArgs> FuncionHtmlRowPrepared;

        private bool _usarViewData = false;
        public bool UsarViewData {
            get { return _usarViewData; }
            set { _usarViewData = value; }
        }
        public string ClaveViewData;
  

        //-------------------- Botones acciones principales (siempre imagenes: ajax o link ) ---------------------------

        private bool usarBotones = true;
        public bool UsarBotones {
            get { return usarBotones; }
            set { usarBotones = value; }
        }

        private bool botonesAjax = false;
        public bool BotonesAjax {  //indica si para los botones automaticos usar botones ajax o link
            get { return botonesAjax; }
            set { botonesAjax = value; }
        }

        //-------------------- Conjunto de acciones secundarias (imagen o texto, ajax o link ) ---------------------------

        public string TextoAcciones2 { get; set; }
        public List<BotonGrilla> Botones2 { get; set; } //lista de botones del grupo secundario,

        //---------------------------------------------------------------------------------

        public List<string> HiddenColumns { get; set; } //columnas que no hay que mostrar
        
        public Dictionary<string, int> TrunkColumns { get; set; } //columnas que hay que truncar (o viene en visibleColumns)

        public List<ColumnaGrilla> VisibleColumns { get; set; }
            //Si es nulo, o esta vacia, muestra todas las columnas de tipo simple del objeto
            //Si hay columnas, muestra esta en el orden en que estan aqui.
        
        //public List<string> VisibleColumns {get; set;} 

        public List<BotonGrilla> Botones { get; set; } 
            //Refiere a los botones con imagen del grupo 1
            //Si viene en null o vacia van los botones por defecto o nada segun el valor de ConBotones.
            //Si viene con botones, van los botones que vienen.


        public void SinBotones() {  //para el grupo principal
            UsarBotones = false;
        }

        public void AddButton (BotonGrilla b){  //agrega botones al grupo principal
            if (Botones==null){
                Botones = new List<BotonGrilla>();
            }
            Botones.Add(b);
        }

        public void AddButton2(BotonGrilla b) { //agrega botones al grupo secundario
            if (Botones2 == null) {
                Botones2 = new List<BotonGrilla>();
            }
            Botones2.Add(b);
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

        public ColumnaGrilla obtenerColumna(string nombreCampo) {
            if (VisibleColumns == null) {
                return null;
            }
            ColumnaGrilla aux = new ColumnaGrilla { Campo = nombreCampo };
            int i = VisibleColumns.IndexOf(aux);
            if (i >= 0) {
                return VisibleColumns[i];
            }
            return null;
        }
    }

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