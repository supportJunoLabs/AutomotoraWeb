using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwitterBootstrapMVC;

namespace AutomotoraWeb.Models {
    public class SellerModel : AbstractModel {

        [Required(ErrorMessage = "El nombre del vendedor es obligatorio")]
        [StringLength(50, ErrorMessage = "El Nombre debe tener un largo máximo 50 caracteres")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "La Dirección debe tener un largo máximo 50 caracteres")]
        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [StringLength(50, ErrorMessage = "El Teléfono debe tener un largo máximo 50 caracteres")]
        [Display(Name = "Teléfono")]
        public string Telephone { get; set; }

        [Required(ErrorMessage = "La fecha de Ingreso del Vendedor es obligatoria")]
        [Display(Name = "Fecha de Ingreso")]
        public DateTime IngressDate { get; set; }

        [StringLength(80, ErrorMessage = "Las Observaciones debe tener un largo máximo 80 caracteres")]
        [Display(Name = "Observaciones")]
        public string Observations { get; set; }

        [Display(Name = "Habilitado")]
        public bool Enabled { get; set; }

        [Display(Name = "Foto")]
        public string Photo { get; set; }
    }
}