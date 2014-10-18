using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DLL_Backend;
using System.ComponentModel.DataAnnotations;
using AutomotoraWeb.Services;

namespace AutomotoraWeb.Models {
    public class ListadoTransaccionesModel {
        public ReciboFiltro Filtro { get; set; }
        public List<Recibo> Resultado { get; set; }
        public string idParametros { get; set; }
        public bool FiltrarFinancista { get; set; }
        public bool FiltrarSucursal { get; set; }
        public bool FiltrarCliente { get; set; }
        public bool FiltrarTipo { get; set; }
        public bool FiltrarUsuario { get; set; }
        public  ACCIONES Accion { get; set; }

        public enum ACCIONES { ACTUALIZAR, IMPRIMIR}

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Desde { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Hasta { get; set; }

        //Constructor
        public ListadoTransaccionesModel() {
            Filtro = new ReciboFiltro();
            Resultado = new List<Recibo>();
            Desde = DateTime.Now.Date;
            Hasta = DateTime.Now.Date;
            FiltrarFinancista = false;
            FiltrarSucursal = false;
            FiltrarCliente = false;
            FiltrarTipo = false;
            FiltrarUsuario = false;
            Accion = ACCIONES.ACTUALIZAR;
        }

        public void obtenerListado (Usuario usuarioConsulta){
            AcomodarFiltro();
            Resultado = Recibo.Recibos(Filtro, usuarioConsulta);
        }

        private void AcomodarFiltro() {
            Filtro.Desde = Desde;
            Filtro.Hasta = Hasta;

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
            if (!FiltrarCliente) {
                Filtro.Cliente = null;
            } else {
                this.Filtro.Cliente.Consultar(); //para completar el nombre 
            }
            if (!FiltrarUsuario) {
                Filtro.Usuario = null;
            } else {
                this.Filtro.Usuario.Consultar(Usuario.MODO_CONSULTA.BASICO); //para completar el nombre 
            }
            if (!FiltrarTipo) {
                Filtro.TipoRecibo = null;
            } else {
                this.Filtro.TipoRecibo.Consultar(); //para completar el nombre 
            }
        }


        public string detallesFiltro() {
            string s ="";
            s += "Fecha: " + ((this.Filtro.Desde) ).ToString("dd/MM/yyyy") + " - " + ((this.Filtro.Hasta) ).ToString("dd/MM/yyyy") + "    ";
            s += "  ";
            if (this.FiltrarFinancista) {
                s += "Financista: " + this.Filtro.Financista.Nombre + "    ";
            }
            if (this.FiltrarSucursal) {
                s += "Sucursal: " + this.Filtro.Sucursal.Nombre + "    ";
            }
            if (this.FiltrarTipo) {
                s += "Tipo Transaccion: " + this.Filtro.TipoRecibo.Descripcion+ "    ";
            }
            if (this.FiltrarCliente) {
                s += "Cliente: " + this.Filtro.Cliente.Nombre + "    ";
            }
            if (this.FiltrarUsuario) {
                s += "Usuario: " + this.Filtro.Usuario.UserName + "    ";
            }
            return s;
        }

    }
}