﻿const usersTable = $("#usersTable")


const getUsers = () => {


    let currentToken = localStorage.getItem(KEY_TOKEN_NAME);;
    if (!currentToken) {
        alert("Inicie sesión primero.")
        window.location.href = "./Login"

        return;
    }

    return $.ajax({

        type: "GET",
        url: API_URL + `/api/User/all`,
        contentType: "application/json",
        crossDomain: true,
        datatype: "json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", `${'Bearer ' + currentToken}`)
        },
        headers: {
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Credentials": "true",
            "Access-Control-Allow-Methods": "GET,HEAD,OPTIONS,POST,PUT",
            "Access-Control-Allow-Headers": "Access-Control-Allow-Headers, Origin,Accept, X-Requested-With, Content-Type, Access-Control-Request-Method, Access-Control-Request-Headers"
        },

    });
}

const fillUserstable = (users) => {
    let tbdy = usersTable.children("tbody");
    //let reportTypes = localStorage.getItem(REPORT_TYPE_NAME);
    /*   //etTypes
       if (!reportTypes) {
           try {
   
               reportTypes = getReporTypes(localStorage.getItem(KEY_TOKEN_NAME));
               //code 200?
           }
           catch (ex) {
   
   
               return;
           }
       }
   */

    for (let currentUserIndex in users) {//Genera cada row
        let currentUser = users[currentUserIndex];
        let newTr = $(
            ` 
                <tr>
                    <td data-th="ID: "   >${currentUser.IdUser}
                    </td>
                    <td data-th="NumEmpleado: " >
                        <input type="text" value="${currentUser.NumEmpleado}"/>
                    </td>
                    <td data-th="UserName: " >
                        <input type="text" value="${currentUser.UserName}" />
                    </td>
                    <td data-th="Pass: " >
                        <input type="text" value="${currentUser.Pass}"/>
                    </td>
                    <td data-th="Name: " >
                        <input type="text" value="${currentUser.Name}" />
                    </td>
                    <td data-th="Enabled: " >

                        <select>
                                <option value="1" ${currentUser.IsEnabled ? "selected" : ""} >Sí</option>
                                <option value="0" ${currentUser.IsEnabled ? "" : "selected"}>No</option>                               
                        </select>
                    </td>
                    <td data-th="Level: " >
                        <select>
                            <option value="1" ${currentUser.AccessLevelName === "BASIC" ? "selected" : ""} > Básico</option>
                            <option value="2" ${currentUser.AccessLevelName === "ADMIN" ? "selected" : ""}> Agente</option>
                            <option value="3" ${currentUser.AccessLevelName === "AGENT" ? "selected" : ""}> Administrador</option>
                            <option value="4" ${currentUser.AccessLevelName === "TI" ? "selected" : ""}> TI</option>
                        </select>
                    </td>


                </tr>
`
        );
        let btnSaveChanges = $(`
           
                <input type="button" value="Guardar" />
            
        `);

        let btnDeleteUser = $(`
           
                <input type="button" value="Elimnar" />
            
        `)

        //set events to btns

        btnSaveChanges.on("click", (e) => saveUserChanges(e, currentUser, extractUserInfo()))
        btnDeleteUser.on("click", (e) => deleteUser(e, currentUser))

        let newTd = $(`<td></td>`);
        newTd.append(btnDeleteUser);
        newTd.append(btnSaveChanges);

        newTr.append(newTd);


        tbdy.append(newTr)
    }
}

const extracUserInfo = (event, oldUser, rowRef) => {

    let userUpdated = {};
    userUpdated.Iduser = oldUser.IdUser;
    userUpdated.NumEmpleado = rowRef.children(`td:nth-child(1)`).val();;
    userUpdated.UserName = rowRef.children('td:nth-child(2)').val();
    userUpdated.Pass = rowRef.children('td:nth-child(3)').val();

    userUpdated.Name = rowRef.children('td:nth-child(4)').val();
    userUpdated.AccessLevel = rowRef.children('td:nth-child(5)').children("select")[0]?.val();//0 o 1
    userUpdated.AccessLevel = rowRef.children('td:nth-child(6)').children("select")[0]?.val();//tipo usuario 0 al 3

    return userUpdated

}

const saveUserChanges = (event, newUserInfo) => {

    let token = checkSession();
    let d = JSON.stringify(newUserInfo);

    $.ajax({
        type: "PUT",
        url: API_URL + "api/User",
        contentType: "application/json",
        crossDomain: true,
        datatype: "json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", `${'Bearer ' + token}`)
        },
        data: d,
        headers: {
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Credentials": "true",
            "Access-Control-Allow-Methods": "GET,HEAD,OPTIONS,POST,PUT",
            "Access-Control-Allow-Headers": "Access-Control-Allow-Headers, Origin,Accept, X-Requested-With, Content-Type, Access-Control-Request-Method, Access-Control-Request-Headers"
        },
        success: (daa, xhr, statusTtext) => {

        },
        complete: (xhr, statusText) => {
            if (xhr.status == 200) window.location.href = "./Usuarios";
            else alert(xhr.responseText);

        },
        error: (xhr, statusText) => {

        }


    });
}

const deleteUser = (event, userInfo) => {

    let token = checkSession();

    let d = JSON.stringify(userInfo.Iduser);

    $.ajax({
        type: "DELETE",
        url: API_URL + "api/User",
        contentType: "application/json",
        crossDomain: true,
        datatype: "json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", `${'Bearer ' + token}`)
        },
        data: d,
        headers: {
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Credentials": "true",
            "Access-Control-Allow-Methods": "GET,HEAD,OPTIONS,POST,PUT",
            "Access-Control-Allow-Headers": "Access-Control-Allow-Headers, Origin,Accept, X-Requested-With, Content-Type, Access-Control-Request-Method, Access-Control-Request-Headers"
        },
        success: (daa, xhr, statusTtext) => {
            //alert("a")
        },
        complete: (xhr, statusText) => {

            if (xhr.status == 200) window.location.href = "./Usuarios";
            else alert(xhr.responseText);

        },
        error: (xhr, statusText) => {
            //alert("a")
        }


    });

}

const init = async () => {
    let result
    try {
        let taskLoad = getUsers();
        result = await taskLoad;
    }
    catch (ex) {
        alert(JSON.stringify(ex) + ex.responseJSON)
    }

    fillUserstable(result.users)



}

init();