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

//Se comenta este. La idea es usuar el siguiente, al que hay indicarle de donde viene el error para tener mas informacion
//function general_showErrorPopup(jqXHR, textStatus, errorThrown) {
//    alert("Se ha producido un error: " + textStatus + " | " + errorThrown);
//}

function general_showErrorPopup(jqXHR, textStatus, errorThrown, metodoInvocado) {
    alert("Se ha producido un error: " + textStatus + " | " + errorThrown + " | " + metodoInvocado);
}

function general_showAvisoPopup(message, usaralert) {
    if (usaralert) { //se puede pedir mostrar usando el alert basico
        alert(message);
    } else { //por defecto muestra el mensaje lindo de _modalConfirmation
        general_showAvisoPopup(message);
    }
}

function general_showAvisoPopup(message) {
    inicializarModal("Aviso", message, null, "OK");
    $('#myModal').modal('show');
}

function general_showErrorAction(idErrorContainerm, textStatus, errorThrown) {
    $("#" + idErrorContainerm).html("Error al intentar realizar la acción: " + textStatus + " | " + errorThrown);
}

function inicializarModal(titulo, contenido, textoAceptar, textoCancelar) {
    //alert(textoCancelar);
    if (titulo) {
        $("#myModalTitle").html(titulo);
    } else {
        $("#myModalTitle").html("");
    }
    if (contenido) {
        $("#myModalContent").html(contenido);
    } else {
        $("#myModalContent").html("");
    }
    if (textoAceptar){
        $("#myModalAcepta").html(textoAceptar);
        $("#myModalAcepta").on ("click", function(){
            $('#myModal').modal('hide');
            callBackAceptar();
        })
    } else {
        $("#myModalAcepta").hide();
    }
    if (textoCancelar) {
        $("#myModalCancela").html(textoCancelar);
    } else {
        $("#myModalCancela").hide();
    }
}