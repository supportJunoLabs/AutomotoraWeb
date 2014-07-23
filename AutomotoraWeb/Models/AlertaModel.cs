using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomotoraWeb.Models {
    public class AlertaModel {

        public static string EstiloAlerta(int tipo){
            if (tipo==1) return  "alert-error";
            if (tipo==2) return "alert-success";
            if (tipo == 3) return "alert-info";
            return "";
        }
    }
}