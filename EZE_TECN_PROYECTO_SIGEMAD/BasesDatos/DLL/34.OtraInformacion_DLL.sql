CREATE TABLE dbo.OtraInformacion (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdIncendio INT NULL FOREIGN KEY REFERENCES Incendio(Id), 
    FechaHora DATETIME2(7) NOT NULL,
    IdMedio INT NOT NULL FOREIGN KEY REFERENCES Medio(Id),
    Asunto NVARCHAR(500) NULL,
    Observaciones NVARCHAR(MAX) NULL,
    ---
    FechaCreacion DATETIME2(7) NOT NULL,
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0
);

CREATE TABLE dbo.OtraInformacion_ProcedenciaDestino (
	Id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdOtraInformacion int NOT NULL FOREIGN KEY REFERENCES OtraInformacion(Id),
	IdProcedenciaDestino int NOT NULL FOREIGN KEY REFERENCES ProcedenciaDestino(Id)
);


----------------------------------------------------------
-- En caso solo es un Procedencia/Destino por informacion
----------------------------------------------------------

CREATE TABLE dbo.OtraInformacionV2 (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdIncendio INT NULL FOREIGN KEY REFERENCES Incendio(Id), 
    FechaHora DATETIME2(7) NOT NULL,
    IdProcedenciaDestino INT NULL FOREIGN KEY REFERENCES ProcedenciaDestino(Id),
    IdMedio INT NOT NULL FOREIGN KEY REFERENCES Medio(Id),
    Asunto NVARCHAR(500) NULL,
    Observaciones NVARCHAR(MAX) NULL,
    ---
    FechaCreacion DATETIME2(7) NOT NULL,
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0
);