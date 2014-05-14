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
            } else {
                this.Filtro.Sucursal.Consultar(); //para completar el nombre de la sucursal
            }
            if (!FiltrarCombustible) {
                Filtro.TipoCombustible = null;
            }
        }

        public string detallesFiltro() {
            string s="";
            switch (this.Filtro.Tipo){
                case Vehiculo.VHC_TIPO_LISTADO.LIBRES:
                    s+="Estado: En Venta    ";
                    break;
                case Vehiculo.VHC_TIPO_LISTADO.SENIADOS:
                    s+="Estado: Señados    ";
                    break;
                case Vehiculo.VHC_TIPO_LISTADO.PARA_ENTREGAR:
                    s+="Estado: Vendidos sin entregar    ";
                    break;
                case Vehiculo.VHC_TIPO_LISTADO.EN_STOCK:
                    s+="Estado: En Stock    ";
                    break;
            }

            switch (this.Filtro.Categoria){

                case VehiculoFiltro.VHC_CATEGORIA_LISTADO.NUEVOS:
                    s+="Vehiculos Nuevos    ";
                    break;
                case VehiculoFiltro.VHC_CATEGORIA_LISTADO.USADOS :
                    s+="Vehiculos Usados    ";
                    break;
            }
            
            if (this.FiltrarFechas){
                s+="Fecha Adquisicion: "+((this.Filtro.Desde)??DateTime.Now).ToString("dd/MM/yyyy")+" - "+((this.Filtro.Hasta)??DateTime.Now).ToString("dd/MM/yyyy")+"    ";
            }

            if (this.FiltrarCombustible){
                s+="Combustible: "+this.Filtro.TipoCombustible.Descripcion+"    ";
            }
            if (this.Filtro.Marca!=null && this.Filtro.Marca.Trim()!=""){
                s+="Marca: "+this.Filtro.Marca.Trim()+"    ";
            }

            if (this.Filtro.Modelo!=null && this.Filtro.Modelo.Trim()!=""){
                s+="Modelo: "+this.Filtro.Modelo.Trim()+"    ";
            }
            if (this.Filtro.Anio>0){
                s+="Año: "+this.Filtro.Anio.ToString()+"    ";
            }

            if (this.FiltrarSucursal){
                s+="Sucursal: "+this.Filtro.Sucursal.Nombre+"    ";
            }
            return s;
        }
    }
}
