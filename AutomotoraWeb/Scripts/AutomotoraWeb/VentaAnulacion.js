$(document).ready(function () {

    inicializarModal("Confirmacion", "Confirma la ANULACION de la venta?", "Aceptar", "Cancelar");
    $("#btnAnularVenta").live('click', function () { //antes de confirmar, traer lista de cheques dev seleccionados
         gridChequesDevolver.GetSelectedFieldValues("Codigo", ObtenerCodigosConfirmarCallBack);
     });
     
});

//obtiene lista de cheques dev selecciondos y lo pone en  chequesDevolverIds y pide confirmacion para hacer el submit
function ObtenerCodigosConfirmarCallBack(values) {
    //alert("hola");
    var selectedIDs;
    selectedIDs = "";
    for (var index = 0; index < values.length; index++) {
        selectedIDs += values[index] + ",";
    }
    //alert(selectedIDs);
    $("#chequesDevolverIds").val(selectedIDs);
    $('#myModal').modal('show');
}

function callBackAceptar() {
    $('form#formPrincipal').submit();
}


//Al seleccionar o des-selecconar un cheque devolver de la grilla, pasarlo a la lista de seleccionados. 
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

//al cargar la pagina, marcar como seleccionados los cheques que vienen en la lista (inicialmente todos)
function ChequesDevInit() {
    //alert($("#chequesDevolverIds").val());
    if ($("#chequesDevolverIds").val()) {
        var clavesch = $("#chequesDevolverIds").val().split(",");
        gridChequesDevolver.SelectRowsByKey(clavesch);
        //gridChequesDevolver.GetSelectedFieldValues("ImporteTexto", GetSelectedFieldChequesDevCallback);
    }
}

