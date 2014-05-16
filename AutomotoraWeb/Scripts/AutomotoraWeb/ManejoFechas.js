function hoy() {
    var today = new Date();
    return today;
}

function fechaTexto(fecha) {

    var dd = fecha.getDate();
    var mm = fecha.getMonth() + 1; //January is 0!
    var yyyy = fecha.getFullYear();

    if (dd < 10) { dd = '0' + dd; }
    if (mm < 10) { mm = '0' + mm; }
    var fechatxt = dd + '/' + mm + '/' + yyyy;
    return fechatxt;
}

function sumarMeses(fecha, meses) {
    //copy the date
    var dt = new Date(fecha);
    dt.setMonth(dt.getMonth() + meses);
    return dt;
}
