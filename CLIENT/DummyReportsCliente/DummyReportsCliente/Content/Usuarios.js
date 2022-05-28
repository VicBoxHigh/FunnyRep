const usersTable = $("#usersTable")


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
    /*   //getTypes
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
                            <option value="0" ${currentUser.AccessLevelName === "BASIC" ? "selected" : ""} > Básico</option>
                            <option value="1" ${currentUser.AccessLevelName === "ADMIN" ? "selected" : ""}> Agente</option>
                            <option value="2" ${currentUser.AccessLevelName === "AGENT" ? "selected" : ""}> Administrador</option>
                            <option value="3" ${currentUser.AccessLevelName === "TI" ? "selected" : ""}> TI</option>
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


        let newTd = $(`<td></td>`);
        newTd.append(btnDeleteUser);
        newTd.append(btnSaveChanges);

        newTr.append(newTd);
        btnSaveChanges.on("click", (e) => saveUserChanges(e,  extractUserInfo(e,currentUser,newTr) )   )
        btnDeleteUser.on("click", (e) => deleteUser(e, currentUser))

      /*  tbdy.children("tr:last").before(newTr)*/

        //row para nuevo se econtrará hasta arriba
        tbdy.append(newTr);
    }
}

const extractUserInfo = (event, oldUser, rowRef) => {

    let userUpdated = {};
    userUpdated.Iduser = oldUser.IdUser;
    userUpdated.NumEmpleado = rowRef.children(`td:nth-child(2)`).children("input[type='text']").val();;
    userUpdated.UserName = rowRef.children('td:nth-child(3)').children("input[type='text']").val();
    userUpdated.Pass = rowRef.children('td:nth-child(4)').children("input[type='text']").val();

    userUpdated.Name = rowRef.children('td:nth-child(5)').children("input[type='text']").val();
    userUpdated.IsEnabled = rowRef.children('td:nth-child(6)').children("select").val() === "1" ? true : false;//0 o 1
    userUpdated.AccessLevel = rowRef.children('td:nth-child(7)').children("select").val();//tipo usuario 0 al 3

    return userUpdated

}

const saveUserChanges = (event, newUserInfo) => {

    let token = checkSession();
    let d = JSON.stringify(newUserInfo);

    $.ajax({
        type: "PUT",
        url: API_URL + `api/User?id=${newUserInfo.Iduser}`,
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
            if (xhr.responseText)
                alert(xhr.responseText);
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
        alert(ex.responseText)
    }

    fillUserstable(result.users)



}

init();