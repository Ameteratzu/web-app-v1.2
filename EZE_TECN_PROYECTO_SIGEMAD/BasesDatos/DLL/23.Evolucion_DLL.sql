DROP TABLE IF EXISTS dbo.Evolucion;
GO

DROP TABLE IF EXISTS dbo.SituacionOperativa;
GO


CREATE TABLE dbo.SituacionOperativa (
	Id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	Descripcion NVARCHAR(255) NOT NULL,
);

CREATE TABLE dbo.Fase (
	Id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	Descripcion NVARCHAR(255) NOT NULL,
);

CREATE TABLE dbo.SituacionEquivalente (
	Id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	Descripcion NVARCHAR(255) NOT NULL,
);


CREATE TABLE dbo.Evolucion (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdIncendio int NOT NULL FOREIGN KEY REFERENCES Incendio(Id),
    ---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0
);

CREATE TABLE dbo.Registro (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdEvolucion int NOT NULL FOREIGN KEY REFERENCES Evolucion(Id),
    FechaHoraEvolucion DATETIME2(7) NOT NULL,
    IdEntradaSalida int NOT NULL FOREIGN KEY REFERENCES EntradaSalida(Id),
    IdMedio int NOT NULL FOREIGN KEY REFERENCES Medio(Id),
    ---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0
);

CREATE TABLE dbo.Registro_ProcedenciaDestino (
	Id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdRegistroEvolucion int NOT NULL FOREIGN KEY REFERENCES RegistroEvolucion(Id),
	IdProcedenciaDestino int NOT NULL FOREIGN KEY REFERENCES ProcedenciaDestino(Id)
);

CREATE TABLE dbo.DatoPrincipal (
	Id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdEvolucion int NOT NULL FOREIGN KEY REFERENCES Evolucion(Id),
    FechaHora DATETIME2(7) NULL,
    Observaciones NVARCHAR(MAX) NULL,
    Prevision NVARCHAR(MAX) NULL,
    ---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0
);


CREATE TABLE dbo.Parametro (
	Id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdEvolucion int NOT NULL FOREIGN KEY REFERENCES Evolucion(Id),
    IdEstadoIncendio int NULL FOREIGN KEY REFERENCES EstadoIncendio(Id),
    FechaFinal DATETIME2(7) NULL,
    SuperficieAfectadaHectarea DECIMAL(10, 2) NULL,
    PlanEmergenciaActivado NVARCHAR(255) NULL,
    IdFase INT NOT NULL FOREIGN KEY REFERENCES Fase(Id),
    IdSituacionOperativa int NOT NULL FOREIGN KEY REFERENCES SituacionOperativa(Id),
    IdSituacionEquivalente INT NOT NULL FOREIGN KEY REFERENCES SituacionEquivalente(Id),
    ---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
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
    GeoPosicion GEOMETRY,
    Observaciones NVARCHAR(MAX) NULL,
    ---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0
);
