
$('#ddlVales').change(function () {
    //alert("hola");
    var selectedVal = $(this).val();
    $("#abtn_verVale").prop("href", "/Vales/ConsultaVale/" + selectedVal);
    $('#divValeRenovacion').html('');
    //alert(selectedID);
    var destino = '/Vales/DetallesValeRenovacion/';
    $.ajax({
        cache: false,
        type: "GET",
        url: destino,
        data: { "idVale": selectedVal },
        success: function (data) {
            $('#divValeRenovacion').html(data);
            reajustarControles();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            general_showErrorPopup(xhr, ajaxOptions, thrownError, destino);
        },
        beforeSend: showLoading,
        complete: hideLoading
    });
});