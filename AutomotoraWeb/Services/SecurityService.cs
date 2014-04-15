﻿using AutomotoraWeb.Controllers.General;
using AutomotoraWeb.Controllers.Sales.Maintenanse;
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
            user.setearAuditoria(userName, ip);

            return user.Login(ip);
        }

        //------------------------------------------------------------

        public void changePassword(string userName, string actualPassword, string newPassword, string repeatNewPassword, string ip) {
            Usuario user = new Usuario();
            user.Username = userName;
            user.Clave = actualPassword;
            user.setearAuditoria(userName, ip);

            user.CambiarClave(actualPassword, newPassword, repeatNewPassword, ip);
        }

        //------------------------------------------------------------

        public bool hasAccess(string action, string controller, string userName, Dictionary<string, Dictionary<string, bool>> dictionaryOptions) {

            controller = controller.ToLower();
            action = action.ToLower();
            if (isHomePageSection(controller, action) || isErrorPage(controller, action)) {
                // Caso homepage de Ventas, Financiaciones y Banco
                return true;
            } else if (isControllerNameMaintenance(controller)
                      && isActionNameMaintenance(action)
                      && (dictionaryOptions.ContainsKey(controller) && (dictionaryOptions[controller][BaseController.SHOW]))) {
                // Caso de opción que no es de menu pero depende de este (ej: create, edit, delete, details de los mantenimientos que dependen de que se tengan permisos sobre el show)
                action = BaseController.SHOW;
            }

            Usuario u = new Usuario();
            u.Username = userName;
            OpcionMenu om = new OpcionMenu();
            om.Accion = action;
            om.Controlador = controller;
            return u.TieneAcceso(om);
        }

        //------------------------------------------------------------

        public Dictionary<string, Dictionary<string, bool>> getPermissiblesControllerAction(){
            
            Dictionary<string, Dictionary<string, bool>> dictionaryControllerAction = new  Dictionary<string,Dictionary<string,bool>>();
            List<OpcionMenu> listOpcionMenu = OpcionMenu.opcionesHabilitables();
            foreach (OpcionMenu opcionMenu in listOpcionMenu){
                if (!dictionaryControllerAction.ContainsKey(opcionMenu.Controlador)) {
                    Dictionary<string, bool> dictionaryAction = new Dictionary<string, bool>();
                    dictionaryAction.Add(opcionMenu.Accion, true);
                    dictionaryControllerAction.Add(opcionMenu.Controlador, dictionaryAction);
                } else {
                    dictionaryControllerAction[opcionMenu.Controlador].Add(opcionMenu.Accion,true);
                }
            }

            return dictionaryControllerAction;
        }

        //------------------------------------------------------------

        //----------------------------------------------------------

        private bool isActionNameMaintenance(string actionName) {
            return (actionName == SellersController.CREATE) ||
                   (actionName == SellersController.DELETE) ||
                   (actionName == SellersController.DETAILS) ||
                   (actionName == SellersController.EDIT) ||
                   (actionName.Substring(0, 4) == SellersController.LIST);
        }

        //----------------------------------------------------------

        private bool isControllerNameMaintenance(string controllerName) {
            return (controllerName == SellersController.SELLERS) ||
                   (controllerName == CustomersController.CUSTOMERS) ||
                   (controllerName == SucursalesController.SUCURSALES);
        }

        //----------------------------------------------------------

        private bool isHomePageSection(string controllerName, string actionName) {
            return ((controllerName == SellersController.SALES) && (actionName == SellersController.INDEX)); // TODO: agregar Financiaciones y Bancos
        }

        //----------------------------------------------------------

        private bool isErrorPage(string controllerName, string actionName) {
            return ((controllerName == ErrorController.ERROR) && 
                    ((actionName == ErrorController.ERROR_403) || 
                     (actionName == ErrorController.ERROR_404) || 
                     (actionName == ErrorController.ERROR_500))); // TODO: agregar nuevos Errores
        }

        //----------------------------------------------------------

        #endregion

    }
}