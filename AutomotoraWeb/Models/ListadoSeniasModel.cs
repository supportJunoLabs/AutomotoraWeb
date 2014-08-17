using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DLL_Backend;
using System.ComponentModel.DataAnnotations;

namespace AutomotoraWeb.Models {
    public class ListadoSeniasModel {
        public SeniaFiltro Filtro { get; set; }
        public List<Senia> Resultado { get; set; }
        public string idParametros { get; set; }
        public bool FiltrarFechas { get; set; }
        public bool FiltrarSucursal { get; set; }
        public bool FiltrarCliente { get; set; }
        public bool FiltrarVendedor { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Desde { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Hasta { get; set; }

        //Constructor
        public ListadoSeniasModel() {


            Filtro = new SeniaFiltro();
            Resultado = new List<Senia>();

            Filtro.Vehiculos = true;
            Filtro.PedidosPendientes = false;
            Filtro.PedidosRecibidos = false;

            Filtro.Vigentes = true;
            Filtro.Vendidas = false;
            Filtro.Anuladas = false;

            FiltrarFechas = false;
            Desde = DateTime.Now.Date.AddMonths(-1);
            Hasta = DateTime.Now.Date;

            FiltrarSucursal = false;
            FiltrarVendedor = false;
            FiltrarCliente = false;

        }

        public void AcomodarFiltro() {
            if (!FiltrarFechas) {
                Filtro.Desde = null;
                Filtro.Hasta = null;
            } else {
                Filtro.Desde = Desde;
                Filtro.Hasta = Hasta;
            }


            if (!FiltrarSucursal) {
                Filtro.Sucursal = null;
            } else {
                this.Filtro.Sucursal.Consultar(); //para completar el nombre de la sucursal
            }

            if (!FiltrarVendedor) {
                Filtro.Vendedor = null;
            } else {
                this.Filtro.Vendedor.Consultar(); //para completar el nombre 
            }

            if (!FiltrarCliente) {
                Filtro.Cliente = null;
            } else {
                this.Filtro.Cliente.Consultar(); //para completar el nombre 
            }

            if (this.Filtro.Vehiculos) {
                this.Filtro.PedidosPendientes = false;
                this.Filtro.PedidosRecibidos = false;
            } else {
                if (this.Filtro.PedidosPendientes || this.Filtro.PedidosRecibidos) {
                    this.Filtro.Vehiculos = false;
                }
            }

        }


        public string detallesFiltro() {
            string s = "Estados: ";
            if (this.Filtro.Vehiculos) {
                s += "Seña de vehículos";
            } else {
                s += "Seña de pedidos: ";
                if (this.Filtro.PedidosPendientes) {
                    s += "Pendientes ";
                }
                if (this.Filtro.PedidosRecibidos) {
                    s += "Recibidos ";
                }
            }
            s += "  ";

            s += "Estados: ";
            if (this.Filtro.Vigentes) {
                s += "Vigentes ";
            }
            if (this.Filtro.Vendidas) {
                s += "Venta Realizada ";
            }
            if (this.Filtro.Anuladas) {
                s += "Anulada";
            }
            s += "  ";
            if (this.FiltrarSucursal) {
                s += "Sucursal: " + this.Filtro.Sucursal.Nombre + "    ";
            }
            s += "  ";
            if (this.FiltrarFechas) {
                s += "Fecha: " + ((this.Filtro.Desde) ?? DateTime.Now).ToString("dd/MM/yyyy") + " - " + ((this.Filtro.Hasta) ?? DateTime.Now).ToString("dd/MM/yyyy") + "    ";
            }
            if (this.FiltrarCliente) {
                s += "Cliente: " + this.Filtro.Cliente.Nombre + "    ";
            }
            if (this.FiltrarVendedor) {
                s += "Vendedor: " + this.Filtro.Vendedor.Nombre + "    ";
            }


            return s;
        }
    }
}