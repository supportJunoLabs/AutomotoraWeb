$('#fechaRenovacion').change(function () {

    var idVale = $('#Codigo').val();
    var fechaRenov = $('#fechaRenovacion').val();

    var destino = '/vales/FechaRenovacionModif/';
    var datos = { "IdVale": idVale, "FechaRenov": fechaRenov };

    $('#divInteresesSugeridos').html('');
    $('#divTotalSugerido').html('');

    $.ajax({
        url: destino,
        type: 'POST',
        datatype: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(datos),
        success: function (data) {
            $('#divInteresesSugeridos').html(data.InteresesTexto);
            $('#divTotalSugerido').html(data.TotalTexto);

            $('#ddlTransaccionImporteMoneda').val(data.CodMoneda);
            $('#txTransaccionImporteMonto').val(data.Total);

        },
        error: function (xhr, ajaxOptions, thrownError) {
            general_showErrorPopup(xhr, ajaxOptions, thrownError, destino);
        },
        beforeSend: showLoading,
        complete: hideLoading
    });
}
   );