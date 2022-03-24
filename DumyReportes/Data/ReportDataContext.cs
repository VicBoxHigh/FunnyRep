using DumyReportes.Flags;
using DumyReportes.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumyReportes.Data
{
    class ReportDataContext : IDataOperation
    {

        /*  public static string QUERY_INSERT_RERPORT =
              @"
                  INSERT INTO [dbo].[Report]
                             ([IdUserWhoNotified]
                             ,[IdLocation]
                             ,[IdStatus]
                             ,[NotifiedDT]
                              ,[Title]
                              ,[Description]
                              )
                       VALUES
                             (
                             @IdUserWhoNotified
                             ,@RepIdLocation
                             ,@IdStatus
                             ,@NotifiedDT
                              ,@RepTitle
                              ,@RepDescription)
              ";

          public static string QUERY_INSER_LOCATION =
              @"

                  INSERT INTO [dbo].[Location]
                             ([Description]
                             ,[lat]
                             ,[long])
                       VALUES
                             (
                              @LocDescription
                             ,@LocLat
                             ,@LocLong
                             )
              ";*/

        public static string QUERY_INSERT_REPORT_LOCATION =
             @"

            BEGIN TRY

	            BEGIN TRANSACTION CreateReportDtlTran;


                       INSERT INTO [dbo].[Location]
                            ([Description]
                            ,[lat]
                            ,[long])
                        VALUES
                            (
			                @LocDescription
                            ,@LocLat
                            ,@LocLong
		                    );


                        INSERT INTO [dbo].[Report]
                            ([IdUserWhoNotified]
                            ,[IdLocation]
                            ,[IdStatus]
                            ,[NotifiedDT]
                            ,[Title]
                            ,[Description]
                            )
                        VALUES
                            (
		                    @IdUserWhoNotified
                            ,(SELECT SCOPE_IDENTITY())
                            ,@IdStatus
                            ,@NotifiedDT
                            ,@RepTitle
                            ,@RepDescription);



	            COMMIT TRANSACTION CreateReportDtlTran;

            END TRY
            BEGIN CATCH
	            ROLLBACK TRANSACTION CreateReportDtlTran;
            END CATCH
            ";

        public ErrorFlag Create(IReportObject reportObject, out string error)
        {
            error = "";
            ErrorFlag result;
            Report report = reportObject as Report;

            //ya está validado
            SqlConnection connection = ConexionBD.getConexion();

            using (SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    command.Connection = connection;

                    command.CommandText = QUERY_INSERT_REPORT_LOCATION;
                    command.Parameters.Add("@LocDescription", System.Data.SqlDbType.VarChar).Value = report.Location.Description;
                    command.Parameters.Add("@LocLat", System.Data.SqlDbType.Decimal).Value = report.Location.lat;
                    command.Parameters.Add("@LocLong", System.Data.SqlDbType.Decimal).Value = report.Location.lon;


                    command.Parameters.Add("@IdUserWhoNotified", System.Data.SqlDbType.Int).Value = report.IdUserWhoNotified;
                    command.Parameters.Add("@IdStatus", System.Data.SqlDbType.Int).Value = (int)report.IdStatus;
                    command.Parameters.Add("@NotifiedDT", System.Data.SqlDbType.DateTime).Value = DateTime.Now;
                    command.Parameters.Add("@RepTitle", System.Data.SqlDbType.VarChar).Value = report.Title;
                    command.Parameters.Add("@RepDescription", System.Data.SqlDbType.VarChar).Value = report.Description;


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        result = ErrorFlag.ERROR_NO_AFECTED_RECORDS;
                        if (reader.NextResult())
                        {
                            //result = ErrorFlag.ERROR_NO_AFECTED_RECORDS;
                            //if (reader.Read())
                            if (reader.RecordsAffected == 1) result = ErrorFlag.ERROR_OK_RESULT;
                        }
                    }


                }
                catch (SqlException ex)
                {

                    result = Flags.ErrorFlag.ERROR_NO_AFECTED_RECORDS;

                }
                return result;

            }


        }

        public ErrorFlag Delete(int id, out string error)
        {
            error = "Un Reporte no es eliminable";
            return ErrorFlag.ERROR_DENIED_OPERATION;
        }

        //Get specific report details
        public ErrorFlag Detail(int id, out List<IReportObject> reportObjects, out string error)
        {
            throw new NotImplementedException();
        }

        public ErrorFlag Exists(int id, out string error)
        {
            throw new NotImplementedException();
        }
        public ErrorFlag Get(int iduser, out IReportObject reportObject, out string error)
        {
            throw new NotImplementedException();
        }

        public static string QUERY_GET_REPORT_BY_USER_WHONOTIFIED =
            @"
                SELECT Rep.IdReport, Rep.IdUserWhoNotified, Rep.NotifiedDT,Rep.Title,
                Loc.IdLocation, Loc.[Description], Loc.lat, Loc.long, 
                RS.IdStatus, RS.titleStatus,
                U.IdUser, U.NumEmpleado, U.Level
                FROM 
	                [Report] Rep
                  LEFT JOIN [Location] Loc
	                on Rep.IdLocation = Loc.IdLocation
                  Left JOIN ReportStatus RS
	                ON Rep.IdStatus = RS.IdStatus
                  LEFT JOIN [User] U
	                ON Rep.IdUserWhoNotified = U.IdUser
                Where U.IdUser = @IdUser
            ";

        public static string QUERY_GET_REPORT_BY_OWNER =
            @"
            SELECT
	            Rep.IdReport, Rep.IdUserWhoNotified, Rep.NotifiedDT,Rep.Title,
	            Loc.IdLocation, Loc.[Description], Loc.lat, Loc.long, 
	            RS.IdStatus, RS.titleStatus,
	            U.IdUser, U.NumEmpleado, U.[Level],        
	            UOR.[DT]
	        FROM [ReportApp].[dbo].[UserOwner_Report] UOR
	            LEFT JOIN [Report] Rep ON Rep.IdReport = UOR.IdReport 
	            LEFT JOIN [Location] Loc on Rep.IdLocation = Loc.IdLocation
	            Left JOIN ReportStatus RS ON Rep.IdStatus = RS.IdStatus
	            LEFT JOIN [User] U ON Rep.IdUserWhoNotified = U.IdUser
	        Where U.IdUser= @IdUser

            ";

        public ErrorFlag GetAllBy(bool isOwner, int idUser, out List<IReportObject> reportObjects, out string error)
        {
            error = "";
            Flags.ErrorFlag result;
            SqlCommand command = new SqlCommand(isOwner ? QUERY_GET_REPORT_BY_OWNER : QUERY_GET_REPORT_BY_USER_WHONOTIFIED, ConexionBD.getConexion());

            command.Parameters.Add("@IdUser", System.Data.SqlDbType.Int).Value = idUser;

            reportObjects = new List<IReportObject>();

            try
            {

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows) result = ErrorFlag.ERROR_OK_RESULT;
                    else
                    {
                        while (reader.Read())
                        {
                            reportObjects.Add(InstanceFromReader(reader));
                        }
                    }
                    result = ErrorFlag.ERROR_OK_RESULT;
                }



            }
            catch (SqlException ex)
            {
                result = ErrorFlag.ERROR_DATABASE;

            }


            return result;

        }

        public ErrorFlag GetAll(out List<IReportObject> reportObjects, out string error)
        {
            throw new NotImplementedException();
        }

        public IReportObject InstanceFromReader(SqlDataReader reader)
        {

            Location location = new Location(
                (int)reader["IdLocation"],
                reader["Loc.Description"].ToString(),
                (decimal)reader["lat"],
                (decimal)reader["long"]
                );

            Report report = new Report(
                (int)reader["IdReport"],
                (int)reader["IdUserWhoNotified"],
                location,
                (ReportStatus)reader["IdStatus"],
                (DateTime)reader["NotifiedDT"],
                reader["Rep.Title"].ToString(),
                reader["Rep.Description"].ToString()

                );

            return report;


        }

        public ErrorFlag Update(IReportObject reportObject, out string error)
        {
            throw new NotImplementedException();
        }
    }
}
