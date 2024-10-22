DROP TABLE IF EXISTS dbo.ProcedenciaDestino;
GO

CREATE TABLE dbo.ProcedenciaDestino (
	Id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	Descripcion varchar(255) NULL,
);
CREATE UNIQUE INDEX IX_ProcedenciaDestino ON dbo.ProcedenciaDestino (Descripcion);