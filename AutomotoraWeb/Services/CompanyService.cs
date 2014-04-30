using DLL_Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace AutomotoraWeb.Services {
    public class CompanyService {

        private static Empresa emp;

        #region Singleton definition

        // Variable estática para la instancia, se necesita utilizar una función lambda ya que el constructor es privado
        private static readonly Lazy<CompanyService> instance = new Lazy<CompanyService>(() => new CompanyService());

        // Constructor privado para evitar la instanciación directa
        private CompanyService() {
            emp = new Empresa();
            emp.Codigo = Int32.Parse(ConfigurationManager.AppSettings["COD_SISTEMA"]);
            emp.Consultar();
        }

        // Propiedad para acceder a la instancia
        public static CompanyService Instance {
            get {
                return instance.Value;
            }
        }

        #endregion

        //------------------------------------------------------------

        #region Services definition

        public string getCompanyName() {
            return emp.NombreEmpresa;
        }

        //------------------------------------------------------------

        public string getSystemName() {
            return emp.NombreSistema;
        }

        public void actualizarDatos(Empresa empre) {
            if (instance != null) {
                emp = empre;
            }
        }

        //------------------------------------------------------------

        #endregion
    }
}
