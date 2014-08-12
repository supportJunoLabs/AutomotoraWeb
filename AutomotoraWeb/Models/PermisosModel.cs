using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DLL_Backend;

namespace AutomotoraWeb.Models {
    public class PermisosModel {
        public string OpcionesHabilitadasTexto { get; set; }
        public List<OpcionMenuEstructura> ListaOpciones { get; set; }
        public Perfil Perfil { get; set; }

        public bool tienePermisos(int idOpcion) { 
            OpcionMenuEstructura oem = new OpcionMenuEstructura(idOpcion);
            int i = ListaOpciones.IndexOf(oem);
            if (i<=0) return false;
            return ListaOpciones[i].Habilitado;
        }
    }
}