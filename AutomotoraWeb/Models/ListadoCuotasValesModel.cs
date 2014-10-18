using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DLL_Backend;
using System.ComponentModel.DataAnnotations;
using AutomotoraWeb.Services;

namespace AutomotoraWeb.Models {
    public class ListadoCuotasValesModel {
        public CuotaValeFiltro Filtro { get; set; }
        public ListadoCuotasVales Resultado { get; set; }
        public string idParametros { get; set; }
        public bool FiltrarFinancista { get; set; }
        public TIPO_LISTADO TipoListado { get; set; }
        //public ListadoCuotasVales.ALCANCE Alcance { get; set; }
        public ACCIONES Accion { get; set; }
        public TABS TabActual { get; set; }

        //Totales
        public Importe ImporteTotal { get; set; }
        public Importe ImporteACuenta { get; set; }
        public Importe ImporteSaldo { get; set; }


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
            Resultado = null;
            Desde = DateTime.Now.Date;
            Hasta = DateTime.Now.Date.AddMonths(1);
            FiltrarFinancista = false;
            TipoListado = TIPO_LISTADO.SITUACION_CUOTAS;
            //Alcance = ListadoCuotasVales.ALCANCE.CUOTAS;
            Accion = ACCIONES.ACTUALIZAR;
            TabActual = TABS.TAB1;
        }

        public void obtenerListado (Usuario usuario){
            AcomodarFiltro();
            Resultado = new ListadoCuotasVales();
            switch (TipoListado) { 
                case TIPO_LISTADO.SITUACION_CUOTAS:
                    Resultado.generarListadoCuotasPeriodo(Filtro, usuario);
                    ImporteTotal = new Importe(Moneda.MonedaDefault,0);
                    ImporteACuenta = new Importe(Moneda.MonedaDefault, 0);
                    ImporteSaldo = new Importe(Moneda.MonedaDefault, 0);
                    foreach (Cuota c in Resultado.Cuotas) { 
                        ImporteTotal+=c.Importe;
                        ImporteACuenta+=c.ImporteCobrado;
                        ImporteSaldo += c.ImporteSaldo;
                    }

                    break;
                case TIPO_LISTADO.VALES_PENDIENTES:
                    Resultado.generarListadoValesPendientes(Filtro);
                    ImporteTotal = Resultado.ImporteTotal;
                    ImporteACuenta = Resultado.ImporteACuenta;
                    ImporteSaldo = Resultado.ImporteSaldo;
                    break;
                case TIPO_LISTADO.CUOTAS_PENDIENTES:
                    Resultado.generarListadoCuotasPendientes(Filtro);
                    ImporteTotal = Resultado.ImporteTotal;
                    ImporteACuenta = Resultado.ImporteACuenta;
                    ImporteSaldo = Resultado.ImporteSaldo;
                    break;
                case TIPO_LISTADO.CUOTAS_VALES_PENDIENTES:
                    Resultado.generarListadoCuotasValesPendientes(Filtro);
                    ImporteTotal = Resultado.ImporteTotal;
                    ImporteACuenta = Resultado.ImporteACuenta;
                    ImporteSaldo = Resultado.ImporteSaldo;
                    break;
            }
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