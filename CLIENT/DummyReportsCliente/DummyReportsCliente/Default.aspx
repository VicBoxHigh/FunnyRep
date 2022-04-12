<!DOCTYPE html>
<html>
<head>
    <title></title>
    <link rel="stylesheet" href="Content/Default.css">
    <link rel="stylesheet" href="Content/normalize.css">
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

</head>
<body>



    <input id="btnTogleNewRep" type="button" value="Nuevo Reporte" />
    <div class="main-container">


        <div class="list-btn-container">
            <img alt="Lista de reportes" src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAABmJLR0QA/wD/AP+gvaeTAAAA0klEQVQ4jWNkgILiKUflGZj+iTAQA/4xvenNsX7IwMDAwAITk5IV7lZVlw4lRv/tm09XMzAwhKEYwMLM9EtAgIcoB7AwM/2CsZmI0oHPMHSBX79+Mzx7+pZBSlqY4dnTtyhyfLxcDEIifPgNYGNjZVBQlGBgYGCA06MuGHIuwAWe3HnCIMXHwvDq3bf/P798g5tMdErk5mRhUJfmY3jz4ce5sggDFwwX/Pn7j+3Dhy84DXj37uu/dU/eXcwO0DZBFmeEMQhlZ+Z//wW6cuz2oosDAD77WY+9Yh4bAAAAAElFTkSuQmCC" />
            <input type="checkbox" class="checkbox" />

            <div id="cntRepHeads" class="container-heads">
                <div class="item-report-head">AÚN NO HAY ENTRADAS EN EL REPORTE</div>

            </div>

        </div>

        <div id="cntNewRep" class="containerNewReport">

            <input id="txtTitle" type="text" name="title" id="titleReport" placeholder="Titulo" />

            <textarea type="text" rows="5" id="txtDescriptionReport" name="descriptionReport" placeholder="Describir problema"></textarea>

            <input type="text" id="txtLugar" placeholder="Lugar" />

            <input type="button" id="btnGetLocation" value="Marcar ubicacion" />


            <video id="webcam" autoplay playsinline></video>
            <canvas id="canvas" class="d-none"></canvas>


            <!--  <input id="buttonSnap" type="button" value="Tomar foto" /> -->


            <input type="button" id="btnGuardar" value="Guardar" />

        </div>


        <div id="cntRepDtl" class="container-repdtl">

            <div id="cntRepHeadExpand" class="container-headexpand">
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
    <script
        type="text/javascript"
        src="Content/webcam-easyV.js"></script>
    <script src="Content/NuevoReporte.js" type="text/javascript"></script>
    <script src="Content/NuevoReporte_Cam.js" type="text/javascript"></script>


</body>
</html>


