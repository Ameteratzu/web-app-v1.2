DROP TABLE IF EXISTS dbo.Evolucion;
GO
CREATE TABLE dbo.Evolucion (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdIncendio int NOT NULL FOREIGN KEY REFERENCES Incendio(Id),
    FechaHoraEvolucion DATETIME2(7) NULL,
    IdEntradaSalida int NULL FOREIGN KEY REFERENCES EntradaSalida(Id),
    IdMedio int NULL FOREIGN KEY REFERENCES Medio(Id),
    IdTipoRegistro int NULL FOREIGN KEY REFERENCES TipoRegistro(Id),
    Observaciones TEXT NULL,
    Prevision TEXT NULL,
    IdEstadoIncendio int NULL FOREIGN KEY REFERENCES EstadoIncendio(Id),
    PlanEmergenciaActivado NVARCHAR(255) NULL,
    SuperficieAfectadaHectarea DECIMAL(10, 2) NULL,
    FechaFinal DATETIME2(7) NULL,
    ---
    FechaCreacion DATETIME2(7) NOT NULL,
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0
);

CREATE TABLE AreaAfectada (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdEvolucion int NOT NULL FOREIGN KEY REFERENCES Evolucion(Id),
    FechaHora DATETIME2(7) NOT NULL,
    IdProvincia int NOT NULL FOREIGN KEY REFERENCES Provincia(Id),
    IdMunicipio int NOT NULL FOREIGN KEY REFERENCES Municipio(Id),
    IdEntidadMenor int NOT NULL FOREIGN KEY REFERENCES EntidadMenor(Id),
    GeoPosicion GEOMETRY
);
