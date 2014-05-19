using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace AutomotoraWeb.Utils {
    public class SessionUtils {

        public static string SESSION_USER = "sessionUser";
        public static string SESSION_USER_NAME = "sessionUserName";
        public static string SESSION_MENU_OPTIONS = "sessionMenuOptions";
        public static string APPLICATION_COMPANY_NAME = "applicationCompanyName";
        public static string APPLICATION_SYSTEM_NAME = "applicationSystemName";
        public static string APPLICATION_PERMISSIBLES_CONTROLLERS_ACTIONS = "applicationPermisiblesActionController";
        public static string ULTIMO_MODULO = "ultimoModulo";
        public static string CODIGO_VEHICULO= "codigoVehiculo";

        public static string generarIdVarSesion(string nomFuncion, string nomUsuario) {
            string s = nomFuncion+"|" +
                 nomUsuario + "|" +
                 DateTime.Now.ToString("yyyyMMddHHmmssffff");
            return s;
        }
    }

    public class Destino {
        public string Controlador { get; set; }
        public string Accion { get; set; }

        public Destino(string accion, string controlador) {
            Controlador = controlador;
            Accion = accion;
        }
    }
}