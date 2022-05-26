const API_URL = "https://it-junior.grupomarves.net:57996/";

const btnLogin = $("#btnLogin");
const txtUsuario = $("#txtUser");
const txtPass = $("#txtPass");
/*const checkAdmin = $("#chekToogleAdmn");*/

/*checkAdmin.on("change", () => {


    txtPass.prop("disabled",
        checkAdmin.prop('checked') ? false : true
    );



});*/

btnLogin.on("click", (e) => {

    let user = txtUsuario.val();
    let pass = txtPass.val();

    if (!user || user == "") {

        alert("Ingrese su usuario por favor.");

        return;
    }

    if (!pass || pass == "") {
        alert("Ingrese contraseña por favor.");
    }

    //Si es checked, entoces el logeo es admin
    /*if (checkAdmin.is(':checked') && !user) {

        alert("Ingrese solamente digitos");
        return;
    }*/

    /*    loginUser(user, checkAdmin.is(':checked') ? pass : user);*/
    loginUser(user, pass);




})

const loginUser = (user, pass) => {

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
            xhr.setRequestHeader("Authorization", `Basic ${btoa(user + ":" + pass)}`);

        },
        success: successPromiseLog,
        error: failPromiseLog,
        complete: completePromese
    });

}

const KEY_TOKEN_NAME = "SESSIONTOKEN";

const completePromese = (xhr, textStatus) => {

    if (xhr.status == 200) {


        window.location.href = "./Default.aspx"

        //Cuando no es un OK
    }

}

//manejo del token
const successPromiseLog = (data, textStatus, xhr) => {


    if (xhr.status == 200) {


        //if (localStorage.getItem("sessionToken"))
        localStorage.setItem(KEY_TOKEN_NAME, data.token);
        localStorage.setItem("LevelUser", data.levelUser);
        localStorage.setItem("LevelName", data.levelName);

        //Cuando no es un OK
    } else {
        alert(textStatus)
    }
}

const failPromiseLog = (xhr, textStatus) => {
    localStorage.removeItem(KEY_TOKEN_NAME);
    localStorage.removeItem("LevelUser");
    localStorage.removeItem("LevelName");
    if (xhr)
        alert(xhr.responseText ? xhr.responseText:
            (xhr.status == 401 ? "Credenciales inválidas" : "Error al enviar la solicitud al servidor.")
        );

}

