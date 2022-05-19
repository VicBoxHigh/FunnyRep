USE [ReportApp9]
GO

INSERT INTO [dbo].[AccessLevel]
           ([Level]
           ,[Name])
     VALUES (0,'PUBLIC'),(10,'AGENT'),(20,'ADMIN'),(30,'SUPERADMIN')
GO

INSERT INTO [dbo].[ReportStatus]
           ([IdStatus]
           ,[titleStatus])
     VALUES(0,'EN ESPERA'),(1,'EN PROCESO'),(2, 'COMPLETADA')
GO

INSERT INTO dbo.[User]
(NumEmpleado,UserName,Pass,IsEnabled,[Level],[Name])
VALUES
('0','Sistema','MarvesTI2022',1,30,'Sistema'),--El primer usuario
('3088','vperez','vperez',1,20,'Victor Alfonso Pérez Espino')
GO


INSERT INTO dbo.[ReportType]
([Name])
VALUES ('Edificio'),('Eléctrico'), ('Mecánico')
GO