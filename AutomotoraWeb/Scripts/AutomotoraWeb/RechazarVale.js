function valeSelected(s, e) {
    //alert("hola");
    var g = gridVales.GetGridView();  //obtener referencia a la grilla
    var valor = g.GetRowKey(g.GetFocusedRowIndex()); //obtener el valor elegido
    //alert(valor);
    $("#codigoVale").val(valor);
}