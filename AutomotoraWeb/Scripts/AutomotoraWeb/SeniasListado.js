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

    var verVehiculos = $("#rb_vehiculos").prop('checked')
    _showHidePedidos(verVehiculos);

});


//Anexar las funciones de respuetsa al evento cambio en los controles

$("#cb_filtrarPeriodo").change(function () {
    var optionSelected = $(this).prop('checked')
    _showHidePeriodo(optionSelected);
});

$("input[name='Filtro.Vehiculos']").on("change", function () {
    var verVehiculos = $("#rb_vehiculos").prop('checked');
    //alert(optionSelected);
    _showHidePedidos(verVehiculos);
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

function _showHidePedidos(verVehiculos) {
    if (verVehiculos == undefined || verVehiculos == true) {
        $("#filtro_pedidos").hide();
        $("#cb_pedidos_pendientes").prop('checked', false) ;
        $("#cb_pedidos_recibidos").prop('checked', false) ;
    } else {
        $("#filtro_pedidos").show();
        $("#cb_pedidos_pendientes").prop('checked', true) ;
        $("#cb_pedidos_recibidos").prop('checked', true) ;
    }
}

