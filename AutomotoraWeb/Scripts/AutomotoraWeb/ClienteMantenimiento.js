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
    //alert(optionSelected);
    $.getJSON("/Clientes/mostrarDatosConyuge", { codEcivil: optionSelected },
        function (data) {
            //alert(data.mostrar);
            if (!data.mostrar) {
                //alert("1");
                $("#spouseBlock").hide();
            } else {
                //alert("2");
                $("#spouseBlock").show();
            }
        },
        function () {
            alert("Se produjo algun error en invocacion a metodo Json");
        }
    );
    //alert("final");
}
