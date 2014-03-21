using AutomotoraWeb.Models;
using DLL_Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomotoraWeb.Services {
    public class MenuService {

        #region Singleton definition

        // Variable estática para la instancia, se necesita utilizar una función lambda ya que el constructor es privado
        private static readonly Lazy<MenuService> instance = new Lazy<MenuService>(() => new MenuService());

        // Constructor privado para evitar la instanciación directa
        private MenuService() {
        }

        // Propiedad para acceder a la instancia
        public static MenuService Instance {
            get {
                return instance.Value;
            }
        }

        #endregion

        //------------------------------------------------------------

        public List<MenuOptionModel> getMenuOptions(String userName, String IP) {
            List<MenuOptionModel> listMenuOptions = new List<MenuOptionModel>();

            Usuario user = new Usuario();
            user.setearAuditoria(userName, IP);
            user.Username = userName;
            List<OpcionMenu> listOpcionMenu = user.OpcionesHabilitadas();

            foreach (OpcionMenu opcionMenu in listOpcionMenu) {
                MenuOptionModel menuOptionModel = this.mapMenuOptionModel(opcionMenu, userName, IP);
                listMenuOptions.Add(menuOptionModel);
            }

            return listMenuOptions;
        }

        //------------------------------------------------------------

        private MenuOptionModel mapMenuOptionModel(OpcionMenu opcionMenu, String userName, String IP) {
            MenuOptionModel menuOptionModel = new MenuOptionModel();
            menuOptionModel.Action = opcionMenu.Accion;
            menuOptionModel.Id = opcionMenu.Codigo;
            menuOptionModel.Controlller = opcionMenu.Controlador;         
            menuOptionModel.Level = opcionMenu.Nivel;
            menuOptionModel.MenuName = opcionMenu.NombreMenu;
            menuOptionModel.OpcionName = opcionMenu.NombreOpcion;
            menuOptionModel.UserName = userName;
            menuOptionModel.IP = IP;

            return menuOptionModel;
        }

        //------------------------------------------------------------

    }
}