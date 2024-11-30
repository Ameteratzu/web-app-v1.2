CREATE TABLE dbo.TipoDocumento (
	Id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	Descripcion NVARCHAR(255) NOT NULL,
);

CREATE TABLE Documentacion (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdIncendio INT NOT NULL FOREIGN KEY REFERENCES Incendio(Id),
    ---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
    CreadoPor UNIQUEIDENTIFIER NULL,
    FechaModificacion DATETIME2(7) NULL,
    ModificadoPor UNIQUEIDENTIFIER NULL,
    FechaEliminacion DATETIME2(7) NULL,
    EliminadoPor UNIQUEIDENTIFIER NULL,
    Borrado BIT NOT NULL DEFAULT 0
);
 
 
CREATE TABLE DetalleDocumentacion (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdDocumentacion INT NOT NULL FOREIGN KEY REFERENCES Documentacion(Id),
    FechaHora DATETIME2(7) NOT NULL,
    FechaHoraSolicitud DATETIME2(7) NOT NULL,
    IdTipoDocumento INT NOT NULL FOREIGN KEY REFERENCES TipoDocumento(Id),
    Descripcion NVARCHAR(255) NOT NULL,
    IdArchivo UNIQUEIDENTIFIER NULL FOREIGN KEY REFERENCES Archivo(Id),
    ---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
    CreadoPor UNIQUEIDENTIFIER NULL,
    FechaModificacion DATETIME2(7) NULL,
    ModificadoPor UNIQUEIDENTIFIER NULL,
    FechaEliminacion DATETIME2(7) NULL,
    EliminadoPor UNIQUEIDENTIFIER NULL,
    Borrado BIT NOT NULL DEFAULT 0
);
 
 
CREATE TABLE dbo.Documentacion_ProcedenciaDestino (
    Id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    IdDetalleDocumentacion int NOT NULL FOREIGN KEY REFERENCES DetalleDocumentacion(Id),
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