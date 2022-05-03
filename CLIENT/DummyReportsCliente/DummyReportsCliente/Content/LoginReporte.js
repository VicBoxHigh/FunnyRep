﻿const API_URL = "https://172.16.9.118:57996/";

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

    //Si es checked, entoces el logeo es admin
    if (checkAdmin.is(':checked') && !numEmpleado) {

        alert("Ingrese solamente digitos");
        return;
    }

    loginUser(numEmpleado, checkAdmin.is(':checked') ? pass : numEmpleado);





})

const loginUser = (numEmpleado, pass) => {

    $.ajax({
        type: "GET",
        url: API_URL + "api/User/",
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
            xhr.setRequestHeader("Authorization", `Basic ${btoa(numEmpleado + ":" + pass)}`);

        },
        success: successPromiseLog,
        error: failPromiseLog,
    });

}

const KEY_TOKEN_NAME = "SESSIONTOKEN";

//manejo del token
const successPromiseLog = (data, textStatus, xhr) => {


    if (xhr.status == 200) {


        //if (localStorage.getItem("sessionToken"))
        localStorage.setItem(KEY_TOKEN_NAME, data.token);
        localStorage.setItem("LevelUser", data.levelUser);
        window.location.href = "./Default.aspx"

        //Cuando no es un OK
    } else {
        alert(textStatus + "v")
    }
}

const failPromiseLog = (xhr, textStatus) => {
    localStorage.removeItem(KEY_TOKEN_NAME);
    localStorage.removeItem("LevelUser");
    if (xhr)
        alert(xhr.status == 401 ? "Credenciales inválidas" : xhr.statusText)

}

