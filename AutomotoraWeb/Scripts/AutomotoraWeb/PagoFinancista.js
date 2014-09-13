
function SelectionChequeChanged(s, e) {
    //al hacer click en la grilla
    s.GetSelectedFieldValues("Cheque.ImporteTexto", RefrescarListaChequesCallback);
    //alert("hola");
}

function SelectionEfectivoChanged(s, e) {
    //al hacer click en la grilla
    s.GetSelectedFieldValues("ImportePagoActual.ImporteTexto", RefrescarListaEfectivoCallback);
    //alert("hola");
}

function OnBeginCallbackCheques(s, e) {
    //alert($(ddlSucursales).val());
    e.customArgs["idFinancista"] = $(ddlFinancistas).val();
    e.customArgs["idSession"] = $(idSession).val();
   
}

function OnEndCallbackEfectivo(s, e) {
    if ($("#ultimoComando").val() == 'UPDATEEDIT') {
        //alert($("#ultimoComando").val());
        s.GetSelectedFieldValues("ImportePagoActual.ImporteTexto", RefrescarListaEfectivoCallback);
    }
}

function OnBeginCallbackEfectivo(s, e) {
    //alert($(ddlSucursales).val());
    e.customArgs["idFinancista"] = $(ddlFinancistas).val();
    e.customArgs["idSession"] = $(idSession).val();
    $("#ultimoComando").val(e.command);
}

$(document).ready(function () {
    $("#btn_confirmar").click(function () {
        gridCheques.GetSelectedFieldValues("Codigo", ObtenerCodigosConfirmarCallBack);
    });

    $("#btn_actualizar").click(function () {
        cambiarFinancista();
    });

    $("#ddlFinancistas").change(function () {
        cambiarFinancista();
    });

    $("#selectAllCheques").live('click', function () {
        gridCheques.SelectRows();
    });

    $("#unSelectAllCheques").live('click', function () {
        gridCheques.UnselectRows();
    });

    $("#selectAllEfectivo").live('click', function () {
        //alert("select");
        gridEfectivo.SelectRows();
    });

    $("#unSelectAllEfectivo").live('click', function () {
        //alert("unselect");
        gridEfectivo.UnselectRows();
    });
});

function cambiarFinancista() {
    $('#divDetallePago').html('');

    //vaciar la lista de cheques elegidos
    //SelectedRowsCheques.ClearItems();
    //SelectedRowsEfectivo.ClearItems();
    //$("#cantCheques").html(0);
    //$("#cantEfectivo").html(0);
    //refrescar la grilla de cheques de la sucursal
    var selectedID = $(ddlFinancistas).val();
    var idSession = $('#idSession').val();
    //alert(selectedID);
    var destino = '/Financistas/FinancistaPagoChanged/';
    $.ajax({
        cache: false,
        type: "GET",
        url: destino,
        data: { "idFinancista": selectedID, "idSession": idSession },
        success: function (data) {
            //alert("regreso");
            $('#divDetallePago').html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            general_showErrorPopup(xhr, ajaxOptions, thrownError, destino);
        },
        beforeSend: showLoading,
        complete: hideLoading
    });
 }

function ObtenerCodigosConfirmarCallBack(values) {
    //al presionar el boton transferir
    //alert("hola");
    var selectedIDs;
    selectedIDs = "";
    for (var index = 0; index < values.length; index++) {
        selectedIDs += values[index] + ",";
    }
    //alert(selectedIDs);
    $("#chequesIds").val(selectedIDs);
    //alert($("#chequesIds").val());

    gridEfectivo.GetSelectedFieldValues("Codigo", ObtenerCodigosConfirmar2CallBack);
}

function ObtenerCodigosConfirmar2CallBack(values) {
    //al presionar el boton transferir
    //alert("hola");
    var selectedIDs;
    selectedIDs = "";
    for (var index = 0; index < values.length; index++) {
        selectedIDs += values[index] + ",";
    }
    //alert(selectedIDs);
    $("#efectivosIds").val(selectedIDs);
    //alert($("#chequesIds").val());

    $('form#formPrincipal').submit();
}


function RefrescarListaChequesCallback(values) {
    SelectedRowsCheques.BeginUpdate();
    try {
        SelectedRowsCheques.ClearItems();
        for (var i = 0; i < values.length; i++) {
            SelectedRowsCheques.AddItem(values[i]);
        }
    } finally {
        SelectedRowsCheques.EndUpdate();
    }
    $("#cantCheques").html(gridCheques.GetSelectedRowCount());

}

function RefrescarListaEfectivoCallback(values) {
    SelectedRowsEfectivo.BeginUpdate();
    try {
        SelectedRowsEfectivo.ClearItems();
        for (var i = 0; i < values.length; i++) {
            SelectedRowsEfectivo.AddItem(values[i]);
        }
    } finally {
        SelectedRowsEfectivo.EndUpdate();
    }
    $("#cantEfectivo").html(gridEfectivo.GetSelectedRowCount());

}