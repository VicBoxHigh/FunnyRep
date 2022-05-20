USE  ReportApp9;
GO
--La vista retorna Los reportes que no hayan sido asignados a un Agente (los que no tienen una relacion en tabla UserOwner_Reportf)
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
order by RepsAsignadosWithLastOwner.NotifiedDT DESC


GO

--Regresa todos los que un usuario básico generó.
CREATE PROCEDURE ReportsByWhoNotified
	@IdUserWhoNotif int
AS

	SELECT * FROM RepNoAsignados WHERE IdUserWhoNotified = @IdUserWhoNotif order by RepNoAsignados.NotifiedDT DESC;
	SELECT * FROM RepsAsignadosWithLastOwner Where  IdUserWhoNotified = @IdUserWhoNotif order by RepsAsignadosWithLastOwner.NotifiedDT DESC;

GO

CREATE PROCEDURE ReportsAllAsignadosNoAsignados
AS

	SELECT * FROM RepNoAsignados order by RepNoAsignados.NotifiedDT DESC;
	SELECT * FROM RepsAsignadosWithLastOwner order by RepsAsignadosWithLastOwner.NotifiedDT DESC;

GO
 



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
				,AL.[Level]
				,AL.[Name] AS AccessLevelName
              FROM [ReportDtlEntry]  RDE
			  LEFT JOIN [User] U ON RDE.IdUserUpdate = U.IdUser
			  LEFT JOIN AccessLevel AL ON U.[Level] = AL.[Level]
              Where RDE.IdReport = @IdReportHeader
              Order by DTUpdate ASC


GO


--Select * from sysobjects where type = 'V' and category = 0
CREATE PROCEDURE InsertDtlEntry
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






CREATE PROCEDURE InsertReport
	@LocDescription nvarchar,
	@LocLat decimal(18,8),
	@LocLon decimal(18,8),

	@IdReportType int,

	@IdUserWhoNotified int,
	@IdStatus int,
	@NotifiedDT datetime,
	@RepTitle	varchar(50),
	@RepDescription varchar(256),
	@fileNameEvidence varchar(50),
	@pathEvidence		varchar(256)
AS


	 
	BEGIN TRY
		BEGIN TRANSACTION CreateReportDtlTran;

			INSERT INTO [dbo].[Location]
				([Description]
				,[lat]
				,[long])
			VALUES(
				@LocDescription,
				@LocLat,
				@LocLon
			);

			DECLARE @LastRowPKInserted int = (SELECT SCOPE_IDENTITY() );

			INSERT INTO [dbo].[Report]
				([IdUserWhoNotified]
				,[IdLocation]
				,[IdStatus]
				,[IdReportType]
				,[FileNameEvidence]
				,[PathEvidence]
				,[NotifiedDT]
				,[InicioReporteDT]
				,[FinReporteDT]
				,[Title]
				,[Description])
			VALUES(
				@IdUserWhoNotified, 
				@LastRowPKInserted,--location PK id inserted
				(SELECT TOP(1) IdStatus FROM ReportStatus WHERE titleStatus like '%ESPERA%'), --Al ser nuevo report, su valor por defecto siempre será En espera
				@IdReportType,
				@fileNameEvidence,
				@pathEvidence,
				@NotifiedDT, 
				NULL, --Inicio de la atención del reporte
				NULL, --Fin de la atención del reporte
				@RepTitle,
				@RepDescription
			);

	COMMIT TRANSACTION CreateReportDtlTran;

	END TRY
	 
	BEGIN CATCH
			SELECT ERROR_MESSAGE() ERROR;
	        ROLLBACK TRANSACTION CreateReportDtlTran;
	END CATCH

GO




/*

CUANDO SE ACUTALICE LA TABLA REPORT COMPROBARÄ SI EL STATUS o TIPO DE REPORTE
CAMBIO,

SI CAMBIARON, GENERARÁ UN DtlEntry con ese cambio (para que sea mostrado en la app)

*/
CREATE TRIGGER [dbo].UpdateReportTriger ON Report
AFTER UPDATE
AS

	DECLARE @tempTitle varchar(50);
	DECLARE @IdReport int = (SELECT IdReport FROM inserted);

	DECLARE @newIdStat int = (SELECT IdStatus as newStat FROM inserted) ;
	DECLARE @newIdRepType int = (SELECT IdReportType FROM inserted);


	--Si cambió el status
	IF(@newIdStat  != (SELECT IdStatus FROM deleted) )
	BEGIN
		SET @tempTitle = CONCAT('El estado del reporte cambió a: ', ( SELECT TOP(1) ReportStatus.titleStatus FROM ReportStatus Where IdStatus = @newIdStat ) );
		--Generar entry dtl
		EXEC InsertDtlEntry @IdReport, @tempTitle , '' , GETDATE, 0, 1	;	
	END

	--Si cambió el typo de reporte
	IF( @newIdRepType != ( SELECT IdReportType FROM deleted ) )
	BEGIN

		SET @tempTitle = CONCAT ('El reporte se re-clasificó a tipo: ',( SELECT TOP(1) ReportType.[Name] FROM ReportType Where IdReportType = @newIdRepType ) );
		EXEC InsertDtlEntry @idReport, @tempTitle, '', GETDATE ,0,1;
		

	END
	

GO


--UPDATE REPORT
/*
CREATE PROCEDURE UpdateReport
	@idReport int,
	@idStatus int,
	@idReportType int
AS

	
	UPDATE [dbo].[Report]
	SET 
		[IdStatus] = @idStatus,
		[IdReportType] = @idReportType

	WHERE [IdReport] = @idReport;


	DECLARE @oldRepStatus int;
	SET @oldRepStatus = (SELECT TOP(1) IdReportType FROM Report WHERE IdReport = @idReport);
	
	DECLARE @tempTitle varchar(50);

	--Cambia el estado con respecto al actual
	IF(@idStatus != (SELECT TOP(1) IdStatus FROM Report WHERE  IdReport = @idReport ))
	BEGIN 

		SET @tempTitle = CONCAT('El estado del reporte cambió a: ', ( SELECT TOP(1) ReportStatus.titleStatus FROM ReportStatus Where IdStatus = @idReportType) );
		--Generar entry dtl
		EXEC InsertDtlEntry @idReport, @tempTitle , '' , GETDATE, 0, 1	;	
	END


	DECLARE @oldRepType int;
	set @oldRepType = (SELECT TOP(1) IdReportType FROM Report WHERE IdReport = @idReport);

	IF(@idReportType != @oldRepType)
	BEGIN
	
		SET @tempTitle = CONCAT ('El reporte se re-clasificó a tipo: ',( SELECT TOP(1) ReportType.[Name] FROM ReportType Where IdReportType = @idReportType) );
		EXEC InsertDtlEntry @idReport, @tempTitle, '', GETDATE ,0,1;
	END



GO*/