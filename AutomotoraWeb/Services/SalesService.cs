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

        #region Services SELLER definition

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
            vendedor.Eliminar(sellerModel.UserName, sellerModel.IP);
        }

        //------------------------------------------------------------

        public List<SellerModel> listSellers() {
            List<Vendedor> vendedores = Vendedor.Vendedores(Vendedor.VEND_TIPO_LISTADO.TODOS);
            List<SellerModel> sellers = new List<SellerModel>();

            foreach (Vendedor vendedor in vendedores) {
                sellers.Add(this.mapSellerModel(vendedor));
            }

            return sellers;
        }

        //------------------------------------------------------------

        public bool existSeller(SellerModel sellerModel){
            Vendedor vendedor = this.mapSellerModel(sellerModel);
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
            vendedor.setearAuditoria(sellerModel.UserName, sellerModel.IP);
            return vendedor;
        }

        #endregion

        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------

        #region Services CUSTOMER definition

        public CustomerModel getCustomer(int code) {
            Cliente cliente = new Cliente();
            cliente.Codigo = code;
            cliente.Consultar();
            return this.mapCustomerModel(cliente);
        }

        //------------------------------------------------------------

        public void createCustomer(CustomerModel customerModel) {
            Cliente cliente = this.mapCustomerModel(customerModel);
            cliente.Agregar();
        }

        //------------------------------------------------------------

        public void updateCustomer(CustomerModel customerModel) {
            Cliente cliente = this.mapCustomerModel(customerModel);
            cliente.ModificarDatos();
        }

        //------------------------------------------------------------

        public void deleteCustomer(CustomerModel customerModel) {
            Cliente cliente = this.mapCustomerModel(customerModel);
            cliente.Eliminar(customerModel.UserName, customerModel.IP);
        }

        //------------------------------------------------------------

        public List<CustomerModel> listSellers() {
            List<Cliente> clientes = Cliente.Clientes();
            List<CustomerModel> customers = new List<CustomerModel>();

            foreach (Cliente cliente in clientes) {
                customers.Add(this.mapCustomerModel(cliente));
            }

            return customers;
        }

        //------------------------------------------------------------

        public bool existCustomer(CustomerModel customerModel) {
            Cliente cliente = this.mapCustomerModel(customerModel);
            cliente.Consultar();
            return false; // TODO
        }

        //------------------------------------------------------------
        //------------------------------------------------------------

        private CustomerModel mapCustomerModel(Cliente cliente) {
            CustomerModel customerModel = new CustomerModel();

            customerModel.Address = cliente.Direccion;
            customerModel.Birthday = cliente.FechaNac;
            customerModel.City = cliente.Ciudad;
            customerModel.Id = cliente.Codigo;
            customerModel.Country = cliente.Pais;
            customerModel.DocumentCI = cliente.Cedula;
            customerModel.DocumentCredential = cliente.Credencial;
            customerModel.DocumentOther = cliente.NumeroOtroDoc;
            customerModel.DocumentTypeOther = cliente.TipoOtroDoc;
            customerModel.Fax = cliente.Fax;
            customerModel.Name = cliente.Nombre;
            customerModel.Observations = cliente.Observaciones;
            customerModel.Rut = cliente.Rut;
            customerModel.SocialReason = cliente.RazonSocial;
            customerModel.Telephone = cliente.Telefono;
            customerModel.TelephoneOther = cliente.OtroTelefono;
            customerModel.ZipCode = cliente.CodigoPostal;

            if ((cliente.Ecivil != null) && (cliente.Ecivil.Codigo != "")) {
                CustomerModel.CustomerMaritalStatus maritalStatus = CustomerModel.CustomerMaritalStatus.MARRIED;
                switch (cliente.Ecivil.Codigo) {
                    case "C":
                        maritalStatus = CustomerModel.CustomerMaritalStatus.MARRIED;
                        break;
                    case "S":
                        maritalStatus = CustomerModel.CustomerMaritalStatus.SINGLE;
                        break;
                    case "D":
                        maritalStatus = CustomerModel.CustomerMaritalStatus.DIVORCED;
                        break;
                    case "V":
                        maritalStatus = CustomerModel.CustomerMaritalStatus.WIDOWER;
                        break;
                    case "U":
                        maritalStatus = CustomerModel.CustomerMaritalStatus.FREE_UNION;
                        break;
                }
                customerModel.MaritalStatus = maritalStatus;
            }

            if (cliente.Conyuge != null) {
                CustomerSpouseModel customerSpouseModel = new CustomerSpouseModel();
                customerSpouseModel.DocumentCI = cliente.Conyuge.Cedula;
                customerSpouseModel.DocumentCredential = cliente.Conyuge.Credencial;
                customerSpouseModel.DocumentOther = cliente.Conyuge.NumeroOtroDoc;
                customerSpouseModel.DocumentTypeOther = cliente.Conyuge.TipoOtroDoc;
                customerSpouseModel.Name = cliente.Conyuge.Nombre;
                customerSpouseModel.Nuptials = cliente.Conyuge.Nupcias;
                customerSpouseModel.SeparationOfProperty = cliente.Conyuge.SepBienes;
                customerModel.Spouse = customerSpouseModel;
            }

            return customerModel;
        }

        //------------------------------------------------------------

        private Cliente mapCustomerModel(CustomerModel customerModel) {
            Cliente cliente = new Cliente();

            cliente.Direccion = customerModel.Address;
            cliente.FechaNac = customerModel.Birthday;
            cliente.Ciudad = customerModel.City;
            cliente.Codigo = customerModel.Id;
            cliente.Pais = customerModel.Country;
            cliente.Cedula = customerModel.DocumentCI;
            cliente.Credencial = customerModel.DocumentCredential;
            cliente.NumeroOtroDoc = customerModel.DocumentOther;
            cliente.TipoOtroDoc = customerModel.DocumentTypeOther;
            cliente.Fax = customerModel.Fax;
            cliente.Nombre = customerModel.Name;
            cliente.Observaciones = customerModel.Observations;
            cliente.Rut = customerModel.Rut;
            cliente.RazonSocial = customerModel.SocialReason;
            cliente.Telefono = customerModel.Telephone;
            cliente.OtroTelefono = customerModel.TelephoneOther;
            cliente.CodigoPostal = customerModel.ZipCode;

            EstadoCivil estadoCivil = new EstadoCivil();

            if (customerModel.MaritalStatus != null) {
                if (customerModel.MaritalStatus.Equals(CustomerModel.CustomerMaritalStatus.MARRIED)) {
                    estadoCivil.Codigo = "C";
                } else if (customerModel.MaritalStatus.Equals(CustomerModel.CustomerMaritalStatus.SINGLE)) {
                    estadoCivil.Codigo = "S";
                } else if (customerModel.MaritalStatus.Equals(CustomerModel.CustomerMaritalStatus.DIVORCED)) {
                    estadoCivil.Codigo = "D";
                } else if (customerModel.MaritalStatus.Equals(CustomerModel.CustomerMaritalStatus.WIDOWER)) {
                    estadoCivil.Codigo = "V";
                } else if (customerModel.MaritalStatus.Equals(CustomerModel.CustomerMaritalStatus.FREE_UNION)) {
                    estadoCivil.Codigo = "U";
                }
            }

            if (cliente.Conyuge != null) {
                ClienteConyuge clienteConyuge = new ClienteConyuge();
                clienteConyuge.Cedula = customerModel.Spouse.DocumentCI;
                clienteConyuge.Credencial = customerModel.Spouse.DocumentCredential;
                clienteConyuge.NumeroOtroDoc = customerModel.Spouse.DocumentOther;
                clienteConyuge.TipoOtroDoc = customerModel.Spouse.DocumentTypeOther;
                clienteConyuge.Nombre = customerModel.Spouse.Name;
                clienteConyuge.Nupcias = customerModel.Spouse.Nuptials;
                clienteConyuge.SepBienes = customerModel.Spouse.SeparationOfProperty;
                cliente.Conyuge = clienteConyuge;
            }

            return cliente;
        }

        #endregion


        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------

        
    }
}