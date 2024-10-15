-- dbo.Provincia definition

-- Drop table
IF OBJECT_ID('dbo.Provincia', 'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.Provincia;
END
GO

CREATE TABLE dbo.Provincia (
	Id int NOT NULL,
	IdCcaa int NOT NULL,
	Descripcion varchar(255) NOT NULL,
	UtmX int NULL,
	UtmY int NULL,
	Huso varchar(3) NULL,
	GeoPosicion GEOMETRY,
	CONSTRAINT Provincias_PK PRIMARY KEY (Id),
	CONSTRAINT CCAAProvincia FOREIGN KEY (IdCcaa) REFERENCES dbo.CCAA(Id) ON DELETE NO ACTION ON UPDATE NO ACTION
);