using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DLL_Backend;
using System.ComponentModel.DataAnnotations;

namespace AutomotoraWeb.Models {
    public class ListadoCuotasValesModel {
        public CuotaValeFiltro Filtro { get; set; }
        public ListadoCuotasVales Resultado { get; set; }
        public string idParametros { get; set; }
        public bool FiltrarFinancista { get; set; }
        public TIPO_LISTADO TipoListado { get; set; }
        public ListadoCuotasVales.ALCANCE Alcance { get; set; }
        public ACCIONES Accion { get; set; }
        public TABS TabActual { get; set; }

        public enum TIPO_LISTADO { SITUACION_CUOTAS, VALES_PENDIENTES, CUOTAS_PENDIENTES, CUOTAS_VALES_PENDIENTES}
        public enum ACCIONES { ACTUALIZAR, IMPRIMIR}
        public enum TABS { TAB1, TAB2, TAB3}
        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Desde { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Hasta { get; set; }

        //Constructor
        public ListadoCuotasValesModel() {
            Filtro = new CuotaValeFiltro();
            Filtro.UsarFechas = true;
            Resultado = null;
            Desde = DateTime.Now.Date;
            Hasta = DateTime.Now.Date.AddMonths(1);
            FiltrarFinancista = false;
            TipoListado = TIPO_LISTADO.SITUACION_CUOTAS;
            Alcance = ListadoCuotasVales.ALCANCE.CUOTAS;
            Accion = ACCIONES.ACTUALIZAR;
            TabActual = TABS.TAB1;
        }

        public void obtenerListado (){
            AcomodarFiltro();
            switch (TipoListado) { 
                case TIPO_LISTADO.SITUACION_CUOTAS:
                    Filtro.Situacion = CuotaValeFiltro.SITUACION.NO_ANULADAS;
                    Alcance = ListadoCuotasVales.ALCANCE.CUOTAS;
                    break;
                case TIPO_LISTADO.VALES_PENDIENTES:
                    Filtro.Situacion = CuotaValeFiltro.SITUACION.PENDIENTES_COBRABLES;
                    Alcance = ListadoCuotasVales.ALCANCE.VALES;
                    break;
                case TIPO_LISTADO.CUOTAS_PENDIENTES:
                    Filtro.Situacion = CuotaValeFiltro.SITUACION.PENDIENTES_COBRABLES;
                    Alcance = ListadoCuotasVales.ALCANCE.CUOTAS;
                    break;
                case TIPO_LISTADO.CUOTAS_VALES_PENDIENTES:
                    Filtro.Situacion = CuotaValeFiltro.SITUACION.PENDIENTES_COBRABLES;
                    Alcance = ListadoCuotasVales.ALCANCE.AMBOS;
                    break;
            }
            Resultado = ListadoCuotasVales.obtenerListado(Filtro, Alcance);
        }

        private void AcomodarFiltro() {
            Filtro.Desde = Desde;
            Filtro.Hasta = Hasta;
            if (!FiltrarFinancista) {
                Filtro.Financista = null;
            } else {
                this.Filtro.Financista.Consultar(); //para completar el nombre 
            }
        }


        public string detallesFiltro() {
            string s ="";
            s += "Fecha: " + ((this.Filtro.Desde) ).ToString("dd/MM/yyyy") + " - " + ((this.Filtro.Hasta) ).ToString("dd/MM/yyyy") + "    ";
            s += "  ";
            if (this.FiltrarFinancista) {
                s += "Financista: " + this.Filtro.Financista.Nombre + "    ";
            }
            return s;
        }

    }
}