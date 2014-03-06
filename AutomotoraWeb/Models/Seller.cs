using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwitterBootstrapMVC;

namespace AutomotoraWeb.Models {
    public class Seller {

        [Required(ErrorMessage = "El nombre del vendedor es obligatorio")]
        [StringLength(30, ErrorMessage = "Largo maximo 30 caracteres")]
        [Display(Name = "Nombre")]
        [Remote("NombreVendedorDisponible", "Vendedores", HttpMethod = "POST", ErrorMessage = "Ya existe un vendedor con este nombre.", AdditionalFields = "Id")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "Largo maximo 50 caracteres")]
        [Display(Name = "Direccion")]
        public string Address { get; set; }

        [StringLength(30, ErrorMessage = "Largo maximo 30 caracteres")]
        [Display(Name = "Telefono")]
        public string Telephone { get; set; }

        [Required(ErrorMessage = "La fecha de Ingreso del Vendedor es obligatoria")]
        [Display(Name = "Fecha de Ingreso")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime IngressDate { get; set; }

        [StringLength(80, ErrorMessage = "Largo maximo 80 caracteres")]
        [Display(Name = "Observaciones")]
        public string Observations { get; set; }
    }
}