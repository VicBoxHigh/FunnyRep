﻿using DumyReportes.Flags;
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
                   ,@UserLevel)
               
            ";


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
                result = Flags.ErrorFlag.ERROR_OK_RESULT;
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
            SELECT  [IdUser]
                  ,[NumEmpleado]
                  ,[UserName]
                  ,[Pass]
                  ,[IsEnabled]
                  ,[Level]
              FROM [ReportApp].[dbo].[User]
              

            ";
        //Where IdUser = @IdUser
        public ErrorFlag GetAll(out List<IReportObject> users, out string error)
        {
            error = "";
            Flags.ErrorFlag result;
            users = new List<IReportObject>();

            SqlCommand command = new SqlCommand(QUERY_GET_USER /*ALL*/, ConexionBD.getConexion());
            try
            {

                using (SqlDataReader sqlDataReader = command.ExecuteReader())
                {

                    if (!sqlDataReader.HasRows) result = Flags.ErrorFlag.ERROR_RECORD_NOT_EXISTS;
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
     
        public ErrorFlag Get(int iduser, out IReportObject user, out string error)
        {
            error = "";
            user = null;
            Flags.ErrorFlag result;

            SqlCommand command = new SqlCommand(QUERY_GET_USER + " Where IdUser = @IdUser", ConexionBD.getConexion());
            command.Parameters.Add("@IdUser", System.Data.SqlDbType.Int).Value = iduser;


            try
            {

                using (SqlDataReader sqlDataReader = command.ExecuteReader())
                {

                    if (!sqlDataReader.HasRows) return Flags.ErrorFlag.ERROR_RECORD_NOT_EXISTS;

                    if (sqlDataReader.Read())
                    {
                        user = InstanceFromReader(sqlDataReader);
                        result = Flags.ErrorFlag.ERROR_OK_RESULT;
                    }
                    else
                    {
                        result = Flags.ErrorFlag.ERROR_RECORD_NOT_EXISTS;
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

            if (user != null) user.IdUser = (int)reader["IdUser"];

            return user;
        }


        public static string QUERY_UPDATE_USER =
            @"
                UPDATE [dbo].[User]
                   SET [NumEmpleado] = @NumEmpleado
                      ,[UserName] = @UserName
                      ,[Pass] = @Pass
                      ,[IsEnabled] = @IsEnabled
                      ,[Level] = @Level
                 WHERE [User].IdUser = @IdUser

                ";

      /*  public static Flags.ErrorFlag UpdateUser(User updatedUser, out string error)
        {
           
        }*/
        public ErrorFlag Update(IReportObject reportObject, out string error)
        {
            User updatedUser = reportObject as User;
            error = "";
            Flags.ErrorFlag result;
            SqlCommand command = new SqlCommand(QUERY_UPDATE_USER, ConexionBD.getConexion());
            command.Parameters.Add("@IdUser", System.Data.SqlDbType.Int).Value = updatedUser.IdUser;

            command.Parameters.Add("@NumEmpleado", System.Data.SqlDbType.VarChar).Value = updatedUser.NumEmpleado;
            command.Parameters.Add("@UserName", System.Data.SqlDbType.VarChar).Value = updatedUser.UserName;
            command.Parameters.Add("@Pass", System.Data.SqlDbType.VarChar).Value = updatedUser.Pass;
            command.Parameters.Add("@IsEnabled", System.Data.SqlDbType.Bit).Value = updatedUser.IsEnabled ? 1 : 0;
            command.Parameters.Add("@Level", System.Data.SqlDbType.Int).Value = (int)updatedUser.AccessLevel;

            try
            {
                int a = command.ExecuteNonQuery();
                if (a == 0) result = Flags.ErrorFlag.ERROR_NO_UPDATED_RECORDS;
                else result = Flags.ErrorFlag.ERROR_OK_RESULT;
            }
            catch (SqlException ex)
            {
                error = ex.Message;
                result = Flags.ErrorFlag.ERROR_CONNECTION_DB;
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
            error = "";
            Flags.ErrorFlag result;
            SqlCommand command = new SqlCommand(QUERY_DELETE_USER, ConexionBD.getConexion());
            command.Parameters.Add("@IdUser", System.Data.SqlDbType.Int).Value = id;

            try
            {
                int rowsModified = command.ExecuteNonQuery();
                if (rowsModified == 0) result = Flags.ErrorFlag.ERROR_RECORD_NOT_EXISTS;
                result = ErrorFlag.ERROR_OK_RESULT;

            }
            catch (SqlException ex)
            {
                result = ErrorFlag.ERROR_DATABASE;
                error = ex.Message;

            }


            return result;
        }

        public static string QUERY_EXIST_USER =
            @"SELECT TOP 1 FROM [ReportApp].[dbo].[User] Where [User].IdUser =  @IdUser";

        public ErrorFlag Exists(int id, out string error)
        {
            error = "";
            ErrorFlag result;
            SqlCommand command = new SqlCommand(QUERY_UPDATE_USER, ConexionBD.getConexion());
            command.Parameters.Add("@IdUser", System.Data.SqlDbType.Int).Value = id;

            try
            {
                int a = command.ExecuteNonQuery();
                if (a == 0) result = ErrorFlag.ERROR_RECORD_NOT_EXISTS;
                else result = ErrorFlag.ERROR_OK_RESULT;
            }
            catch (SqlException ex)
            {
                error = ex.Message;
                result = ErrorFlag.ERROR_CONNECTION_DB;
            }


            return result;

        }

        public ErrorFlag Detail(int id, out List<IReportObject> reportObjects, out string error)
        {
            throw new NotImplementedException();
            
        }
    }
}