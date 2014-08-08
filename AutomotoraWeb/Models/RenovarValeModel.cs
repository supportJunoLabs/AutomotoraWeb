using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DLL_Backend;

namespace AutomotoraWeb.Models {
    public class RenovarValeModel : AbstractModel {
        public Cliente Cliente { get; set; }
        public TRValeRenovacion Transaccion { get; set; }
        public ValeCobroSugerido Sugerido {get; set;}

        public RenovarValeModel() { 
            //el que usa el MVC para crear el objeto del postback
            Cliente = new Cliente();
            Sugerido = null;
            Transaccion = new TRValeRenovacion();
            Transaccion.Fecha = DateTime.Now.Date;
            Transaccion.Sucursal = new Sucursal();
            Transaccion.Vencimiento = DateTime.Now.Date;
            Transaccion.Vale = new Vale();
        }

        //constructor
        public RenovarValeModel(Usuario u) {
            Cliente = new Cliente();
            Sugerido = null;
            Transaccion = new TRValeRenovacion();
            Transaccion.Fecha = DateTime.Now.Date;
            Transaccion.Sucursal = u.Sucursal;
            Transaccion.Vencimiento = DateTime.Now.Date;
            Transaccion.Vale = new Vale();
        }
    }
}