$(document).ready(function () {
    $("#btn_transferir").click(function () {
        gridCheques.GetSelectedFieldValues("Codigo", CodigosSeleccionadosCallBack);
    });


});

function CodigosSeleccionadosCallBack(values) {
    //alert("hola");
    var selectedIDs;
    selectedIDs = "";
    for (var index = 0; index < values.length; index++) {
        selectedIDs += values[index] + ",";
    }
    //alert(selectedIDs);
    $("#chequesIds").val(selectedIDs);
    //alert($("#chequesIds").val());
    $('form#formPrincipal').submit();
}

function SelectionChanged(s, e) {
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