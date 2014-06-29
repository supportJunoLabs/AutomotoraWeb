$(document).ready(function () {
    var filtrarSucursal = $("#cb_filtrarSucursal").prop('checked')
    _showHideSucursal(filtrarSucursal);

    var filtrarFinancista = $("#cb_filtrarFinancista").prop('checked')
    _showHideFinancista(filtrarFinancista);

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