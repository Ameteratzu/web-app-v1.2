DROP TABLE IF EXISTS dbo.ClasificacionMedio;
GO

CREATE TABLE dbo.ClasificacionMedio (
	Id int PRIMARY KEY IDENTITY(1,1),
	Descripcion varchar(255) NOT NULL
);