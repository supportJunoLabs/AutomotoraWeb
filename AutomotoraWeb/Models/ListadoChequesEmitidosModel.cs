using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DLL_Backend;
using System.ComponentModel.DataAnnotations;

namespace AutomotoraWeb.Models {
    public class ListadoChequesEmitidosModel {
        public ChequeEmitidoFiltro Filtro { get; set; }
        public List<ChequeEmitido> Resultado { get; set; }
        public Importe Total{get; set;}
        public string idParametros { get; set; }
        public bool FiltrarCuenta { get; set; }
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
            Resultado = null;
            Desde = DateTime.Now.Date;
            Hasta = DateTime.Now.Date.AddMonths(1);
            FiltrarCuenta = false;
            Accion = ACCIONES.ACTUALIZAR;
        }

        public void obtenerListado (){
            AcomodarFiltro();
            Resultado=ChequeEmitido.Cheques(Filtro);
            if (FiltrarCuenta) {
                Total = new Importe(Filtro.Cuenta.Moneda, 0);
                foreach (var ch in Resultado) {
                    Total.Monto += ch.Importe.Monto;
                }
            } else {
                Total = new Importe(Moneda.MonedaDefault, 0);
                foreach (var ch in Resultado) {
                    Total.Monto += ch.Importe.ValorEnMonedaDefault();
                }
            }
        }

        private void AcomodarFiltro() {
            Filtro.Desde = Desde;
            Filtro.Hasta = Hasta;
            if (!FiltrarCuenta) {
                Filtro.Cuenta = null;
            } else {
                this.Filtro.Cuenta.Consultar(); //para completar el nombre 
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
            return s;
        }

    }
}