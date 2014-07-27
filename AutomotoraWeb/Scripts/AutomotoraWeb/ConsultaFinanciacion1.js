$(document).ready(function () {
    var selectedCli = $("#ddlClientes").val();
    if (selectedCli) {
        $("#abtn_verCliente").prop("href", "/Clientes/details/" + selectedCli);
    } else {
        $("#abtn_verCliente").prop("href", "#");
    }
});

$('#ddlClientes').change(function () {
    /* Get the selected value of dropdownlist */
    var selectedCli = $(this).val();
    $("#abtn_verCliente").prop("href", "/Clientes/details/" + selectedCli);
    $('#divVentasCliente').html('');
    $('#divDetalleFinanciacion').html('');
    var destino='/ConsultasFin/VentasFinCliente/';
    $.ajax({
        cache: false,
        type: "GET",
        url: destino,
        data: { "idCliente": selectedCli },
        success: function (data) {
            $('#divVentasCliente').html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            general_showErrorPopup(xhr, ajaxOptions, thrownError, destino);
        },
        beforeSend: showLoading,
        complete: hideLoading
    });
});



$('#btn_refresh').click(function () {
    var selectedCli = $("#ddlClientes").val();
    var selectedVenta = $("#codigoVenta").val();
    if (selectedVenta) {
        window.location = "/ConsultasFin/ConsultaFinanciacion/" + selectedVenta;
        return;
    }
    if (selectedCli) {
        window.location = "/ConsultasFin/ConsultaValesCliente/" + selectedCli;
        return;
    }

});



