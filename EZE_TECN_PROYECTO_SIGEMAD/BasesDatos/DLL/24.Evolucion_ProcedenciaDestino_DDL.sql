DROP TABLE IF EXISTS dbo.Evolucion_ProcedenciaDestino;
GO

CREATE TABLE dbo.Evolucion_ProcedenciaDestino (
	Id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdEvolucion int NOT NULL FOREIGN KEY REFERENCES Evolucion(Id),
	IdProcedenciaDestino int NOT NULL FOREIGN KEY REFERENCES ProcedenciaDestino(Id)
);