function chequeSelected(s, e) {
    //alert("hola");
    var g = gridCheques.GetGridView();  //obtener referencia a la grilla
    var valor = g.GetRowKey(g.GetFocusedRowIndex()); //obtener el valor elegido
    //alert(valor);
    $("#codigoCheque").val(valor);
}