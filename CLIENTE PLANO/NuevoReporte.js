let containerRep = $("#cntNewRep");

let txtTitle = $("#txtTitle");
let txtDescription = $("#txtDescriptionReport");
let txtLugar = $("#txtLugar");

let btnGetLocation = $("#btnGetLocation");

let btnGuardar = $("#btnGuardar");

let iframeMap = document.createElement("iframe");

let lat = 0;
let lon = 0;

let btnStartCam = $("#btnStartCam");
let btnTakePic = $("#btnTakePic");

const webcamElement = $("#webcam");
const canvasElement = $("#canvas");
const snapSoundElement = $("#snapSound");
/* const webcam = new Webcam(
  webcamElement,
  "user",
  canvasElement,
  snapSoundElement
); */

const generarMapa = (position) => {
  let lat = position.coords.latitude;
  let lon = position.coords.longitude;

  cntMap.after(iframeMap);
};

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

btnStartCam.on("click", () => {
    
});

btnTakePic.on("click", () => {

    var picture = webcam.snap();



}, false);

btnGuardar.on(
  "click",
  (e) => {
    webcam.stop();

    $.ajax({
      type: "POST",
      url: "localhost://api/report",
      user: "vperez",
      password: "vperez",
      data: {
        a: 1,
        IdReport: 0,
        IdUserWhoNotified: 12,
        Location: {
          IdLocation: 0,
          Description: txtLugar.val(),
          lat: lat,
          lon: Long,
        },
        IdStatus: 0,
        DTCreation: new Date()
          .toISOString()
          .slice(0, 19)
          .replace("/-/g", "/")
          .replace("T", " "),
        ReportUpdates: [],
        Title: txtTitle.val(),
        Description: txtDescription.val(),
      },
    });
  },
  false
);
