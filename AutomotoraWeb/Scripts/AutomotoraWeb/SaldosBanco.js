$(document).ready(function () {
    $("#btn_actualizar").click(function () {
        $("#accion").val("ACTUALIZAR");
        $('form#formPrincipal').submit();
    });

    $("#btn_imprimir").click(function () {
        $("#accion").val("IMPRIMIR");
        $('form#formPrincipal').submit();
    });

});
