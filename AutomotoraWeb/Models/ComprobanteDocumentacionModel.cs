using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DLL_Backend;

namespace AutomotoraWeb.Models {
    public class ComprobanteDocumentacionModel {
        public ComprobanteDocumentacion Comprobante { get; set; }
        public string DocsIds { get; set; }
        public string idParametros { get; set; }

        public ComprobanteDocumentacionModel() {
            Comprobante = new ComprobanteDocumentacion();
        }
    }
}