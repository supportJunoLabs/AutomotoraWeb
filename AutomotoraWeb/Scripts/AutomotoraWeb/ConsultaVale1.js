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
        $('#divValesCliente').html('');
        $('#divDetalleVale').html('');
        $.ajax({
            cache: false,
            type: "GET",
            url: '/Vales/ValesCliente/',
            data: { "idCliente": selectedCli },
            success: function (data) {
                $('#divValesCliente').html(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {

                alert('Error al traer los datos.');
            },
            beforeSend: showLoading,
            complete: hideLoading
        });
    });



$('#btn_refreshVales').click(function () {
    var selectedCli = $("#ddlClientes").val();
    var selectedVal = $("#ddlVales").val();
    if (selectedVal) {
        window.location = "/Vales/ConsultaVale/" + selectedVal;
        return;
    }
    if (selectedCli) {
        window.location = "/Vales/ConsultaValesCliente/" + selectedCli;
        return;
    }

});



 