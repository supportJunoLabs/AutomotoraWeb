using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DLL_Backend;
using System.ComponentModel.DataAnnotations;

namespace AutomotoraWeb.Models {
    public class ListadoValesModel {
        public ValeFiltro Filtro { get; set; }
        public ListadoVales Resultado { get; set; }
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
        public ListadoValesModel() {
            Filtro = new ValeFiltro();
            Filtro.UsaFechas = true;
            Resultado = null;
            Desde = DateTime.Now.Date;
            Hasta = DateTime.Now.Date.AddMonths(1);
            FiltrarFinancista = false;
            Accion = ACCIONES.ACTUALIZAR;
        }

        public void obtenerListado (Usuario u){
            AcomodarFiltro();
            Resultado = new ListadoVales();
            Resultado.obtenerListado(Filtro, u);
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
            s += "Estado: " + (this.Filtro.Estado.ToString());
            s += "  ";
            if (this.FiltrarFinancista) {
                s += "Financista: " + this.Filtro.Financista.Nombre + "    ";
            }
            return s;
        }

    }
}