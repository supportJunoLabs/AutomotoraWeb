using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DLL_Backend;

namespace AutomotoraWeb.Models {
    public class ListadoVehiculosModel {
        public enum FORMATO_LISTADO { COMPLETO, ABREVIADO }

        public VehiculoFiltro Filtro { get; set; }
        public List<Vehiculo> Resultado { get; set; }
        public FORMATO_LISTADO Formato { get; set; }
        public bool FiltrarFechas { get; set; }
        public bool FiltrarSucursal { get; set; }
        public bool FiltrarCombustible { get; set; }
        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }
        public string id { get; set; }

        //Constructor
        public ListadoVehiculosModel() {
            Filtro = new VehiculoFiltro();
            Resultado = new List<Vehiculo>();
            Formato = FORMATO_LISTADO.ABREVIADO;
            FiltrarFechas = false;
            Desde = DateTime.Now.Date.AddMonths(-1);
            Hasta = DateTime.Now.Date;
            FiltrarSucursal = false;
            FiltrarCombustible = false;
        }

        public void AcomodarFiltro() {
            if (!FiltrarFechas) {
                Filtro.Desde = new DateTime(1980, 01, 01);
                Filtro.Hasta = new DateTime(2100, 12, 31);
            } else {
                Filtro.Desde = Desde;
                Filtro.Hasta = Hasta;
            }
            if (!FiltrarSucursal) {
                Filtro.Sucursal = null;
            }
            if (!FiltrarCombustible) {
                Filtro.TipoCombustible = null;
            }
        }
    }
}