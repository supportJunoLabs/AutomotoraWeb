$(document).ready(function () {
    var filtrarCuenta = $("#cb_filtrarCuenta").prop('checked')
    _showHideCuenta(filtrarCuenta);

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


$("#cb_filtrarCuenta").change(function () {
    var optionSelected = $(this).prop('checked')
    _showHideCuenta(optionSelected);
});

function _showHideCuenta(filtrar) {
    if (filtrar == undefined || filtrar == false) {
        $("#cuentas").hide();
    } else {
        $("#cuentas").show();
    }
}
