using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DLL_Backend;

namespace AutomotoraWeb.Models {
    public class UsuarioModel {

        public Usuario Usuario { get; set; }
        public string PerfilesTexto { get; set; }

        public List<Perfil> PerfilesDisponibles() {
            List<Perfil> todos = Perfil.Perfiles();
            List<Perfil> ll = new List<Perfil>();
            foreach (Perfil p in todos) {
                if (!Usuario.Perfiles.Contains(p)) {
                    ll.Add(p);
                }
            }
            return ll;
        }
    }
}