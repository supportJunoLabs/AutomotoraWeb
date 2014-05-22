//Configuracion inicial segun valores iniciales de los controles
$(document).ready(function () {

    var filtrarPeriodo = $("#cb_filtrarPeriodo").prop('checked')
    _showHidePeriodo(filtrarPeriodo)

    var filtrarSucursal = $("#cb_filtrarSucursal").prop('checked')
    _showHideSucursal(filtrarSucursal);

    var filtrarCliente = $("#cb_filtrarCliente").prop('checked')
    _showHideCliente(filtrarCliente);

    var filtrarVendedor = $("#cb_filtrarVendedores").prop('checked')
    _showHideVendedor(filtrarVendedor);

    var filtrarCombustible = $("#cb_filtrarCombustible").prop('checked')
    _showHideCombustible(filtrarCombustible);


});


//Anexar las funciones de respuetsa al evento cambio en los controles

$("#cb_filtrarPeriodo").change(function () {
    var optionSelected = $(this).prop('checked')
    _showHidePeriodo(optionSelected);
});

$("#cb_filtrarSucursal").change(function () {
    var optionSelected = $(this).prop('checked')
    _showHideSucursal(optionSelected);
});

$("#cb_filtrarCliente").change(function () {
    var optionSelected = $(this).prop('checked')
    _showHideCliente(optionSelected);
});

$("#cb_filtrarVendedores").change(function () {
    var optionSelected = $(this).prop('checked')
    _showHideVendedor(optionSelected);
});

$("#cb_filtrarCombustible").change(function () {
    var optionSelected = $(this).prop('checked')
    _showHideCombustible(optionSelected);
});

//Funciones que ocultan o muestran los filtro segun la opcion ingresada por el usuario

function _showHidePeriodo(filtrar) {
    if (filtrar == undefined || filtrar == false) {
        $("#periodo").hide();
        $("#fdesde").val(fechaTexto(sumarMeses( hoy(), -1) ) ) ;
        $("#fhasta").val(fechaTexto( hoy() ) );
    } else {
        $("#periodo").show();
    }
}

function _showHideSucursal(filtrar) {
    if (filtrar == undefined || filtrar == false) {
        $("#sucursales").hide();
    } else {
        $("#sucursales").show();
    }
}

function _showHideCliente(filtrar) {
    if (filtrar == undefined || filtrar == false) {
        $("#clientes").hide();
    } else {
        $("#clientes").show();
    }
}

function _showHideVendedor(filtrar) {
    if (filtrar == undefined || filtrar == false) {
        $("#vendedores").hide();
    } else {
        $("#vendedores").show();
    }
}

function _showHideCombustible(filtrarCombustible) {
    if (filtrarCombustible == undefined || filtrarCombustible == false) {
        $("#combustibles").hide();
    } else {
        $("#combustibles").show();
    }
}


