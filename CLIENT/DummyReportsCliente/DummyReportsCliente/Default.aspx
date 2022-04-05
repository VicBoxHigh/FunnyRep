<!DOCTYPE html>
<html>
<head>
    <title></title>
        <link rel="stylesheet" href="Content/Default.css">
</head>
<body>
    <div class="main-container">

        <div id="cntRepHeads" class="container-heads">
            <div class="item-report-head">
                <div class="item-report-head__idReport">#999 </div>
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

            <input id="buttonSnap" type="button" value="Tomar foto" />


            <input type="button" id="btnGuardar" value="Guardar" />

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


