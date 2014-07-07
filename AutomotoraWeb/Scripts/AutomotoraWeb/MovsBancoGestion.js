$(document).ready(function () {

    $(".boton-conciliar").live("click", function () {

        var id = _getId($(this));

        $.ajax({
            url: '/Movimientos/Conciliar/',
            //url: '@(Url.Action("Conciliar", "Movimientos"))',
            type: 'POST',
            async: true,
            data: 'id=' + id,
            success: function (data) {
                if (data.Result == "OK") {
                    grilla_movs.Refresh();
                }
                else {
                    general_showAvisoPopup(data.ErrorMessage);
                }
            },
            error: general_showErrorPopup,
            beforeSend: showLoading,
            complete: hideLoading
        });
    });

    /* ============ */

    $(".boton-desconciliar").live("click", function () {

        var id = _getId($(this));

        $.ajax({
            url: '/Movimientos/Desconciliar/',
            //url: '@(Url.Action("Desconciliar", "Movimientos"))',
            type: 'POST',
            async: true,
            data: 'id=' + id,
            success: function (data) {
                if (data.Result == "OK") {
                    grilla_movs.Refresh();
                }
                else {
                    general_showAvisoPopup(data.ErrorMessage);
                }
            },
            error: general_showErrorPopup,
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
