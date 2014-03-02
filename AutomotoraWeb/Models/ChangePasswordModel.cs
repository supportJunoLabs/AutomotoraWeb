using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AutomotoraWeb.Models {
    public class ChangePasswordModel {

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Contraseña actual")]
        public string ActualPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Nueva contraseña")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Repita la nueva contraseña")]
        public string RepeatNewPassword { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}