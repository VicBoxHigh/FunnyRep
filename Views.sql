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
		@titleUpdate nvarchar(45), 
		@description nvarchar(512),  
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

SI CAMBIARON, GENERARÁ UN DtlEntry con ese cambio (para que sea mostrado en la app), este es un evento de sistema (ligado al usuario 1)

*/
CREATE TRIGGER [dbo].ReportChange  ON Report
INSTEAD OF UPDATE
AS

	DECLARE @tempDescription varchar(50);
	DECLARE @IdReport int = (SELECT IdReport FROM inserted);
 
 	DECLARE @newIdStat int = (SELECT IdStatus as newStat FROM inserted);
	DECLARE @oldIdStat int = (SELECT IdStatus FROM deleted);

	--Indica la clasificacion de reporte, electrico, edificio, mecanico
	DECLARE @newIdRepType int = (SELECT IdReportType FROM inserted);
	DECLARE @oldIdRepType int = (SELECT IdReportType FROM deleted);


	DECLARE @currentDT datetime =GETDATE();
 

	--Si cambió el typo de reporte
	IF( @newIdRepType != @oldIdRepType )
	BEGIN

		SET @tempDescription = CONCAT ('El reporte se re-clasificó a tipo: ',( SELECT TOP(1) ReportType.[Name] FROM ReportType Where IdReportType = @newIdRepType ) );
		EXEC InsertDtlEntry @idReport,1 ,'', @tempDescription, @currentDT ,0;


	END


	--El estado del report cambia
	IF((@newIdStat = 1  and @oldIdStat = 0 ) )--ESPERA a PROCESO
	BEGIN 
		UPDATE Report
		SET
			InicioReporteDT = @currentDT,
			IdStatus = inserted.IdStatus
		From inserted
		WHERE Report.IdReport = inserted.IdReport;


		SET @tempDescription = CONCAT('El reporte se marcó como: ', ( SELECT TOP(1) ReportStatus.titleStatus FROM ReportStatus Where IdStatus = @newIdStat ) );
		--Generar entry dtl
		EXEC InsertDtlEntry @IdReport,1 ,'',@tempDescription   , @currentDT, 0;

	END
	ELSE IF  (@newIdStat = 2 and @oldIdStat = 1) 
	BEGIN 
		UPDATE Report
		SET
			FinReporteDT = @currentDT,
			IdStatus = inserted.IdStatus
		From inserted
		WHERE Report.IdReport = inserted.IdReport;


		SET @tempDescription = CONCAT('El reporte se marcó como: ', ( SELECT TOP(1) ReportStatus.titleStatus FROM ReportStatus Where IdStatus = @newIdStat ) );
		--Generar entry dtl
		EXEC InsertDtlEntry @IdReport,1 ,'',@tempDescription   , @currentDT, 0;
	
	END
	

GO

/*
El cambio de estado de un reporte, solo puede subir
ESPERA < PROCESO < COMPLETADO

si el estatus anterior es menor al nuevo, entonces:

--Genera el evento de sistema en ReportDtl
--Reprt Inicio se coloca a la fecha actual
--Actualiza Report Status

--Todo queda restringido siempre que se cumple la primera regla
*/
/*
CREATE TRIGGER [dbo].UpdateReportChangeStatusTriger on Report
INSTEAD OF UPDATE
AS

	DECLARE @tempDescription varchar(50);
	DECLARE @IdReport int = (SELECT IdReport FROM inserted);

	--Indica  si  está en espera, proceso o completado
	DECLARE @newIdStat int = (SELECT IdStatus as newStat FROM inserted);
	DECLARE @oldIdStat int = (SELECT IdStatus FROM deleted);

	DECLARE @currentDT datetime =GETDATE();


	
	IF((@newIdStat = 1  and @oldIdStat = 0 ) )--ESPERA a PROCESO
	BEGIN 
		UPDATE Report
		SET
			InicioReporteDT = @currentDT,
			IdStatus = inserted.IdStatus
		From inserted
		WHERE Report.IdReport = inserted.IdReport;


		SET @tempDescription = CONCAT('El reporte se marcó como: ', ( SELECT TOP(1) ReportStatus.titleStatus FROM ReportStatus Where IdStatus = @newIdStat ) );
		--Generar entry dtl
		EXEC InsertDtlEntry @IdReport,1 ,'',@tempDescription   , @currentDT, 0;

	END
	ELSE IF  (@newIdStat = 2 and @oldIdStat = 1) 
	BEGIN 
		UPDATE Report
		SET
			FinReporteDT = @currentDT,
			IdStatus = inserted.IdStatus
		From inserted
		WHERE Report.IdReport = inserted.IdReport;


		SET @tempDescription = CONCAT('El reporte se marcó como: ', ( SELECT TOP(1) ReportStatus.titleStatus FROM ReportStatus Where IdStatus = @newIdStat ) );
		--Generar entry dtl
		EXEC InsertDtlEntry @IdReport,1 ,'',@tempDescription   , @currentDT, 0;
	
	END
	--Acepted Cases
	--si new = 1, and old = 0 -> set inicio date
	--si new = 2, and old = 1 -> set Fin Date
	



	--no acpted Cases
	--si new = 2 and old = 0 -> 
	--cualqueir otro 

GO
*/

CREATE PROCEDURE dbo.UpdateUserInfo 
	@idUser int,
	@numEmpleado varchar(25),
	@userName varchar(25),
	@pass varchar(25),
	@name varchar(60),
	@isEnabled bit,
	@userLevel int
AS

	UPDATE TOP(1) [dbo].[User]
	   SET [NumEmpleado] = @numEmpleado
		  ,[UserName] = @userName
		  ,[Pass] = @pass
		  ,[Name] = @name
		  ,[IsEnabled] = @isEnabled
		  ,[Level] = @userLevel
	 WHERE [User].IdUser  = @idUser


GO
