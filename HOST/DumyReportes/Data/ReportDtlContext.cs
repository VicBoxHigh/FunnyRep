using DumyReportes.Flags;
using DumyReportes.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace DumyReportes.Data
{
    public class ReportDtlContext : IDataOperation
    {

        /*public static string QUERY_INSERT_REPORT_DTL =
            @"

                INSERT INTO [dbo].[ReportDtlEntry]
                           ([IdReport]
                           ,[IdEvidence]
                           ,[TitleUpdate]
                           ,[Description]
                           ,[DTUpdate]
                           ,[isOwnerUpdate])
                     VALUES
                           (
		                   @idReport
                           ,@idEvidence
                           ,@titleUpdate
                           ,@description
                           ,@dtUpdate
                           ,@isOwnerUpdate
		                   )
            ";

        public static string QUERY_INSERT_EVIDENCE =
            @"
                   
            INSERT INTO [dbo].[Evidence]
                       ([FileName]
                       ,[Path])
                 VALUES
                       @fileName
                       ,@path);

            ";*/


        public static string QUERY_CHECK_EXISTENCE_OWNER_OR_CREATE =
            //(SELECT TOP (1) IdUser FROM Report R Where R.IdReport = @idReport)
            @"

            IF NOT EXISTS (SELECT TOP(1) * FROM ReportApp.dbo.UserOwner_Report UOR where UOR.IdReport = @idReport order by DT DESC)
            BEGIN 
	            
	            INSERT INTO [dbo].[UserOwner_Report]
                       ([IdReport]
                       ,[IdUser]
                       ,[DT])
	            VALUES (
	                @idReport,
	                @idUserOwner,
	                GETDATE()
                )


            END

            ";
        /*   if (SELECT TOP(1) IdStatus FROM Report Where Report.IdReport =  1) != 2
   BEGIN

   SELECT  ;

   END*/
        public static string QUERY_INSERT_REPORT_DTL =
            @"


		            INSERT INTO [dbo].[ReportDtlEntry]
                                    ([IdReport]                                   
                                    ,[TitleUpdate]
                                    ,[Description]
                                    ,[DTUpdate]
                                    ,[isOwnerUpdate]
                                    ,[IdUserUpdate]
                                    )
                                VALUES
                                    (
		                            @idReport                                   
                                    ,@titleUpdate
                                    ,@description
                                    ,@dtUpdate
                                    ,@isOwnerUpdate
                                    ,@idUserUpdate
		                            );

            ";
        //Si el reporte aun no tiene un owner (persona asignada), relacionará el Owner que añada una entrada 
        [Obsolete("La db cambio, Los reportes ahora no se auto asignan, solo basta con insertar el EntryDtl, por tanto la lógica de transaction ya no es nnecesaria, usar función Create2.")]
        public ErrorFlag Create(IReportObject reportObject, User user, out string error)
        {
            ReportDtlEntry reportDtlEntry = reportObject as ReportDtlEntry;

            error = "";
            ErrorFlag result;


            SqlConnection connection = ConexionBD.getConexion();
            using (connection)
            using (SqlCommand command = connection.CreateCommand())
            {
                SqlTransaction transaction = connection.BeginTransaction("INSERT REPORT DTL ENTRY");
                command.Connection = connection;
                command.Transaction = transaction;

                try
                {


                    //si no hay entries, ese Reporte no tiene OWNER, por lo tanto se asignará al usuario que actualice primero.
                    //siempre que no sea un usuario level 0
                    //ID report en commún en ambas consulas del transaction
                    command.Parameters.Add("@idReport", System.Data.SqlDbType.VarChar).Value = reportDtlEntry.IdReport;

                    if (!user.AccessLevel.Equals(Flags.AccessLevel.PUBLIC))
                    {
                        command.CommandText = QUERY_CHECK_EXISTENCE_OWNER_OR_CREATE;
                        command.Parameters.Add("@idUserOwner", System.Data.SqlDbType.VarChar).Value = user.IdUser;
                        int rowsAffected = command.ExecuteNonQuery();

                    }
                    command.CommandText = QUERY_INSERT_REPORT_DTL;
                    command.Parameters.Add("@fileName", System.Data.SqlDbType.VarChar).Value = reportDtlEntry.FileNameEvidence;
                    command.Parameters.Add("@path", System.Data.SqlDbType.VarChar).Value = reportDtlEntry.PathEvidence;

                    //command.Parameters.Add("@idReport", System.Data.SqlDbType.Int).Value = reportDtlEntry.IdReport;
                    command.Parameters.Add("@titleUpdate", System.Data.SqlDbType.VarChar).Value = reportDtlEntry.TitleUpdate;
                    command.Parameters.Add("@description", System.Data.SqlDbType.VarChar).Value = reportDtlEntry.Description;
                    command.Parameters.Add("@dtUpdate", System.Data.SqlDbType.DateTime).Value = DateTime.Now;/*reportDtlEntry.DTUpdate*/;//el servidor se encarga 
                    command.Parameters.Add("@isOwnerUpdate", System.Data.SqlDbType.Bit).Value = reportDtlEntry.IsOwnerUpdate;
                    int rowsAffected2 = command.ExecuteNonQuery();
                    transaction.Commit();
                    result = ErrorFlag.ERROR_OK_RESULT;

                    transaction.Dispose();


                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    result = ErrorFlag.ERROR_CREATION_ENITITY;
                }
                catch (Exception ex)
                {
                    result = ErrorFlag.FATAL;
                }
            }



            return result;

        }

        public ErrorFlag Create2(IReportObject reportObject, User user, out string error)
        {
            ReportDtlEntry reportDtlEntry = reportObject as ReportDtlEntry;

            error = "";
            ErrorFlag result;

            using (SqlCommand command = new SqlCommand("dbo.InsertDtlEntry", ConexionBD.getConexion()))
            {
                try
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@idReport", System.Data.SqlDbType.VarChar).Value = reportDtlEntry.IdReport;
                    command.Parameters.Add("@idUserUpdate", System.Data.SqlDbType.Int).Value = user.IdUser;

                    command.Parameters.Add("@titleUpdate", System.Data.SqlDbType.VarChar).Value = reportDtlEntry.TitleUpdate;
                    command.Parameters.Add("@description", System.Data.SqlDbType.VarChar).Value = reportDtlEntry.Description;
                    command.Parameters.Add("@dtUpdate", System.Data.SqlDbType.DateTime).Value = DateTime.Now;/*reportDtlEntry.DTUpdate*/;//el servidor se encarga de asinar la fecha.
                    command.Parameters.Add("@isOwnerUpdate", System.Data.SqlDbType.Bit).Value = reportDtlEntry.IsOwnerUpdate;
                    int rowsAffected2 = command.ExecuteNonQuery();
                    result = ErrorFlag.ERROR_OK_RESULT;

                }
                catch (SqlException ex)
                {
                    error = "Error al intentar ingresar una entrada al reporte.";
                    result = ErrorFlag.ERROR_CREATION_ENITITY;
                }
                catch(InvalidCastException ice)
                {
                    result = ErrorFlag.ERROR_PARSE;
                    error = "Error en el formato del reporte";
                }
                catch (Exception ex)
                {
                    error = "Error desconocido";
                    result = ErrorFlag.UNKNOWN;
                }
            }



            return result;

        }

        public ErrorFlag Delete(int id, out string error)
        {
            throw new NotImplementedException();
        }

        public ErrorFlag Detail(int id, out List<IReportObject> reportObjects, out string error)
        {
            throw new NotImplementedException();
        }

        public ErrorFlag Exists(int id, out string error)
        {
            throw new NotImplementedException();
        }

        public ErrorFlag Get(int id, out IReportObject reportObject, out string error)
        {
            throw new NotImplementedException();
        }
        public static string QUERY_SELECT_REPORT_DTL_ENTRY =
         @"
            SELECT RDE.[IdReportDtlEntry]
                  ,RDE.[IdReport]
	              ,RDE.[FileNameEvidence]
	              ,RDE.[PathEvidence]
                  ,RDE.[TitleUpdate]
                  ,RDE.[Description]
                  ,RDE.[DTUpdate]
                  ,RDE.[isOwnerUpdate]
                  

              FROM [ReportDtlEntry]  RDE

              Where RDE.IdReport = @IdReport
              Order by DTUpdate ASC
            ";
        //Trae todos los ReportDtlEntry de un reporte especifico
        public ErrorFlag GetAll(int id, out List<IReportObject> reportObjects, out string error)
        {
            error = "";
            reportObjects = new List<IReportObject>();
            SqlCommand command = new SqlCommand("dbo.ReportDtlEntries" /*QUERY_SELECT_REPORT_DTL_ENTRY*/, ConexionBD.getConexion());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("@IdReportHeader", System.Data.SqlDbType.Int).Value = id;

            try
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    if (!reader.HasRows) return ErrorFlag.ERROR_NOT_EXISTS;

                    while (reader.Read())
                    {
                        reportObjects.Add(InstanceFromReader(reader));
                    }
                }

            }
            catch (IndexOutOfRangeException ioore)//cuando no encuentra alguna propiedad en el reader.
            {

                error = "Error al intentar leer la información obtenida.";
                return ErrorFlag.ERROR_PARSE;
            }
            catch (SqlException ex)
            {
                error = "Error desconocido.";
                return ErrorFlag.ERROR_DATABASE;
            }

            return ErrorFlag.ERROR_OK_RESULT;

        }
        public ErrorFlag GetAll(out List<IReportObject> reportObjects, out string error)
        {
            throw new NotImplementedException();
        }

        public IReportObject InstanceFromReader(SqlDataReader reader)
        {


            ReportDtlEntry reportDtlEntry = new ReportDtlEntry()
            {
                IdReportUpdate = (int)reader["IdReportDtlEntry"],
                IdReport = (int)reader["IdReport"],
                TitleUpdate = reader["TitleUpdate"].ToString(),
                Description = reader["Description"].ToString(),
                DTUpdate = DateTime.Parse(reader["DTUpdate"].ToString()),
                IsOwnerUpdate = (bool)reader["isOwnerUpdate"],
                UserWhoUpdate = new User()
                {
                    IdUser = (int)reader["IdUser"],
                    NumEmpleado = reader["NumEmpleado"].ToString(),
                    Name = reader["Name"].ToString(),
                    IsEnabled = (bool)reader["IsEnabled"],
                    AccessLevel = (AccessLevel)reader["Level"],
                    AccessLevelName = (reader["AccessLevelName"]).ToString()
                }

            };


            return reportDtlEntry;
        }

        public ErrorFlag Update(IReportObject reportObject, out string error)
        {
            throw new NotImplementedException();
        }

        ErrorFlag IDataOperation.Create(IReportObject reportObject, out string error)
        {
            throw new NotImplementedException();
        }
    }
}