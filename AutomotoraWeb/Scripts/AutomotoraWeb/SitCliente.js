$('#ddlClientes').change(function () {

    /* Get the selected value of dropdownlist */
    var selectedID = $(this).val();

    //alert(selectedID);

    $.ajax({
        cache: false,
        type: "GET",
        url:'/ConsultasFin/SitClientePartial/',
        data: { "idCliente": selectedID },
    success: function (data)
    {
        $('#divListado').html('');
        $('#divListado').html(data);
    },
    error: function (xhr, ajaxOptions, thrownError)
    {
        alert('Error al traer los datos.');                    
    }
});

   

});