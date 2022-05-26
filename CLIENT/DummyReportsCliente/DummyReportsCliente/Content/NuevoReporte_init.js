
const containerNewRep = $("#cntNewRep");
const containerRepDtl = $("#cntRepDtl");

const btnToogleNewRep = $("#btnTogleNewRep")

const repHeadsContainer = $("#cntRepHeads");


const txtRepDtlUserInput = $("#txtRepDtlUserInput")
const btnSendRepDtlUpdate = $("#btnSendRepDtlUpdate")

const selStatusRep = $("#selStat");
const btnSaveStatus = $("#btnSaveStatus");



//const selRepType_Dtl = $("#selRepType_Dtl");

//INIT UI - Según el usuario

//BASIC - 0
//Solo da accion de nuevo reporte y le muestra los que el ha creado, sin importar si se ecuentran asignados o no
//Muestra todos los filtros
//Muestra botón de nuevo reporte


//AGENT - 
//Mostrará reps unicamente asignados a este usuario.
//No muestra botón de nuevo reporte
//Muestra todos los filtros



//ADMIN
//Mostrará todos los reportes existentes, permitiendo asignar 1 reporte a cualquier agente.
//No muestra botón de nuevo reporte
//Muestra todos los filtros



//TI
//Lo mismo que ADMIN, y tiene acceso al admin de usuarios
//No muestra botón de nuevo reporte
//Muestra todos los filtros


const checkSessionLevel = () => {
    let lu = localStorage.getItem("LevelUser");

    if (!lu ) {

        window.location.href = "./Login"
        return
    }
    

    //si es un usuario publico, podrá hacer toogle a la ventana de nuevo reporte.
    //btnToogleNewRep.prop("display", lu == 0 ? "block" : "none");
    if (lu != 0) { //ningún admin sube reps
        btnToogleNewRep.addClass("no-render")

    } else {//usuario basic, no puede cambiar el stat del reporte

        selStatusRep.addClass("no-render");
        btnSaveStatus.addClass("no-render");

    }
    //Por defecto el contenedor Nuevo reporte será escondido, no importa el usuario
    //seg+un si es usuario basic, al hacer clic en NEW, se mostrará
    containerNewRep.addClass("no-render");

    containerRepDtl.removeClass("no-render")


    //cambio de clasificación, solo la hará un ADMIN
    //El HeaderDtl Expand es dinamico, por lo tanto no puede ser colocado aquí.


    if (lu == 150000) {
        //render users page
    }

}


