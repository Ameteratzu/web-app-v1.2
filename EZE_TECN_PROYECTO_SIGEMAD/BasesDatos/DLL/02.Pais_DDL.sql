-- dbo.Pais definition

-- Drop table

-- DROP TABLE dbo.Pais;

CREATE TABLE dbo.Pais (
	Id int NOT NULL,
	Descripcion varchar(255) NOT NULL,
	X decimal(18,6) NULL,
	Y decimal(18,6) NULL,
	GeoPosicion Geometry,
	CONSTRAINT Pais_PK PRIMARY KEY (Id)
);