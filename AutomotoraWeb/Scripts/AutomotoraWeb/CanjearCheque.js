function chequeSelected(s, e) {
    //alert("hola");
    //var g = gridCheques.GetGridView();  //obtener referencia a la grilla
    //var valor = g.GetRowKey(g.GetFocusedRowIndex()); //obtener el valor elegido
    //alert(valor);
    var valor = s.GetRowKey(e.visibleIndex);
    $("#codigoCheque").val(valor);
    $("#abtn_verCheque").prop("href", "/Cheques/ConsultaCheque/" + valor);
}