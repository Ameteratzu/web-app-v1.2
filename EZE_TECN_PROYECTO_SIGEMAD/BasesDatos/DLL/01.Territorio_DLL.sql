-- dbo.Territorio definition

-- Drop table

-- DROP TABLE dbo.Territorio;

CREATE TABLE dbo.Territorio (
	Id int NOT NULL,
	Descripcion varchar(255) NOT NULL,
	CONSTRAINT Territorio_PK PRIMARY KEY (Id)
);