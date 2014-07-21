using DevExpress.Web.ASPxGridView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomotoraWeb.Models {

    public class InfoGrilla {
        //clase donde se definen las caracteristicas que quiero que tenga la grilla

        public enum TIPO_CONTROL { GRILLA, GRIDLOOKUP }

        public string NameGrid { get; set; }
        public string Controller { get; set; }
        public string ActionCallbackRoute { get; set; }
        public string KeyFieldName { get; set; }

        private TIPO_CONTROL _tipoControl = TIPO_CONTROL.GRILLA;
        public TIPO_CONTROL TipoControl {
            get { return _tipoControl; }
            set { _tipoControl = value; }
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

        private bool _estatica = false;
        public bool Estatica {
            get { return _estatica; }
            set { _estatica=value;}
        }

        //para endless mode
        private bool _endlessMode = false;
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

        #region Columnas

        public List<ColumnaGrilla> VisibleColumns { get; set; }
        //Si es nulo, o esta vacia, muestra todas las columnas de tipo simple del objeto por reflection
        //Si hay columnas, muestra esta en el orden en que estan aqui.

        public Type TypeOfModel { get; set; } //Se usar para encontrar las columnas por reflection o para traer las propiedades de metadata de los objetos
        public List<string> HiddenColumns { get; set; } //columnas que no hay que mostrar, ya sea porque las encontre por reflection o porque vienen cargadas manualmente pero no las quiero poner por algun motivo

        public void AddVisibleColumn(string campo, string titulo = null, int ancho = 0, int largoMax = 0, ColumnaGrilla.ALINEACIONES alin = ColumnaGrilla.ALINEACIONES.DEFAULT) {
            ColumnaGrilla cg = new ColumnaGrilla {
                Campo = campo, Titulo = titulo,
                Ancho = ancho, LargoMax = largoMax,
                Alineacion = alin
            };
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

        public bool anchosDefinidos() {
            return totalAnchos() > 0;
        }

        private int totalAnchos() {
            if (VisibleColumns == null) {
                return 0;
            }
            int tot = 0;
            foreach (ColumnaGrilla cg in VisibleColumns) {
                tot += cg.Ancho;
            }
            return tot;
        }


        #endregion


        #region Botones

        //Botones acciones principales (SON siempre imagenes: ajax o link )
        private bool _botonesMtoEstandar = false;  //poner en true para que se generen los botone insert, delete, detail automaticamente
        public bool BotonesMtoEstandar {
            get { return _botonesMtoEstandar; }
            set { _botonesMtoEstandar = value; }
        }

        private bool _botonesMtoEstandarAjax = false;  //indica si son ajax o estandar los botones de mto automatico
        public bool BotonesMtoEstandarAjax {
            get { return _botonesMtoEstandarAjax; }
            set { _botonesMtoEstandarAjax = value; }
        }

        public List<BotonGrilla> Botones { get; set; }  //botones de accion del grupo 1 agregados manualmente
        public void AddButton(BotonGrilla b) {  //agrega botones al grupo principal
            if (Botones == null) {
                Botones = new List<BotonGrilla>();
            }
            Botones.Add(b);
        }

        // Conjunto de acciones secundarias (imagen o texto, ajax o link ) 
        public string TextoAcciones2 { get; set; }
        public List<BotonGrilla> Botones2 { get; set; } //lista de botones del grupo secundario,
        public void AddButton2(BotonGrilla b) { //agrega botones al grupo secundario
            if (Botones2 == null) {
                Botones2 = new List<BotonGrilla>();
            }
            Botones2.Add(b);
        }

        private bool accionesAlComienzo = false;
        public bool AccionesAlComienzo {
            get { return accionesAlComienzo; }
            set { accionesAlComienzo = value; }
        }


        #endregion


    }
}