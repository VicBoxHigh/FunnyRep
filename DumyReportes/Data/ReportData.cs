using DumyReportes.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DumyReportes.Data
{
    public class ReportData
    {

        public static string QUERY_CREATE_LOCATION =
            @" INSERT INTO [dbo].[Location]
           ([Description]
           ,[lat]
           ,[long])
            VALUES
           (@Description
           ,@Lat
           ,@Lon)";

        public static string QUERY_CREATE_USER =
            @"                
             INSERT INTO [dbo].[User]
                   ([NumEmpleado]
                   ,[UserName]
                   ,[Pass]
                   ,[IsEnabled]
                   ,[Level])
             VALUES
                   (@NumEmpleado,
                   @User
                   ,@Pass
                   ,@IsEnable
                   ,@UserLevel)
               
            ";


        //Insertará un row para tabla usuario
        public static Flags.ErrorFlag createUser(User user, out string error)
        {
            error = "";
            Flags.ErrorFlag result;
            SqlCommand command = new SqlCommand(QUERY_CREATE_USER, ConexionBD.getConexion());
            command.Parameters.Add("@NumEmpleado", System.Data.SqlDbType.VarChar).Value = user.NumEmpleado;
            command.Parameters.Add("@User", System.Data.SqlDbType.VarChar).Value = user.UserName;
            command.Parameters.Add("@Pass", System.Data.SqlDbType.VarChar).Value = user.Pass;
            command.Parameters.Add("@IsEnabled", System.Data.SqlDbType.Bit).Value = user.IsEnable;
            command.Parameters.Add("@Level", System.Data.SqlDbType.Int).Value = user.AccessLevel;

            try
            {
                int a = command.ExecuteNonQuery();
                result = Flags.ErrorFlag.ERROR_OK_RESULT;
            }
            catch (SqlException ex)
            {
                error = ex.Message;
                result = Flags.ErrorFlag.ERROR_CONNECTION_DB;
            }

            
            return result;

        } 




    }
}