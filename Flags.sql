USE [ReportApp]
GO

INSERT INTO [dbo].[AccessLevel]
           ([Level]
           ,[Name])
     VALUES (0,'PUBLIC'),(10,'AGENT'),(20,'ADMIN')
GO

INSERT INTO [dbo].[ReportStatus]
           ([IdStatus]
           ,[titleStatus])
     VALUES(0,'EN ESPERA'),(1,'EN PROCESO'),(2, 'COMPLETADA')
GO

INSERT INTO dbo.[User]
(NumEmpleado,UserName,Pass,IsEnabled,[Level])
VALUES('3088','vperez','vperez',1,20)
GO