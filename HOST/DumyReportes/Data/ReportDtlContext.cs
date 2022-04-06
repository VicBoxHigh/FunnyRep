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


        public static string QUERY_INSERT_EVIDENCE_AND_REPORT =
            @"

            BEGIN TRY

	            BEGIN TRANSACTION CreateReportDtlTran;

		            INSERT INTO [dbo].[Evidence]
                                    ([FileName]
                                    ,[Path])
                                VALUES(
                                    @fileName
                                    ,@path);

		
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
                                    ,(SELECT SCOPE_IDENTITY())
                                    ,@titleUpdate
                                    ,@description
                                    ,@dtUpdate
                                    ,@isOwnerUpdate
		                            );


	            COMMIT TRANSACTION CreateReportDtlTran;

            END TRY
            BEGIN CATCH
                SELECT ERROR_MESSAGE() ERROR;
	            ROLLBACK TRANSACTION CreateReportDtlTran;
            END CATCH
            ";

        public ErrorFlag Create(IReportObject reportObject, out string error)
        {
            ReportDtlEntry reportDtlEntry = reportObject as ReportDtlEntry;
             
            error = "";
            ErrorFlag result;

            SqlCommand command = new SqlCommand(QUERY_INSERT_EVIDENCE_AND_REPORT, ConexionBD.getConexion());

            
                command.Parameters.Add("@fileName", System.Data.SqlDbType.VarChar).Value = reportDtlEntry.FileNameEvidence;
                command.Parameters.Add("@path", System.Data.SqlDbType.VarChar).Value = reportDtlEntry.PathEvidence;
 
            command.Parameters.Add("@idReport", System.Data.SqlDbType.Int).Value = reportDtlEntry.IdReport;
            command.Parameters.Add("@titleUpdate", System.Data.SqlDbType.VarChar).Value = reportDtlEntry.TitleUpdate;
            command.Parameters.Add("@description", System.Data.SqlDbType.VarChar).Value = reportDtlEntry.Description;
            command.Parameters.Add("@dtUpdate", System.Data.SqlDbType.DateTime).Value = reportDtlEntry.DTUpdate;
            command.Parameters.Add("@isOwnerUpdate", System.Data.SqlDbType.Bit).Value = reportDtlEntry.IsOwnerUpdate;


            using (SqlDataReader reader = command.ExecuteReader())
            {
                try
                {

                    result = ErrorFlag.ERROR_NO_AFECTED_RECORDS;

                    if (reader.RecordsAffected == 2) result = ErrorFlag.ERROR_OK_RESULT;
                    //!= 2 rows affected -> no cambios en DB , así que se puede generar un error code en el query
                    if (reader.HasRows)//si hay un ROWS significa exception interno en SQL
                    {
                        result = ErrorFlag.ERROR_CREATION_ENITITY;
                        //result = ErrorFlag.ERROR_NO_AFECTED_RECORDS;
                        //if (reader.Read())

                    }


                }
                catch (SqlException ex)
                {
                    result = ErrorFlag.ERROR_CONNECTION_DB;
                }

                return result;

            }
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
                  
                  ,RDE.[TitleUpdate]
                  ,RDE.[Description]
                  ,RDE.[DTUpdate]
                  ,RDE.[isOwnerUpdate]
	              ,RDE.[FileNameEvidence]
	              ,RDE.[PathEvidence]

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