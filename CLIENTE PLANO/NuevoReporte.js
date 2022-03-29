let containerRep = document.getElementById("cntNewRep");

let txtTitle = $("#txtTitle");
let txtDescription = $("#txtDescriptionReport");
let txtLugar = $("#txtLugar");

let btnGetLocation = $("#btnGetLocation");

let btnGuardar = document.getElementById("btnGuardar");

let iframeMap = document.createElement("iframe");

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

btnGuardar.addEventListener("click", (e) => {
  $.ajax({
    type: "POST",
    url: "http://localhost:44381/api/Report",
    contentType: "application/json; charset=utf-8",
    dataType: "jsonp",
    /*  user: "vperez",
      password: "vperez", */
    data: {
      a: 1,
      IdReport: 0,
      IdUserWhoNotified: 12,
      Location: {
        IdLocation: 0,
        Description: txtLugar.val(),
        lat: lat,
        lon: lon,
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
    succes: function (data) {
      alert(data);
    },
    error: function (err) {
      alert(err);
    },
  });
  /*  webcam.stop(); */
});

const snapF = () => {
  alert("snap");

  /*  let picture = webcam.snap();
 
  document.querySelector("#download-photo").href = picture;
  document.querySelector("#download-photo").download = "foto1F.png"; */
};
butonSnap.addEventListener("click", snapF, false);

webcam
  .start()
  .then((result) => {
    console.log("webcam started");
  })
  .catch((err) => {
    console.log(err);
  });
