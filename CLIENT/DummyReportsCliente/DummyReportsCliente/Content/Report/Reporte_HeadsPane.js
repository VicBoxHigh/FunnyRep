//Lógica del listado de reportes heads que se muestran.
const repHeadsContainer = $("#cntRepHeads");
//Filtros
const radios = repHeadsContainer.children(`input[name="AsginacionStat"]`);//2
const checks = repHeadsContainer.children(`input[name="repStatus"]`);//3


const mediatorTask = () => {

    //Reports are saved at this point
    //De acuerdo con los estados de lo filtros, de owned y no owned, someter

    let reportsAsignados = REPORTES_ASIGNADOS//JSON.parse(localStorage.getItem(REPORTS_ASIGNADOS_NAME));
    let reportsNoAsignados = REPORTES_NO_ASIGNADOS// JSON.parse(localStorage.getItem(REPORTS_NO_ASIGNADOS_NAME));

    if (!reportsAsignados && !reportsNoAsignados) {
        //alert("No hay ")

        return;
    }


    let repSelected = repHeadsContainer.children(`input[name="AsginacionStat"]:checked`).val();

    let statsSelected = []
    repHeadsContainer.children(`input[type="checkbox"]:checked`).each((i, element) => {
        statsSelected[i] = element.value;
    });


    let repsFiltered = filterWith(repSelected == 1 ? reportsAsignados : reportsNoAsignados, statsSelected);

    renderRepHeads(repsFiltered);
}



const filterWith = (repsToWorkWith, statsOptions) => {

    //render en pantalla mientras filtra? 
    //o filtro y luego entrega a render?
    let repsFiltered = [];
    let i = 0;
    $.each(repsToWorkWith, (index, element) => {
        if ($.inArray(element.IdStatus + "", statsOptions) !== -1) {

            repsFiltered[i] = element;
            i++;
        }
    })



    return repsFiltered;

}

const renderRepHeads = (repsToShow) => {

    repHeadsContainer.children('.item-report-head').remove();
    for (let currentRep in repsToShow) {
        repHeadsContainer.append(
            generateRepHead(repsToShow[currentRep])
        )
    }


}


//genera cada uno de las vistas básicas del Head
const generateRepHead = (individualRepHead) => {

    let dateRep = new Date(individualRepHead.DTCreation);

    let dateRepStr = dateRep.getDate() + "/" + (dateRep.getMonth() + 1) + "/" + dateRep.getFullYear();



    let repHead = $(`

            <div class="item-report-head"    >
                <div class="item-report-head__idReport"> Reporte #${individualRepHead.IdReport} </div>
                <div class="item-report-head__title">${individualRepHead.Title}</div>
                 
                <div class="item-report-head__date">${dateRepStr}</div>
                <div class="item-report-head__numEmpleadoWhoNotified">${individualRepHead.UserWhoNotified.NumEmpleado ?? "# Nómina: " + individualRepHead.UserWhoNotified.NumEmpleado}</div>
                <div class="item-report-head__nameWhoNotified"> ${individualRepHead.UserWhoNotified.Name} </div>

            </div>

    `);
    repHead.on("click", (e) => { clickReportHead(e, individualRepHead) });
    return repHead;


}


radios.on("change", (e) => {
    mediatorTask();

})

checks.on("change", (e) => {

    mediatorTask();

})
