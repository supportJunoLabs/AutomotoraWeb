using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DLL_Backend;


namespace AutomotoraWeb.Models {
    public class SituacionCliente {
        public Cliente Cliente { get; set; }
        public List<Cuota> Cuotas { get; set; }
        public List<Vale> Vales { get; set; }
        public List<Cheque> Cheques { get; set; }

        public Importe TotalCuotas { get; set; }
        public Importe TotalPagoCuotas { get; set; }
        public Importe TotalSaldoCuotas { get; set; }

        public Importe TotalVales { get; set; }
        public Importe TotalPagoVales { get; set; }
        public Importe TotalSaldoVales { get; set; }

        public Importe TotalCheques { get; set; }

        public Importe TotalDeuda { get; set; }

        public void generarSituacion(Cliente cli) {
            Cliente = cli;

            Cuotas = cli.CuotasPendientesNoFinalizadas();
            Cheques = cli.ChequesPendientesNoFinalizados();
            Vales = cli.ValesPendientesNoFinalizados();

            TotalCuotas = new Importe(Moneda.MonedaDefault, 0);
            TotalPagoCuotas = new Importe(Moneda.MonedaDefault, 0);
            TotalSaldoCuotas = new Importe(Moneda.MonedaDefault, 0);
            foreach (Cuota c in Cuotas) {
                TotalCuotas.Monto += c.Importe.ValorEnMonedaDefault();
                TotalPagoCuotas.Monto += c.ImporteCobrado.ValorEnMonedaDefault();
            }
            TotalSaldoCuotas.Monto = TotalCuotas.Monto - TotalPagoCuotas.Monto;

            TotalVales = new Importe(Moneda.MonedaDefault, 0);
            TotalPagoVales = new Importe(Moneda.MonedaDefault, 0);
            TotalSaldoVales = new Importe(Moneda.MonedaDefault, 0);

            foreach (Vale v in Vales) {
                TotalVales.Monto += v.Importe.ValorEnMonedaDefault();
                TotalPagoVales.Monto += v.ImporteCobrado.ValorEnMonedaDefault();
            }
            TotalSaldoVales.Monto = TotalVales.Monto - TotalPagoVales.Monto;

            TotalCheques= new Importe(Moneda.MonedaDefault, 0);
            foreach (Cheque ch in Cheques) {
                TotalCheques.Monto += ch.Importe.ValorEnMonedaDefault();
            }
            TotalDeuda = new Importe(Moneda.MonedaDefault, 0);
            TotalDeuda.Monto = TotalSaldoCuotas.Monto + TotalSaldoVales.Monto + TotalCheques.Monto;
        }
    }
    
}