using AutomotoraWeb.Models;
using DLL_Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomotoraWeb.Services {
    public class MenuService {

        public static string MENU_SALES = "sales";
        public static string MENU_FINANCING = "financing";
        public static string MENU_BANK = "bank";
        public static string MENU_CONFIGURATION = "configuration";
        public static string MENU_USER = "user";

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

        public Dictionary<string, Dictionary<int, MenuOptionModel>> getMenuOptions(string userName, string IP) {

            Dictionary<string, Dictionary<int, MenuOptionModel>> dictionaryMenuOptions = new Dictionary<string, Dictionary<int, MenuOptionModel>>();
            dictionaryMenuOptions.Add(MENU_SALES, new Dictionary<int, MenuOptionModel>());
            dictionaryMenuOptions.Add(MENU_FINANCING, new Dictionary<int, MenuOptionModel>());
            dictionaryMenuOptions.Add(MENU_BANK, new Dictionary<int, MenuOptionModel>());
            dictionaryMenuOptions.Add(MENU_CONFIGURATION, new Dictionary<int, MenuOptionModel>());
            dictionaryMenuOptions.Add(MENU_USER, new Dictionary<int, MenuOptionModel>());

            Usuario user = new Usuario();
            user.setearAuditoria(userName, IP);
            user.UserName = userName;
            List<OpcionMenu> listOpcionMenu = user.OpcionesHabilitadas();
            var orderedListOpcionMenu =
                from m in listOpcionMenu
                orderby m.Nivel ascending
                select m;

            foreach (OpcionMenu opcionMenu in orderedListOpcionMenu) {
                MenuOptionModel menuOptionModel = this.mapMenuOptionModel(opcionMenu, userName, IP);
                if (opcionMenu.NombreMenu.Equals("Ventas")) {
                    this.addMenuOption(parseLevel(opcionMenu.Nivel), menuOptionModel, dictionaryMenuOptions[MENU_SALES]);
                } else if (opcionMenu.NombreMenu.Equals("Financiaciones")) {
                    this.addMenuOption(parseLevel(opcionMenu.Nivel), menuOptionModel, dictionaryMenuOptions[MENU_FINANCING]);
                } else if (opcionMenu.NombreMenu.Equals("Banco")) {
                    this.addMenuOption(parseLevel(opcionMenu.Nivel), menuOptionModel, dictionaryMenuOptions[MENU_BANK]);
                } else if (opcionMenu.NombreMenu.Equals("Configuracion")) {
                    this.addMenuOption(parseLevel(opcionMenu.Nivel), menuOptionModel, dictionaryMenuOptions[MENU_CONFIGURATION]);
                } else if (opcionMenu.NombreMenu.Equals("Usuario")) {
                    this.addMenuOption(parseLevel(opcionMenu.Nivel), menuOptionModel, dictionaryMenuOptions[MENU_USER]);
                }
            }

            return dictionaryMenuOptions;

        }

        //------------------------------------------------------------

        private void addMenuOption(List<int> levels, MenuOptionModel menuOptionModel, Dictionary<int, MenuOptionModel> listOpcionMenu) {
            if (levels.Count == 1) {
                listOpcionMenu.Add(levels[0], menuOptionModel);
            } else {
                int firstLevel = levels[0];
                levels.RemoveAt(0);
                if (listOpcionMenu.ContainsKey(firstLevel)) {//agregado para evitar error cuando tiene permisos en un hijo pero le falta en el padre
                    Dictionary<int, MenuOptionModel> subMenu = listOpcionMenu[firstLevel].SubMenu;
                    addMenuOption(levels, menuOptionModel, subMenu);
                }
            }
        }

        //------------------------------------------------------------

        List<int> parseLevel(string levels) {
            List<int> arrayLevels = new List<int>();

            int i = 0;
            do {
                int level = int.Parse(levels.Substring(i, 2));
                arrayLevels.Add(level);
                i = i + 2;
            }
            while (i < levels.Length);

            return arrayLevels;
        }


        //------------------------------------------------------------

        private MenuOptionModel mapMenuOptionModel(OpcionMenu opcionMenu, String userName, String IP) {
            MenuOptionModel menuOptionModel = new MenuOptionModel();
            menuOptionModel.Action = opcionMenu.Accion;
            menuOptionModel.Id = opcionMenu.Codigo;
            menuOptionModel.Controlller = opcionMenu.Controlador;
            menuOptionModel.OpcionName = opcionMenu.NombreOpcion;
            menuOptionModel.UserName = userName;
            menuOptionModel.IP = IP;
            menuOptionModel.SubMenu = new Dictionary<int, MenuOptionModel>();

            return menuOptionModel;
        }

        //------------------------------------------------------------

    }
}