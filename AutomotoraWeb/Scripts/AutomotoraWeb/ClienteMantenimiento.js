$(document).ready(function () {

    
    var ecivilConulta = $("#codigoECivilConsulta").val()
    if (undefined != ecivilConulta) {
        _showHideSpouseBlock(ecivilConulta);
        //alert("hola 2")
    }

    var actualMaritalStatus = $("#ddlMaritalStatus option:selected").val()
    if (undefined != actualMaritalStatus) {
        //alert("hola 3")
        _showHideSpouseBlock(actualMaritalStatus);
    }

    
});

$("#ddlMaritalStatus").change(function () {
    var optionSelected = $(this).val();
    _showHideSpouseBlock(optionSelected);
});

function _showHideSpouseBlock(optionSelected) {
    var destino = '/Clientes/mostrarDatosConyuge';
    $.ajax({
        cache: false,
        type: "GET",
        url: destino,
        data: { "codEcivil": optionSelected },
        success: function (data) {
            if (!data.mostrar) {
                $("#spouseBlock").hide();
            } else {
                $("#spouseBlock").show();
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            general_showErrorPopup(xhr, ajaxOptions, thrownError, destino);
        },
        beforeSend: showLoading,
        complete: hideLoading
    });


    //$.getJSON("/Clientes/mostrarDatosConyuge", { codEcivil: optionSelected },
    //    function (data) {
    //        if (!data.mostrar) {
    //            $("#spouseBlock").hide();
    //        } else {
    //            $("#spouseBlock").show();
    //        }
    //    },
    //    function (xhr, ajaxOptions, thrownError) {
    //        general_showErrorPopup(xhr, ajaxOptions, thrownError, destino);
    //    }
    //);
}
