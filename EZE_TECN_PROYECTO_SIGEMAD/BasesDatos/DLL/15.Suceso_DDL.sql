-- dbo.Suceso definition

-- Drop table

-- DROP TABLE dbo.Suceso;

CREATE TABLE dbo.Suceso (
	Id int NOT NULL IDENTITY(1,1),
	IdTipo int DEFAULT ((1)) NOT NULL,
	IdSigeMigracion int NULL,
	CONSTRAINT Suceso_PK PRIMARY KEY (Id),
	CONSTRAINT TipoSucesoSuceso FOREIGN KEY (IdTipo) REFERENCES dbo.TipoSuceso(Id)
);