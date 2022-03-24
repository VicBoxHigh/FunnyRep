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
	            ROLLBACK TRANSACTION CreateReportDtlTran;
            END CATCH
            ";

        public ErrorFlag Create(IReportObject reportObject, out string error)
        {
            ReportDtlEntry reportDtlEntry = reportObject as ReportDtlEntry;
            Evidence evidence = reportDtlEntry.Evidence;
            error = "";

            SqlCommand command = new SqlCommand(QUERY_INSERT_EVIDENCE_AND_REPORT, ConexionBD.getConexion());

            if (evidence != null)
            {
                command.Parameters.Add("@fileName", System.Data.SqlDbType.Int).Value = reportDtlEntry.Evidence;
                command.Parameters.Add("@path", System.Data.SqlDbType.Int).Value = reportDtlEntry.Evidence;

            }
            command.Parameters.Add("@idReport", System.Data.SqlDbType.Int).Value = reportDtlEntry.Evidence;
            command.Parameters.Add("@titleUpdate", System.Data.SqlDbType.VarChar).Value = reportDtlEntry.Evidence;
            command.Parameters.Add("@description", System.Data.SqlDbType.VarChar).Value = reportDtlEntry.Evidence;
            command.Parameters.Add("@dtUpdate", System.Data.SqlDbType.DateTime).Value = reportDtlEntry.Evidence;
            command.Parameters.Add("@isOwnerUpdate", System.Data.SqlDbType.Bit).Value = reportDtlEntry.Evidence;


            using (SqlDataReader reader = command.ExecuteReader())
            {
                try
                {
                    reader.NextResult();
                    if (reader.RecordsAffected == 0) //si el segundo INSERT falla (los rowsAffected son  = 0), el Rollback se realizó y
                        return Flags.ErrorFlag.ERROR_NO_AFECTED_RECORDS;

                    else return Flags.ErrorFlag.ERROR_OK_RESULT;

                }
                catch (SqlException ex)
                {
                    return ErrorFlag.ERROR_CONNECTION_DB;
                }


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
                  ,E.IdEvidence
                  ,RDE.[TitleUpdate]
                  ,RDE.[Description]
                  ,RDE.[DTUpdate]
                  ,RDE.[isOwnerUpdate]
	              ,E.[FileName]
	              ,E.[Path]

              FROM [ReportDtlEntry]  RDE
              LEFT JOIN Evidence E on RDE.IdEvidence = E.IdEvidence
              Where RDE.IdReport = @IdReport
              Order by DTUpdate ASC
            ";
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

                    if (!reader.HasRows) return ErrorFlag.ERROR_RECORD_NOT_EXISTS;

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
            Evidence evidence = null;

            evidence = reader["E.IdEvidence"] == null ? null : new Evidence(
                (int)reader["E.IdEvidence"],
                reader["FileName"].ToString(),
                reader["Path"].ToString()
                ); ;

            ReportDtlEntry reportDtlEntry = new ReportDtlEntry
                (
                (int)reader["IdReportDtlEntry"],
                (int)reader["IdReport"],
                evidence,
                reader["RDE.Title"].ToString(),
                reader["RDE.Description"].ToString(),
                (bool)reader["isOwnerUpdate"]

                );

            return reportDtlEntry;
        }

        public ErrorFlag Update(IReportObject reportObject, out string error)
        {
            throw new NotImplementedException();
        }
    }
}