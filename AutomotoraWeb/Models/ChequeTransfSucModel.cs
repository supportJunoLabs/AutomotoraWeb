using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DLL_Backend;
using System.ComponentModel.DataAnnotations;

namespace AutomotoraWeb.Models {
    public class ChequeTransfSucModel {
        [Required(ErrorMessage = "La Sucursal Origen es requerida")]
        [Display(Name = "Sucursal Origen")]
        public Sucursal SucursalOrigen { get; set; }

        [Required(ErrorMessage = "La Sucursal Destino es requerida")]
        [Display(Name = "Sucursal Destino")]
        public Sucursal SucursalDestino { get; set; }

        public string ChequesIds { get; set; }

        [Display(Name = "Observaciones")]
        [StringLength(150, ErrorMessage = "Observaciones: Largo maximo 150 caracteres")]
        public Cheque Observaciones { get; set; }
    }
}