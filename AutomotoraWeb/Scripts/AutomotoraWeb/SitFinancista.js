$(document).ready(function () {
    var selectedID = $('#ddlFinancistas').val();
    if (selectedID) {
        $("#abtn_VerFinancista").prop("href", "/Financistas/details/" + selectedID);
    }
});

$('#ddlFinancistas').change(function () {

    /* Get the selected value of dropdownlist */
    var selectedID = $(this).val();
    $("#abtn_VerFinancista").prop("href", "/Financistas/details/" + selectedID);
    $('#divSituacion').html('');
    var destino = '/Financistas/ConsultaSituacionPartial/';
    $.ajax({
        cache: false,
        type: "GET",
        url: destino,
        data: { "id": selectedID },
        success: function (data) {
            $('#divSituacion').html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            general_showErrorPopup(xhr, ajaxOptions, thrownError, destino);
        },
        beforeSend: showLoading,
        complete: hideLoading
    });

});