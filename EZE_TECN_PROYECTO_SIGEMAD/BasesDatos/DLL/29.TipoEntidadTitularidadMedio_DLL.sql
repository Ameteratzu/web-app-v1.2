DROP TABLE IF EXISTS dbo.TipoEntidadTitularidadMedio;
GO

CREATE TABLE dbo.TipoEntidadTitularidadMedio (
	Id int PRIMARY KEY IDENTITY(1,1),
	Descripcion varchar(250) NOT NULL
);