

const getRepsByUser = () => {

    let currentToken = localStorage.getItem(KEY_TOKEN_NAME);

    if (!currentToken) {
        alert("Inicie sesión primero")
        window.location.href = "./Login"
        return
    }

    $.ajax({
        type: "Get",
        url: API_URL + "api/Report",
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

        success: function (data, textStatus, xhr) {

            if (xhr.status != 200 && xhr.status != 204)
                alert(textStatus)

            if (data) {
                REPORTES_ASIGNADOS = data.reportesAsignados;
                REPORTES_NO_ASIGNADOS = data.reportesNoAsignados;

                /*localStorage.setItem(REPORTS_ASIGNADOS_NAME, JSON.stringify(data.reportesAsignados));
                localStorage.setItem(REPORTS_NO_ASIGNADOS_NAME, JSON.stringify(data.reportesNoAsignados));
                */mediatorTask();
            }
            //renderRepHeads(data.reportesAsignados, data.reportesNoAsignados)

        },
        complete: (xhr, textStatus) => {

        },
        error: function (xhr, textStatus) {

            alert("Error en la solicitud." + xhr.responseText);
        },
    });
}
 

const init = async () => {
    checkSessionLevel();
    await initNewReportView();
    getRepsByUser();

    ////fillReportTypesNewRep(selRepType_Dtl) //este no, se carga dinamico
    // initCam();
}

init();


