<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" Inherits="DummyReportsCliente.Usuarios" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <title>Usuarios</title>
    <link rel="stylesheet" href="Content/normalize.css" />

    <link rel="stylesheet" href="Content/Usuarios.css" />
</head>
<body>
    <form id="form1" runat="server">
            <div class="header">
                
                <input type="button" value="← Atrás" id="btnAtras" />
            </div>

        <div class="virtual-body">


            <table id="usersTable">
                <thead>
                    <tr>

                        <th>ID</th>
                        <th>Num Empleado</th>
                        <th>Usuario</th>
                        <th>Contraseña</th>
                        <th>Nombre</th>
                        <th>Activo</th>
                        <th>Nivel</th>
                        <th>Acciones</th>

                    </tr>

                </thead>
                <tbody>
                    <tr>

                        <td data-th="ID: "></td>
                        <td data-th="NumEmpleado: ">
                            <input type="text" id="txtNewNumEmpleado" /></td>
                        <td data-th="UserName: ">
                            <input type="text" id="txtNewUsuario" /></td>
                        <td data-th="Pass: ">
                            <input type="text" id="txtNewContrasena" /></td>
                        <td data-th="Name: ">
                            <input type="text" id="txtNewNombre" /></td>

                        <td data-th="Enabled: ">
                            <select type="text" id="selNewStatus">
                                <option value="1" selected>ACTIVO </option>
                                <option value="0">INACTIVO </option>
                            </select>
                        </td>
                        <td data-th="Level: ">
                            <select type="text" id="selNewLevel">
                                <option value="0" selected>BASICO </option>
                                <option value="1">AGENTE </option>
                                <option value="2">ADMINISTRADOR </option>
                                <option value="3">TI </option>
                            </select>
                        </td>
                        <td>
                            <input type="button" value="Nuevo Usuario" />
                        </td>
                    </tr>
                </tbody>
            </table>


        </div>
    </form>

    <script src="Content/jquery-3.6.0.min.js" type="text/javascript"></script>
    <script src="Content/Util.js" type="text/javascript"> </script>
    <script src="Content/Usuarios.js" type="text/javascript"></script>
</body>
</html>
