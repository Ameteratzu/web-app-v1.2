
DROP TABLE IF EXISTS dbo.MunicipioExtranjero;
GO

CREATE TABLE dbo.MunicipioExtranjero (
	Id int NOT NULL PRIMARY KEY,
	IdDistrito int NULL,
	Descripcion varchar(255) NOT NULL,
	UtmX int NULL,
	UtmY int NULL,
	Huso int NULL,
	GeoPosicion GEOMETRY,
	CONSTRAINT DistritoMunicipioExtranjero FOREIGN KEY (IdDistrito) REFERENCES dbo.Distrito(Id)
);