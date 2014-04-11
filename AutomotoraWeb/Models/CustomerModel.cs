using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AutomotoraWeb.Models {
    public class CustomerModel : AbstractModel {

        public enum CustomerMaritalStatus {
            MARRIED = "Casado",
            SINGLE = "Soltero",
            DIVORCED = "Divorciado",
            WIDOWER = "Viudo",
            FREE_UNION = "Unión Libre"
        }

        [Required(ErrorMessage = "El Nombre del Cliente es obligatorio")]
        [StringLength(60, ErrorMessage = "El Nombre debe tener un largo máximo 60 caracteres")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [StringLength(100, ErrorMessage = "La dirección debe tener un largo máximo 100 caracteres")]
        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [StringLength(40, ErrorMessage = "La ciudad debe tener un largo máximo 40 caracteres")]
        [Display(Name = "Ciudad")]
        public string City { get; set; }

        [StringLength(15, ErrorMessage = "El código postal debe tener un largo máximo 15 caracteres")]
        [Display(Name = "Código Postal")]
        public string ZipCode { get; set; }

        [StringLength(60, ErrorMessage = "El Telefono debe tener un largo máximo 60 caracteres")]
        [Display(Name = "Telefono")]
        public string Telephone { get; set; }

        [StringLength(60, ErrorMessage = "El Otro Telefono debe tener un largo máximo 60 caracteres")]
        [Display(Name = "Otro Telefono")]
        public string TelephoneOther { get; set; }

        [StringLength(60, ErrorMessage = "El Fax debe tener un largo máximo 60 caracteres")]
        [Display(Name = "Fax")]
        public string Fax { get; set; }

        [StringLength(20, ErrorMessage = "La Cedula de Identidad debe tener un largo máximo 20 caracteres")]
        [Display(Name = "Cedula de Identidad")]
        public string DocumentCI { get; set; }

        [StringLength(30, ErrorMessage = "La Credencial Civica debe tener un largo máximo 30 caracteres")]
        [Display(Name = "Credencial Civica")]
        public string DocumentCredential { get; set; }

        
        [StringLength(30, ErrorMessage = "El Tipo de Otro Documento debe tener un largo máximo 30 caracteres")]
        [Display(Name = "Tipo de Otro Documento")]
        public string DocumentTypeOther { get; set; }

        [StringLength(30, ErrorMessage = "El Otro Documento debe tener un largo máximo 30 caracteres")]
        [Display(Name = "Otro Documento")]
        public string DocumentOther { get; set; }

        [Display(Name = "Fecha de Nacimiento")]
        public DateTime? Birthday { get; set; }

        [StringLength(30, ErrorMessage = "El País debe tener un largo máximo 30 caracteres")]
        [Display(Name = "País")]
        public string Country { get; set; }

        [StringLength(40, ErrorMessage = "El RUT debe tener un largo máximo 40 caracteres")]
        [Display(Name = "RUT")]
        public string Rut { get; set; }

        [StringLength(80, ErrorMessage = "La Razon Social debe tener un largo máximo 80 caracteres")]
        [Display(Name = "Razon Social")]
        public string SocialReason { get; set; }

        [StringLength(256, ErrorMessage = "las observaciones deben tener un largo máximo 256 caracteres")]
        [Display(Name = "Observaciones")]
        public string Observations { get; set; }

        [Display(Name = "Conyugue")]
        public CustomerSpouseModel Spouse { get; set; }

        [Display(Name = "Estado Civil")]
        public CustomerMaritalStatus MaritalStatus { get; set; }


    }
}