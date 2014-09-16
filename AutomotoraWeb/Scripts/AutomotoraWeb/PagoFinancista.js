
function SelectionChequeChanged(s, e) {
    //al hacer click en la grilla
    s.GetSelectedFieldValues("Cheque.ImporteTexto", SelectionChangedChequesCallback);
    //alert("hola");
}

function SelectionEfectivoChanged(s, e) {
    //al hacer click en la grilla
    s.GetSelectedFieldValues("ImportePagoActual.ImporteTexto", SelectionChangedEfectivoCallback);
    //alert("hola");
}



function OnBeginCallbackCheques(s, e) {
    //alert($(ddlSucursales).val());
    e.customArgs["idFinancista"] = $(ddlFinancistas).val();
    e.customArgs["idSession"] = $(idSession).val();
   
}


var efectivoUltimoComando;
function OnBeginCallbackEfectivo(s, e) {
    e.customArgs["idFinancista"] = $(ddlFinancistas).val();
    e.customArgs["idSession"] = $(idSession).val();
    //$("#ultimoComando").val(e.command); --lo hacia usando un hidden, pero ahora lo cambie por variable de javascript que se referencia desde el endcallback
    if (e.command == 'UPDATEEDIT') {
        efectivoUltimoComando = e.command
        //alert(e.command);
    } else {
        efectivoUltimoComando = "";
    }

}

function OnEndCallbackEfectivo(s, e) {
    //alert(efectivoUltimoComando);
    //if ($("#ultimoComando").val() == 'UPDATEEDIT') {
    if (efectivoUltimoComando == 'UPDATEEDIT') {
        s.GetSelectedFieldValues("ImportePagoActual.ImporteTexto", RefrescarListaEfectivoCallback);
    }
}


function EfectivoInit() {
    if ($("#efectivosIds").val()) {
        //alert($("#efectivosIds").val());
        var clavesef = $("#efectivosIds").val().split(",");
        gridEfectivo.SelectRowsByKey(clavesef);
    }
}

function ChequesInit() {
    if ($("#chequesIds").val()) {
        //alert($("#chequesIds").val());
        var clavesch = $("#chequesIds").val().split(",");
        gridCheques.SelectRowsByKey(clavesch);
    }
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
    var selectedID = $(ddlFinancistas).val();
    var idSession = $('#idSession').val();
    var destino = '/Financistas/FinancistaPagoChanged/';
    $.ajax({
        cache: false,
        type: "GET",
        url: destino,
        data: { "idFinancista": selectedID, "idSession": idSession },
        success: function (data) {
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
    //al presionar el boton transferir, para pasar los ids de cheques seleccionados
    var selectedIDs;
    selectedIDs = "";
    for (var index = 0; index < values.length; index++) {
        selectedIDs += values[index] + ",";
    }
    $("#chequesIds").val(selectedIDs);
    gridEfectivo.GetSelectedFieldValues("Codigo", ObtenerCodigosConfirmar2CallBack);
}

function ObtenerCodigosConfirmar2CallBack(values) {
    //al presionar el boton transferir, para pasar los ids de efectivos seleccionados
    var selectedIDs;
    selectedIDs = "";
    for (var index = 0; index < values.length; index++) {
        selectedIDs += values[index] + ",";
    }
    $("#efectivosIds").val(selectedIDs);

    $('form#formPrincipal').submit();
}


function SelectionChangedChequesCallback(values) {
    SelectedRowsCheques.BeginUpdate();
    var cant = 0;
    try {
        SelectedRowsCheques.ClearItems();
        for (var i = 0; i < values.length; i++) {
            SelectedRowsCheques.AddItem(values[i]);
            cant++;
        }
    } finally {
        SelectedRowsCheques.EndUpdate();
    }
    $("#cantCheques").html(cant);

}

function SelectionChangedEfectivoCallback(values) {
    //SelectedRowsEfectivo.BeginUpdate();
    var cant = 0;
    try {
        SelectedRowsEfectivo.ClearItems();
        for (var i = 0; i < values.length; i++) {
            SelectedRowsEfectivo.AddItem(values[i]);
            cant++;
        }
    } finally {
        //SelectedRowsEfectivo.EndUpdate();
    }
    //$("#cantEfectivo").html(gridEfectivo.GetSelectedRowCount());
    $("#cantEfectivo").html(cant);

}