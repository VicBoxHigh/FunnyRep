
const btnLogin = $("#btnLogin");
const txtNumEmpleado = $("#txtUser");
const txtPass = $("#txtPass");
const checkAdmin = $("#chekToogleAdmn");

checkAdmin.on("change", () => {


    txtPass.prop("disabled",
        checkAdmin.prop('checked') ? false : true
    );



});

btnLogin.on("click", (e) => {

    let numEmpleado = txtNumEmpleado.val();
    let pass = txtPass.val();

    if (txtNumEmpleado.val() == "") {

        alert("Ingrese su número de empleado por favor");

        return;
    }


    if (!isNaN(txtNumEmpleado.val())) {

        alert("Ingrese solamente digitos");
        return;
    }


    if (checkAdmin.is(':checked'))
        loginAdmin(numEmpleado, pass);
    else
        loginUser(numEmpleado);




})

const loginUser = (numEmpleado) => {

    $.ajax({
        type: "POST",
        url: "http://dumyhost.com:8003/api/User/LoginUser/",
        contentType: "application/json",
        crossDomain: true,
        datatype: "json",
        headers: corsObjs.headers,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", `Basic ${numEmpleado}:${numEmpleado}`)
        },
        success: successPromiseLog,
        error: failPromiseLog,
    });

}
const corsObjs = {
    headers: {
        "Access-Control-Allow-Origin": "*",
        "Access-Control-Allow-Credentials": "true",
        "Access-Control-Allow-Methods": "GET,HEAD,OPTIONS,POST,PUT",
        "Access-Control-Allow-Headers": "Access-Control-Allow-Headers, Origin,Accept, X-Requested-With, Content-Type, Access-Control-Request-Method, Access-Control-Request-Headers"
    },
}
const loginAdmin = (numEmpleado, pass) => {

    $.ajax({
        type: "GET",
        url: "http://localhost:57995/api/User/",
        contentType: "application/json",
        crossDomain: true,
        datatype: "json",
        headers: {
            'Access-Control-Allow-Origin': '*',
            "Access-Control-Allow-Credentials": "true",
            "Access-Control-Allow-Methods": "GET,HEAD,OPTIONS,POST,PUT",
            "Access-Control-Allow-Headers": "Access-Control-Allow-Headers, Origin,Accept, X-Requested-With, Content-Type, Access-Control-Request-Method, Access-Control-Request-Headers"
        },

        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", `Basic ${btoa(numEmpleado + ":" + pass)}`)
        },
        success: successPromiseLog,
        error: failPromiseLog,
    });


};

const KEY_TOKEN_NAME = "SESSIONTOKEN";

//manejo del token
const successPromiseLog = (data, textStatus, xhr) => {


    if (data.token) {


        //if (localStorage.getItem("sessionToken"))
        localStorage.setItem(KEY_TOKEN_NAME, data.token);
        window.location.href = "/Default.aspx"

    } else {
        alert("Algo salió mal.")
    }
}

const failPromiseLog = (xhr, textStatus) => {
    localStorage.removeItem(KEY_TOKEN_NAME)
    alert("Falló la solicitud al servidor." + xhr.responseText)

}

