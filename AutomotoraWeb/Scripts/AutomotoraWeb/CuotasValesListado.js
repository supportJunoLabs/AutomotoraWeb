$(document).ready(function () {
    var filtrarFinancista = $("#cb_filtrarFinancista").prop('checked')
    _showHideFinancista(filtrarFinancista);

});


$("#cb_filtrarFinancista").change(function () {
    var optionSelected = $(this).prop('checked')
    _showHideFinancista(optionSelected);
});


function _showHideFinancista(filtrarFinancista) {
    if (filtrarFinancista == undefined || filtrarFinancista == false) {
        $("#financistas").hide();
    } else {
        $("#financistas").show();
    }
}