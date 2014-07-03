$(document).ready(function () {
    var filtrarFinancista = $("#cb_filtrarFinancista").prop('checked')
    _showHideFinancista(filtrarFinancista);

    var filtrarSucursal = $("#cb_filtrarSucursal").prop('checked')
      _showHideSucursal(filtrarSucursal);

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


$("input[name='Filtro.Situacion']").on("change", function () {
    var mostrarSuc = false;
    if ($("#rb_pendientes").prop('checked') || $("#rb_rechazados").prop('checked')) {
        mostrarSuc = true;
    }
    _showHideSucursalFiltro(mostrarSuc);
});


$("#cb_filtrarFinancista").change(function () {
    var optionSelected = $(this).prop('checked')
    _showHideFinancista(optionSelected);
});

$("#cb_filtrarSucursal").change(function () {
    var optionSelected = $(this).prop('checked')
    _showHideSucursal(optionSelected);
});

function _showHideFinancista(filtrarFinancista) {
    if (filtrarFinancista == undefined || filtrarFinancista == false) {
        $("#financistas").hide();
    } else {
        $("#financistas").show();
    }
}

function _showHideSucursal(filtrarSucursal) {
    if (filtrarSucursal == undefined || filtrarSucursal == false) {
        $("#sucursales").hide();
    } else {
        $("#sucursales").show();
    }
}

function _showHideSucursalFiltro(mostrar) {
    if (mostrar == true) {
        $("#filtro_sucursal").show();
    } else {
        $("#filtro_sucursal").hide();
    }
}