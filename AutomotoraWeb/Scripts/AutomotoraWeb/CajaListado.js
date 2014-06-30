$(document).ready(function () {
    var filtrarSucursal = $("#cb_filtrarSucursal").prop('checked')
    _showHideSucursal(filtrarSucursal);

    var filtrarFinancista = $("#cb_filtrarFinancista").prop('checked')
    _showHideFinancista(filtrarFinancista);

    $("#btn_actualizar").click(function () {
        //alert("click");
        $("#accion").val ( "ACTUALIZAR");
        $('form#formPrincipal').submit();
    });

    $("#btn_imprimir_efectivo").click(function () {
        //alert("click");
        $("#accion").val("IMPRIMIR_EFECTIVO");
        $('form#formPrincipal').submit();
    });

    $("#btn_imprimir_cheques").click(function () {
        //alert("click");
        $("#accion").val("IMPRIMIR_CHEQUES");
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


function _showHideSucursal(filtrarSucursal) {
    if (filtrarSucursal == undefined || filtrarSucursal == false) {
        $("#sucursales").hide();
    } else {
        $("#sucursales").show();
    }
}


function _showHideFinancista(filtrarFinancista) {
    if (filtrarFinancista == undefined || filtrarFinancista == false) {
        $("#financistas").hide();
    } else {
        $("#financistas").show();
    }
}