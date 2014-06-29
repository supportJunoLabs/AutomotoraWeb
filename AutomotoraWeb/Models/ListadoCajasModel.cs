using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DLL_Backend;
using System.ComponentModel.DataAnnotations;

namespace AutomotoraWeb.Models {
    public class ListadoCajasModel {
        public CajaFiltro Filtro { get; set; }
        public ListadoMovimientosCaja Resultado { get; set; }
        public string idParametros { get; set; }
        public bool FiltrarSucursal { get; set; }
        public bool FiltrarFinancista { get; set; }
        public CAJA_REPORTE TipoReporte { get; set; }

        public enum CAJA_REPORTE { EFECTIVO, CHEQUES };

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Desde { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Hasta { get; set; }

        //Constructor
        public ListadoCajasModel() {
            Filtro = new CajaFiltro();
            Resultado = null;
            Desde = DateTime.Now.Date;
            Hasta = DateTime.Now.Date;
            FiltrarSucursal = false;
            FiltrarFinancista = false;
            TipoReporte = CAJA_REPORTE.EFECTIVO;
        }

        public void AcomodarFiltro() {
                Filtro.Desde = Desde;
                Filtro.Hasta = Hasta;

            if (!FiltrarSucursal) {
                Filtro.Sucursal = null;
            } else {
                this.Filtro.Sucursal.Consultar(); //para completar el nombre de la sucursal
            }

            if (!FiltrarFinancista) {
                Filtro.Financista = null;
            } else {
                this.Filtro.Financista.Consultar(); //para completar el nombre 
            }
            if (this.Filtro.Moneda != null) {
                this.Filtro.Moneda.Consultar();
            } else {
                this.Filtro.Moneda = Moneda.MonedaDefault;
            }

        }


        public string detallesFiltro() {
            string s ="";
            s += "Fecha: " + ((this.Filtro.Desde) ).ToString("dd/MM/yyyy") + " - " + ((this.Filtro.Hasta) ).ToString("dd/MM/yyyy") + "    ";
            s += "  ";
            s += "Moneda: " + this.Filtro.Moneda.Nombre;
            s += "  ";
            if (this.FiltrarSucursal) {
                s += "Sucursal: " + this.Filtro.Sucursal.Nombre + "    ";
            }
            if (this.FiltrarFinancista) {
                s += "Financista: " + this.Filtro.Financista.Nombre + "    ";
            }
            return s;
        }

    }
}