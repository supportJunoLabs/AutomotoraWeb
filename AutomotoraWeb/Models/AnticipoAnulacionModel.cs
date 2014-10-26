using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DLL_Backend;

namespace AutomotoraWeb.Models {
    public class AnticipoAnulacionModel {
        //Es necesario tener esta clase, para no tener campos con los mismos nombres de la tr original y de la anulacion
        public TRACuentaVentaAnulacion TrAnulacion { get; set; }
    }
}