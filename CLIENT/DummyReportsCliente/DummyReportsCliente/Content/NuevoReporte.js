
const containerNewRep = $("#cntNewRep");
const containerRepDtl = $("#cntRepDtl");

const btnToogleNewRep = $("#btnTogleNewRep")

const txtTitle = $("#txtTitle");
const txtDescription = $("#txtDescriptionReport");
const txtLugar = $("#txtLugar");

const btnGetLocation = $("#btnGetLocation");
const repHeadsContainer = $("#cntRepHeads");

const txtRepDtlUserInput = $("#txtRepDtlUserInput")
const btnSendRepDtlUpdate = $("#btnSendRepDtlUpdate")


const btnGuardar = document.getElementById("btnGuardar");

const iframeMap = document.createElement("iframe");


let lat = 0;
let lon = 0;

const webcamElement = document.getElementById("webcam");
const canvasElement = document.getElementById("canvas");
const butonSnap = document.getElementById("buttonSnap");
//const snapSoundElement = document.getElementById("snapSound");
const webcam = new Webcam(webcamElement, "user", canvasElement, null);

/* const webcam = new Webcam(
  webcamElement,
  "user",
  canvasElement,
  snapSoundElement
); */

/* const generarMapa = (position) => {
  let lat = position.coords.latitude;
  let lon = position.coords.longitude;

  cntMap.after(iframeMap);
}; */

const guardarLocation = (position) => {
    lat = position.coords.latitude;
    lon = position.coords.longitude;
};
/* https://www.google.com/maps?q=19.4185605,-102.045651 */
const getLocation = () => {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(guardarLocation);
    } else {
        alert("La ubicación no es soportada o no se encuentra disponible");
    }
};

btnGetLocation.on("click", () => {
    getLocation();
});
const KEY_TOKEN_NAME = "SESSIONTOKEN";

btnGuardar.addEventListener("click", (e) => {

    //might be invalid token or not exists
    let currentToken = localStorage.getItem(KEY_TOKEN_NAME);
    if (!currentToken) {
        alert("Inicie sesión primero.")
        return;
    }

    let data = extractReportData();
    let dataStr = JSON.stringify(data);
    $.ajax({
        type: "POST",
        url: "http://localhost:57995/api/Report",
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

        data: dataStr,
        succes: function (data, textStatus, xhr) {
            alert(data);
        },
        error: function (xhr, textStatus) {
            alert("Error en la solicitud" + xhr.responseText);
        },
    });
    /*  webcam.stop(); */
});

btnToogleNewRep.on("click", () => {


    if (containerNewRep.hasClass("no-render")) {

        containerNewRep.removeClass("no-render");

        containerRepDtl.addClass("no-render")


    }
    else {

        containerNewRep.addClass("no-render");

        containerRepDtl.removeClass("no-render")


    }



});

const extractReportData = () => {
    let d = {

        IdReport: 0,
        IdUserWhoNotified: 1,
        Location: {
            IdLocation: 0,
            Description: txtLugar.val(),
            lat: lat,
            lon: lon,
        },
        IdStatus: 0,
        Pic64: webcam.snap().substring(22),
        DTCreation: new Date()
            .toISOString()
            .slice(0, 19)
            .replace("/-/g", "/")
            .replace("T", " "),
        ReportUpdates: [],
        Title: txtTitle.val(),
        Description: txtDescription.val(),
    };

    return d;
}


const getRepsByUser = () => {

    let currentToken = localStorage.getItem(KEY_TOKEN_NAME);

    if (!currentToken) {
        alert("Inicie sesión primero")
    }

    $.ajax({
        type: "Get",
        url: "http://localhost:57995/api/Report",
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
            //alert(data);
            renderRepHeads(data.reports)
        },
        error: function (xhr, textStatus) {
            alert("Error en la solicitud" + xhr.responseText);
        },
    });
}


const renderRepHeads = (repHeads) => {

    for (let currentRep in repHeads) {
        repHeadsContainer.append(
            generateRepHead(repHeads[currentRep])
        )

    }


}

//el callback para cuando se de click en una de las tarjetas Head de un reporte
const clickReportHead = async (event, individualRepHead) => {

    clearReportDtl()//limpia el Head expanded y las entries
    txtRepDtlUserInput.val("");
    reFillReportDtl(individualRepHead);//llena el head expanded


    let taskGetEntries = getRepDtlEntries(individualRepHead);

    //do animation loading?
    /*  {
  
      }*/

    let data = await taskGetEntries;//unvelop the entries from the promise

    individualRepHead.ReportUpdates = data.reportDtlEntries;
    reFillReportDtlEntries(individualRepHead);//llena las entries;

    btnSendRepDtlUpdate.off("click");
    btnSendRepDtlUpdate.on("click", async (e) => {

        try {
            let value = await sendNewEntry(individualRepHead);
            

        } catch (err) {
            console.log("Error al actualizar los detalles locales del reporte.");
        }
        event.target.click();
    });

}

const sendNewEntry = (newEntry) => {

    let currentToken = localStorage.getItem(KEY_TOKEN_NAME);
    let userLvl = localStorage.getItem("LevelUser");

    if (!currentToken) {
        alert("Inicie sesión primero")
    }

    let data = {
        IdReport: newEntry.IdReport,//ese Id es colocado cuando se da click en el head
        TitleUpdate: "",
        Description: txtRepDtlUserInput.val(),
        IsOwnerUpdate: userLvl == 0 ? false : true/*newEntry.IsOwnerUpdate*/,
        DTUpdate: new Date()
            .toISOString()
            .slice(0, 19)
            .replace("/-/g", "/")
            .replace("T", " "),
        Pic64: ""//Pendiente
    }

    let dataStr = JSON.stringify(data);

    return $.ajax({
        type: "POST",
        url: "http://localhost:57995/api/ReportDtl",
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

const reFillReportDtl = (individualRepHead) => {

    let reportDtlHeadExpanded = $(`
                <div class="container-headexpand__idReport">${individualRepHead.IdReport}</div>
                <div class="container-headexpand__title">${individualRepHead.Title}</div>
                <div class="container-headexpand__description">${individualRepHead.Description}</div>
                <div class="container-headexpand__numEmpleadoNotif">${individualRepHead.IdUserWhoNotified}</div>
                <div class="container-headexpand__location">${individualRepHead.Location}</div>
                <div class="container-headexpand__status">${individualRepHead.IdStatus}</div>
                <div class="container-headexpand__FileNameEvidence">${individualRepHead.FileNameEvidence}</div>
                <div class="container-headexpand__notifiedDt">${individualRepHead.DTCreation}</div>
    `)

    containerRepDtl.children(".container-headexpand").append(reportDtlHeadExpanded);

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
                : "";


        entriesContainer.append(`
                <div class="container-reportDtlEntry ${entryPositionClass} ">

                    <div class="container-reportDtlEntry__title">${currentEnry.TitleUpdate}</div>
                    <div class="container-reportDtlEntry__description">${currentEnry.Description}</div>
                    <div class="container-reportDtlEntry__fileNameEvidence">${currentEnry.FileNameEvidence}</div>
                    <div class="container-reportDtlEntry__fechaHoraEntry">${currentEnry.DTUpdate}</div>

                </div>
        `);

    }






};


const getRepDtlEntries = (individualRepHead) => {

    let currentToken = localStorage.getItem(KEY_TOKEN_NAME);;
    if (!currentToken) {
        alert("Inicie sesión primero.")
        return;
    }

    return $.ajax({
        type: "GET",
        url: `http://localhost:57995/api/ReportDtl/${individualRepHead.IdReport}`,
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

        /*success: function (data, textStatus, xhr) {
             individualRepHead.ReportUpdates = data.reportDtlEntries;
             
        },
        error: function (xhr, textStatus) {
            alert("Error en la solicitud" + xhr.responseText);
        },*/
    })


}

//genera cada uno de las vistas básicas del Head
const generateRepHead = (individualRepHead) => {

    let repHead = $(`

            <div class="item-report-head"    >
                <div class="item-report-head__idReport">#${individualRepHead.IdReport} </div>
                <div class="item-report-head__title">${individualRepHead.Title}</div>
                <div class="item-report-head__title">${individualRepHead.Description}</div>
                <div class="item-report-head__date">${individualRepHead.DTCreation}</div>
                <div class="item-report-head__numEmpleado">${individualRepHead.IdUserWhoNotified}</div>

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
        url: `http://localhost:57995/api/ReportDtl/`,
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

/*const snapF = () => {
    alert("snap");

    *//*  let picture = webcam.snap();
 
document.querySelector("#download-photo").href = picture;
document.querySelector("#download-photo").download = "foto1F.png"; *//*
};
butonSnap.addEventListener("click", snapF, false);
*/
const initCam = () => {
    webcam
        .start()
        .then((result) => {
            console.log("webcam started");
        })
        .catch((err) => {
            console.log(err);
        });
}


//Si el usuario
const checkSessionLevel = () => {
    let lu = localStorage.getItem("LevelUser");

    if (lu == undefined) alert("ERROR");

    //si es un usuario publico, podrá hacer toogle a la ventana de nuevo reporte.
    //btnToogleNewRep.prop("display", lu == 0 ? "block" : "none");
    if (lu != 0)
        btnToogleNewRep.addClass("no-render")


    //Por defecto el contenedor Nuevo reporte será escondido, no importa el usuario

    containerNewRep.addClass("no-render");

    containerRepDtl.removeClass("no-render")



}

const init = () => {
    checkSessionLevel();
    getRepsByUser();
    // initCam();
}

init();


