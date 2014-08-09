function ventaSelected(s, e) {
    //var g = gridVentas.GetGridView();  //obtener referencia a la grilla
    //var valor = g.GetRowKey(g.GetFocusedRowIndex()); //obtener el valor elegido
    var valor = s.GetRowKey(e.visibleIndex);
    $("#codigoVenta").val(valor);
    //alert("hola");
    actualizarVenta();
}


function actualizarVenta() {
    var selectedVenta = $("#codigoVenta").val();
    //alert(selectedVenta);
    $("#btn_imprimir").prop("href", "/ConsultasFin/ReportFinanciacion/" + selectedVenta);
    $('#divDetalleFinanciacion').html('');
    var destino = '/ConsultasFin/DetallesFinanciacion/'
    $.ajax({
        cache: false,
        type: "GET",
        url: destino,
        data: { "idVenta": selectedVenta },
        success: function (data) {
            $('#divDetalleFinanciacion').html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            general_showErrorPopup(xhr, ajaxOptions, thrownError, destino );
            //alert('Error al traer los datos.');
        },
        beforeSend: showLoading,
        complete: hideLoading
    });
}