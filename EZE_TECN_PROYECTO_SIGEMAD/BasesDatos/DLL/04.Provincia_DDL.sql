-- dbo.Provincia definition

-- Drop table

-- DROP TABLE dbo.Provincia;

CREATE TABLE dbo.Provincia (
	Id int NOT NULL,
	IdCCAA int NOT NULL,
	Descripcion varchar(255) NOT NULL,
	UtmX int NULL,
	UtmY int NULL,
	Huso varchar(3) NULL,
	GeoPosicion GEOMETRY,
	CONSTRAINT Provincias_PK PRIMARY KEY (Id),
	CONSTRAINT CCAAProvincia FOREIGN KEY (IdCCAA) REFERENCES dbo.CCAA(Id) ON DELETE NO ACTION ON UPDATE NO ACTION
);