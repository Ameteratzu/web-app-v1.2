CREATE TABLE dbo.SituacionOperativa (
	Id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	Descripcion NVARCHAR(255) NOT NULL,
);

CREATE TABLE SituacionEquivalente (
	Id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	Descripcion NVARCHAR(255) NOT NULL,
    Obsoleto BIT NOT NULL,
    Prioridad INT NOT NULL DEFAULT 0
);


CREATE TABLE dbo.Evolucion (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdSuceso int NOT NULL FOREIGN KEY REFERENCES Suceso(Id),
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
    Id int NOT NULL PRIMARY KEY FOREIGN KEY REFERENCES Evolucion(Id),
    FechaHoraEvolucion DATETIME2(7) NULL,
    IdEntradaSalida int NULL FOREIGN KEY REFERENCES EntradaSalida(Id),
    IdMedio int NULL FOREIGN KEY REFERENCES Medio(Id),
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
	IdRegistro int NOT NULL FOREIGN KEY REFERENCES Evolucion(Id),
	IdProcedenciaDestino int NOT NULL FOREIGN KEY REFERENCES ProcedenciaDestino(Id),
    ---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
    CreadoPor UNIQUEIDENTIFIER NULL,
    FechaModificacion DATETIME2(7) NULL,
    ModificadoPor UNIQUEIDENTIFIER NULL,
    FechaEliminacion DATETIME2(7) NULL,
    EliminadoPor UNIQUEIDENTIFIER NULL,
    Borrado BIT NOT NULL DEFAULT 0
);

CREATE TABLE dbo.DatoPrincipal (
	--Id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Id int NOT NULL PRIMARY KEY FOREIGN KEY REFERENCES Evolucion(Id),
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
	Id int NOT NULL PRIMARY KEY FOREIGN KEY REFERENCES Evolucion(Id),
    IdEstadoIncendio int NULL FOREIGN KEY REFERENCES EstadoIncendio(Id),
    FechaFinal DATETIME2(7) NULL,
    IdPlanEmergencia INT NULL FOREIGN KEY REFERENCES PlanEmergencia(Id),
    IdFaseEmergencia INT NULL FOREIGN KEY REFERENCES FaseEmergencia(Id),
    IdPlanSituacion INT NULL FOREIGN KEY REFERENCES PlanSituacion(Id),
    IdSituacionEquivalente INT NULL FOREIGN KEY REFERENCES SituacionEquivalente(Id),
    SuperficieAfectadaHectarea DECIMAL(10, 2) NULL,
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
    IdEvolucion INT NOT NULL FOREIGN KEY REFERENCES Evolucion(Id),
    FechaHora DATETIME2(7) NOT NULL,
    IdProvincia INT NOT NULL FOREIGN KEY REFERENCES Provincia(Id),
    IdMunicipio INT NOT NULL FOREIGN KEY REFERENCES Municipio(Id),
    IdEntidadMenor INT NULL FOREIGN KEY REFERENCES EntidadMenor(Id),
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
