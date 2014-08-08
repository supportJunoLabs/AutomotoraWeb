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
        $('#divValeRenovacion').html('');
        var destino = '/Vales/ValesRenovablesCliente/'
        $.ajax({
            cache: false,
            type: "GET",
            url: destino,
            data: { "idCliente": selectedCli },
            success: function (data) {
                $('#divValesCliente').html(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                general_showErrorPopup(xhr, ajaxOptions, thrownError, destino);
            },
            beforeSend: showLoading,
            complete: hideLoading
        });
    });



$('#btn_refreshVales').click(function () {
    var selectedCli = $("#ddlClientes").val();
    var selectedVal = $("#ddlVales").val();
    if (selectedVal) {
        window.location = "/Vales/Renovar/" + selectedVal;
        return;
    }
    if (selectedCli) {
        window.location = "/Vales/RenovarCliente/" + selectedCli;
        return;
    }

});



 