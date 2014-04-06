<html>
    <% Response.StatusCode = 404 %>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width" />
    <title>Error 404</title>
    <%: Styles.Render("~/Content/css") %>

    <body class="http-error">
    <div class="row-fluid">
        <div class="http-error">
            <h1>
                Oops!</h1>
            <p class="info">
                No se ha encontrado el recurso solicitad.
            </p>
            <p>
                <i class="icon-home"></i>
            </p>
            <a class="btn btn-default " href="/">Volver al inicio</a>
        </div>
    </div>
</body>
</html>

