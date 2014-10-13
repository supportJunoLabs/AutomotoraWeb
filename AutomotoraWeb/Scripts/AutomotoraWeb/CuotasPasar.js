$(document).ready(function () {

    inicializarModal("Confirmacion", "Confirma transferir estas cuotas?", "Aceptar", "Cancelar");

    $("#selectAllCuotas").live('click', function () {
        gridCuotasTransf.SelectRows();
    });

    $("#unSelectAllCuotas").live('click', function () {
        gridCuotasTransf.UnselectRows();
    });

    $("#btn_confirmar").live('click', function () {
        gridCuotasTransf.GetSelectedFieldValues("Codigo", ObtenerCodigosConfirmarCallBack);
    });
});

function OnBeginCallbackCuotas(s, e) {
    e.customArgs["idSession"] = $('#idSession').val();
}

function SelectionChangedCuotas(s, e) {
    s.GetSelectedFieldValues("Saldo.Monto", SelectionChangedCuotasCallback);
}

function CuotasInit(s, e) {
    if ($("#cuotasIds").val()) {
        //alert($("#cuotasIds").val());
        var claves = $("#cuotasIds").val().split(",");
        //alert(claves);
        s.SelectRowsByKey(claves);
    }
}

function SelectionChangedCuotasCallback(values) {
    var cant = 0;
    var total = 0;
    for (var i = 0; i < values.length; i++) {
        total += values[i];
        cant++;
    }
    $("#spanTotalCantidad").text(cant);
    $("#spanTotalImporte").text(total);

}

function ObtenerCodigosConfirmarCallBack(values) {
    //alert(values);
    //al presionar el boton confirmar
    var selectedIDs;
    selectedIDs = "";
    for (var index = 0; index < values.length; index++) {
        selectedIDs += values[index] + ",";
    }
    //alert(selectedIDs);
    $("#cuotasIds").val(selectedIDs);
    $('#myModal').modal('show');
    //$('form#formPrincipal').submit();
}

function callBackAceptar() {
    $('form#formPrincipal').submit();
}

function OnBeginCallbackVentas(s, e) {
    e.customArgs["idSession"] = $('#idSession').val();
}

function ventaSelected(s, e) {
    var valor = s.GetRowKey(e.visibleIndex);
    $("#codigoSubfin").val(valor);
    $("#cuotasIds").val("");
    var idVenta = "";
    if (valor) {
        idVenta = (valor.split("-"))[0];
        $("#abtn_verFinanciacion").prop("href", "/ConsultasFin/ConsultaFinanciacion/" + idVenta);
    } else {
        $("#abtn_verFinanciacion").prop("href", "#");
    }

    $('#divOperacion').html('');
    var destino = '/Cuotas/DetalleVentaPasar/';
    $.ajax({
        cache: false,
        type: "GET",
        url: destino,
        data: { "idSubfin": valor, "idSession": $('#idSession').val() },
        success: function (data) {
            $('#divOperacion').html(data);
            //var interno = $("#finInterno").val();
            //if (interno == "True") {
            //    $("#liPago").show();
            //    $("#pago_tab").show();
            //} else {
            //    $("#liPago").hide();
            //    $("#pago_tab").hide();
            //}
            reajustarControles();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            general_showErrorPopup(xhr, ajaxOptions, thrownError, destino);
        },
        beforeSend: showLoading,
        complete: hideLoading
    });
}
