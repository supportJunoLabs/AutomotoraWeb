$(document).ready(function () {

    var filtrarPeriodo = $("#cb_filtrarPeriodo").prop('checked')
    _showHidePeriodo(filtrarPeriodo)

    var filtrarCombustible = $("#cb_filtrarCombustible").prop('checked')
    _showHideCombustible(filtrarCombustible);

    var filtrarSucursal = $("#cb_filtrarSucursal").prop('checked')
    _showHideSucursal(filtrarSucursal);

});


$("#cb_filtrarPeriodo").change(function () {
    var optionSelected = $(this).prop('checked')
    _showHidePeriodo(optionSelected);
});

$("#cb_filtrarCombustible").change(function () {
    var optionSelected = $(this).prop('checked')
    _showHideCombustible(optionSelected);
});

$("#cb_filtrarSucursal").change(function () {
    var optionSelected = $(this).prop('checked')
    _showHideSucursal(optionSelected);
});



function _showHidePeriodo(filtrarPeriodo) {
    if (filtrarPeriodo == undefined || filtrarPeriodo == false) {
        $("#periodo").hide();
        $("#fdesde").val(fechaTexto(sumarMeses( hoy(), -1) ) ) ;
        $("#fhasta").val(fechaTexto( hoy() ) );
    } else {
        $("#periodo").show();
    }
}

function _showHideCombustible(filtrarCombustible) {
    if (filtrarCombustible == undefined || filtrarCombustible == false) {
        $("#combustibles").hide();
    } else {
        $("#combustibles").show();
    }
}



function _showHideSucursal(filtrarSucursal) {
    if (filtrarSucursal == undefined || filtrarSucursal == false) {
        $("#sucursales").hide();
    } else {
        $("#sucursales").show();
    }
}