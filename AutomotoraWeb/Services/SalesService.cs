using AutomotoraWeb.Models;
using DLL_Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomotoraWeb.Services {
    public class SalesService {

        #region Singleton definition

        // Variable estática para la instancia, se necesita utilizar una función lambda ya que el constructor es privado
        private static readonly Lazy<SalesService> instance = new Lazy<SalesService>(() => new SalesService());

        // Constructor privado para evitar la instanciación directa
        private SalesService() {
        }

        // Propiedad para acceder a la instancia
        public static SalesService Instance {
            get {
                return instance.Value;
            }
        }

        #endregion

        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------

        #region Services Seller definition

        public SellerModel getSeller(int code) {
            Vendedor vendedor = new Vendedor();
            vendedor.Codigo = code;
            vendedor.Consultar();
            return this.mapSellerModel(vendedor);
        }

        //------------------------------------------------------------

        public void createSeller(SellerModel sellerModel) {
            Vendedor vendedor = this.mapSellerModel(sellerModel);
            vendedor.Agregar();
        }

        //------------------------------------------------------------

        public void updateSeller(SellerModel sellerModel) {
            Vendedor vendedor = this.mapSellerModel(sellerModel);
            vendedor.ModificarDatos();
        }

        //------------------------------------------------------------

        public void deleteSeller(SellerModel sellerModel) {
            Vendedor vendedor = this.mapSellerModel(sellerModel);
            vendedor.Eliminar();
        }

        //------------------------------------------------------------

        public List<SellerModel> listSellers(SellerModel sellerModel) {
            List<Vendedor> vendedores = Vendedor.Vendedores(Vendedor.VEND_TIPO_LISTADO.TODOS);
            List<SellerModel> sellers = new List<SellerModel>();

            foreach (Vendedor vendedor in vendedores) {
                sellers.Add(this.mapSellerModel(vendedor));
            }

            return sellers;
        }

        //------------------------------------------------------------

        public bool existSeller(SellerModel seller){
            Vendedor vendedor = this.mapSellerModel(seller);
            vendedor.Consultar();
            return false; // TODO
        }

        //------------------------------------------------------------
        //------------------------------------------------------------

        private SellerModel mapSellerModel(Vendedor vendedor) {
            SellerModel sellerModel = new SellerModel();
            sellerModel.Address = vendedor.Direccion;
            sellerModel.Id = vendedor.Codigo;
            sellerModel.IngressDate = vendedor.FechaIngreso;
            sellerModel.Name = vendedor.Nombre;
            sellerModel.Observations = vendedor.Observaciones;
            sellerModel.Telephone = vendedor.Telefono;
            sellerModel.Enabled = vendedor.Habilitado;
            sellerModel.Photo = vendedor.Foto;
            return sellerModel;
        }

        //------------------------------------------------------------

        private Vendedor mapSellerModel(SellerModel sellerModel) {
            Vendedor vendedor = new Vendedor();
            vendedor.Direccion = sellerModel.Address;
            vendedor.Codigo = sellerModel.Id;
            vendedor.FechaIngreso = sellerModel.IngressDate;
            vendedor.Nombre = sellerModel.Name;
            vendedor.Observaciones = sellerModel.Observations;
            vendedor.Telefono = sellerModel.Telephone;
            vendedor.Habilitado = sellerModel.Enabled;
            vendedor.Foto = sellerModel.Photo;
            return vendedor;
        }

        #endregion

        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------



        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------

        
    }
}