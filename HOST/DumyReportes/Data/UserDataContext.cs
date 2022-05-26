using DumyReportes.Flags;
using DumyReportes.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DumyReportes.Data
{
    public class UserDataContext : IDataOperation
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
             IF NOT EXISTS(
	            SELECT UserName FROM [User] WHERE UserName =  @User
              ) 
              BEGIN
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
                               ,@IsEnabled
                               ,@UserLevel);
              END
   
            ";


        public static string QUERY_CREDENTIALS_EXIST =
            @"
               SELECT U.* ,U.[Name] AS U_Name, AL.[Name] AS A_Name
                FROM [User] U
                INNER JOIN AccessLevel AL ON U.[Level] = AL.[Level]
                Where UserName = @user and  Pass =  @pass and IsEnabled = 1  ;
 
            ";
        //La usa el Login Validator Helper
        internal ErrorFlag CredentialsExist(string userName, string password, out User userResult, out string errorResult)
        {
            userResult = null;
            ErrorFlag operationResult = ErrorFlag.ERROR_NOT_EXISTS;

            SqlCommand command = new SqlCommand(QUERY_CREDENTIALS_EXIST, ConexionBD.getConexion());
            command.Parameters.Add("@user", System.Data.SqlDbType.VarChar).Value = userName;
            command.Parameters.Add("@pass", System.Data.SqlDbType.VarChar).Value = password;

            //bad password?
            errorResult = "";

            try
            {
                using (command)
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        errorResult = "Credenciales inválidas.";
                        operationResult = ErrorFlag.ERROR_NOT_EXISTS;

                    }
                    else if (reader.Read())
                    {
                        userResult = (User)InstanceFromReader(reader);
                        operationResult = ErrorFlag.ERROR_OK_RESULT;
                    }
                }


            }
            catch (SqlException ex)
            {
                errorResult = "Error al intentar válidar credenciales.";
                operationResult = ErrorFlag.ERROR_CONNECTION_DB;
            }

            return operationResult;
        }

        private string QUERY_EXIST_EMPLOYEE = @"

        SELECT TOP (1) [NumEmpleado]
            FROM dbo.[Empleados]
            WHERE NumEmpleado = @numEmpleado

        ";

        public ErrorFlag EmployeeExists(string numEmpleado)
        {
            throw new NotImplementedException();
            /* ErrorFlag result = ErrorFlag.ERROR_OK_RESULT;
             SqlConnection sqlConnection = ConexionBD.getConexion(ConexionBD.ConnectionDB.CHECADAS);
             using (SqlCommand command = new SqlCommand(QUERY_EXIST_EMPLOYEE, sqlConnection))
             {
                 command.Parameters.Add("@numEmpleado", System.Data.SqlDbType.VarChar).Value = numEmpleado;
                 try
                 {

                     using (SqlDataReader sqlDataReader = command.ExecuteReader())
                     {

                         if (!sqlDataReader.HasRows) result = ErrorFlag.ERROR_NOT_EXISTS;


                     }
                 }
                 catch (SqlException ex)
                 {
                     result = ErrorFlag.ERROR_CONNECTION_DB;
                 }
             }


             return result;*/
        }

        //Insertará un row para tabla usuario


        public ErrorFlag Create(IReportObject reportObject, out string error)
        {
            User user = reportObject as User;
            error = "";
            Flags.ErrorFlag result;
            SqlCommand command = new SqlCommand(QUERY_CREATE_USER, ConexionBD.getConexion());
            command.Parameters.Add("@NumEmpleado", System.Data.SqlDbType.VarChar).Value = user.NumEmpleado;
            command.Parameters.Add("@User", System.Data.SqlDbType.VarChar).Value = user.UserName;
            command.Parameters.Add("@Pass", System.Data.SqlDbType.VarChar).Value = user.Pass;
            command.Parameters.Add("@IsEnabled", System.Data.SqlDbType.Bit).Value = user.IsEnabled;
            command.Parameters.Add("@UserLevel", System.Data.SqlDbType.Int).Value = user.AccessLevel;

            try
            {
                int a = command.ExecuteNonQuery();
                if (a == 0)//ya existe
                    result = ErrorFlag.ERROR_RECORD_EXISTS;
                if (a > 0)//siempre será 1
                    result = Flags.ErrorFlag.ERROR_OK_RESULT;
                else //a<0
                    result = ErrorFlag.ERROR_NO_AFECTED_RECORDS;
            }
            catch (SqlException ex)
            {
                error = ex.Message;
                result = Flags.ErrorFlag.ERROR_CONNECTION_DB;
            }


            return result;
        }

        public static string QUERY_GET_USER =
            @"
            SELECT TOP (1000) [IdUser]
                  ,[NumEmpleado]
                  ,[UserName]
                  ,[Pass]
                  ,U.[Name] AS U_Name
                  ,[IsEnabled]
                  ,U.[Level]
				  ,AL.[Name] AS A_Name
              FROM [dbo].[User] U
			  INNER JOIN AccessLevel AL ON U.[Level] = AL.[Level]
              
              
              

            ";
        //Where IdUser = @IdUser
        public ErrorFlag GetAll(out List<IReportObject> users, out string error)
        {
            error = "";
            Flags.ErrorFlag result;
            users = new List<IReportObject>();

            SqlCommand command = new SqlCommand(QUERY_GET_USER + "WHERE IdUser > 1 order by IdUser ASC"/*ALL*/, ConexionBD.getConexion());
            try
            {

                using (SqlDataReader sqlDataReader = command.ExecuteReader())
                {

                    if (!sqlDataReader.HasRows) result = Flags.ErrorFlag.ERROR_NOT_EXISTS;
                    else result = Flags.ErrorFlag.ERROR_OK_RESULT;

                    while (sqlDataReader.Read())
                    {
                        users.Add(InstanceFromReader(sqlDataReader));

                    }


                }
            }
            catch (SqlException ex)
            {
                result = Flags.ErrorFlag.ERROR_DATABASE;
                error = ex.Message;
            }

            return result;

        }

        //token lo usa
        public ErrorFlag Get(int iduser, out IReportObject user, out string error)
        {
            error = "";
            user = null;
            Flags.ErrorFlag result;

            SqlCommand command = new SqlCommand(QUERY_GET_USER + " Where IdUser = @IdUser order by IdUser ASC", ConexionBD.getConexion());
            command.Parameters.Add("@IdUser", System.Data.SqlDbType.Int).Value = iduser;


            try
            {
                using (command)
                using (SqlDataReader sqlDataReader = command.ExecuteReader())
                {

                    if (!sqlDataReader.HasRows)
                    {
                        error = "Credenciales no validas";
                        return Flags.ErrorFlag.ERROR_NOT_EXISTS;
                    }

                    if (sqlDataReader.Read())
                    {

                        user = InstanceFromReader(sqlDataReader);
                        result = Flags.ErrorFlag.ERROR_OK_RESULT;
                    }
                    else
                    {
                        error = "Error al leer datos, intente más tarde";
                        result = Flags.ErrorFlag.ERROR_NOT_EXISTS;
                    }

                }

            }
            catch (SqlException ex)
            {
                error = ex.Message;
                result = Flags.ErrorFlag.ERROR_CONNECTION_DB;
            }


            return result;
        }


        public IReportObject InstanceFromReader(SqlDataReader reader)
        {

            User user = new User(
                   (string)reader["NumEmpleado"],
                   (string)reader["UserName"],
                   (string)reader["Pass"],
                   Boolean.Parse(reader["IsEnabled"].ToString()),
                   (Flags.AccessLevel)reader["Level"]
               );

            if (user != null)
            {
                user.IdUser = (int)reader["IdUser"];
                user.Name = reader["U_Name"].ToString();
                user.AccessLevelName = reader["A_Name"].ToString();
            }

            return user;
        }

        /*
                public static string QUERY_UPDATE_USER =
                    @"
                        UPDATE [dbo].[User]
                           SET [NumEmpleado] = @NumEmpleado
                              ,[UserName] = @UserName
                              ,[Pass] = @Pass
                              ,[IsEnabled] = @IsEnabled
                              ,[Level] = @Level
                         WHERE [User].IdUser = @IdUser

                        ";*/

        /*  public static Flags.ErrorFlag UpdateUser(User updatedUser, out string error)
          {

          }*/
        public ErrorFlag Update(IReportObject reportObject, out string error)
        {
            User updatedUser = reportObject as User;
            error = "";
            Flags.ErrorFlag result;
            SqlCommand command = new SqlCommand("dbo.UpdateUserInfo", ConexionBD.getConexion());
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.Add("@idUser", System.Data.SqlDbType.Int).Value = updatedUser.IdUser;

            command.Parameters.Add("@numEmpleado", System.Data.SqlDbType.VarChar).Value = updatedUser.NumEmpleado;
            command.Parameters.Add("@userName", System.Data.SqlDbType.VarChar).Value = updatedUser.UserName;
            command.Parameters.Add("@pass", System.Data.SqlDbType.VarChar).Value = updatedUser.Pass;
            command.Parameters.Add("@isEnabled", System.Data.SqlDbType.Bit).Value = updatedUser.IsEnabled ? 1 : 0;
            command.Parameters.Add("@level", System.Data.SqlDbType.Int).Value = (int)updatedUser.AccessLevel;

            try
            {
                int a = command.ExecuteNonQuery();
                if (a == 0)
                {
                    result = Flags.ErrorFlag.ERROR_NO_UPDATED_RECORDS;

                    error = "El registro no existe o fue cambiado, No se guardaron cambios.";
                }
                else result = Flags.ErrorFlag.ERROR_OK_RESULT;
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                result = ErrorFlag.ERROR_CONFLICT_CANT_DELETE;
                error = "El usuario tiene ligado almenos 1 reporte, por lo tanto no es posible eliminarlo.";

            }
            catch (SqlException ex)
            {
                result = ErrorFlag.ERROR_DATABASE;
                error = "Error desconocido en base de datos";
            }


            return result;

        }

        public static string QUERY_DELETE_USER =
            @"
                DELETE FROM [dbo].[User]
                WHERE [User].IdUser  = @IdUser
            ";


        public ErrorFlag Delete(int id, out string error)
        {
            //throw new NotImplementedException();
            error = "";
            Flags.ErrorFlag result;
            SqlCommand command = new SqlCommand(QUERY_DELETE_USER, ConexionBD.getConexion());
            command.Parameters.Add("@IdUser", System.Data.SqlDbType.Int).Value = id;

            try
            {
                int rowsModified = command.ExecuteNonQuery();
                if (rowsModified == 0) result = Flags.ErrorFlag.ERROR_NOT_EXISTS;
                result = ErrorFlag.ERROR_OK_RESULT;

            }
            catch (SqlException ex) when ( ex.Number == 547)
            {
                result = ErrorFlag.ERROR_CONFLICT_CANT_DELETE;
                error = "El usuario tiene ligado almenos 1 reporte, por lo tanto no es posible eliminarlo.";

            }
            catch(SqlException ex)
            {
                result = ErrorFlag.ERROR_DATABASE;
                error = "Error desconocido en base de datos";
            }


            return result;
        }

        public static string QUERY_EXIST_USER =
            @"SELECT TOP 1 FROM [ReportApp].[dbo].[User] Where [User].IdUser =  @IdUser";
        [Obsolete]
        public ErrorFlag Exists(int id, out string error)
        {
            throw new NotImplementedException();
            /*error = "";
            ErrorFlag result;
            SqlCommand command = new SqlCommand(QUERY_UPDATE_USER, ConexionBD.getConexion());
            command.Parameters.Add("@IdUser", System.Data.SqlDbType.Int).Value = id;
            using (command)
                try
                {
                    int a = command.ExecuteNonQuery();
                    if (a == 0) result = ErrorFlag.ERROR_NOT_EXISTS;
                    else result = ErrorFlag.ERROR_OK_RESULT;
                }
                catch (SqlException ex)
                {
                    error = ex.Message;
                    result = ErrorFlag.ERROR_CONNECTION_DB;
                }


            return result;*/

        }

        public ErrorFlag Detail(int id, out List<IReportObject> reportObjects, out string error)
        {
            throw new NotImplementedException();

        }

        public static string QUERY_EXISTS =
           @"SELECT TOP (1) * FROM [ReportApp].[dbo].[User] Where [User].UserName =  @IdUser and [User].Pass ) = @Pass";
        [Obsolete]
        public ErrorFlag Exist(string userName, string pass, out User user)
        {
            throw new NotImplementedException();
            /* ErrorFlag result;
             SqlCommand command = new SqlCommand(QUERY_UPDATE_USER, ConexionBD.getConexion());
             command.Parameters.Add("@IdUser", System.Data.SqlDbType.VarChar).Value = userName;
             command.Parameters.Add("@Pass", System.Data.SqlDbType.VarChar).Value = pass;
             user = null;
             result = ErrorFlag.ERROR_OK_RESULT;
             try
             {
                 using (SqlDataReader reader = command.ExecuteReader())
                 {

                     if (reader.HasRows)
                         user = InstanceFromReader(reader) as User;
                     else result = ErrorFlag.ERROR_NOT_EXISTS;
                 }

             }
             catch (SqlException ex)
             {
                 result = ErrorFlag.ERROR_CONNECTION_DB;
             }


             return result;*/

        }


    }
}