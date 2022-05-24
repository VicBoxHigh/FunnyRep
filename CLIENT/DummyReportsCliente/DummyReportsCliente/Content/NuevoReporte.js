
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

    repHeadsContainer.children('.item-report-head').remove();
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



const clearReportDtl = () => {

    containerRepDtl.children(".container-reportDtlEntries").children("container-reportDtlEntry ").remove();
    let containerHeadExpanded = containerRepDtl.children(".container-headexpand");

    containerHeadExpanded.children().remove();

}


const reFillReportDtlEntries = (individualRepHead) => {

    let entriesContainer = containerRepDtl.children(".container-reportDtlEntries");
    entriesContainer.children(".container-reportDtlEntry  ").remove();

    let sessionLevel = localStorage.getItem("LevelUser");

    for (let currentEntryIndex in individualRepHead.ReportUpdates) {

        let currentEnry = individualRepHead.ReportUpdates[currentEntryIndex]
        //Si la sesión es PUBLIC, el owner va a la izquierda -> 
        //SI la sesión de NO PUBLIC, el owner va a la derecha

        let entryPositionClass = "";//default is right (end)

        entryPositionClass =
            currentEnry.UserWhoUpdate.IdUser == 1 ? "entry-center" :
                (sessionLevel > 0 && !currentEnry.IsOwnerUpdate) || (sessionLevel == 0 && currentEnry.IsOwnerUpdate)
                    ? "entry-left"
                    : "entry-right";

        let userInfoToShow =
            currentEnry.UserWhoUpdate.IdUser == 1 ? "" :
                currentEnry.UserWhoUpdate.Name + (currentEnry.UserWhoUpdate.AccessLevel > 0 ? ' - ' + currentEnry.UserWhoUpdate.AccessLevelName : '')


        let dateEntry = new Date(currentEnry.DTUpdate);

        let dateEntryStr = getDateStr(dateEntry);

        entriesContainer.append(`
                <div class="container-reportDtlEntry ${entryPositionClass} ">

                    <div class="container-reportDtlEntry__whomReply"> ${userInfoToShow}</div>

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

    let dateRepStr = dateRep.getDate() + "/" + (dateRep.getMonth() + 1) + "/" + dateRep.getFullYear();



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




const init = () => {
    checkSessionLevel();
    initNewReportView()
    getRepsByUser();

    fillReportTypesNewRep(selRepType_Dtl)
    // initCam();
}

init();


