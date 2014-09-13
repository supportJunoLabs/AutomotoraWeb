using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DLL_Backend;

namespace AutomotoraWeb.Models {
    public class PagoFinancistaModel {
        public TRFinancistaPago Transaccion { get; set; }
        public List<FinancistaPagoCheque> listaCheques { get; set; }
        public List<FinancistaPagoEfectivo> listaEfectivo { get; set; }
        public string chequesIds { get; set; }
        public string efectivosIds { get; set; }

        public PagoFinancistaModel() {
            listaCheques = new List<FinancistaPagoCheque>();
            listaEfectivo = new List<FinancistaPagoEfectivo>();
        }

    }
}