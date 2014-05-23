using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DLL_Backend;
using System.ComponentModel.DataAnnotations;

namespace AutomotoraWeb.Models {
    public class ListadoVentasModel {
        public VentaFiltro Filtro { get; set; }
        public List<Venta> Resultado { get; set; }
        public string idParametros { get; set; }
        public bool FiltrarFechas { get; set; }
        public bool FiltrarSucursal { get; set; }
        public bool FiltrarCliente { get; set; }
        public bool FiltrarVendedor { get; set; }
        public bool FiltrarCombustible { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Desde { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Hasta { get; set; }

        //Constructor
        public ListadoVentasModel() {


            Filtro = new VentaFiltro();
            Resultado = new List<Venta>();

            Filtro.VendidoEntregado = true;
            Filtro.VendidoPendienteEntrega = true;
            Filtro.Anulada = false;

            FiltrarFechas = true;
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

            if (!FiltrarCombustible) {
                this.Filtro.VehiculoFiltro.TipoCombustible = null;
            } 

        }


        public string detallesFiltro() {
            string s ="";
            if (this.FiltrarFechas) {
                s += "Fecha: " + ((this.Filtro.Desde) ?? DateTime.Now).ToString("dd/MM/yyyy") + " - " + ((this.Filtro.Hasta) ?? DateTime.Now).ToString("dd/MM/yyyy") + "    ";
                s += "  ";
            }
            s+= "Estados: ";
            if (this.Filtro.VendidoEntregado) {
                s += "Finalizadas  ";
            } 
            if (this.Filtro.VendidoPendienteEntrega) {
                s += "Pendiente Entrega  ";
            }
            if (this.Filtro.Anulada) {
                s += "Anuladas  ";
            }
            s += "  ";

            if (this.FiltrarSucursal) {
                s += "Sucursal: " + this.Filtro.Sucursal.Nombre + "    ";
            }
            
            if (this.FiltrarCliente) {
                s += "Cliente: " + this.Filtro.Cliente.Nombre + "    ";
            }
            if (this.FiltrarVendedor) {
                s += "Vendedor: " + this.Filtro.Vendedor.Nombre + "    ";
            }
            if (this.Filtro.VehiculoFiltro.Categoria != VehiculoFiltro.VHC_CATEGORIA_LISTADO.TODOS) {
                switch (this.Filtro.VehiculoFiltro.Categoria) {

                    case VehiculoFiltro.VHC_CATEGORIA_LISTADO.NUEVOS:
                        s += "Vehiculos Nuevos    ";
                        break;
                    case VehiculoFiltro.VHC_CATEGORIA_LISTADO.USADOS:
                        s += "Vehiculos Usados    ";
                        break;
                }
            }
            if (this.FiltrarCombustible) {
                s += "Combustible: " + this.Filtro.VehiculoFiltro.TipoCombustible.Descripcion + "    ";
            }
            if (!string.IsNullOrWhiteSpace(this.Filtro.VehiculoFiltro.Marca )) {
                s += "Marca: " + this.Filtro.VehiculoFiltro.Marca.Trim() + "    ";
            }

            if (!string.IsNullOrWhiteSpace(this.Filtro.VehiculoFiltro.Modelo )) {
                s += "Modelo: " + this.Filtro.VehiculoFiltro.Modelo.Trim() + "    ";
            }
            if (this.Filtro.VehiculoFiltro.Anio > 0) {
                s += "Año: " + this.Filtro.VehiculoFiltro.Anio.ToString() + "    ";
            }


            return s;
        }
    }
}