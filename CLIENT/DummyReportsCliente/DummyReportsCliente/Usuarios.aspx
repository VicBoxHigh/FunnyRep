<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" Inherits="DummyReportsCliente.Usuarios" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <title>Usuarios</title>
    <link rel="stylesheet" href="Content/normalize.css"/>

    <link rel="stylesheet" href="Content/Usuarios.css"/>
 </head>
<body>
    <form id="form1" runat="server">
        <div class="virtual-body">
            <table  id="usersTable">

                <tr>

                    <th>ID</th>
                    <th>Num Empleado</th>
                    <th>Usuario</th>
                    <th>Contraseña</th>
                    <th>Nombre</th>
                    <th>Activo</th>
                    <th>Nivel</th>

                </tr>
                 

            </table>


        </div>
    </form>

    <script src="Content/jquery-3.6.0.min.js" type="text/javascript"></script>
    <script src="Content/Util.js" type="text/javascript"> </script>
    <script src="Content/Usuarios.js" type="text/javascript"></script>
</body>
</html>
