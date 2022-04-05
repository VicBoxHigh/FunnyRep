<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="DummyReportsCliente.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <label for="checkToogleAdmn" title="Admin login"></label>
            <input id="chekToogleAdmn" name="checkToogleAdmn" type="checkbox" value ="Admin"  />
        </div>
        <div>
            <input id="txtUser" type="text" placeholder="Usuario" />
            <input id="txtPass" type="password" placeholder="Contraseña" />
            <input id="btnLogin" type="button" value="Iniciar sesión"/>
        </div>


    </form>
    <script src="Content/jquery-3.6.0.min.js" type="text/javascript"></script>
    <script src="Content/LoginReporte.js" type="text/javascript" ></script>

     

</body>
</html>
