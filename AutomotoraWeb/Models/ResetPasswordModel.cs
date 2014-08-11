using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AutomotoraWeb.Models {
    public class ResetPasswordModel {

        [Required]
        [DisplayName("Usuario Actual")]
        [StringLength(30, ErrorMessage = "Usuario Actual: Largo maximo 30 caracteres")]
        public string UsuarioActual { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Contraseña Usuario Actual")]
        [StringLength(30, ErrorMessage = "Clave Usuario Actual: Largo maximo 30 caracteres")]
        public string ClaveUsuarioActual { get; set; }

        [Required]
        [DisplayName("Usuario a Modificar")]
        [StringLength(30, ErrorMessage = "Usuario a Modificar: Largo maximo 30 caracteres")]
        public string UsuarioReset { get; set; }

        [Required]
        [DisplayName("Nueva Clave")]
        [StringLength(30, ErrorMessage = "Nueva Clave: Largo maximo 30 caracteres")]
        public string ClaveUsuarioReset { get; set; }

        [Required]
        [DisplayName("Verificación Nueva Clave")]
        [StringLength(30, ErrorMessage = "Verificación Nueva Clave: Largo maximo 30 caracteres")]
        public string VerificacionClaveUsuarioReset { get; set; }



    }
}