$(document).ready(function () {

    $(".boton-conciliar").live("click", function () {

        var id = _getId($(this));
        var destino = '/Movimientos/Conciliar/';
        $.ajax({
            url: destino,
            //url: '@(Url.Action("Conciliar", "Movimientos"))',
            type: 'POST',
            async: true,
            data: 'id=' + id,
            success: function (data) {
                if (data.Result == "OK") {
                    grilla_movs.Refresh();
                }
                else {
                    //Esto no es error de javascript sino que no pudo conciliar porque ya estaba conciliado.
                    general_showAvisoPopup(data.ErrorMessage);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                general_showErrorPopup(xhr, ajaxOptions, thrownError, destino);
            },
            beforeSend: showLoading,
            complete: hideLoading
        });
    });

    /* ============ */

    $(".boton-desconciliar").live("click", function () {

        var id = _getId($(this));
        var destino = '/Movimientos/Desconciliar/';
        $.ajax({
            url: destino,
            //url: '@(Url.Action("Desconciliar", "Movimientos"))',
            type: 'POST',
            async: true,
            data: 'id=' + id,
            success: function (data) {
                if (data.Result == "OK") {
                    grilla_movs.Refresh();
                }
                else {
                    //Esto no es error de javascript sino que no pudo conciliar porque ya estaba conciliado.
                    general_showAvisoPopup(data.ErrorMessage);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                general_showErrorPopup(xhr, ajaxOptions, thrownError, destino);
            },
            beforeSend: showLoading,
            complete: hideLoading
        });
    });

});

/* ================================================== */

function _getId(elem) {
    var str = elem.attr("id");
    return str.split("_")[1];
}
