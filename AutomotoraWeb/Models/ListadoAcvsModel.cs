using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DLL_Backend;
using System.ComponentModel.DataAnnotations;

namespace AutomotoraWeb.Models {
    public class ListadoAcvsModel {
        public AcuentaVentaFiltro Filtro { get; set; }
        public List<ACuentaVenta> Resultado { get; set; }
        public string idParametros { get; set; }
        public bool FiltrarFechas { get; set; }
        public bool FiltrarSucursal { get; set; }
        public bool FiltrarCliente { get; set; }
        public bool FiltrarVendedor { get; set; }

        [Display(Name = "Codigo")]
        public string CodigoVhc { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Desde { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Hasta { get; set; }

        //Constructor
        public ListadoAcvsModel() {
            Filtro = new AcuentaVentaFiltro();
            Resultado = new List<ACuentaVenta>();

            FiltrarFechas = false;
            Desde = DateTime.Now.Date.AddMonths(-1);
            Hasta = DateTime.Now.Date;

            FiltrarSucursal = false;
            FiltrarVendedor = false;
            FiltrarCliente = false;

            CodigoVhc = "";

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
            if (this.Filtro.Vehiculo != null) {
                this.Filtro.Vehiculo.Consultar(); //para terminar de  completar la ficha
            }
        }

        public void AcomodarFiltroActivosVehiculo(int idvehiculo) {
            FiltrarCliente = false;
            FiltrarVendedor = false;
            FiltrarFechas = false;
            FiltrarSucursal = false;
            Filtro.Vehiculo = new Vehiculo();
            Filtro.Vehiculo.Codigo = idvehiculo;
            CodigoVhc = idvehiculo.ToString();
            Filtro.TipoEstado = AcuentaVentaFiltro.ACV_ESTADO.VIGENTE;
            AcomodarFiltro();
        }


        public string detallesFiltro() {
            string s ="";
            if (this.FiltrarFechas) {
                s += "Fecha: " + ((this.Filtro.Desde) ?? DateTime.Now).ToString("dd/MM/yyyy") + " - " + ((this.Filtro.Hasta) ?? DateTime.Now).ToString("dd/MM/yyyy") + "    ";
                s += "  ";
            }
            s+= "Estado: "+Filtro.TipoEstado.ToString();
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
            if (this.Filtro.Vehiculo!=null && Filtro.Vehiculo.Codigo!=0) {
                s += "Vehiculo: " + this.Filtro.Vehiculo.Ficha+ "    ";
            }

            return s;
        }

        public void obtenerListado() {
            AcomodarFiltro();
            Resultado = ACuentaVenta.ACuentaVentas(this.Filtro);
        }
    }
}