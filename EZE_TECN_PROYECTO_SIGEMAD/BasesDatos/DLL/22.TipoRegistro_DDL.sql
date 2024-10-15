DROP TABLE IF EXISTS dbo.TipoRegistro;
GO

CREATE TABLE dbo.TipoRegistro (
	Id int NOT NULL IDENTITY(1,1),
	Descripcion varchar (255) NOT NULL,	
    CONSTRAINT PK_TipoRegistro PRIMARY KEY (Id)	
);