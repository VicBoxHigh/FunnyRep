const containerRepDtl = $("#cntRepDtl");

//Incluye inicialización de todos los elementos del header del reporte
//he = HeadExpand 

const heTitle = $(".container-headexpand__title");
const heIdReport = $(".container-headexpand__idReport");
const heNumEmpleadoWhoNotif = $(".item-report-head__numEmpleadoWhoNotified");
const heWhoNotif = $(".container-headexpand__EmpleadoNotif");
const heDescription = $(".container-headexpand__description");
const heSelectReportType = $(".selRepType_Dtl");
const heLocation = $(".container-headexpand__location");//<a></a> nested
const heStatus = $(".container-headexpand__status");
const heNotifiedDT = $(".container-headexpand__notifiedDT");
const heInicioDT = $(".container-headexpand__InicioDt");
const heFinDT = $(".container-headexpand__FinDt");
const heEvidencePic = $(".container-headexpand__EvidencePic");


const selStatusRep = $("#selStat");
const btnSaveStatus = $("#btnSaveStatus");


const refillReportHeadExpand = (individualRepHead, targetHead) => {
    let statusRepStr = individualRepHead.IdStatus == 0 ? "EN ESPERA" : individualRepHead.IdStatus == 1 ? "EN PROCESO" : "COMPLETADA";

    let dateRep = new Date(individualRepHead.DTCreation);
    let dateRepInicio = new Date(individualRepHead.InicioReporteDT)
    let dateRepFin = new Date(individualRepHead.FinReprteDT)

    let dateRepStr = getDateStr(dateRep);
    let dateRepInicioStr = getDateStr(dateRepInicio);
    let dateRepFinStr = getDateStr(dateRepFin);

    heTitle.text(individualRepHead.Title);
    heIdReport.text(individualRepHead.IdReport);
    heNumEmpleadoWhoNotif.text(individualRepHead.UserWhoNotified.NumEmpleado ?
        "# Nómina: " + individualRepHead.UserWhoNotified.NumEmpleado :
        '');
    heWhoNotif.text("Notificó:" + individualRepHead.UserWhoNotified.Name);
    heDescription.text(individualRepHead.Description);
    //heSelectReportType .clear y append opstions

    heLocation.children().remove();//Elimina el elemento a
    heLocation.append(`<a target="_blank"
                         href="https://www.google.com/maps?q=${individualRepHead.Location.lat + ',' + individualRepHead.Location.lon}">
                          ${individualRepHead.Location.Description ? individualRepHead.Location.Description : "Ubicación"}
                         </a>`);

    heStatus.text(statusRepStr);
    heNotifiedDT.text(dateRepStr);
    heInicioDT.text(dateRepInicioStr);
    heFinDT.text(dateRepFinStr);
    heEvidencePic.attr("src", `data:image/png;base64,${individualRepHead.Pic64}`)


    //clasificación de reporte
    fillReportTypesNewRep(heSelectReportType);
    heSelectReportType.val(individualRepHead.IdReportType);

    //Marca el statis
    selStatusRep.val(individualRepHead.IdStatus);

    btnSaveStatus.off("click");
    btnSaveStatus.on("click", (event) => statusClasifChange(event, individualRepHead));

    heSelectReportType.off("change")
    heSelectReportType.on("change", (event) => statusClasifChange(event, individualRepHead))


}



const clearReportDtl = () => {

    containerRepDtl.children(".container-reportDtlEntries").children("container-reportDtlEntry ").remove();

    let containerHeadExpanded = containerRepDtl.children(".container-headexpand");

    containerHeadExpanded.children().children().remove();

}



const statusClasifChange = async (event, individualRepHead) => {
    let task = saveStatus(individualRepHead);

    try {
        let result = await task;
        targetHead.click();
    }
    catch (ex) {
        alert("Error: " + ex)
    }
}



//Guarda el status del reporte
const saveStatus = (individualRepHead) => {
    let currentToken = checkSession();

    let userLvl = localStorage.getItem("LevelUser");
    
    if (!userLvl || userLvl == 0) alert("No tiene permiso para realizar esta acción.");

    let newClasif = heSelectReportType.val();

    let newStatus = selStatusRep.val();


    //individualRepHead.IdStatus = selStatusRep.val();

    return $.ajax({
        type: "PUT",
        url: API_URL + `api/Report?id=${individualRepHead.IdReport}&newClasif=${newClasif}&newStatus=${newStatus}`,
        contentType: "application/json",
        crossDomain: true,
        datatype: "json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", `${'Bearer ' + currentToken}`)
        },
        data: d,
        headers: {
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Credentials": "true",
            "Access-Control-Allow-Methods": "GET,HEAD,OPTIONS,POST,PUT",
            "Access-Control-Allow-Headers": "Access-Control-Allow-Headers, Origin,Accept, X-Requested-With, Content-Type, Access-Control-Request-Method, Access-Control-Request-Headers"
        },


    })
}
