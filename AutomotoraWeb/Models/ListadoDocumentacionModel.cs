using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DLL_Backend;
using System.ComponentModel.DataAnnotations;

namespace AutomotoraWeb.Models {
    public class ListadoDocumentacionModel {
        public List<EstadoDocumento> EstadosPosibles { get; set; }
        public int[] EstadosConsultar { get; set; }
        public List<DocAuto> Resultado { get; set; }
        public string idParametros { get; set; }


        //Constructor
        public ListadoDocumentacionModel() {
            Resultado = new List<DocAuto>();
        }

        public void AcomodarFiltro() {
        }


        public string detallesFiltro() {
            string s ="Estados: ";
            EstadosPosibles = EstadoDocumento.EstadosDocumentoListables();

            if (EstadosConsultar != null) {
                foreach (var i in EstadosConsultar) {
                    EstadoDocumento aux = new EstadoDocumento { Codigo = i };
                    int j = EstadosPosibles.IndexOf(aux);
                    if (j >= 0) {
                        s += EstadosPosibles[j].Descripcion + "  ";
                    }
                }
            }
            return s;
        }
    }
}