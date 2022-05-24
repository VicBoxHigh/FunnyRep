const btnGuardar = document.getElementById("btnGuardar");
const btnGetLocation = $("#btnGetLocation");

const txtTitle = $("#txtTitle");
const txtDescription = $("#txtDescriptionReport");
const txtLugar = $("#txtLugar");

const selRepType_NewRep = $("#selRepType_NewRep");


let lat = 0;
let lon = 0;

const webcamElement = document.getElementById("webcam");
const canvasElement = document.getElementById("canvas");
const btnFlipCamera = document.getElementById("btnFlipCamera");
//const butonSnap = document.getElementById("buttonSnap");
//const snapSoundElement = document.getElementById("snapSound");
let webcam //= new Webcam(webcamElement, "user", canvasElement, null);


const iframeMap = document.createElement("iframe");

const REPORT_TYPE_NAME = "REPORT_TYPES";
const KEY_TOKEN_NAME = "SESSIONTOKEN";


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

        data: dataStr,
        success: function (data, textStatus, xhr) {
            /* getRepsByUser();*/
            if (xhr.status == 200 || xhr.status == 201 || xhr.status == 202) {
                window.location.href = "./Default"
            }
            else
                alert(textStatus)


        },
        error: function (xhr, textStatus) {
            alert("Error en la solicitud" + xhr.responseText);
        },
    });
    /*  webcam.stop(); */
});

btnFlipCamera.addEventListener("click", (e) => {
    if (webcam) {
        webcam.flip()
        webcam.start()

    }
})

const extractReportData = () => {
    let d = {

        IdReport: 0,//IGNORED when create
        IdUserWhoNotified: 1,//IGNORED when create
        Location: {
            IdLocation: 0, //IGNORED when create
            Description: txtLugar.val(),
            lat: lat,
            lon: lon,
        },
        IdReportType: selRepType_NewRep.val(),
        IdStatus: 0,//IGNORED when create
        Pic64: webcam.snap().substring(22),
        DTCreation: "" /*new Date()//La API no lo lee, toma la hora del host
            .toISOString()
            .slice(0, 19)
            .replace("/-/g", "/")
            .replace("T", " ")*/,
        ReportUpdates: [],//IGNORED when create, 
        Title: txtTitle.val(),
        Description: txtDescription.val(),
    };

    return d;
}

const initCam = () => {
    webcam = new Webcam(webcamElement, "environment", canvasElement, null);
    webcam
        .start()
        .then((result) => {
            //alert("Webcam started")

        })
        .catch((err) => {
            console.log(err);
            alert("Error abriendo la cámara" + err)
        });
}

const stopCam = () => {

    webcam.stop();

}


const initNewReportView = () => {


    if (localStorage.getItem(REPORT_TYPE_NAME)) {
        fillReportTypesNewRep(selRepType_NewRep)
        return;
    }
    //might be invalid token or not exists
    let currentToken = localStorage.getItem(KEY_TOKEN_NAME);
    if (!currentToken) {
        alert("Inicie sesión primero.")
        return;
    }

    $.ajax({
        type: "GET",
        url: API_URL + "api/ReportType",
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
        // data: dataStr,
        success: function (data, textStatus, xhr) {
            /* getRepsByUser();*/
            if (xhr.status == 200 || xhr.status == 201 || xhr.status == 202) {

                localStorage.setItem(REPORT_TYPE_NAME, JSON.stringify(data.reportTypes));

                fillReportTypes(selRepType_NewRep)//con la data de local storage



            }
            else
                alert(textStatus);


        },
        error: function (xhr, textStatus) {
            alert("Error en la solicitud" + xhr.responseText);
        },


    });



}

