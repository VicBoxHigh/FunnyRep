using DumyReportes.Flags;
using DumyReportes.Models;
using DumyReportes.Util;
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

        /*  public static string QUERY_INSERT_REPORT_LOCATION =
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
                              ,[FileNameEvidence]
                              ,[PathEvidence]
                              )
                          VALUES
                              (
                              @IdUserWhoNotified
                              ,(SELECT SCOPE_IDENTITY())
                              ,@IdStatus
                              ,@NotifiedDT
                              ,@RepTitle
                              ,@RepDescription
                              ,@fileNameEvidence
                              ,@pathEvidence
                          );


                  COMMIT TRANSACTION CreateReportDtlTran;

              END TRY
              BEGIN CATCH
                  SELECT ERROR_MESSAGE() ERROR;
                  ROLLBACK TRANSACTION CreateReportDtlTran;
              END CATCH
              ";*/




        public ErrorFlag Create(IReportObject reportObject, out string error)
        {
            error = "";
            ErrorFlag result;
            Report report = reportObject as Report;
            SqlCommand command = new SqlCommand("dbo.InsertReport", ConexionBD.getConexion());
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.Add("@LocDescription", System.Data.SqlDbType.VarChar).Value = report.Location.Description;
            command.Parameters.Add("@LocLat", System.Data.SqlDbType.Decimal).Value = report.Location.lat;
            command.Parameters.Add("@LocLon", System.Data.SqlDbType.Decimal).Value = report.Location.lon;

            command.Parameters.Add("@IdReportType", System.Data.SqlDbType.Int).Value = report.IdReportType;

            command.Parameters.Add("@IdUserWhoNotified", System.Data.SqlDbType.Int).Value = report.IdUserWhoNotified;
            command.Parameters.Add("@IdStatus", System.Data.SqlDbType.Int).Value = (int)report.IdStatus;
            command.Parameters.Add("@NotifiedDT", System.Data.SqlDbType.DateTime).Value = DateTime.Now;
            command.Parameters.Add("@RepTitle", System.Data.SqlDbType.VarChar).Value = report.Title;
            command.Parameters.Add("@RepDescription", System.Data.SqlDbType.VarChar).Value = report.Description;
            command.Parameters.Add("@fileNameEvidence", System.Data.SqlDbType.VarChar).Value = report.FileNameEvidence;
            command.Parameters.Add("@pathEvidence", System.Data.SqlDbType.VarChar).Value = report.PathEvidence;

            using (command)
            using (SqlDataReader reader = command.ExecuteReader())
            {
                result = ErrorFlag.ERROR_NO_AFECTED_RECORDS;
                if (reader.RecordsAffected == 2) result = ErrorFlag.ERROR_OK_RESULT;
                //!= 2 rows affected -> no cambios en DB , así que se puede generar un error code en el query
                if (reader.HasRows)//si hay un ROWS significa exception interno en SQL
                {
                    result = ErrorFlag.ERROR_CREATION_ENITITY;
                    if (reader.Read())
                        error = reader.GetString(0);

                }
            }


            return result;

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

        public static string QUERY_GET_SPECIFIC_HEAD =
            @"
SELECT TOP (1) [IdReport]
      ,[IdUserWhoNotified]
      ,[IdLocation]
      ,[IdStatus]
      ,[FileNameEvidence]
      ,[PathEvidence]
      ,[NotifiedDT]
      ,[Title]
      ,[Description] RepDescription
  FROM [dbo].[Report]
  WHERE IdReport = @idReport

";

        public ErrorFlag Get(int idReport, out IReportObject reportObject, out string error)
        {

            SqlCommand command = new SqlCommand(QUERY_GET_SPECIFIC_HEAD, ConexionBD.getConexion());
            command.Parameters.Add("@idReport", System.Data.SqlDbType.Int).Value = idReport;

            ErrorFlag result = ErrorFlag.ERROR_OK_RESULT;
            reportObject = null;
            error = "";
            try
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows) result = ErrorFlag.ERROR_NOT_EXISTS;
                    else if (!reader.Read()) result = ErrorFlag.ERROR_INVALID_OBJECT;

                    else reportObject = InstanceFromReader(reader);
                }



            }
            catch (SqlException ex)
            {
                result = ErrorFlag.ERROR_CONNECTION_DB;
                error = ex.Message;
            }

            return result;
        }



        private static string QUERY_UPDATE_STATUS =
          @"
                UPDATE [dbo].[Report]
                   SET 
                      [IdStatus] = @newStat,
                      [IdReportType] = @idReportType

                 WHERE [IdReport] = @idReport
            ";

        public ErrorFlag Update(int idReport, int newClasif, int newStatus, out string error)
        {

            SqlCommand command = new SqlCommand(QUERY_UPDATE_STATUS, ConexionBD.getConexion());
            command.Parameters.Add("@newStat", System.Data.SqlDbType.Int).Value = newStatus;
            command.Parameters.Add("@idReportType", System.Data.SqlDbType.Int).Value = newClasif;

            command.Parameters.Add("@idReport", System.Data.SqlDbType.Int).Value = idReport;

            ErrorFlag resultOp = ErrorFlag.ERROR_OK_RESULT;
            error = "";
            try
            {

                int rowsAffected = command.ExecuteNonQuery();

                resultOp = ErrorFlag.ERROR_OK_RESULT;


            }
            catch (SqlException ex)
            {
                resultOp = ErrorFlag.ERROR_CONNECTION_DB;
                error = "Error al actualizar";
            }


            return resultOp;

        }


        public static string QUERY_GET_REPORT_BY_USER_WHONOTIFIED =
            @"
                SELECT Rep.IdReport, Rep.IdUserWhoNotified, Rep.NotifiedDT,Rep.Title,Rep.Description RepDescription,FileNameEvidence,
                Loc.IdLocation, Loc.[Description], Loc.lat, Loc.long, 
                RS.IdStatus, RS.titleStatus,
                U.IdUser,  U.NumEmpleado NumEmpNotified , U.Level
                FROM 
	                [Report] Rep
                  LEFT JOIN [Location] Loc
	                on Rep.IdLocation = Loc.IdLocation
                  Left JOIN ReportStatus RS
	                ON Rep.IdStatus = RS.IdStatus
                  LEFT JOIN [User] U
	                ON Rep.IdUserWhoNotified = U.IdUser
                Where U.IdUser = @IdUser
                ORDER BY Rep.IdReport DESC

            ";

        //todos los reportes donde el dueño sea  @IdUser, y todos los reportes que no tengan owner aún
        public static string QUERY_GET_REPORT_BY_OWNER =
        /*  @"
          SELECT
              Rep.IdReport, Rep.IdUserWhoNotified, Rep.NotifiedDT,Rep.Title, Rep.Description RepDescription,Rep.FileNameEvidence,

              Loc.IdLocation, Loc.[Description], Loc.lat, Loc.long, 
              RS.IdStatus, RS.titleStatus,
              U.IdUser, U.NumEmpleado, U.[Level],        
              UOR.[DT]
          FROM [ReportApp].[dbo].[UserOwner_Report] UOR
              LEFT JOIN [Report] Rep ON Rep.IdReport = UOR.IdReport 
              LEFT JOIN [Location] Loc on Rep.IdLocation = Loc.IdLocation
              Left JOIN ReportStatus RS ON Rep.IdStatus = RS.IdStatus
              LEFT JOIN [User] U ON UOR.IdUser = U.IdUser
          Where U.IdUser= @IdUser

          ";*/
        /*        @"
                   SELECT
                          Rep.IdReport, Rep.IdUserWhoNotified, Rep.NotifiedDT,Rep.Title, Rep.Description RepDescription,Rep.FileNameEvidence,

                          Loc.IdLocation, Loc.[Description], Loc.lat, Loc.long, 
                          RS.IdStatus, RS.titleStatus,
                          U.IdUser, U.NumEmpleado, U.[Level],        
                          UOR.[DT]
                      FROM Report Rep
                          LEFT JOIN  [UserOwner_Report] UOR ON Rep.IdReport = UOR.IdReport 
                          LEFT JOIN [Location] Loc on Rep.IdLocation = Loc.IdLocation
                          Left JOIN ReportStatus RS ON Rep.IdStatus = RS.IdStatus
                          LEFT JOIN [User] U ON UOR.IdUser = U.IdUser
                      Where U.IdUser= @IdUser or U.IdUser is NULL;


      ";*/

        @"
        SELECT SpecificUserReports.*,UOR.*,U.NumEmpleado NumEmpOwner ,
	        Loc.IdLocation, Loc.[Description], Loc.lat, Loc.long, 
	        RS.IdStatus, RS.titleStatus,
	        U.IdUser, U.NumEmpleado, U.[Level],        
	        UOR.[DT]
        FROM
	
         (SELECT
	        Rep.IdReport, Rep.IdUserWhoNotified, Rep.IdLocation,Rep.IdStatus, U.NumEmpleado NumEmpNotified, Rep.NotifiedDT,Rep.Title, Rep.Description RepDescription,Rep.FileNameEvidence
               FROM Report Rep
	           LEFT JOIN [User] U ON  Rep.IdUserWhoNotified = U.IdUser
	          
	           ) SpecificUserReports 
	           LEFT JOIN  [UserOwner_Report] UOR ON UOR.IdReport = SpecificUserReports.IdReport
	           LEFT JOIN [User] U ON UOR.IdUser = U.IdUser
	           LEFT JOIN [Location] Loc on SpecificUserReports.IdLocation = Loc.IdLocation
	           LEFT JOIN ReportStatus RS ON SpecificUserReports.IdStatus = RS.IdStatus
        WHERE UOR.IdUser = @IdUser or UOR.IdReport is NULL 
        ORDER BY SpecificUserReports.IdReport DESC



        ";
        [Obsolete("Se hicieron proceidmientos almacenados, ahora se usan diferentes funciones.")]
        public ErrorFlag GetAllBy(bool isOwner, int idUser, out List<IReportObject> reportObjects, out string error)
        {
            error = "";
            Flags.ErrorFlag result;
            SqlCommand command = new SqlCommand(isOwner ? QUERY_GET_REPORT_BY_OWNER : QUERY_GET_REPORT_BY_USER_WHONOTIFIED, ConexionBD.getConexion());

            command.Parameters.Add("@IdUser", System.Data.SqlDbType.Int).Value = idUser;

            reportObjects = new List<IReportObject>();

            try
            {
                using (command)
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows) result = ErrorFlag.ERROR_OK_RESULT;
                    else
                    {
                        //bool reading = reader.Read(); 

                        while (reader.Read())
                        {
                            Report report = InstanceFromReader(reader) as Report;

                            ErrorFlag errorFlag = EvidenceHelper.GetEvidenceImg(report.FileNameEvidence, out string base64Img);
                            report.Pic64 = base64Img;
                            reportObjects.Add(report);
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

        public ErrorFlag GetReportsByWhoNotified(User whoNotified, out List<Report> reportsAsignados, out List<Report> reportsNoAsignados)
        {
            SqlCommand command = new SqlCommand("dbo.ReportsByWhoNotified", ConexionBD.getConexion());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("@IdUserWhoNotif", System.Data.SqlDbType.Int).Value = whoNotified.IdUser;

            using (command)
            using (SqlDataReader reader = command.ExecuteReader())
            {
                //Reps que ya tienen un owner
                bool isOwnedReps = false;
                reportsAsignados = new List<Report>();
                reportsNoAsignados = new List<Report>();
                //Deberá traer 2 results, el primero es Reportes que ya han sido asignados y el segundo reps que no
                do
                {
                    if (reader.HasRows)
                        while (reader.Read())
                        {
                            Report newRep = InstanceFromReader2(reader, isOwnedReps) as Report;
                            ErrorFlag errorFlag = EvidenceHelper.GetEvidenceImg(newRep.FileNameEvidence, out string base64Img);
                            newRep.Pic64 = base64Img;

                            if (isOwnedReps) reportsAsignados.Add(newRep);
                            else reportsNoAsignados.Add(newRep);
                        }
                    isOwnedReps = true;

                } while (reader.NextResult());//segunda vuelva trae los que aun no han sido asignados


            }

            return ErrorFlag.ERROR_OK_RESULT;

        }

        //Llamada por usuarios Agent
        public ErrorFlag GetReportsByOwner(User owner, out List<Report> reportObjs)
        {
            SqlCommand command = new SqlCommand("dbo.RepsByOwner", ConexionBD.getConexion());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("@IdUserOwner", System.Data.SqlDbType.Int).Value = owner.IdUser;


            using (command)
            using (SqlDataReader reader = command.ExecuteReader())
            {
                reportObjs = new List<Report>();

                if (reader.HasRows)
                    while (reader.Read())
                        reportObjs.Add(InstanceFromReader2(reader, ownerInfoExist: true));//reports de owners

            }


            return ErrorFlag.ERROR_OK_RESULT;

        }
        //llamada por admin y superadmin
        public ErrorFlag GetAllReports(out List<Report> reportAsigned, out List<Report> reportsNoAsigned)
        {
            SqlCommand command = new SqlCommand("dbo.ReportsAllAsignadosNoAsignados", ConexionBD.getConexion());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            //Sin params

            reportAsigned = new List<Report>();
            reportsNoAsigned = new List<Report>();
            try
            {
                using (command)
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    //Deberá traer 2 results, el primero es Reportes que ya han sido asignados y el segundo reps que no
                    bool isOwnedReps = false;
                    do
                    {

                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                Report newRep = InstanceFromReader2(reader, isOwnedReps) as Report;
                                ErrorFlag errorFlag = EvidenceHelper.GetEvidenceImg(newRep.FileNameEvidence, out string base64Img);
                                newRep.Pic64 = base64Img;

                                if (isOwnedReps) reportAsigned.Add(newRep);
                                else reportsNoAsigned.Add(newRep);
                            }



                    } while (reader.NextResult());

                }
            }
            catch (SqlException e)
            {

            }



            return ErrorFlag.ERROR_OK_RESULT;

        }


        public ErrorFlag GetAll(out List<IReportObject> reportObjects, out string error)
        {
            throw new NotImplementedException();
        }

        public IReportObject InstanceFromReader(SqlDataReader reader)
        {
            Location location = null;
            Report report = new Report(
           (int)reader["IdReport"],
           (int)reader["IdUserWhoNotified"],
           location,
           (ReportStatus)reader["IdStatus"],
           (DateTime)reader["NotifiedDT"],
           reader["Title"].ToString(),
           reader["RepDescription"].ToString()

           );
            try
            {

                location = new Location(
                    (int)reader["IdLocation"],
                    reader["Description"].ToString(),
                    (decimal)reader["lat"],
                    (decimal)reader["long"]
                    );
                report.NumEmpleadoWhoNotified = reader["NumEmpNotified"].ToString();
                report.FileNameEvidence = reader["FileNameEvidence"].ToString();
            }
            catch (Exception ex)
            {
                //no location columns
            }

            report.Location = location;
            return report;
        }

        private Report InstanceFromReader2(SqlDataReader reader, bool ownerInfoExist)
        {

            Report report = null;
           

            report = new Report()
            {
                IdReport = (int)reader["IdReport"],
                // IdUserWhoNotified = (int)reader["IdUserWhoNotified"],
                IdStatus = (ReportStatus)reader["IdStatus"],
                DTCreation = (DateTime)reader["NotifiedDT"],
                Title = reader["Title"].ToString(),
                Description = reader["Description"].ToString(),
                IdReportType = (int)reader["IdReportType"],
                FileNameEvidence = reader["FileNameEvidence"].ToString(),
                PathEvidence = reader["PathEvidence"].ToString(),
                FinReprteDT = reader["InicioReporteDT"] == null ? (DateTime)reader["FinReporteDT"] : DateTime.MinValue,
                InicioReporteDT = reader["FinReporteDT"] == null ? (DateTime)reader["FinReporteDT"] : DateTime.MinValue,
                UserWhoNotified = new User()
                {
                    IdUser = (int)reader["UNotif_IdUser"],
                    Name = reader["UNotif_Name"].ToString(),
                    IsEnabled = (bool)reader["UNotif_IsEnabled"],
                    AccessLevel = (AccessLevel)reader["UNotif_Level"],
                    NumEmpleado = reader["UNotif_NumEmpleado"].ToString()


                },
                CurrentOwner = !ownerInfoExist ? null : new User()
                {
                    IdUser = (int)reader["UOwner_IdUserOwner"],
                    Name = reader["UOwner_Name"].ToString(),
                    NumEmpleado = reader["UOwner_NumEmpleado"].ToString(),
                    AccessLevel = (AccessLevel)reader["UOwner_Level"],
                },



            };

            report.Location = new Location(
                 (int)reader["IdLocation"],
                 reader["Location_Description"].ToString(),
                 (decimal)reader["lat"],
                 (decimal)reader["long"]
                 );
            report.FileNameEvidence = reader["FileNameEvidence"].ToString();




            return report;
        }


     
        public ErrorFlag Update(IReportObject reportObject, out string error)
        {
            Report report = reportObject as Report;
            ErrorFlag resultOp;
            error = "";
            SqlCommand command = new SqlCommand(QUERY_UPDATE_STATUS, ConexionBD.getConexion());
            command.Parameters.Add("@newStat", System.Data.SqlDbType.Int).Value = (int)report.IdStatus;
            command.Parameters.Add("@idReport", System.Data.SqlDbType.Int).Value = report.IdReport;


            try
            {

                int rowsAffected = command.ExecuteNonQuery();

                resultOp = ErrorFlag.ERROR_OK_RESULT;


            }
            catch (SqlException ex)
            {
                resultOp = ErrorFlag.ERROR_CONNECTION_DB;
            }


            return resultOp;

            throw new NotImplementedException();
        }
    }
}
