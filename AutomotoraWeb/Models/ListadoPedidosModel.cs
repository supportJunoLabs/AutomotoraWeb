using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DLL_Backend;

namespace AutomotoraWeb.Models {
    public class ListadoPedidosModel {
        public PedidoFiltro Filtro { get; set; }
        public List<Pedido> Resultado { get; set; }

        //Constructor
        public ListadoPedidosModel() {
            Filtro = new PedidoFiltro();
            Resultado = new List<Pedido>();
        }
    }
}