const btnLogin = $("#btnLogin");
const txtNumEmpleado = $("#txtNumEmpleado");


btnLogin.on("click", (e) => {

    if (txtNumEmpleado.val() !== "") {

        alert("Ingrese su número de empleado por favor");
        return;
    }


    if (!isNaN(txtNumEmpleado.val())) {

        alert("Ingrese solamente digitos");
        return;
    }


    login(numEmpleado);


})

const login = (numEmpleado) => {




    $.ajax({
        type: "POST",
        url: "http://dumyhost.com:8003/api/Report/",
        contentType: "application/json",
        crossDomain: true,
        datatype: "json",
        data: dataStr,
        succes: function (data) {
            alert(data);
        },
        error: function (err) {
            alert(err);
        },
    })

}


