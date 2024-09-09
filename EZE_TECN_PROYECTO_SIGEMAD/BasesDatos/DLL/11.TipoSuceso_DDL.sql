-- dbo.TipoSuceso definition

-- Drop table

-- DROP TABLE dbo.TipoSuceso;

CREATE TABLE dbo.TipoSuceso (
	Id int NOT NULL IDENTITY(1,1),
	Descripcion varchar(255) NOT NULL,
	CONSTRAINT PK_TipoSuceso PRIMARY KEY (Id)
);