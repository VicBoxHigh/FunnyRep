<!DOCTYPE html>
<html>
<head>
    <title></title>
    <link rel="stylesheet" href="Content/normalize.css">
    <link rel="stylesheet" href="Content/Default.css">
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
 
</head>
<body>


    <div class="navbar">

        <div class="list-btn-container">
            <input type="checkbox" class="checkbox" />

            <div class="menu-item">Mis Reportes</div>

            <div id="cntRepHeads" class="container-heads">

                <input type="radio" id="checkAsignados" name="AsginacionStat" checked  value="1">
                <label for="checkAsignados">Asignados</label><br>
                <input type="radio" id="checkSinAsignar" name="AsginacionStat" value="0">
                <label for="checkSinAsignar">No asignados</label><br>


                <input type="checkbox" id="checkEspera" name="repStatus" checked  value="0">
                <label for="checkEspera">En espera</label><br>

                <input type="checkbox" id="checkProceso" name="repStatus" checked value="1">
                <label for="checkProceso">En proceso</label><br>

                <input type="checkbox" id="checkCompletado" name="repStatus" checked value="2">
                <label for="checkCompletado">Completados</label><br>




                <div class="item-report-head">AÚN NO HAY REPORTES REGISTRADOS</div>

            </div>

        </div>

        <input id="btnTogleNewRep" type="button" value="Nuevo Reporte" />
        <input id="btnUsuarios" type="button" value="Usuarios" />
    </div>
    <div class="main-container">


        <div id="cntNewRep" class="containerNewReport">

            <input id="txtTitle" type="text" name="title"  placeholder="Titulo" />

            <textarea type="text" rows="5" cols="50" id="txtDescriptionReport" name="descriptionReport" placeholder="Describir problema"></textarea>

            <input type="text" id="txtLugar" placeholder="Lugar" />

            <select name="selRepType_NewRep" id="selRepType_NewRep">
            </select>
            <input type="button" id="btnGetLocation" value="Marcar ubicación" />

            <input type="button" id="btnFlipCamera" value="Cambiar camara" />
            <video id="webcam" autoplay playsinline></video>
            <canvas id="canvas" class="d-none"></canvas>

            <!--  <input id="buttonSnap" type="button" value="Tomar foto" /> -->


            <input type="button" id="btnGuardar" value="Guardar" />

        </div>


        <div id="cntRepDtl" class="container-repdtl">

            <div id="cntRepHeadExpand" class="container-headexpand">

                <div class="container-headexpand__title"></div>
                <div class="container-headexpand__idReport"> </div>
                <div class="item-report-head__numEmpleadoWhoNotified"> </div>

                <div class="container-headexpand__EmpleadoNotif"> </div>
                <div class="container-headexpand__description"> </div>
                <select name="selRepType_Dtl" class="selRepType_Dtl" id="selRepType_Dtl">
                </select>
                <select name="selAsignarUser" class="selAsignarUser">

                </select>
                <div class="container-headexpand__location">
                    
                </div>
                <div class="container-headexpand__status"> </div>
                <div class="container-headexpand__notifiedDt"> </div>

                <span class="container-headexpand__InicioDt"> </span>
                <span class="container-headexpand__notifiedDt"> </span>

                <img class="container-headexpand__EvidencePic"  ></img>


            </div>

            <div id="cntRepDtlEntries" class="container-reportDtlEntries  ">
                <div>AÚN NO HAY ENTRADAS EN EL REPORTE</div>

            </div>


            <div id="cntRepDtlUserInput" class="container-repdtl__user-input">
                <textarea id="txtRepDtlUserInput" class="container-repdtl__user-input" rows="5" cols="50"></textarea>
                <input id="btnSendRepDtlUpdate" type="button" class="container-repdtl__send" value="Enviar" />

                <select name="selStat" id="selStat">
                    <option value="0">PENDIENTE</option>
                    <option value="1">EN PROCESO</option>
                    <option value="2">COMPLETADO</option>
                </select>

                <input id="btnSaveStatus" type="button" value="Guardar" />
            </div>


        </div>


    </div>

    <!-- <script src="webcam.js" type="text/javascript"></script> -->
    <script src="Content/jquery-3.6.0.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="Content/webcam-easyV.js"></script>

    <script type="text/javascript" src="Content/Util.js" ></script>
    <script type="text/javascript" src="Content/Report/Reporte_newReport.js"></script>
    <script src="Content/Report/Reporte_init.js" type="text/javascript"></script>
    <script src="Content/Report/Reporte_HeadsPane.js" type="text/javascript"></script>
    <script src="Content/Report/Report_HeadExpand.js" type="text/javascript"></script>
    <script src="Content/Report/Report_DtlEnries.js" type="text/javascript"></script>
    <script src="Content/Report/Reporte_Eventos.js" type="text/javascript"></script>
    <script src="Content/NuevoReporte.js" type="text/javascript"></script>

</body>
</html>


