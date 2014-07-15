$(document).ready(function () {

    inicializarModal("Confirmacion", "Este cheque es anterior a la fecha de hoy. <br> Se registrara como debitado y se genera el debito bancario correspondiente.<br/> Esta seguro?", "Aceptar", "Cancelar");

    $("#btn_guardar").click(function () {
        //alert("1");
        var fecha = fechaDesdeTexto($("#fechaVencimiento").val());
        if (fecha <= hoy() && $("#formPrincipal").valid()) {
            $('#myModal').modal('show');
        } else {
            callBackAceptar();
        }
    });

});

function callBackAceptar() {
    $("#formPrincipal").submit();
}