const containerNewRep = $("#cntNewRep");
const containerRepDtl = $("#cntRepDtl");

const btnToogleNewRep = $("#btnTogleNewRep")

const txtTitle = $("#txtTitle");
const txtDescription = $("#txtDescriptionReport");
const txtLugar = $("#txtLugar");

const btnGetLocation = $("#btnGetLocation");
const repHeadsContainer = $("#cntRepHeads");

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

        data: dataStr,
        succes: function (data, textStatus, xhr) {
            alert(data);
        },
        error: function (xhr, textStatus) {
            alert("Error en la solicitud" + xhr.responseText);
        },
    });
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

    if (!lu == undefined) alert("ERROR");

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
    //  getRepsByUser();
    // initCam();
}

init();


