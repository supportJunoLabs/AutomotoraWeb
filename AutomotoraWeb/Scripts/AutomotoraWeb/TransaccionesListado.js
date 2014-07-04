$(document).ready(function () {
    var filtrarSucursal = $("#cb_filtrarSucursal").prop('checked')
    _showHideSucursal(filtrarSucursal);

    var filtrarFinancista = $("#cb_filtrarFinancista").prop('checked')
    _showHideFinancista(filtrarFinancista);

    var filtrarTransaccion = $("#cb_filtrarTransaccion").prop('checked')
    _showHideTransaccion(filtrarTransaccion);

    var filtrarUsuario = $("#cb_filtrarUsuario").prop('checked')
    _showHideUsuario(filtrarUsuario);

    var filtrarCliente = $("#cb_filtrarCliente").prop('checked')
    _showHideCliente(filtrarCliente);

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



$("#cb_filtrarSucursal").change(function () {
    var optionSelected = $(this).prop('checked')
    _showHideSucursal(optionSelected);
});


$("#cb_filtrarFinancista").change(function () {
    var optionSelected = $(this).prop('checked')
    _showHideFinancista(optionSelected);
});


$("#cb_filtrarTransaccion").change(function () {
    var optionSelected = $(this).prop('checked')
    _showHideTransaccion(optionSelected);
});


$("#cb_filtrarUsuario").change(function () {
    var optionSelected = $(this).prop('checked')
    _showHideUsuario(optionSelected);
});


$("#cb_filtrarCliente").change(function () {
    var optionSelected = $(this).prop('checked')
    _showHideCliente(optionSelected);
});


function _showHideSucursal(filtrar) {
    if (filtrar == undefined || filtrar == false) {
        $("#sucursales").hide();
    } else {
        $("#sucursales").show();
    }
}


function _showHideFinancista(filtrar) {
    if (filtrar == undefined || filtrar == false) {
        $("#financistas").hide();
    } else {
        $("#financistas").show();
    }
}

function _showHideTransaccion(filtrar) {
    if (filtrar == undefined || filtrar == false) {
        $("#transacciones").hide();
    } else {
        $("#transacciones").show();
    }
}

function _showHideUsuario(filtrar) {
    if (filtrar == undefined || filtrar == false) {
        $("#usuarios").hide();
    } else {
        $("#usuarios").show();
    }
}

function _showHideCliente(filtrar) {
    if (filtrar == undefined || filtrar == false) {
        $("#clientes").hide();
    } else {
        $("#clientes").show();
    }
}