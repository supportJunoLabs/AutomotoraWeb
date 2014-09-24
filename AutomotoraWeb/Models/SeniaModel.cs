using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DLL_Backend;

namespace AutomotoraWeb.Models {
    public class SeniaModel {
        public int PedidoVehiculo { get; set; } //1=vehiculo, 2=pedido
        public Senia Senia { get; set; }
        public PrecondicionesOperacion Precondicion { get; set; }
        public bool TienePermuta { get; set; }
        public TRSeniaDevolucion SeniaDev { get; set; } //se usa en la devolucion solamente
        public string ChequesDevolver { get; set; }
            
        public SeniaModel() {
            Senia = new Senia();
            PedidoVehiculo = 1;
            TienePermuta = false;
        }

        public void asignarPrecondicion(bool esPostback) {
            if (PedidoVehiculo == 1) {
                this.Precondicion = Senia.Vehiculo.ObtenerPrecondicionesSenia();
            } else {
                this.Precondicion = Senia.Pedido.ObtenerPrecondicionesSenia();
            }
            if (!esPostback) {
                Senia.Cliente = Precondicion.Cliente;
            }
            if (!esPostback) {
                Senia.Vendedor = Precondicion.Vendedor;
            }
        }

    }
}