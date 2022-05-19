<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="DummyReportsCliente.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <title></title>
    <link rel="stylesheet" href="Content/normalize.css"/>
    <link rel="stylesheet" href="Content/Login.css" />
</head>
<body>


    <form class="login-container" id="form1" runat="server">
        <img class="login-logo" src="Content/Logotipo.png" alt="Logo Marves" />
        <h1>Reportes</h1>
        <input id="txtUser" type="text" placeholder="Usuario" />
        <input id="txtPass" type="password" placeholder="Contraseña" />
        <span class="check-container">

            <input id="chekToogleAdmn" title="Admin login" name="checkToogleAdmn" checked type="checkbox" value="Admin" />
            <label for="checkToogleAdmn" title="Admin login">Administrador</label>
        </span>
        <input id="btnLogin" type="button" value="Iniciar sesión" />
    </form>
    <script src="Content/jquery-3.6.0.min.js" type="text/javascript"></script>
    <script src="Content/LoginReporte.js" type="text/javascript"></script>



</body>
</html>
