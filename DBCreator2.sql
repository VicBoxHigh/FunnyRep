  CREATE DATABASE ReportApp9;
  GO 

  USE ReportApp9;
 GO

CREATE TABLE AccessLevel(

[Level] int NOT NULL PRIMARY KEY, --UNIQUE and NOT NULL
[Name] varchar(25) NOT NULL

);

/*
El Reporte puede clasificarse como;
Mecanico, Electrico, Edificio, permitirá reclasificar los tipos. //añadir más sería posible 
*/
CREATE TABLE ReportType(
	[IdReportType] int NOT NULL IDENTITY(1,1) PRIMARY KEY ,
	[Name] varchar(50) NOT NULL 

);

CREATE TABLE [User](
	IdUser int NOT NULL IDENTITY(1,1) PRIMARY KEY ,	
	NumEmpleado varchar(25) NOT NULL,
	UserName varchar(25) NOT NULL,
	Pass varchar(25) NOT NULL,
	[Name] varchar(60) NOT NULL,
	IsEnabled bit DEFAULT 1 NOT NULL,
	[Level] int DEFAULT 0 NOT NULL
	FOREIGN KEY([Level]) REFERENCES AccessLevel([Level])


);
/*
CREATE TABLE SessionToken(

	IdToken int NOT NULL  IDENTITY(1,1) PRIMARY KEY,
	Token varchar(256) NOT NULL,

	[Level] int  NOT NULL,
	FOREIGN KEY([Level]) REFERENCES AccessLevel([Level]),
	
	CreationDT datetime NOT NULL,
	ExpirationDT datetime NOT NULL,
	
	IdUser int NOT NULL,
	FOREIGN KEY(IdUser) REFERENCES [User](IdUser)


);
*/
CREATE TABLE [Location](

	IdLocation INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[Description] varchar (100) NOT NULL,
	lat decimal(18,8) NOT NULL,
	long decimal(18,8) NOT null

);

CREATE TABLE ReportStatus(

	IdStatus INT NOT NULL PRIMARY KEY,
	titleStatus varchar(25)

);
 
CREATE TABLE Report(

	IdReport INT NOT NULL IDENTITY ( 1,1) PRIMARY KEY,

	IdUserWhoNotified int NOT NULL,
	FOREIGN KEY(IdUserWhoNotified) REFERENCES [User](IdUser),
	
	IdLocation int NOT NULL,
	FOREIGN KEY(IdLocation) REFERENCES [Location](IdLocation),

	IdStatus int NOT NULL,
	FOREIGN KEY(IdStatus) REFERENCES ReportStatus(IdStatus),

	IdReportType int NOT NULL,
	FOREIGN KEY(IdReportType) REFERENCES ReportType(IdReportType),


	FileNameEvidence varchar(50),
	PathEvidence varchar(256),

	NotifiedDT datetime NOT NULL,
	
	InicioReporteDT datetime not null,
	FinReporteDT datetime not null,


	Title varchar(50) NOT NULL,

	[Description] varchar (256 )

);
/*
ALTER TABLE Report
DROP CONSTRAINT  FK__Report__IdLocati__33D4B598

SELECT * 
FROM sys.foreign_keys
WHERE referenced_object_id = object_id('IdLocation')
*/

/*ALTER TABLE Report
ADD FOREIGN KEY(IdLocation) REFERENCES [Location](IdLocation)
*/

CREATE TABLE UserOwner_Report(

	IdUser int NOT NULL,
	FOREIGN KEY(IdUser) REFERENCES [User](IdUser),

	IdReport int NOT NULL,
	FOREIGN KEY(IdReport ) REFERENCES [Report](IdReport ),

	DT dateTime NOT NULL


);

 
CREATE TABLE ReportDtlEntry (

	IdReportDtlEntry INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	
	IdReport int NOT NULL,
	FOREIGN KEY(IdReport ) REFERENCES [Report](IdReport ),

	TitleUpdate varchar(45) NOT NULL,
	[Description] varchar(512) NOT NULL,

	DTUpdate datetime NOT NULL,

	isOwnerUpdate bit NOT NULL,

	IdUserUptade int NOT NULL,
	FOREIGN KEY(IdUserUptade) REFERENCES [User](Iduser)




);


 