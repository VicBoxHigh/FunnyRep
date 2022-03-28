let listaReports = $("#olReports");

$.ajax({
  type: "GET",
  url: `localhost://api/report?isOwner=${1},idUser=${4}`,
  user: "vperez",
  password: "vperez",
  success: function (result) {
    listaReports.empty();
    result.report.forEach((element, index) => {
        listaReports.append(`<li>${element.Title}</li>`);
    })

},
  error: function (XMLHttpRequest, textStatus, errorThrown) {},
});
