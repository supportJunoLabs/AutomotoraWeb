$(document).ready(function () {
    var inputType = $('#cb_reservado').attr('type');
    //alert(inputType);
    var pedidoReservado = true;
    if (inputType.toLowerCase() == "checkbox") {
        pedidoReservado = $("#cb_reservado").prop('checked')
    } else {
        pedidoReservado = ( $("#cb_reservado").val().toLocaleLowerCase() == "true");
    }
    _showHideReserva(pedidoReservado)

});

$("#cb_reservado").change(function () {
    var optionSelected = $(this).prop('checked')
    _showHideReserva(optionSelected);
    //alert("hola");
});

function _showHideReserva(reservado) {
    //alert(reservado);
    if (reservado == undefined || reservado == false) {
        $("#div_reserva").hide();
        //alert(1);
    } else {
        $("#div_reserva").show();
        //alert(2);
    }
}
