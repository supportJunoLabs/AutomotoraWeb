//Configuracion inicial segun valores iniciales de los controles
$(document).ready(function () {

    var filtrarPeriodoPeriodo = $("#cb_filtrarPeriodoPedido").prop('checked')
    _showHidePeriodoPedido(filtrarPeriodoPeriodo)

    var filtrarPeriodoEsperado = $("#cb_filtrarPeriodoEsperado").prop('checked')
    _showHidePeriodoEsperado(filtrarPeriodoEsperado);

    var filtrarPeriodoRecibido = $("#cb_filtrarPeriodoRecibido").prop('checked')
    _showHidePeriodoRecibido(filtrarPeriodoRecibido);

    var filtrarReservados = $("#rb_reserva_con").prop('checked')
    _showHideFiltrosReserva(filtrarReservados);

    var filtrarSucursal = $("#cb_filtrarSucursal").prop('checked')
    _showHideSucursal(filtrarSucursal);

    var filtrarCliente = $("#cb_filtrarCliente").prop('checked')
    _showHideCliente(filtrarCliente);

    var filtrarVendedor = $("#cb_filtrarVendedores").prop('checked')
    _showHideVendedor(filtrarVendedor);

    var filtrarPeriodoReserva = $("#cb_filtrarPeriodoReserva").prop('checked')
    _showHidePeriodoReserva(filtrarPeriodoReserva);

});


//Anexar las funciones de respuetsa al evento cambio en los controles

$("#cb_filtrarPeriodoPedido").change(function () {
    var optionSelected = $(this).prop('checked')
    _showHidePeriodoPedido(optionSelected);
});

$("#cb_filtrarPeriodoEsperado").change(function () {
    var optionSelected = $(this).prop('checked')
    _showHidePeriodoEsperado(optionSelected);
});

$("#cb_filtrarPeriodoRecibido").change(function () {
    var optionSelected = $(this).prop('checked')
    _showHidePeriodoRecibido(optionSelected);
});

$("input[name='Filtro.Reservado']").on("change", function () {
    var optionSelected = $("#rb_reserva_con").prop('checked');
    //alert(optionSelected);
    _showHideFiltrosReserva(optionSelected);
});

//$("#rb_reserva_con").change(function () {
//    var optionSelected = $(this).prop('checked')
//    _showHideFiltrosReserva(optionSelected);
//});

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

$("#cb_filtrarPeriodoReserva").change(function () {
    var optionSelected = $(this).prop('checked')
    _showHidePeriodoReserva(optionSelected);
});



//Funciones que ocultan o muestran los filtro segun la opcion ingresada por el usuario

function _showHidePeriodoPedido(filtrar) {
    if (filtrar == undefined || filtrar == false) {
        $("#periodoPedido").hide();
        $("#fdesdePedido").val(fechaTexto(sumarMeses( hoy(), -1) ) ) ;
        $("#fhastaPedido").val(fechaTexto( hoy() ) );
    } else {
        $("#periodoPedido").show();
    }
}

function _showHidePeriodoEsperado(filtrar) {
    if (filtrar == undefined || filtrar == false) {
        $("#periodoEsperado").hide();
        $("#fdesdeEsperado").val(fechaTexto(sumarMeses( hoy(), -1) ) ) ;
        $("#fhastaEsperado").val(fechaTexto( hoy() ) );
    } else {
        $("#periodoEsperado").show();
    }
}


function _showHidePeriodoRecibido(filtrar) {
    if (filtrar == undefined || filtrar == false) {
        $("#periodoRecibido").hide();
        $("#fdesdeRecibido").val(fechaTexto(sumarMeses( hoy(), -1) ) ) ;
        $("#fhastaRecibido").val(fechaTexto( hoy() ) );
    } else {
        $("#periodoRecibido").show();
    }
}


function _showHideSucursal(filtrar) {
    if (filtrar == undefined || filtrar == false) {
        $("#sucursales").hide();
    } else {
        $("#sucursales").show();
    }
}

function _showHideFiltrosReserva(filtrar) {
    if (filtrar == true) {
        $("#filtros_reservacion").show();
    }else{
        $("#filtros_reservacion").hide();
        $("#fdesdeReserva").val(fechaTexto(sumarMeses( hoy(), -1) ) ) ;
        $("#fhastaReserva").val(fechaTexto( hoy() ) );
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

function _showHidePeriodoReserva(filtrar) {
    if (filtrar == undefined || filtrar == false) {
        $("#periodoReserva").hide();
        $("#fdesdeReserva").val(fechaTexto(sumarMeses( hoy(), -1) ) ) ;
        $("#fhastaReserva").val(fechaTexto( hoy() ) );
    } else {
        $("#periodoReserva").show();
    }
}

