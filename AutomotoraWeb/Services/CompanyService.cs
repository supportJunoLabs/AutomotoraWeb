using DLL_Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace AutomotoraWeb.Services {
    public class CompanyService {

        //private Empresa emp;

        #region Singleton definition


        //MF: Asi como esta, se instancia la variable instance cada vez que se invoca
        //y entonces cada vez va a buscar otra vez los datos de la empresa a la base, demorando innecesariamente
        //Se cambia por otra forma similar de variable estatica, que se instancia al comienzo para ser compartida por todos los usuarios
        //NO es necesario hacerla lazy ya que se va a instanciar una unica vez y es estatica.
        //Para el caso de SecurityService no importa que se instancie cada vez, porque no tiene ningun objeto que lee la base como en este caso Empresa, por eso solo cambio esta clase.

        //LC: Variable estática para la instancia, se necesita utilizar una función lambda ya que el constructor es privado
        //private static readonly Lazy<CompanyService> instance = new Lazy<CompanyService>(() => new CompanyService());

        //MF 5/5: El backend es quien decide cual es la empresa activa, menos logica en el front. Se agrego para los
        //cabezales de reportes, asi que se agrega para el encabezado de las paginas tambien.
        //El singleton queda hecho del lado del backend.

        //private static CompanyService instance;

        //// Constructor privado para evitar la instanciación directa
        //private CompanyService() {
        //}

        //public static int CodigoEmpresaActiva() {
        //    return Int32.Parse(ConfigurationManager.AppSettings["COD_SISTEMA"]);
        //}

        //// Propiedad para acceder a la instancia
        //private static CompanyService Instance {
        //    get {
        //        if (instance == null) {
        //            instance = new CompanyService();
        //            instance.emp = new Empresa();
        //            instance.emp.Codigo = Int32.Parse(ConfigurationManager.AppSettings["COD_SISTEMA"]);
        //            instance.emp.Consultar();
        //        }
        //        return instance;
        //    }
        //}

        //public static void actualizarDatos(Empresa empre){
        //    if (instance!=null){
        //        instance.emp=empre;
        //    }
        //}

        #endregion

        //------------------------------------------------------------

        #region Services definition

        public static string getCompanyName() {
            return Automotora.GetEmpresaActiva().NomEmpresa ; 
        }

        //------------------------------------------------------------

        public static string getSystemName() {
            return Automotora.GetEmpresaActiva().NomSistema;  //quiero la de la empresa que estoy consultando, no la que se hereda de auditable que es calculada de la empresa activa
        }

        //public static Empresa Empresa() {
        //    return Instance.emp;
        //}

        //------------------------------------------------------------

        #endregion
    }
}