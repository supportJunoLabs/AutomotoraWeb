
$('#ddlVales').change(function () {
    //alert("hola");
    var selectedVal = $(this).val();
    $("#btn_imprimir").prop("href", "/Vales/ReportVale/" + selectedVal);
    $('#divDetalleVale').html('');
    $('#divValesAnteriores').html('');
    //alert(selectedID);
    var destino = '/Vales/DetallesVale/';
    $.ajax({
        cache: false,
        type: "GET",
        url: destino,
        data: { "idVale": selectedVal },
        success: function (data) {
            $('#divDetalleVale').html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            general_showErrorPopup(xhr, ajaxOptions, thrownError, destino);
        },
        beforeSend: showLoading,
        complete: hideLoading
    });

    destino = '/Vales/DetallesValesAnteriores/';
    $.ajax({
        cache: false,
        type: "GET",
        url: destino,
        data: { "idVale": selectedVal },
        success: function (data) {
            $('#divValesAnteriores').html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            general_showErrorPopup(xhr, ajaxOptions, thrownError, destino);
        },
        beforeSend: showLoading,
        complete: hideLoading
    });
});