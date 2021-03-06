﻿$(document).ready(function () {
    var esFinancista = $("#rb_financista").prop('checked');
    _opcionFinancista(esFinancista);

    $("#codigoCheque").attr("data-val-required", "El cheque es Requerido");

    inicializarModal("Confirmacion", "Confirma pasar estos cheques?", "Aceptar", "Cancelar");

    $("#btnAceptar").live('click', function () {
        $('#myModal').modal('show');
    });
});

function callBackAceptar() {
    $('form#formPrincipal').submit();
}

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
    //var g = gridCheques.GetGridView();  //obtener referencia a la grilla
    //var valor = g.GetRowKey(g.GetFocusedRowIndex()); //obtener el valor elegido
    var valor = s.GetRowKey(e.visibleIndex);
    //alert(valor);
    $("#codigoCheque").val(valor);
    $("#abtn_verCheque").prop("href", "/Cheques/ConsultaCheque/" + valor);
}