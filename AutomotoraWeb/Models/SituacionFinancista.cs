using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DLL_Backend;


namespace AutomotoraWeb.Models {
    public class SituacionFinancista {
        public Financista Financista { get; set; }
        public List<FinancistaPagoCheque> Cheques { get; set; }
        public List<FinancistaPagoEfectivo> Efectivo { get; set; }

        public Importe TotalCheques { get; set; }
        public Importe TotalEfectivo { get; set; }
        public Importe TotalGeneral { get; set; }

        public void generarSituacion(Financista fin) {
            Financista = fin;
            Cheques = fin.PagosChequesPendientes();
            Efectivo = fin.PagosEfectivoPendientes();

            TotalCheques = new Importe(Moneda.MonedaDefault, 0);
            TotalEfectivo = new Importe(Moneda.MonedaDefault, 0);
            foreach (FinancistaPagoCheque pf in Cheques) {
                TotalCheques += pf.Cheque.Importe;
            }
            foreach (FinancistaPagoEfectivo pf in Efectivo) {
                TotalEfectivo += (pf.ImporteOrig - pf.ImportePagoAnt);
            }
            TotalGeneral = TotalEfectivo + TotalCheques;
        }
    }
    
}