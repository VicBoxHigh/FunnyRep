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

    for (let currentUser in users) {//Genera cada row
        tbdy.append(
            ` 
                <tr>
                    <td data-th="ID "   >${users[currentUser].IdUser}
                    </td>
                    <td data-th="NumEmpleado" >
                        <input type="text" value="${users[currentUser].NumEmpleado}"/>
                    </td>
                    <td data-th="UserName" >
                        <input type="text" value="${users[currentUser].UserName}" />
                    </td>
                    <td data-th="Pass" >
                        <input type="text" value="${users[currentUser].Pass}"/>
                    </td>
                    <td data-th="Name" >
                        <input type="text" value="${users[currentUser].Name}" />
                    </td>
                    <td data-th="Enabled" >

                        <select>
                                <option value="1" ${users[currentUser].IsEnabled ? "selected" : ""} >Sí</option>
                                <option value="0" ${users[currentUser].IsEnabled ? "" : "selected"}>No</option>                               
                        </select>
                    </td>
                    <td data-th="Level" >
                        <select>
                            <option value="1" ${users[currentUser].AccessLevelName === "BASIC" ? "selected" : ""} > Básico</option>
                            <option value="2" ${users[currentUser].AccessLevelName === "ADMIN" ? "selected" : ""}> Agente</option>
                            <option value="3" ${users[currentUser].AccessLevelName === "AGENT" ? "selected" : ""}> Administrador</option>
                            <option value="4" ${users[currentUser].AccessLevelName === "TI" ? "selected" : ""}> TI</option>
                        </select>
                    </td>
                    <div class="tableRow-options">
                        <input type="button" val="Guardar"/>
                        <input type="button" val="Eliminar"/>
                    </div>
                </tr>
`
        );

    }
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