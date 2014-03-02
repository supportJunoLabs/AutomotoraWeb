using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AutomotoraWeb.Models {
    public class LoginModel {

        [Required]
        [DisplayName("Nombre de Usuario")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Contraseña")]
        public string Password { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

    }
}