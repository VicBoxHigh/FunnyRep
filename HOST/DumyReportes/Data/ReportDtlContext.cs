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
            @"

            IF NOT EXISTS (SELECT TOP(1) * FROM ReportApp.dbo.UserOwner_Report UOR where UOR.IdReport = @idReport order by DT DESC)
            BEGIN 
	            
	            INSERT INTO [dbo].[UserOwner_Report]
                       ([IdReport]
                       ,[IdUser]
                       ,[DT])
	            VALUES (
	                @idReport,
	                (SELECT TOP (1) IdReport FROM Report R Where R.IdReport = @idReport),
	                GETDATE()
                )


            END

            ";

        public static string QUERY_INSERT_REPORT_DTL =
            @"


		            INSERT INTO [dbo].[ReportDtlEntry]
                                    ([IdReport]
                                    ,[PathEvidence]
                                    ,[FileNameEvidence]
                                    ,[TitleUpdate]
                                    ,[Description]
                                    ,[DTUpdate]
                                    ,[isOwnerUpdate])
                                VALUES
                                    (
		                            @idReport
                                    ,@path
                                    ,@fileName
                                    ,@titleUpdate
                                    ,@description
                                    ,@dtUpdate
                                    ,@isOwnerUpdate
		                            );

            ";

        public ErrorFlag Create(IReportObject reportObject, out string error)
        {
            ReportDtlEntry reportDtlEntry = reportObject as ReportDtlEntry;

            error = "";
            ErrorFlag result;


            SqlConnection connection = ConexionBD.getConexion();
            using(connection)
            using (SqlCommand command = connection.CreateCommand())
            {
                SqlTransaction transaction = connection.BeginTransaction("INSERT REPORT DTL ENTRY");
                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = QUERY_CHECK_EXISTENCE_OWNER_OR_CREATE;
                    command.Parameters.Add("@idReport", System.Data.SqlDbType.VarChar).Value = reportDtlEntry.IdReport;

                    int rowsAffected = command.ExecuteNonQuery();

                    //si no hay entries, ese Reporte no tiene OWNER, por lo tanto se asignará al usuario que actualice primero.

                    command.CommandText = QUERY_INSERT_REPORT_DTL;
                    command.Parameters.Add("@fileName", System.Data.SqlDbType.VarChar).Value = reportDtlEntry.FileNameEvidence;
                    command.Parameters.Add("@path", System.Data.SqlDbType.VarChar).Value = reportDtlEntry.PathEvidence;

                    //command.Parameters.Add("@idReport", System.Data.SqlDbType.Int).Value = reportDtlEntry.IdReport;
                    command.Parameters.Add("@titleUpdate", System.Data.SqlDbType.VarChar).Value = reportDtlEntry.TitleUpdate;
                    command.Parameters.Add("@description", System.Data.SqlDbType.VarChar).Value = reportDtlEntry.Description;
                    command.Parameters.Add("@dtUpdate", System.Data.SqlDbType.DateTime).Value = reportDtlEntry.DTUpdate;
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
            SqlCommand command = new SqlCommand(QUERY_SELECT_REPORT_DTL_ENTRY, ConexionBD.getConexion());

            command.Parameters.Add("@IdReport", System.Data.SqlDbType.Int).Value = id;

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
            catch (SqlException ex)
            {
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
                IdReport = (int)reader["IdReport"],
                IdReportUpdate = (int)reader["IdReportDtlEntry"],
                TitleUpdate = reader["TitleUpdate"].ToString(),
                Description = reader["Description"].ToString(),
                IsOwnerUpdate = (bool)reader["isOwnerUpdate"],
                DTUpdate = DateTime.Parse(reader["DTUpdate"].ToString()),
                FileNameEvidence = reader["FileNameEvidence"].ToString(),
                PathEvidence = reader["PathEvidence"].ToString()
            };


            return reportDtlEntry;
        }

        public ErrorFlag Update(IReportObject reportObject, out string error)
        {
            throw new NotImplementedException();
        }
    }
}