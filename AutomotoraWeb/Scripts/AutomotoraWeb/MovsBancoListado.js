$(document).ready(function () {
    $("#btn_actualizar").click(function () {
        $("#accion").val("ACTUALIZAR");
        $('form#formPrincipal').submit();
    });

    $("#btn_imprimir").click(function () {
        $("#accion").val("IMPRIMIR");
        $('form#formPrincipal').submit();
    });

    if ($("#rb_todos").prop('checked') ){
        $("#siniGeneral").show();
        $("#sfinGeneral").show();
        $("#siniConciliado").hide();
        $("#sfinConciliado").hide();
    }

    if ($("#rb_conciliados").prop('checked')) {
        $("#siniGeneral").hide();
        $("#sfinGeneral").hide();
        $("#siniConciliado").show();
        $("#sfinConciliado").show();
    }

    if ($("#rb_noconciliados").prop('checked') ){
        $("#siniGeneral").hide();
        $("#sfinGeneral").hide();
        $("#siniConciliado").hide();
        $("#sfinConciliado").hide();
    }

});
