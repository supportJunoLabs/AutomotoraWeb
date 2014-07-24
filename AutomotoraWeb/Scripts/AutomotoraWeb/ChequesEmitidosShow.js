//$(document).ready(function () {
//    $("#ddlCuentas").attr("data-val-required", "La cuenta es requerida");
//}
//   );

$('#ddlCuentas').change(function () {
    /* Get the selected value of dropdownlist */
    var selectedID = $(this).val();
    $("#abtn_agregar").prop("href", "/ChEmitidos/Create/" + selectedID);
    //alert(selectedID);
    $.ajax({
        cache: false,
        type: "GET",
        url: '/ChEmitidos/CuentaSelected/',
        data: { "idCuenta": selectedID },
        success: function (data) {
            $('#divGrilla').html('');
            $('#divGrilla').html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert('Error al traer los datos.');
        },
        beforeSend: showLoading,
        complete: hideLoading
    });
});
