using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DLL_Backend;

namespace AutomotoraWeb.Models {
    public class VendedorModel : AbstractModel {
        public Vendedor Vendedor { get; set; }
        public string idsesion { get; set; }

        //Constructor
        public VendedorModel() {
            Vendedor = new Vendedor();
            idsesion = "";
            Vendedor.FechaIngreso = DateTime.Now.Date;
            Vendedor.Habilitado = true;
        }
    }
}