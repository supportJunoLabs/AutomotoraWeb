$('#ddlClientes').change(function () {

    /* Get the selected value of dropdownlist */
    var selectedID = $(this).val();

    //alert(selectedID);
    $("#abtn_VerCliente").prop("href", "/Clientes/details/" + selectedID);
    var destino = '/ConsultasFin/ListSitClientePartial/';
    $.ajax({
        cache: false,
        type: "GET",
        url: destino,
        data: { "idCliente": selectedID },
        success: function (data) {
            $('#divListado').html('');
            $('#divListado').html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            general_showErrorPopup(xhr, ajaxOptions, thrownError, destino);
        },
        beforeSend: showLoading,
        complete: hideLoading
    });

});

//$('#btn_VerCliente').click(function () {
//    var selectedID = $('#ddlClientes').val();
//    if (selectedID) {
//        window.location = "/Clientes/details/" + selectedID;
//    }
//});
   
