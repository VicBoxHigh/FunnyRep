<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DummyReportsCliente._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <div id="cntNewRep" class="containerNewReport">

        <input id="txtTitle" type="text" name="title" id="titleReport" placeholder="Titulo"></input>

        <textarea type="text" rows="5" id="txtDescriptionReport" name="descriptionReport" placeholder="Describir problema"></textarea>

        <input type="text" id="txtLugar" placeholder="Lugar" />

        <input type="button" id="btnGetLocation" value="Marcar ubicacion" />


        <span>
            <video id="webcam" autoplay playsinline></video>
            <canvas id="canvas" class="d-none"></canvas>

        </span>


        <input id="buttonSnap" type="button" value="Tomar foto" />


        <input type="button" id="btnGuardar" value="Guardar" />

    </div>


    <script src="https://code.jquery.com/jquery-3.6.0.min.js" integrity="sha256-/xUj+3OJU5yExlq6GSYGSHk7tPXikynS7ogEvDej/m4=" crossorigin="anonymous"></script>
    <!-- <script src="webcam.js" type="text/javascript"></script> -->
    <script
        type="text/javascript"
        src="Content/webcam-easyV.js"></script>
    <script src="Content/NuevoReporte.js" type="text/javascript"></script>

</asp:Content>
