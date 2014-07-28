//$(document).ready(function () {
//    $("#ddlCuentas").attr("data-val-required", "La cuenta es requerida");
//}
//   );

$('#ddlCuentas').change(function () {
    /* Get the selected value of dropdownlist */
    var selectedID = $(this).val();
    $("#abtn_agregar").prop("href", "/ChEmitidos/Create/" + selectedID);
    //alert(selectedID);
    var destino='/ChEmitidos/CuentaSelected/';
    $.ajax({
        cache: false,
        type: "GET",
        url: destino,
        data: { "idCuenta": selectedID },
        success: function (data) {
            $('#divGrilla').html('');
            $('#divGrilla').html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            general_showErrorPopup(xhr, ajaxOptions, thrownError, destino);
        },
        beforeSend: showLoading,
        complete: hideLoading
    });
});
