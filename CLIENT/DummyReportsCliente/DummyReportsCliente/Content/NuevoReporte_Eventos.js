
//el callback para cuando se de click en una de las tarjetas Head de un reporte
//event es la entidad que regresa el head al darle clic, sirve para invocar
const clickReportHead = async (event, individualRepHead) => {

    clearReportDtl()//limpia el Head expanded y las entries
    txtRepDtlUserInput.val("");
    reFillReportDtl(individualRepHead, event.target);//llena el head expanded


    let taskGetEntries = getRepDtlEntries(individualRepHead);

    //do animation loading?
    /*  {
  
      }*/
    let data = null;
    try {
        data = await taskGetEntries;//unvelop the entries from the promise

    } catch (ex) {

        alert("Error al solicitar el detalle.")
    }

    //OK
    if (taskGetEntries.status == 200) {
        individualRepHead.ReportUpdates = data.reportDtlEntries;//asigna las DtlEntries al header del reporte
        reFillReportDtlEntries(individualRepHead);//manda el objeto de dats de Reporte junto con las entries para que sea renderizado;


    }

    btnSendRepDtlUpdate.off("click");
    btnSendRepDtlUpdate.on("click", async (e) => {

        if (individualRepHead.IdStatus == 2) {
            alert("El reporte está marcado como completado. No se realizarán cambios.");
            return
        }

        try {
            let promiseV = await sendNewEntry(individualRepHead);

            //  let value = await promiseV;

        } catch (err) {
            alert(err.responseText);
            console.log("Error al actualizar el reporte.");
        }
        event.target.click();//click en el HEAD para que actualicé
    });

}


//Envia una entrada del reporte, 
const sendNewEntry = (newEntry) => {

    let currentToken = localStorage.getItem(KEY_TOKEN_NAME);
    let userLvl = localStorage.getItem("LevelUser");

    if (!currentToken) {
        alert("Inicie sesión primero")
        window.location.href = "./Login"

        return
    }

    let data = {
        IdReport: newEntry.IdReport,//ese Id es colocado cuando se da click en el head
        TitleUpdate: "",
        Description: txtRepDtlUserInput.val(),
        IsOwnerUpdate: userLvl == 0 ? false : true/*newEntry.IsOwnerUpdate*/,
        DTUpdate: ""/* new Date()
            .toISOString()
            .slice(0, 19)
            .replace("/-/g", "/")
            .replace("T", " ")//La API toma la hora del servidor.*/,
        Pic64: ""//Pendiente
    }

    let dataStr = JSON.stringify(data);

    return $.ajax({
        type: "POST",
        url: API_URL + "api/ReportDtl",
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



//Guarda el status del reporte
const saveStatus = (individualRepHead) => {

    let currentToken = localStorage.getItem(KEY_TOKEN_NAME);
    let userLvl = localStorage.getItem("LevelUser");

    if (!currentToken) {
        alert("Inicie sesión primero")
        window.location.href = "./Login"

        return
    }

    if (!userLvl || userLvl == 0) alert("No tiene permiso para realizar esta acción.");

    individualRepHead.IdStatus = selStatusRep.val();
    let d = JSON.stringify({ "id": individualRepHead.IdReport , "newClasif": individualRepHead.IdReportType, "newStatus": individualRepHead.IdStatus })//JSON.stringify(individualRepHead);

    let newClasif = containerRepDtl.children(".container-headexpand").children("#selRepType_Dtl").val();    

    let newStatus = selStatusRep.val();

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