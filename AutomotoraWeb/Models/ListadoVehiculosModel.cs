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

        //Constructor
        public ListadoVehiculosModel() {
            Filtro = new VehiculoFiltro();
            Resultado = new List<Vehiculo>();
            Formato = FORMATO_LISTADO.ABREVIADO;
            FiltrarFechas = false;
            Filtro.Desde = DateTime.Now.Date.AddMonths(-1);
            Filtro.Hasta = DateTime.Now.Date;
            FiltrarSucursal = false;
            FiltrarCombustible = false;
        }
    }
}