USE [ReportApp2]
GO

INSERT INTO [dbo].[AccessLevel]
           ([Level]
           ,[Name])
     VALUES (0,'PUBLIC'),(10,'AGENT'),(20,'ADMIN')
GO

INSERT INTO [dbo].[ReportStatus]
           ([IdStatus]
           ,[titleStatus])
     VALUES(0,'EN ESPERA'),(2,'EN PROCESO'),(3, 'COMPLETADA')
GO

