﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DLL_Backend;
using System.ComponentModel.DataAnnotations;

namespace AutomotoraWeb.Models {
    public class ListadoPedidosModel {
        public PedidoFiltro Filtro { get; set; }
        public List<Pedido> Resultado { get; set; }
        public string idParametros { get; set; }
        public bool FiltrarFechasPedido { get; set; }
        public bool FiltrarFechasEsperado { get; set; }
        public bool FiltrarFechasRecibido { get; set; }
        public bool FiltrarSucursal { get; set; }
        public bool FiltrarCliente { get; set; }
        public bool FiltrarVendedor { get; set; }
        public bool FiltrarFechasReservado { get; set; }


        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DesdePedido { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime HastaPedido { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DesdeEsperado { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime HastaEsperado { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DesdeRecibido { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime HastaRecibido { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DesdeReservado { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime HastaReservado { get; set; }

        //Constructor
        public ListadoPedidosModel() {

            
            Filtro = new PedidoFiltro();
            Resultado = new List<Pedido>();
            Filtro.Reservado = PedidoFiltro.PED_RESERVA_LISTADO.TODOS;

            Filtro.Pendientes = true;
            Filtro.Anulados = false;
            Filtro.Recibidos = false;


            FiltrarFechasPedido = false;
            DesdePedido = DateTime.Now.Date.AddMonths(-1);
            HastaPedido = DateTime.Now.Date;

            FiltrarFechasEsperado = false;
            DesdeEsperado = DateTime.Now.Date.AddMonths(-1);
            HastaEsperado = DateTime.Now.Date;

            FiltrarFechasRecibido = false;
            DesdeRecibido = DateTime.Now.Date.AddMonths(-1);
            HastaRecibido = DateTime.Now.Date;

            FiltrarSucursal = false;
            FiltrarVendedor = false;
            FiltrarCliente = false;

            FiltrarFechasReservado = false;
            DesdeReservado = DateTime.Now.Date.AddMonths(-1);
            HastaReservado = DateTime.Now.Date;

            Filtro.Seniado = PedidoFiltro.PED_SENIA_LISTADO.TODOS;
        }

        public void AcomodarFiltro() {
            if (!FiltrarFechasPedido) {
                Filtro.Desde = null;
                Filtro.Hasta = null;
            } else {
                Filtro.Desde = DesdePedido;
                Filtro.Hasta = HastaPedido;
            }

            if (!FiltrarFechasEsperado) {
                Filtro.EsperadoDesde = null;
                Filtro.EsperadoHasta = null;
            } else {
                Filtro.EsperadoDesde = DesdeEsperado;
                Filtro.EsperadoHasta = HastaEsperado;
            }

            if (!FiltrarFechasRecibido) {
                Filtro.RecibidoDesde = null;
                Filtro.RecibidoHasta = null;
            } else {
                Filtro.RecibidoDesde = DesdeRecibido;
                Filtro.RecibidoHasta = HastaRecibido;
            }

            if (!FiltrarFechasReservado) {
                Filtro.ReservaDesde = null;
                Filtro.ReservaHasta = null;
            } else {
                Filtro.ReservaDesde = DesdeReservado;
                Filtro.ReservaHasta = HastaReservado;
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

        }


        public string detallesFiltro() {
            string s = "Estados: ";
            if (this.Filtro.Pendientes) {
                s += "Pendientes ";
            }
            if (this.Filtro.Recibidos) {
                s += "Recibidos ";
            }
            if (this.Filtro.Anulados) {
                s += "Anulados ";
            }
            s += "  ";

            if (this.FiltrarFechasPedido) {
                s += "Fecha Pedido: " + ((this.Filtro.Desde) ?? DateTime.Now).ToString("dd/MM/yyyy") + " - " + ((this.Filtro.Hasta) ?? DateTime.Now).ToString("dd/MM/yyyy") + "    ";
            }

            if (this.FiltrarFechasEsperado) {
                s += "Fecha Esperado: " + ((this.Filtro.EsperadoDesde) ?? DateTime.Now).ToString("dd/MM/yyyy") + " - " + ((this.Filtro.EsperadoHasta) ?? DateTime.Now).ToString("dd/MM/yyyy") + "    ";
            }

            if (this.FiltrarFechasRecibido) {
                s += "Fecha Recibido: " + ((this.Filtro.RecibidoDesde) ?? DateTime.Now).ToString("dd/MM/yyyy") + " - " + ((this.Filtro.RecibidoHasta) ?? DateTime.Now).ToString("dd/MM/yyyy") + "    ";
            }

            if (this.FiltrarSucursal) {
                s += "Sucursal: " + this.Filtro.Sucursal.Nombre + "    ";
            }

            if (this.Filtro.Marca != null && this.Filtro.Marca.Trim() != "") {
                s += "Marca: " + this.Filtro.Marca.Trim() + "    ";
            }

            if (this.Filtro.Modelo != null && this.Filtro.Modelo.Trim() != "") {
                s += "Modelo: " + this.Filtro.Modelo.Trim() + "    ";
            }
            if (!string.IsNullOrWhiteSpace(this.Filtro.Color)) {
                s += "Color: " + this.Filtro.Color.Trim() + "    ";
            }
            if (Filtro.Reservado == PedidoFiltro.PED_RESERVA_LISTADO.SIN_RESERVA) {
                s += " Pedidos Sin Reserva" + "    ";
            }
            if (Filtro.Reservado == PedidoFiltro.PED_RESERVA_LISTADO.CON_RESERVA) {
                s += " Pedidos Reservados" + "    ";
                if (this.FiltrarCliente) {
                    s += "Cliente: " + this.Filtro.Cliente.Nombre + "    ";
                }
                if (this.FiltrarVendedor) {
                    s += "Vendedor: " + this.Filtro.Vendedor.Nombre + "    ";
                }
                if (this.FiltrarFechasReservado) {
                    s += "Fecha Reserva: " + ((this.Filtro.ReservaDesde) ?? DateTime.Now).ToString("dd/MM/yyyy") + " - " + ((this.Filtro.ReservaHasta) ?? DateTime.Now).ToString("dd/MM/yyyy") + "    ";
                }
                if (this.Filtro.Seniado == PedidoFiltro.PED_SENIA_LISTADO.CON_SENIA) {
                    s += "Reserva Señada";
                }
                if (this.Filtro.Seniado == PedidoFiltro.PED_SENIA_LISTADO.SIN_SENIA) {
                    s += "Reserva sin señar";
                }
            }
            
            return s;
        }
    }
}