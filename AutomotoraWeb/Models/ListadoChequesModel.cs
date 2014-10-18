using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DLL_Backend;
using System.ComponentModel.DataAnnotations;

namespace AutomotoraWeb.Models {
    public class ListadoChequesModel {
        public ChequeFiltro Filtro { get; set; }
        public ListadoCheques Resultado { get; set; }
        public string idParametros { get; set; }
        public bool FiltrarFinancista { get; set; }
        public bool FiltrarSucursal { get; set; }
        public  ACCIONES Accion { get; set; }

        public enum ACCIONES { ACTUALIZAR, IMPRIMIR}

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Desde { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Hasta { get; set; }

        //Constructor
        public ListadoChequesModel() {
            Filtro = new ChequeFiltro();
            Filtro.UsaFechas = true;
            Resultado = null;
            Desde = DateTime.Now.Date;
            Hasta = DateTime.Now.Date.AddMonths(1);
            FiltrarFinancista = false;
            FiltrarSucursal = false;
            Accion = ACCIONES.ACTUALIZAR;
        }

        public void obtenerListado (Usuario u){
            AcomodarFiltro();
            Resultado = new ListadoCheques();
            Resultado.obtenerListado(Filtro, u);
        }

        private void AcomodarFiltro() {
            Filtro.Desde = Desde;
            Filtro.Hasta = Hasta;
            this.Filtro.AcomodarFiltro();
            if (!FiltrarFinancista) {
                Filtro.Financista = null;
            } else {
                this.Filtro.Financista.Consultar(); //para completar el nombre 
            }
            if (!FiltrarSucursal) {
                Filtro.Sucursal = null;
            } else {
                this.Filtro.Sucursal.Consultar(); //para completar el nombre 
            }
        }


        public string detallesFiltro() {
            string s ="";
            s += "Fecha: " + ((this.Filtro.Desde) ).ToString("dd/MM/yyyy") + " - " + ((this.Filtro.Hasta) ).ToString("dd/MM/yyyy") + "    ";
            s += "  ";
            s += "Estado: " + (this.Filtro.Situacion.ToString());
            s += "  ";
            if (this.FiltrarFinancista) {
                s += "Financista: " + this.Filtro.Financista.Nombre + "    ";
            }
            if (this.FiltrarSucursal) {
                s += "Sucursal: " + this.Filtro.Sucursal.Nombre + "    ";
            }
            return s;
        }

    }
}