using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DLL_Backend;
using System.ComponentModel.DataAnnotations;

namespace AutomotoraWeb.Models {
    public class ListadoChequesEmitidosModel {
        public ChequeEmitidoFiltro Filtro { get; set; }
        public ListadoChequesEmitidos Resultado { get; set; }
        public string idParametros { get; set; }
        public bool FiltrarCuenta { get; set; }
        public bool FiltrarMoneda { get; set; }
        public  ACCIONES Accion { get; set; }

        public enum ACCIONES { ACTUALIZAR, IMPRIMIR}

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Desde { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Hasta { get; set; }

        //Constructor
        public ListadoChequesEmitidosModel() {
            Filtro = new ChequeEmitidoFiltro();
            Filtro.UsaFechas = true;
            Filtro.Situacion = ChequeEmitidoFiltro.SITUACION.POR_ESTADO;
            Filtro.Status = ChequeEmitido.CHEQUE_PENDIENTE;
            Resultado = new ListadoChequesEmitidos();
            Resultado.Cheques = new List<ChequeEmitido>();
            Resultado.Total = new Importe(Moneda.MonedaDefault, 0);
            Desde = DateTime.Now.Date;
            Hasta = DateTime.Now.Date.AddMonths(1);
            FiltrarCuenta = false;
            FiltrarMoneda = false;
            Accion = ACCIONES.ACTUALIZAR;
        }

        public void obtenerListado (){
            AcomodarFiltro();
            Resultado = ListadoChequesEmitidos.obtenerListado(Filtro);
        }

        private void AcomodarFiltro() {
            Filtro.Desde = Desde;
            Filtro.Hasta = Hasta;
            if (!FiltrarCuenta) {
                Filtro.Cuenta = null;
            } else {
                this.Filtro.Cuenta.Consultar(); //para completar el nombre 
            }
            if (!FiltrarMoneda) {
                Filtro.Moneda = null;
            } else {
                this.Filtro.Moneda.Consultar(); //para completar el nombre 
            }
         
        }


        public string detallesFiltro() {
            string s ="";
            s += "Fecha: " + ((this.Filtro.Desde) ).ToString("dd/MM/yyyy") + " - " + ((this.Filtro.Hasta) ).ToString("dd/MM/yyyy") + "    ";
            s += "  ";
            s += "Estado: " + ChequeEmitido.DescEstado(this.Filtro.Status);
            s += "  ";
            if (this.FiltrarCuenta) {
                s += "Cuenta: " + this.Filtro.Cuenta.ToString() + "    ";
            }
            if (this.FiltrarMoneda) {
                s += "Moneda: " + this.Filtro.Moneda.ToString() + "    ";
            }
            return s;
        }

    }
}