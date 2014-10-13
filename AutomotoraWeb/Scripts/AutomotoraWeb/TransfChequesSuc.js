﻿$(document).ready(function () {
    inicializarModal("Confirmacion", "Confirma transferir cheques a otra sucursal?", "Aceptar", "Cancelar");

    $("#btn_transferir").click(function () {
        gridCheques.GetSelectedFieldValues("Codigo", CodigosSeleccionadosCallBack);
    });

    $("#btn_actualizar").click(function () {
        cambiarSucursalOrigen();
    });

    $("#ddlSucursales").change(function () {
        cambiarSucursalOrigen();
    });
});


function cambiarSucursalOrigen() {
    //vaciar la lista de cheques elegidos
    SelectedRows.ClearItems();
    $("#count").html(0);
    //refrescar la grilla de cheques de la sucursal
    var selectedID = $(ddlSucursales).val();
    //alert(selectedID);
    var destino='/Cheques/SucursalOrigenChanged/';
    $.ajax({
        cache: false,
        type: "GET",
        url: destino,
        data: { "idSucursal": selectedID },
        success: function (data) {
            $('#divGrillaCheques').html('');
            $('#divGrillaCheques').html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            general_showErrorPopup(xhr, ajaxOptions, thrownError, destino);
        },
        beforeSend: showLoading,
        complete: hideLoading
    });
}

function CodigosSeleccionadosCallBack(values) {
    //al presionar el boton transferir
    var selectedIDs;
    selectedIDs = "";
    for (var index = 0; index < values.length; index++) {
        selectedIDs += values[index] + ",";
    }
    $("#chequesIds").val(selectedIDs);
    $('#myModal').modal('show');
    //$('form#formPrincipal').submit();
}

function callBackAceptar() {
    $('form#formPrincipal').submit();
}

function SelectionChanged(s, e) {
    //al hacer click en la grilla
    s.GetSelectedFieldValues("ImporteTexto", GetSelectedFieldValuesCallback);
}
function GetSelectedFieldValuesCallback(values) {
    SelectedRows.BeginUpdate();
    try {
        SelectedRows.ClearItems();
        for (var i = 0; i < values.length; i++) {
            SelectedRows.AddItem(values[i]);
        }
    } finally {
        SelectedRows.EndUpdate();
    }
    $("#count").html(gridCheques.GetSelectedRowCount());

}