using DLL_Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomotoraWeb.Services {
    public class SecurityService {

        #region Singleton definition

        // Variable estática para la instancia, se necesita utilizar una función lambda ya que el constructor es privado
        private static readonly Lazy<SecurityService> instance = new Lazy<SecurityService>(() => new SecurityService());

        // Constructor privado para evitar la instanciación directa
        private SecurityService() {
        }

        // Propiedad para acceder a la instancia
        public static SecurityService Instance {
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