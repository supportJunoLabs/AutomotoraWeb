$(document).ready(function () {

    inicializarModal("Confirmacion", "Confirma la devolucion de la seña?", "Aceptar", "Cancelar");
    $("#btn_confirmar").live('click', function () {
         gridChequesDevolver.GetSelectedFieldValues("Codigo", ObtenerCodigosConfirmarCallBack);
     });
     
});

function ObtenerCodigosConfirmarCallBack(values) {
    //alert("hola");
    //al presionar el boton confirmar
    var selectedIDs;
    selectedIDs = "";
    for (var index = 0; index < values.length; index++) {
        selectedIDs += values[index] + ",";
    }
    //alert(selectedIDs);
    $("#chequesDevolverIds").val(selectedIDs);
    $('#myModal').modal('show');
    //$('form#formPrincipal').submit();
}

function callBackAceptar() {
    $('form#formPrincipal').submit();
}

function SelectionChangedChequesDev(s, e) {
    //al hacer click en la grilla
    s.GetSelectedFieldValues("ImporteTexto", GetSelectedFieldChequesDevCallback);
}
function GetSelectedFieldChequesDevCallback(values) {
    //alert("xx");
    //alert(values);
    SelectedRows.BeginUpdate();
    var tot = 0;
    try {
        SelectedRows.ClearItems();
        for (var i = 0; i < values.length; i++) {
            SelectedRows.AddItem(values[i]);
            tot++;
        }
    } finally {
        SelectedRows.EndUpdate();
    }
    $("#count").html(tot);
}

function ChequesDevInit() {
    //alert($("#chequesDevolverIds").val());
    if ($("#chequesDevolverIds").val()) {
        var clavesch = $("#chequesDevolverIds").val().split(",");
        gridChequesDevolver.SelectRowsByKey(clavesch);
        //gridChequesDevolver.GetSelectedFieldValues("ImporteTexto", GetSelectedFieldChequesDevCallback);
    }
}

