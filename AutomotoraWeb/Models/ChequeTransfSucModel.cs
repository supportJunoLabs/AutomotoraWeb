using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DLL_Backend;

namespace AutomotoraWeb.Models {
    public class ChequeTransfSucModel {
        public Sucursal SucursalOrigen { get; set; }
        public Sucursal SucursalDestino { get; set; }
        public string ChequesIds { get; set; }

    }
}