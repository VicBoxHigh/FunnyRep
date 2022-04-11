<!DOCTYPE html>
<html>
<head>
    <title></title>
    <link rel="stylesheet" href="Content/Default.css">
</head>
<body>
    <input id="btnTogleNewRep" type="button" value="Nuevo Reporte" />
    <div class="main-container">

        <div id="cntRepHeads" class="container-heads">
            <div class="item-report-head">
                <div class="item-report-head__idReport">Reporte #999 </div>
                <div class="item-report-head__title">Titulo</div>
                <div class="item-report-head__title">Pequeña descripción.</div>
                <div class="item-report-head__date"></div>
                <div class="item-report-head__numEmpleado"></div>

            </div>
        </div>

        <div id="cntNewRep" class="containerNewReport">

            <input id="txtTitle" type="text" name="title" id="titleReport" placeholder="Titulo" />

            <textarea type="text" rows="5" id="txtDescriptionReport" name="descriptionReport" placeholder="Describir problema"></textarea>

            <input type="text" id="txtLugar" placeholder="Lugar" />

            <input type="button" id="btnGetLocation" value="Marcar ubicacion" />

            <div>
                <video id="webcam" autoplay playsinline></video>
                <canvas id="canvas" class="d-none"></canvas>

            </div>

          <!--  <input id="buttonSnap" type="button" value="Tomar foto" /> -->


            <input type="button" id="btnGuardar" value="Guardar" />

        </div>


        <div id="cntRepDtl" class="container-repdtl">

            <div id="cntRepHeadExpand" class="container-headexpand">
               <!--
                <div class="container-headexpand__idReport">#999</div>
                <div class="container-headexpand__title">#Se rompió lavabo</div>
                <div class="container-headexpand__description">#Se rompió el.</div>

                <div class="container-headexpand__numEmpleadoNotif">#NumEmpleado</div>
                <div class="container-headexpand__location">LOCATION</div>
                <div class="container-headexpand__status">0</div>
                <div class="container-headexpand__notifiedDt">#999</div>
                <div class="container-headexpand__evidence">Evidence.jpg</div>
               -->
            </div>

            <div id="cntRepDtlEntries" class="container-reportDtlEntries  " >
                <div>AÚN NO HAY ENTRADAS EN EL REPORTE</div>
                <%--<div class="container-reportDtlEntry entry-left">

                    <div class="container-reportDtlEntry__fileNameEvidence">Mi evidencia1.jpeg</div>
                    <div class="container-reportDtlEntry__title">Evidencia Owner</div>
                    <div class="container-reportDtlEntry__description">Descripcion del Owner</div>
                    <div class="container-reportDtlEntry__fechaHoraEntry">2021-1-4 12:34:00</div>

                </div>

                
                <div class="container-reportDtlEntry ">

                    <div class="container-reportDtlEntry__fileNameEvidence">Mi evidencia2.jpeg</div>
                    <div class="container-reportDtlEntry__title">Evidencia </div>
                    <div class="container-reportDtlEntry__description">Descripcion Normal</div>
                    <div class="container-reportDtlEntry__fechaHoraEntry">2021-1-4 12:34:00</div>

                </div>--%>

            </div>


            <div id="cntRepDtlUserInput" class="container-repdtl__user-input">
                <textarea id="txtRepDtlUserInput" class="container-repdtl__user-input"  rows="5" cols="50"></textarea>
                <input id="btnSendRepDtlUpdate" type="button" class="container-repdtl__send" value="Enviar" />
                <select name="selStat" id="selStat" >
                    <option value="0">PENDIENTE</option>
                    <option value="1">EN PROCESO</option>
                    <option value="2">COMPLETADO</option>
                </select>
                <input id="btnSaveStatus" type="button" value="Guardar"/>
            </div>


        </div>


    </div>

    <!-- <script src="webcam.js" type="text/javascript"></script> -->
    <script src="Content/jquery-3.6.0.min.js" type="text/javascript"></script>
    <script
        type="text/javascript"
        src="Content/webcam-easyV.js"></script>
    <script src="Content/NuevoReporte.js" type="text/javascript"></script>


</body>
</html>


