
$(document).ready(function () {

    $('#deleteFotoBtn').click(function () {
        //alert("borrar foto");
        $.ajax({
            type: "POST",
            url: '/Vendedores/DeleteFoto',
            data: $("#camposesion").val(),
            processData: false,
            success: function (data) {
                    $("#imgPhoto").attr("src", "../../Content/Images/ExampleImage.png");
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("Error al desasociar foto");
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

        $.ajax({
            type: "POST",
            url: '/Vendedores/Upload',
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