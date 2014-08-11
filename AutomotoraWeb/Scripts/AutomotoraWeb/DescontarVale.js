//Saco las validacion de required y las dejo para hacer del server, porque los mensajes por defecto son feos y dicen siempre codigo.
$(document).ready(function () {
    $("#ddlCuentas").attr("data-val-required", "La cuenta es requerida");
    }
);


function valeSelected(s, e) {
    //alert("hola");
    //var g = gridVales.GetGridView();  //obtener referencia a la grilla
    //var valor = g.GetRowKey(g.GetFocusedRowIndex()); //obtener el valor elegido
    var valor = s.GetRowKey(e.visibleIndex);
    //alert(valor);
    $("#codigoVale").val(valor);
}