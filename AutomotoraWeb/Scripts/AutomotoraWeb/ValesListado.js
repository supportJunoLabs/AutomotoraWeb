$(document).ready(function () {
    var filtrarFinancista = $("#cb_filtrarFinancista").prop('checked')
    _showHideFinancista(filtrarFinancista);

   $("#btn_actualizar").click(function () {
        //alert("click");
        $("#accion").val("ACTUALIZAR");
        $('form#formPrincipal').submit();
    });

    $("#btn_imprimir").click(function () {
        //alert("click");
        $("#accion").val("IMPRIMIR");
        $('form#formPrincipal').submit();
    });
});

$("#cb_filtrarFinancista").change(function () {
    var optionSelected = $(this).prop('checked')
    _showHideFinancista(optionSelected);
});

function _showHideFinancista(filtrarFinancista) {
    if (filtrarFinancista == undefined || filtrarFinancista == false) {
        $("#financistas").hide();
    } else {
        $("#financistas").show();
    }
}
