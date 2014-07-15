//Saco las validacion de required y las dejo para hacer del server, porque los mensajes por defecto son feos y dicen siempre codigo.
$(document).ready(function () {
    $("#ddlCuentas").attr("data-val-required", "La cuenta es requerida");
    }
);


function chequeSelected(s, e) {
    //alert("hola");
    var g = gridCheques.GetGridView();  //obtener referencia a la grilla
    var valor = g.GetRowKey(g.GetFocusedRowIndex()); //obtener el valor elegido
    //alert(valor);
    $("#codigoCheque").val(valor);
}