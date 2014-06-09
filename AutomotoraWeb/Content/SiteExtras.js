/* ================================================== */

function general_showPopup(data, idContainer, valueHeight, valueWidth) {
    $("#" + idContainer + "Form").html(data);
    $("#" + idContainer).dialog({
        height: valueHeight,
        width: valueWidth,
        modal: true
    });

    $('.calendarAW').datepicker({
        changeMonth: true,
        dateFormat: "dd/mm/yy",
        changeYear: true,
        showAnim: 'slideDown',
        yearRange: '1900:2100'
    });
}

function general_closePopup(idContainer) {
    $("#" + idContainer).dialog("close");
    $("#" + idContainer + "Form").html("");
}

function general_showErrorPopup(jqXHR, textStatus, errorThrown) {
    alert("Error al obtener datos para generar popup: " + textStatus + " | " + errorThrown);
}

function general_showErrorAction(idErrorContainerm, textStatus, errorThrown) {
    $("#" + idErrorContainerm).html("Error al intentar realizar la acción: " + textStatus + " | " + errorThrown);
}

/* ================================================== */