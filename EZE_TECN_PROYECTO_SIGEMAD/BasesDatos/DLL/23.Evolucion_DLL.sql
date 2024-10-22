DROP TABLE IF EXISTS dbo.Evolucion;
GO
CREATE TABLE dbo.Evolucion (
    Id INT PRIMARY KEY IDENTITY(1,1),
    IdIncendio int NOT NULL FOREIGN KEY REFERENCES Incendio(Id),
    FechaHoraEvolucion DATETIME NOT NULL,
    IdEntradaSalida int NOT NULL,
    IdMedio int NOT NULL,
    IdTecnico UNIQUEIDENTIFIER NOT NULL,
    IdTipoRegistro int NOT NULL FOREIGN KEY REFERENCES TipoRegistro(Id),
    Resumen bit NOT NULL,
    Observaciones TEXT NULL,
    Prevision TEXT NULL,
    IdEstadoIncendio int NOT NULL,
    SuperficieAfectadaHectarea DECIMAL(10, 2) NULL,
    FechaFinal DATETIME NULL,
    IdProvinciaAfectada int NOT NULL,
    IdMunicipioAfectado int NOT NULL,
    IdEntidadMenor int NOT NULL FOREIGN KEY REFERENCES EntidadMenor(Id),
    GeoPosicionAreaAfectada GEOMETRY,
    ---
    FechaCreacion DATETIME2(7) NOT NULL,
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0
    CONSTRAINT FK_Evolucion_EntradaSalida FOREIGN KEY (IdEntradaSalida) REFERENCES dbo.EntradaSalida(Id),
    CONSTRAINT FK_Evolucion_Medio FOREIGN KEY (IdMedio) REFERENCES dbo.Medio(Id),
    CONSTRAINT FK_Evolucion_ApplicationUsers FOREIGN KEY (IdTecnico) REFERENCES dbo.ApplicationUsers(Id),
    CONSTRAINT FK_Evolucion_Provincia FOREIGN KEY (IdProvinciaAfectada) REFERENCES dbo.Provincia(Id),
    CONSTRAINT FK_Evolucion_Municipio FOREIGN KEY (IdMunicipioAfectado) REFERENCES dbo.Municipio(Id),
    CONSTRAINT FK_Evolucion_EstadoIncendio FOREIGN KEY (IdEstadoIncendio) REFERENCES dbo.EstadoIncendio(Id)
);
