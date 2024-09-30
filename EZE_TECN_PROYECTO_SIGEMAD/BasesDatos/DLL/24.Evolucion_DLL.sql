DROP TABLE IF EXISTS dbo.Evolucion;
GO
CREATE TABLE dbo.Evolucion (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FechaHoraEvolucion DATETIME NOT NULL,
    IdEntradaSalida int NOT NULL,
    IdMedio int NOT NULL,
    IdProcedenciaDestino int NULL,
    IdTecnico UNIQUEIDENTIFIER NOT NULL,
    Resumen bit NOT NULL,
    Observaciones TEXT NULL,
    Prevision TEXT NULL,
    Estado VARCHAR(100) NOT NULL,
    SuperficieAfectadaHectarea DECIMAL(10, 2) NULL,
    FechaFinal DATETIME NULL,
    IdProvinciaAfectada int NOT NULL,
    IdMunicipioAfectado int NOT NULL,
    GeoPosicionAreaAfectada GEOMETRY,
    FechaCreacion datetime,
	CreadoPor varchar(500),
	FechaModificacion datetime,
	ModificadoPor varchar(500),
    CONSTRAINT FK_Evolucion_EntradaSalida FOREIGN KEY (IdEntradaSalida) REFERENCES dbo.EntradaSalida(Id),
    CONSTRAINT FK_Evolucion_Medio FOREIGN KEY (IdMedio) REFERENCES dbo.Medio(Id),
    CONSTRAINT FK_Evolucion_ProcedenciaDestino FOREIGN KEY (IdProcedenciaDestino) REFERENCES dbo.ProcedenciaDestino(Id),
    CONSTRAINT FK_Evolucion_ApplicationUsers FOREIGN KEY (IdTecnico) REFERENCES dbo.ApplicationUsers(Id),
    CONSTRAINT FK_Evolucion_Provincia FOREIGN KEY (IdProvinciaAfectada) REFERENCES dbo.Provincia(Id),
    CONSTRAINT FK_Evolucion_Municipio FOREIGN KEY (IdMunicipioAfectado) REFERENCES dbo.Municipio(Id)
	
);
