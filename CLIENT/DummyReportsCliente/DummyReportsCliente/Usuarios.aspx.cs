using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DummyReportsCliente
{
    public partial class Usuarios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //cargar tabla con usuarios y botón para el edit del nivel
        }

    /*    private DataTable GetUsersTable(List<IReportObject> users)



        {
            DataTable usersTable = SetupColumns();

            foreach (User current in users)
            {

                DataRow currenDR = usersTable.NewRow();

                currenDR["ID"] = current.IdUser;
                currenDR["NumEmpleado"] = current.NumEmpleado;
                currenDR["Usuario"] = current.UserName;
                currenDR["Pass"] = current.Pass;
                currenDR["Nombre"] = current.Name;
                currenDR["Activo"] = current.IsEnabled;
                currenDR["NivelUsuario"] = current.AccessLevel;

                usersTable.Rows.Add(currenDR);

            }


            return usersTable;

        }*/

        private DataTable SetupColumns()
        {
            DataTable usersTable = new DataTable("Usuarios");

            DataColumn dataColumn = new DataColumn();
            dataColumn.DataType = Type.GetType("System.Int32");
            dataColumn.ColumnName = "ID";
            dataColumn.AutoIncrement = false;
            dataColumn.Caption = "ID";
            dataColumn.ReadOnly = false;
            dataColumn.Unique = false;
            usersTable.Columns.Add(dataColumn);



            dataColumn = new DataColumn();
            dataColumn.DataType = Type.GetType("System.String");
            dataColumn.ColumnName = "NumEmpleado";
            dataColumn.AutoIncrement = false;
            dataColumn.Caption = "# Empleado";
            dataColumn.ReadOnly = false;
            dataColumn.Unique = false;
            usersTable.Columns.Add(dataColumn);


            dataColumn = new DataColumn();
            dataColumn.DataType = Type.GetType("System.String");
            dataColumn.ColumnName = "Usuario";
            dataColumn.AutoIncrement = false;
            dataColumn.Caption = "Usuario";
            dataColumn.ReadOnly = false;
            dataColumn.Unique = false;
            usersTable.Columns.Add(dataColumn);


            dataColumn = new DataColumn();
            dataColumn.DataType = Type.GetType("System.String");
            dataColumn.ColumnName = "Pass";
            dataColumn.AutoIncrement = false;
            dataColumn.Caption = "Pass";
            dataColumn.ReadOnly = false;
            dataColumn.Unique = false;
            usersTable.Columns.Add(dataColumn);

            dataColumn = new DataColumn();
            dataColumn.DataType = Type.GetType("System.String");
            dataColumn.ColumnName = "Nombre";
            dataColumn.AutoIncrement = false;
            dataColumn.Caption = "Nombre";
            dataColumn.ReadOnly = false;
            dataColumn.Unique = false;
            usersTable.Columns.Add(dataColumn);

            dataColumn = new DataColumn();
            dataColumn.DataType = Type.GetType("System.String");
            dataColumn.ColumnName = "Activo";
            dataColumn.AutoIncrement = false;
            dataColumn.Caption = "Activo";
            dataColumn.ReadOnly = false;
            dataColumn.Unique = false;
            usersTable.Columns.Add(dataColumn);

            dataColumn = new DataColumn();
            dataColumn.DataType = Type.GetType("System.String");
            dataColumn.ColumnName = "NivelUsuario";
            dataColumn.AutoIncrement = false;
            dataColumn.Caption = "Nivel Usuario";
            dataColumn.ReadOnly = false;
            dataColumn.Unique = false;
            usersTable.Columns.Add(dataColumn);

            return usersTable;
        }

    }
}