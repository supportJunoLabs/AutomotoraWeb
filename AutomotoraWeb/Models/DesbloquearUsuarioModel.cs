using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AutomotoraWeb.Models {
    public class DesbloquearUsuarioModel {

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
        [DisplayName("Usuario a Desbloquear")]
        [StringLength(30, ErrorMessage = "Usuario a Modificar: Largo maximo 30 caracteres")]
        public string UsuarioReset { get; set; }

    }
}