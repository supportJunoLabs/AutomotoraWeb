 var actualMaritalStatus = $("#ddlMaritalStatus option:selected").val()
 _showHideSpouseBlock(actualMaritalStatus);

 var ecivilConulta = $("#codigoECivilConsulta").val()
 _showHideSpouseBlock(ecivilConulta);

$("#ddlMaritalStatus").change(function () {
    var optionSelected = $(this).val();
    _showHideSpouseBlock(optionSelected);
});

function _showHideSpouseBlock(optionSelected) {
    //alert(optionSelected);
    $.getJSON("/Clientes/mostrarDatosConyuge", { codEcivil: optionSelected},
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
