using DLL_Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomotoraWeb.Services {
    public class SecurityManager {

        #region Singleton definition

        // Variable estática para la instancia, se necesita utilizar una función lambda ya que el constructor es privado
        private static readonly Lazy<SecurityManager> instance = new Lazy<SecurityManager>(() => new SecurityManager());

        // Constructor privado para evitar la instanciación directa
        private SecurityManager() {
        }

        // Propiedad para acceder a la instancia
        public static SecurityManager Instance {
            get {
                return instance.Value;
            }
        }

        #endregion

        //------------------------------------------------------------

        #region Services definition

        public bool login(string userName, string password, string ip) {
            Usuario user = new Usuario();
            user.Username = userName;
            user.Clave = password;

            return user.Login(ip);
        }

        //------------------------------------------------------------

        public void changePassword(string userName, string actualPassword, string newPassword, string repeatNewPassword) {
            Usuario user = new Usuario();
            user.Username = userName;
            user.Clave = actualPassword;

            user.CambiarClave(actualPassword, newPassword, repeatNewPassword);
        }

        //------------------------------------------------------------

        #endregion

    }
}