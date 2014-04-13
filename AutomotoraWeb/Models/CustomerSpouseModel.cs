using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AutomotoraWeb.Models {
    public class CustomerSpouseModel : AbstractModel {

        [StringLength(60, ErrorMessage = "El Nombre del Conygue debe tener un largo máximo 60 caracteres")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [StringLength(20, ErrorMessage = "La Cedula de Identidad del Conygue debe tener un largo máximo 60 caracteres")]
        [Display(Name = "Cedula")]
        public string DocumentCI { get; set; }

        [StringLength(30, ErrorMessage = "La Credencial Civica del Conyugue debe tener un largo máximo 30 caracteres")]
        [Display(Name = "Credencial Civica")]
        public string DocumentCredential { get; set; }

        [StringLength(30, ErrorMessage = "El Tipo de Otro Documento del Conyugue debe tener un largo máximo 30 caracteres")]
        [Display(Name = "Tipo de Otro Documento")]
        public string DocumentTypeOther { get; set; }

        [StringLength(30, ErrorMessage = "El Otro Documento del Conyugue debe tener un largo máximo 30 caracteres")]
        [Display(Name = "Otro Documento")]
        public string DocumentOther { get; set; }

        [Display(Name = "Nupcias")]
        public int Nuptials { get; set; }

        [Display(Name = "Separacion de bienes")]
        public bool SeparationOfProperty  { get; set; }
    }
}