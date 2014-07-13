$(document).ready(function () {
    var esFinancista = $("#rb_financista").prop('checked');
    _opcionFinancista(esFinancista);
});

$("input[name='TipoDestino']").on("change", function () {
    var esFinancista = $("#rb_financista").prop('checked');
   _opcionFinancista(esFinancista);
});

function _opcionFinancista(esFinancista) {
    if (esFinancista == true) {
        $("#financistas").show();
        $("#tercero").hide();
    } else {
        $("#financistas").hide();
        $("#tercero").show();
    }
}

function chequeSelected(s, e) {
    //alert("hola");
    var g = gridCheques.GetGridView();  //obtener referencia a la grilla
    var valor = g.GetRowKey(g.GetFocusedRowIndex()); //obtener el valor elegido
    //alert(valor);
    $("#codigoCheque").val(valor);
}