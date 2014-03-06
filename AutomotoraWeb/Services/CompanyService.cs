using DLL_Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomotoraWeb.Services {
    public class CompanyService {

        private static Empresa emp;

        #region Singleton definition

        // Variable estática para la instancia, se necesita utilizar una función lambda ya que el constructor es privado
        private static readonly Lazy<CompanyService> instance = new Lazy<CompanyService>(() => new CompanyService());

        // Constructor privado para evitar la instanciación directa
        private CompanyService() {
        }

        // Propiedad para acceder a la instancia
        public static CompanyService Instance {
            get {
                emp = new Empresa();
                emp.Codigo = 1;
                emp.Consultar();
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

        //------------------------------------------------------------

        #endregion
    }
}