
//el callback para cuando se de click en una de las tarjetas Head de un reporte
//event es la entidad que regresa el head al darle clic, sirve para invocar
const clickReportHead = async (event, individualRepHead) => {

    clearReportDtl()//limpia el Head expanded y las entries
    txtRepDtlUserInput.val("");
    refillReportHeadExpand(individualRepHead, event.target);//llena el head expanded


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

