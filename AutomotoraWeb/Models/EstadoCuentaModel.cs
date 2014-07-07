using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DLL_Backend;

namespace AutomotoraWeb.Models {
    public class EstadoCuentaModel {
        public EstadoCuenta EstadoCuenta { get; set; }
        public string idParametros { get; set; }
        public ACCIONES Accion { get; set; }

        public enum ACCIONES { ACTUALIZAR, IMPRIMIR }

        public EstadoCuentaModel(){
            EstadoCuenta = new EstadoCuenta();
            EstadoCuenta.Desde = DateTime.Now.Date.AddMonths(-1);
            EstadoCuenta.Hasta = DateTime.Now.Date;
            EstadoCuenta.Tipo = EstadoCuenta.TIPO.ESTANDAR;
            Accion = ACCIONES.ACTUALIZAR;
        }

    }
}