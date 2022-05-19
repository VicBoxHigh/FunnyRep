USE  ReportApp9;
GO
--La vista retorna objetos Report unicamente.
CREATE VIEW dbo.RepNoAsignados 
AS

	SELECT 
		R.*, 
		RT.[Name] AS ReportType_Name,
		RS.titleStatus ,
		L.[Description] AS Location_Description,
		L.lat,
		L.long,
		UNotif.IdUser AS UNotif_IdUser,
		UNotif.[Name] AS UNotif_Name,
		UNotif.IsEnabled As UNotif_IsEnabled,
		UNotif.[Level] AS UNotif_Level,
		UNotif.NumEmpleado AS UNotif_NumEmpleado


	FROM     dbo.Report R
	LEFT JOIN  dbo.UserOwner_Report ON R.IdReport = dbo.UserOwner_Report.IdReport
	--HeaderExpand Data, Info fuera de la entidad report
	LEFT JOIN  dbo.ReportType RT ON R.IdReportType = RT.IdReportType 
	LEFT JOIN dbo.ReportStatus RS ON R.IdStatus = RS.IdStatus
	LEFT JOIN dbo.[Location] L ON R.IdLocation = L.IdLocation
	LEFT join dbo.[User] UNotif ON R.IdUserWhoNotified = UNotif.IdUser
	--no trae data de userowner data ya que es un no asignado.

	--Filter para reportHeader
	WHERE dbo.UserOwner_Report.IdReport IS NULL

GO

--La vista retorna tuplas Report.
CREATE VIEW dbo.RepsAsignadosWithLastOwner
AS

--Obtiene todos los reportes existentes, con el último empleado asignado a ese reporte
--Teniendo en cuenta que UOR es principal, 
--Con el result es necesario join a report y sus dependencias
--Esas vistas se deberán joine
WITH GroupUOR AS(

	SELECT UOR.IdReport, MAX(UOR.DT) AS LastAgentByDate
	FROM [dbo].[UserOwner_Report] UOR
	GROUP BY IdReport

)
SELECT R.*,
		UOR.IdUser AS UOR_IDUserOwner , 
		UOR.DT AS UORDT_UserOwner,
		UOwner.[Name] AS UOwner_Name,
		UOwner.NumEmpleado AS UOwner_NumEmpleado,
		UOwner.[Level] AS UOwner_Level,
		RT.[Name] AS ReportType_Name,
		RS.titleStatus ,
		L.[Description] AS Location_Description,
		L.lat,
		L.long,
		UNotif.IdUser,
		UNotif.[Name],
		UNotif.IsEnabled,
		UNotif.[Level],
		UNotif.NumEmpleado
		
	FROM [ReportApp9].[dbo].[UserOwner_Report] UOR
	INNER JOIN GroupUOR ON GroupUOR.IdReport = UOR.IdReport AND GroupUOR.LastAgentByDate = UOR.DT
	INNER JOIN [dbo].Report R ON UOR.IdReport = R.IdReport
	--Header Expand
	--HeaderExpand Data, Info fuera de la entidad report
	LEFT JOIN  dbo.ReportType RT ON R.IdReportType = RT.IdReportType 
	LEFT JOIN dbo.ReportStatus RS ON R.IdStatus = RS.IdStatus
	LEFT JOIN dbo.[Location] L ON R.IdLocation = L.IdLocation
	LEFT join dbo.[User] UNotif ON R.IdUserWhoNotified = UNotif.IdUser
	--Trae data de user ya que es un asignado.
	LEFT JOIN dbo.[User] UOwner ON R.IdUserWhoNotified = UOwner.IdUser


GO


CREATE PROCEDURE ReportsByOwner
	@IdUserOwner int
AS

 SELECT * FROM RepsAsignadosWithLastOwner 
 WHERE UOR_IDUserOwner = @IdUserOwner
	 

GO

--Regresa todos los que un usuario básico generó.
CREATE PROCEDURE ReportsByWhoNotified
	@IdUserWhoNotif int
AS

	SELECT * FROM RepNoAsignados WHERE IdUserWhoNotified = @IdUserWhoNotif ;
	SELECT * FROM RepsAsignadosWithLastOwner Where  IdUserWhoNotified = @IdUserWhoNotif;

GO

CREATE PROCEDURE ReportsAllAsignadosNoAsignados
AS

	SELECT * FROM RepNoAsignados;
	SELECT * FROM RepsAsignadosWithLastOwner;

GO

/*
CREATE PROCEDURE ReportsBy
	@IdUserNotified int,
	@IdUserOwner int
As

	IF(@IdUserNotified > -1)
		begin
			SELECT ' ';
		end
	ELSE IF(@IdUserOwner > -1)
		BEGIN

			SELECT ' ';

		END
	ELSE
		THROW 512345, 'Sin parámetro definido al intentar obtener los reportes.',1;



GO
*/


--Obtiene las entries del Report  especificado
CREATE PROCEDURE ReportDtlEntries
@IdReportHeader int
AS

	SELECT RDE.[IdReportDtlEntry]
                  ,RDE.[IdReport]	               
                  ,RDE.[TitleUpdate]
                  ,RDE.[Description]
                  ,RDE.[DTUpdate]
                  ,RDE.[isOwnerUpdate]
				,U.IdUser
				,U.NumEmpleado
				,U.[Name]
				,U.IsEnabled
				,U.[Level]
              FROM [ReportDtlEntry]  RDE
			  LEFT JOIN [User] U ON RDE.IdUserUptade = U.IdUser
              Where RDE.IdReport = @IdReportHeader
              Order by DTUpdate ASC


GO


CREATE PROCEDURE InsertDtlEnry
		@idReport int,
		@idUserUpdate int,
		@titleUpdate nvarchar, 
		@description nvarchar, 
		@dtUpdate datetime, 
		@isOwnerUpdate bit

AS

	
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


GO