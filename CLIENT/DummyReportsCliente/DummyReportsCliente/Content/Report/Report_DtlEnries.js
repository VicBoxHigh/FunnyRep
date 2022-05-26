
const txtRepDtlUserInput = $("#txtRepDtlUserInput")
const btnSendRepDtlUpdate = $("#btnSendRepDtlUpdate")


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



//Envia una entrada del reporte, 
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


