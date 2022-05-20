
const API_URL = "https://172.16.9.118:57996/";
const containerNewRep = $("#cntNewRep");
const containerRepDtl = $("#cntRepDtl");

const btnToogleNewRep = $("#btnTogleNewRep")

const repHeadsContainer = $("#cntRepHeads");

const txtRepDtlUserInput = $("#txtRepDtlUserInput")
const btnSendRepDtlUpdate = $("#btnSendRepDtlUpdate")

const selStatusRep = $("#selStat");
const btnSaveStatus = $("#btnSaveStatus");

const selRepType_Dtl = $("#selRepType_Dtl");




btnToogleNewRep.on("click", () => {

    if (containerNewRep.hasClass("no-render")) {

        containerNewRep.removeClass("no-render");

        containerRepDtl.addClass("no-render")
        btnToogleNewRep.val("Detalle")
        initCam();
        initNewReportView();
    }
    else {

        containerNewRep.addClass("no-render");

        containerRepDtl.removeClass("no-render")

        btnToogleNewRep.val("Nuevo Reporte");
        stopCam();

    }

});


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

            if (data)
                renderRepHeads(data.reportesAsignados, data.reportesNoAsignados)

        },
        error: function (xhr, textStatus) {
            alert("Error en la solicitud" + JSON.stringify(xhr));
        },
    });
}


const renderRepHeads = (repHeadAsignados, repHeadsNoAsignados) => {

    repHeadsContainer.children().remove();
    for (let currentRep in repHeadAsignados) {
        repHeadsContainer.append(
            generateRepHead(repHeadAsignados[currentRep])
        )
    }
    for (let currentRep in repHeadsNoAsignados) {
        repHeadsContainer.append(
            generateRepHead(repHeadsNoAsignados[currentRep])
        )
    }


}

//el callback para cuando se de click en una de las tarjetas Head de un reporte
//event es la entidad que regresa el head al darle clic, sirve para invocar
const clickReportHead = async (event, individualRepHead) => {

    clearReportDtl()//limpia el Head expanded y las entries
    txtRepDtlUserInput.val("");
    reFillReportDtl(individualRepHead, event.target);//llena el head expanded


    let taskGetEntries = getRepDtlEntries(individualRepHead);

    //do animation loading?
    /*  {
  
      }*/
    let data = null;
    try {
        data = await taskGetEntries;//unvelop the entries from the promise

    } catch (ex) {

        alert("Error al solicitar el detalle.")
    }

    //OK
    if (taskGetEntries.status == 200) {
        individualRepHead.ReportUpdates = data.reportDtlEntries;//asigna las DtlEntries al header del reporte
        reFillReportDtlEntries(individualRepHead);//manda el objeto de dats de Reporte junto con las entries para que sea renderizado;


    }

    btnSendRepDtlUpdate.off("click");
    btnSendRepDtlUpdate.on("click", async (e) => {

        if (individualRepHead.IdStatus == 2) {
            alert("El reporte está marcado como completado. No se realizarán cambios.");
            return
        }

        try {
            let promiseV = await sendNewEntry(individualRepHead);

            //  let value = await promiseV;

        } catch (err) {
            alert(err.responseText);
            console.log("Error al actualizar el reporte.");
        }
        event.target.click();//click en el HEAD para que actualicé
    });

}

const sendNewEntry = (newEntry) => {

    let currentToken = localStorage.getItem(KEY_TOKEN_NAME);
    let userLvl = localStorage.getItem("LevelUser");

    if (!currentToken) {
        alert("Inicie sesión primero")
        window.location.href = "./Login"

        return
    }

    let data = {
        IdReport: newEntry.IdReport,//ese Id es colocado cuando se da click en el head
        TitleUpdate: "",
        Description: txtRepDtlUserInput.val(),
        IsOwnerUpdate: userLvl == 0 ? false : true/*newEntry.IsOwnerUpdate*/,
        DTUpdate: ""/* new Date()
            .toISOString()
            .slice(0, 19)
            .replace("/-/g", "/")
            .replace("T", " ")//La API toma la hora del servidor.*/,
        Pic64: ""//Pendiente
    }

    let dataStr = JSON.stringify(data);

    return $.ajax({
        type: "POST",
        url: API_URL + "api/ReportDtl",
        contentType: "application/json",
        crossDomain: true,
        datatype: "json",
        data: dataStr,
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


const clearReportDtl = () => {

    containerRepDtl.children(".container-reportDtlEntries").children().remove();
    let containerHeadExpanded = containerRepDtl.children(".container-headexpand");

    containerHeadExpanded.children().remove();

}

const reFillReportDtl = (individualRepHead, targetHead) => {
    let statusRepStr = individualRepHead.IdStatus == 0 ? "EN ESPERA" : individualRepHead.IdStatus == 1 ? "EN PROCESO" : "COMPLETADA";

    let dateRep = new Date(individualRepHead.DTCreation);
    let dateRepInicio = new Date(individualRepHead.InicioReporteDT)
    let dateRepFin = new Date(individualRepHead.FinReprteDT)

    let dateRepStr = getDateStr(dateRep);
    let dateRepInicioStr = getDateStr(dateRepInicio);
    let dateRepFinStr = getDateStr(dateRepFin);


    let reportDtlHeadExpanded = $(`
                
                    <div class="container-headexpand__title">${individualRepHead.Title}</div>
                    <div class="container-headexpand__idReport">Reporte #${individualRepHead.IdReport}</div>
                    <div class="item-report-head__numEmpleadoWhoNotified">${individualRepHead.UserWhoNotified.NumEmpleado ? "# Nómina: " + individualRepHead.UserWhoNotified.NumEmpleado : ''}</div>

                    <div class="container-headexpand__numEmpleadoNotif">Notificó: ${individualRepHead.UserWhoNotified.Name}</div>
                    <div class="container-headexpand__description">${individualRepHead.Description}</div>
                    <select name="selRepType_Dtl" id="selRepType_Dtl">
                    </select>

                    <div class="container-headexpand__location">
                        <a target="_blank"
                        href="https://www.google.com/maps?q=${individualRepHead.Location.lat + ',' + individualRepHead.Location.lon}">
                         ${individualRepHead.Location.Description ? individualRepHead.Location.Description : "Ubicación"}
                        </a>
                    </div>
                    <div class="container-headexpand__status">${statusRepStr}</div>
                    <div class="container-headexpand__notifiedDt">Fecha creación: ${dateRepStr}</div>
                    
                    <span class="container-headexpand__InicioDt">Fecha de inicio: ${dateRepInicioStr === '1/01/1 0:00 AM' ? '-----' : dateRepInicioStr}</span>
                    <span class="container-headexpand__notifiedDt">Fecha de termino: ${dateRepFinStr === '1/01/1 0:00 AM' ? '-----' : dateRepFinStr}</span>
                
                    <img class="container-headexpand__EvidencePic" src="data:image/png;base64,${individualRepHead.Pic64}"  ></img>
    `)

    containerRepDtl.children(".container-headexpand").append(reportDtlHeadExpanded);

    let select = containerRepDtl.children(".container-headexpand").children('#selRepType_Dtl');

    fillReportTypesNewRep(
        select
    );

    select.val(individualRepHead.IdReportType);

    selStatusRep.val(individualRepHead.IdStatus);

    btnSaveStatus.off("click");
    btnSaveStatus.on("click", async (event) => {
        let task = saveStatus(individualRepHead);

        try {
            let result = await individualRepHead;
            targetHead.click();
        }
        catch (ex) {
            //alert("Error: " + ex)
        }
    })
}

const getDateStr = (date) => {

    let hora = date.getHours();
    let minutos = date.getMinutes();

    let dateStr = date.getDate() + "/" + date.getMonth() + 1 + "/" + date.getFullYear() + " " +
        (hora > 12 ? hora - 12 : hora) + ":" + (minutos < 10 ? "0" + minutos : minutos) + (hora > 12 ? " PM" : " AM");

    return dateStr;

}

const saveStatus = (individualRepHead) => {

    let currentToken = localStorage.getItem(KEY_TOKEN_NAME);
    let userLvl = localStorage.getItem("LevelUser");

    if (!currentToken) {
        alert("Inicie sesión primero")
        window.location.href = "./Login"

        return
    }

    if (!userLvl || userLvl == 0) alert("No tiene permiso para realizar esta acción.");

    individualRepHead.IdStatus = selStatusRep.val();
    let d = JSON.stringify(individualRepHead);


    return $.ajax({
        type: "PUT",
        url: API_URL + `api/Report/${individualRepHead.IdReport}`,
        contentType: "application/json",
        crossDomain: true,
        datatype: "json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", `${'Bearer ' + currentToken}`)
        },
        data: d,
        headers: {
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Credentials": "true",
            "Access-Control-Allow-Methods": "GET,HEAD,OPTIONS,POST,PUT",
            "Access-Control-Allow-Headers": "Access-Control-Allow-Headers, Origin,Accept, X-Requested-With, Content-Type, Access-Control-Request-Method, Access-Control-Request-Headers"
        },


    })
}

const reFillReportDtlEntries = (individualRepHead) => {

    let entriesContainer = containerRepDtl.children(".container-reportDtlEntries");

    let sessionLevel = localStorage.getItem("LevelUser");

    for (let currentEntryIndex in individualRepHead.ReportUpdates) {

        let currentEnry = individualRepHead.ReportUpdates[currentEntryIndex]
        //Si la sesión es PUBLIC, el owner va a la izquierda -> 
        //SI la sesión de NO PUBLIC, el owner va a la derecha

        let entryPositionClass = "";//default is right (end)

        entryPositionClass =
            (sessionLevel > 0 && !currentEnry.IsOwnerUpdate) || (sessionLevel == 0 && currentEnry.IsOwnerUpdate)
                ? "entry-left"
                : "entry-right";


        let dateEntry = new Date(currentEnry.DTUpdate);

        let dateEntryStr = getDateStr(dateEntry);
        entriesContainer.append(`
                <div class="container-reportDtlEntry ${entryPositionClass} ">

                    <div class="container-reportDtlEntry__whomReply">
                ${
            currentEnry.UserWhoUpdate.Name + (currentEnry.UserWhoUpdate.AccessLevel > 0 ? ' - ' +  currentEnry.UserWhoUpdate.AccessLevelName : '')
            }</div>

                    <div class="container-reportDtlEntry__description">${currentEnry.Description}</div>
                    <div class="container-reportDtlEntry__fechaHoraEntry">${dateEntryStr}</div>

                </div>
        `);

    }


};


const getRepDtlEntries = (individualRepHead) => {

    let currentToken = localStorage.getItem(KEY_TOKEN_NAME);;
    if (!currentToken) {
        alert("Inicie sesión primero.")
        window.location.href = "./Login"

        return;
    }

    return $.ajax({
        type: "GET",
        url: API_URL + `api/ReportDtl/${individualRepHead.IdReport}`,
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

    })


}

//genera cada uno de las vistas básicas del Head
const generateRepHead = (individualRepHead) => {

    let dateRep = new Date(individualRepHead.DTCreation);

    let dateRepStr = dateRep.getDate() + "/" + dateRep.getMonth() + "/" + dateRep.getFullYear();



    let repHead = $(`

            <div class="item-report-head"    >
                <div class="item-report-head__idReport"> Reporte #${individualRepHead.IdReport} </div>
                <div class="item-report-head__title">${individualRepHead.Title}</div>
                 
                <div class="item-report-head__date">${dateRepStr}</div>
                <div class="item-report-head__numEmpleadoWhoNotified">${individualRepHead.UserWhoNotified.NumEmpleado ?? "# Nómina: " + individualRepHead.UserWhoNotified.NumEmpleado}</div>
                <div class="item-report-head__nameWhoNotified"> ${individualRepHead.UserWhoNotified.Name} </div>

            </div>

    `);
    repHead.on("click", (e) => { clickReportHead(e, individualRepHead) });
    return repHead;


}

const enviarActualización = (actualizacionData) => {

    let repDtlEntry = {
        "IdReport": 3,
        "FileNameEvidence": "NoMyFile.jpeg",
        "PathEvidence": "C:/files/evidences/",
        "TitleUpdate": " Gracias",
        "Description": " De nada segunda description ",
        "IsOwnerUpdate": true,
        "DTUpdate": "2021-03-14 02:40:15"
    }

    return $.ajax({
        type: "POST",
        url: API_URL + `api/ReportDtl/`,
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
    })

}


//Si el usuario
const checkSessionLevel = () => {
    let lu = localStorage.getItem("LevelUser");

    if (lu == undefined) {

        window.location.href = "./Login"
        return
    }

    //si es un usuario publico, podrá hacer toogle a la ventana de nuevo reporte.
    //btnToogleNewRep.prop("display", lu == 0 ? "block" : "none");
    if (lu != 0) {
        btnToogleNewRep.addClass("no-render")

    } else {

        selStatusRep.addClass("no-render");
        btnSaveStatus.addClass("no-render");

    }
    //Por defecto el contenedor Nuevo reporte será escondido, no importa el usuario

    containerNewRep.addClass("no-render");

    containerRepDtl.removeClass("no-render")



}

const init = () => {
    checkSessionLevel();
    initNewReportView()
    getRepsByUser();

    fillReportTypesNewRep(selRepType_Dtl)
    // initCam();
}

init();


