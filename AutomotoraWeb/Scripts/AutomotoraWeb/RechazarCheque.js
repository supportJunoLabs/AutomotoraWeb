function chequeSelected(s, e) {
    //alert("hola");
    //var g = gridCheques.GetGridView();  //obtener referencia a la grilla
    //var valor = g.GetRowKey(g.GetFocusedRowIndex()); //obtener el valor elegido
    var valor = s.GetRowKey(e.visibleIndex);
    //alert(valor);
    $("#codigoCheque").val(valor);
    $("#abtn_verCheque").prop("href", "/Cheques/ConsultaCheque/" + valor);
    g.GetRowValues(g.GetFocusedRowIndex(), 'DescripcionAdicionalDestino', MostrarDestinoAnt);
}

function MostrarDestinoAnt(value) {
    //alert(value);
    $("#destinoAnt").html(value);
}

//$('#btn_VerCheque').click(function () {
//    var selectedID = $('#codigoCheque').val();
//    if (selectedID) {
//        window.location = "/Cheques/ConsultaCheque/" + selectedID;
//    }
//});
