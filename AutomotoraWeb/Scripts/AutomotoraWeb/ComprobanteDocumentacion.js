$(document).ready(function () {
    $("#btn_generar").click(function () {
        gridDocumentos.GetSelectedFieldValues("Codigo", CodigosSeleccionadosCallBack);
    });

    var selectedIDs = $("#docsIds").val();
    //alert(selectedIDs);
    if (selectedIDs) {
        var array = selectedIDs.split(',');
        //alert(array);
        gridDocumentos.SelectRowsByKey(array);
    }

    var entregar = $("#rb_entregar").prop('checked')
    EntregarRecibir(entregar);

});

$("input[name='Comprobante.Tipo']").on("change", function () {
    var entregar = $("#rb_entregar").prop('checked')
    //alert(entregar);
    EntregarRecibir(entregar);
});


function EntregarRecibir(entregar) {
    if (entregar) {
        $("#label_destino").text("Entregado a ");
    } else {
        $("#label_destino").text("Recibido de ");
    }
}

function CodigosSeleccionadosCallBack(values) {
    //al presionar el boton transferir
    //alert("hola");
    var selectedIDs;
    selectedIDs = "";
    for (var index = 0; index < values.length; index++) {
        selectedIDs += values[index] + ",";
    }
    //alert(selectedIDs);
    $("#docsIds").val(selectedIDs);
    //alert($("#chequesIds").val());
    $('form#formPrincipal').submit();
}