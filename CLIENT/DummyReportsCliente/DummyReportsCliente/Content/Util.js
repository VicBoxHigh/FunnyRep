
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
