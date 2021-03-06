﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DLL_Backend;

namespace AutomotoraWeb.Models {

    //se usa para listado sin saldos, NO es estado de cuenta, se filtra por infoAntigua solamente
    public class MovimientosBancoModel {
        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }
        public CuentaBancaria Cuenta {get; set;}
        public List<MovBanco> Resultado { get; set; }
        public string idParametros { get; set; }

        public MovimientosBancoModel() {
            Desde = DateTime.Now.Date.AddMonths(-1);
            Hasta = DateTime.Now.Date;
            Cuenta = new CuentaBancaria();
            Resultado = new List<MovBanco>();
        }

        public void GenerarListado(Usuario u) {
            if (Cuenta == null || Cuenta.Codigo <= 0) {
                Resultado = new List<MovBanco>();
                return;
            }
            Resultado = Cuenta.Movimientos(Desde, Hasta, u);
        }
    }

}