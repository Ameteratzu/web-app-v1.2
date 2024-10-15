
DROP TABLE IF EXISTS dbo.EntidadMenor;
GO

CREATE TABLE dbo.EntidadMenor (
	Id int NOT NULL,
	IdDistrito int NULL,
	Descripcion varchar(255) NOT NULL,
	UtmX int NULL,
	UtmY int NULL,
	Huso int NULL,
	GeoPosicion GEOMETRY,
	CONSTRAINT Entidadmenor_PK PRIMARY KEY (Id),
	CONSTRAINT DistritoEntidadmenor FOREIGN KEY (IdDistrito) REFERENCES dbo.Distrito(Id)
);