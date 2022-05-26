const API_URL = "https://it-junior.grupomarves.net:57996/";

const REPORT_TYPE_NAME = "REPORT_TYPES";
const KEY_TOKEN_NAME = "SESSIONTOKEN";
const REPORTS_ASIGNADOS_NAME = "REPASIGNADOS";
const REPORTS_NO_ASIGNADOS_NAME = "REPNOASIGNADOS"


//objeto date a string
const getDateStr = (date) => {

    let hora = date.getHours();
    let minutos = date.getMinutes();

    let dateStr = date.getDate() + "/" + (date.getMonth() + 1) + "/" + date.getFullYear() + " " +
        (hora > 12 ? hora - 12 : hora) + ":" + (minutos < 10 ? "0" + minutos : minutos) + (hora > 12 ? " PM" : " AM");

    return dateStr;

}


//coloca los optionsn en el select dado
const fillReportTypesNewRep = (selecToSet) => {

    let reportTypes = localStorage.getItem(REPORT_TYPE_NAME);
    selecToSet.children().remove();
    let listParsed = JSON.parse(reportTypes)

    for (let currentType in listParsed) {
        selecToSet.append(`<option value='${currentType}' >${listParsed[currentType]}</option>`)
    }
}


const checkSession = () => {
    let token = localStorage.getItem(KEY_TOKEN_NAME)
    if (!token) {
        alert("Inicie sesión primero.");
        window.href = './Login'
    }
    return token;
}




const getReporTypes =   (token) => {


    return $.ajax({
        type: "GET",
        url: API_URL + "api/ReportType",
        contentType: "application/json",
        crossDomain: true,
        datatype: "json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", `${'Bearer ' + token}`)

        },
        headers: {
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Credentials": "true",
            "Access-Control-Allow-Methods": "GET,HEAD,OPTIONS,POST,PUT",
            "Access-Control-Allow-Headers": "Access-Control-Allow-Headers, Origin,Accept, X-Requested-With, Content-Type, Access-Control-Request-Method, Access-Control-Request-Headers"
        },


    });


}