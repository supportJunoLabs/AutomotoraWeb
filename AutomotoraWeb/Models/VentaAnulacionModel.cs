using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DLL_Backend;

namespace AutomotoraWeb.Models {

    //necesito este modelo para que los campos del model no se llamen igual a los campos de la venta, porque si no, se confunde en el postback.

    public class VentaAnulacionModel {
        public TRVentaAnulacion VentaDev { get; set; }
    }
}