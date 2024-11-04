DROP TABLE IF EXISTS DetalleOtraInformacion_ProcedenciaDestino
GO

DROP TABLE IF EXISTS DetalleOtraInformacion
GO

DROP TABLE IF EXISTS OtraInformacion
GO


CREATE TABLE dbo.OtraInformacion (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdIncendio INT NULL FOREIGN KEY REFERENCES Incendio(Id), 
    ---
    FechaCreacion DATETIME2(7) NOT NULL,
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0
);

CREATE TABLE dbo.DetalleOtraInformacion (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdOtraInformacion int NOT NULL FOREIGN KEY REFERENCES OtraInformacion(Id),
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

CREATE TABLE dbo.DetalleOtraInformacion_ProcedenciaDestino (
	Id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdDetalleOtraInformacion int NOT NULL FOREIGN KEY REFERENCES DetalleOtraInformacion(Id),
	IdProcedenciaDestino int NOT NULL FOREIGN KEY REFERENCES ProcedenciaDestino(Id),
    ---
    FechaCreacion DATETIME2(7) NOT NULL,
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0
);
