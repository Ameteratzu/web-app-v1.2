DROP TABLE IF EXISTS dbo.CaracterMedio;
GO

CREATE TABLE dbo.CaracterMedio (
	Id int PRIMARY KEY IDENTITY(1,1),
	Descripcion varchar(250) NULL,
	Obsoleto BIT NOT NULL
);