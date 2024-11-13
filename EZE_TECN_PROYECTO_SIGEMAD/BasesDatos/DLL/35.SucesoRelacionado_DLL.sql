CREATE TABLE SucesoRelacionado (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    IdSucesoPrincipal INT NOT NULL FOREIGN KEY REFERENCES Suceso(Id),
    IdSucesoAsociado INT NOT NULL FOREIGN KEY REFERENCES Suceso(Id),
    Observaciones NVARCHAR(MAX) NULL,
    ---
    FechaCreacion DATETIME2(7) NOT NULL,
    CreadoPor UNIQUEIDENTIFIER NULL,
    FechaModificacion DATETIME2(7) NULL,
    ModificadoPor UNIQUEIDENTIFIER NULL,
    FechaEliminacion DATETIME2(7) NULL,
    EliminadoPor UNIQUEIDENTIFIER NULL,
    Borrado BIT NOT NULL DEFAULT 0,
    CONSTRAINT CHK_SucesoRelacionados_Diferentes CHECK (IdSucesoPrincipal <> IdSucesoAsociado)
);