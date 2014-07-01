$(document).ready(function () {

    $("#tabActual").val("EFECTIVO");//porque al volver siempre quedo en el primer tab

    var filtrarSucursal = $("#cb_filtrarSucursal").prop('checked')
    _showHideSucursal(filtrarSucursal);

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

    $("#titulo_tab1").click(function () {
        //alert("click");
        $("#tabActual").val("EFECTIVO");
    });

    $("#titulo_tab2").click(function () {
        //alert("click");
        $("#tabActual").val("CHEQUES");
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