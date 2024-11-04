-- dbo.CCAA definition

-- Drop table

-- DROP TABLE dbo.CCAA;

CREATE TABLE dbo.CCAA (
	Id int NOT NULL,
	Descripcion varchar(255) NOT NULL,
	IdPais int NOT NULL FOREIGN KEY REFERENCES dbo.Pais(Id),
	CONSTRAINT CCAA_PK PRIMARY KEY (Id)
);