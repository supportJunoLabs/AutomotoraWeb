using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DLL_Backend;

namespace AutomotoraWeb.Models {
    public class PerfilModel {

        public Perfil Perfil { get; set; }
        public string UsuariosTexto { get; set; }

        public List<Usuario> UsuariosDisponibles() {
            List<Usuario> todos = Usuario.Usuarios();
            List<Usuario> ll = new List<Usuario>();
            foreach (Usuario p in todos) {
                if (!Perfil.Usuarios.Contains(p)) {
                    ll.Add(p);
                }
            }
            return ll;
        }
    }
}