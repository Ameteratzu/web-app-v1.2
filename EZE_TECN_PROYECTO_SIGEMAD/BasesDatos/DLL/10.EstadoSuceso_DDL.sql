DROP TABLE IF EXISTS dbo.EstadoSuceso;
GO

CREATE TABLE dbo.EstadoSuceso (
	Id int NOT NULL,
	Descripcion varchar (255) NOT NULL,	
    CONSTRAINT PK_EstadoSuceso PRIMARY KEY (Id)	
);