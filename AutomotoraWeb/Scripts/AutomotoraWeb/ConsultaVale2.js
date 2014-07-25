
$('#ddlVales').change(function () {
    //alert("hola");
    var selectedVal = $(this).val();
    $("#btn_imprimir").prop("href", "/Vales/ReportVale/" + selectedVal);
    //alert(selectedID);
    $.ajax({
        cache: false,
        type: "GET",
        url: '/Vales/DetallesVale/',
        data: { "idVale": selectedVal },
        success: function (data) {
            $('#divDetalleVale').html('');
            $('#divDetalleVale').html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {

            alert('Error al traer los datos.');
        },
        beforeSend: showLoading,
        complete: hideLoading
    });
});