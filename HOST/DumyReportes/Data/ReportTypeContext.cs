using DumyReportes.Flags;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DumyReportes.Data
{
    public class ReportTypeContext
    {

        public string QUERY_GET_ALL_REPORT_TYPES =
            @"

                SELECT TOP (1000) [IdReportType]
                      ,[Name]
                  FROM [ReportApp9].[dbo].[ReportType]

            ";

        public ErrorFlag GetAll(out Dictionary<int,string> reportTypes, out string error)
        {

            SqlCommand command = new SqlCommand(QUERY_GET_ALL_REPORT_TYPES, ConexionBD.getConexion());

            reportTypes = new Dictionary<int,string>();
            ErrorFlag resultGet = ErrorFlag.ERROR_OK_RESULT;
            error = "";

            using (command)
            using (SqlDataReader reader = command.ExecuteReader())
            {

                if (!reader.HasRows)
                {
                    resultGet = ErrorFlag.ERROR_NOT_EXISTS;
                    error = "Error al intentar obtener los tipos de reporte";
                }
                while (reader.Read())
                    reportTypes.Add((int)reader["IdReportType"],reader["Name"].ToString());
            }

            
            return resultGet;

        }




    }
}