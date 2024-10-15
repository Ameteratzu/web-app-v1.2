-- dbo.Municipio definition

-- Drop table

IF OBJECT_ID('dbo.Municipio', 'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.Municipio;
END
GO

CREATE TABLE dbo.Municipio (
	Id int NOT NULL,
	IdProvincia int NOT NULL,
	Descripcion varchar(255) NOT NULL,
	UtmX int NULL,
	UtmY int NULL,
	Huso varchar(3) NULL,
	GeoPosicion GEOMETRY,
	CONSTRAINT Municipios_PK PRIMARY KEY (Id),
	CONSTRAINT ProvinciaMunicipio FOREIGN KEY (IdProvincia) REFERENCES Provincia(Id) ON DELETE NO ACTION ON UPDATE NO ACTION
);
CREATE INDEX IX_Municipio ON dbo.Municipio (IdProvincia);