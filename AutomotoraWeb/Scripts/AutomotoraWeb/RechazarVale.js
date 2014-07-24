function valeSelected(s, e) {
    //alert("hola");
    var g = gridVales.GetGridView();  //obtener referencia a la grilla
    var valor = g.GetRowKey(g.GetFocusedRowIndex()); //obtener el valor elegido
    //alert(valor);
    $("#codigoVale").val(valor);
    $("#abtn_verVale").prop("href", "/Vales/ConsultaVale/" + valor);
    g.GetRowValues(g.GetFocusedRowIndex(), 'InfoMovDescuento', MostrarDestinoAnt);
}

function MostrarDestinoAnt(value) {
    //alert(value);
    $("#destinoAnt").html(value);
}
