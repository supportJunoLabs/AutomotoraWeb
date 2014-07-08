using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DLL_Backend;

namespace AutomotoraWeb.Models {
    public class BancoSaldoModel {
    
        public DateTime Fecha { get; set; }
        public string idParametros { get; set; }
        public ACCIONES Accion { get; set; }
        public List<BancoSaldoLineaModel> Saldos { get; set; }

        public enum ACCIONES { ACTUALIZAR, IMPRIMIR }

        //constructor
        public BancoSaldoModel() {
            Fecha = DateTime.Now.Date;
            Accion = ACCIONES.ACTUALIZAR;
            Saldos = new List<BancoSaldoLineaModel>();
        }

        public void generarListado() {
            Saldos = new List<BancoSaldoLineaModel>();
            foreach (CuentaBancaria c in CuentaBancaria.CuentasBancarias) {
                BancoSaldoLineaModel lin = new BancoSaldoLineaModel();
                lin.Cuenta = c;
                lin.Saldo = c.SaldoCuenta(Fecha);
                lin.SaldoConciliado = c.SaldoConciliadoCuenta(Fecha);
                Saldos.Add(lin);
            }
        }
    }

    public class BancoSaldoLineaModel {
        public CuentaBancaria Cuenta { get; set; }
        public Importe Saldo { get; set; }
        public Importe SaldoConciliado { get; set; }
        public int CodigoCuenta {
            get {
                return Cuenta.Codigo;
            }
        }
    }

    
}