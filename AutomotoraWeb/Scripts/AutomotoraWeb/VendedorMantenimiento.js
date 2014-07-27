
$(document).ready(function () {

    $('#deleteFotoBtn').click(function () {
        //alert("borrar foto");
        var destino='/Vendedores/DeleteFoto';
        $.ajax({
            type: "POST",
            url: destino,
            data: $("#camposesion").val(),
            processData: false,
            success: function (data) {
                    $("#imgPhoto").attr("src", "../../Content/Images/ExampleImage.png");
            },
            error: function (jqXHR, textStatus, errorThrown) {
                general_showErrorPopup(xhr, ajaxOptions, thrownError, destino);
            },
            dataType: 'json',
            contentType: false,
            mimeType: 'multipart/form-data',

        });
    });

    $('#uploadFileBtn').click(function () {

        var formdata = new FormData(); //FormData object
        var fileInputFoto = $("#fileInputFoto");
        //Iterating through each files selected in fileInputFoto
        for (i = 0; i < fileInputFoto.prop('files').length; i++) {
            //Appending each file to FormData object
            formdata.append(fileInputFoto.prop('files')[i].name, fileInputFoto.prop('files')[i]);
        }

        formdata.append("idsesion", $("#camposesion").val());

        //alert($("#camposesion").val());
        var destino='/Vendedores/Upload';
        $.ajax({
            type: "POST",
            url: destino,
            data: formdata,
            processData: false,
            success: function (data) {
                //alert(data.fileName);
                if (data.fileName != null) {
                    $("#imgPhoto").attr("src", "../../Content/Images/tmp/" + data.fileName);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("Error al subir archivo");
                general_showErrorPopup(xhr, ajaxOptions, thrownError, destino);
            },
            dataType: 'json',
            contentType: false,
            mimeType: 'multipart/form-data',
            beforeSend: function (x) {
                if (x && x.overrideMimeType) {
                    x.overrideMimeType("multipart/form-data");
                }
            }
        });
    });

});