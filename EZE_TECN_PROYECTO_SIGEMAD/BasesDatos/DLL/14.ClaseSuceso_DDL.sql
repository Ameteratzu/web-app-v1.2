-- dbo.ClaseSuceso definition

-- Drop table

-- DROP TABLE dbo.ClaseSuceso;

CREATE TABLE dbo.ClaseSuceso (
	Id int NOT NULL IDENTITY(1,1),
	Descripcion varchar(255) NOT NULL,
	CONSTRAINT PK_ClaseSuceso PRIMARY KEY (Id)
);