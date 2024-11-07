
DROP TABLE IF EXISTS dbo.ImpactoEvolucion;
GO
DROP TABLE IF EXISTS dbo.ValidacionImpactoClasificado;
GO
DROP TABLE IF EXISTS dbo.ImpactoClasificado;
GO


CREATE TABLE dbo.ImpactoClasificado (
    Id INT PRIMARY KEY IDENTITY(1,1),
    TipoImpacto NVARCHAR(50) NOT NULL,  -- Ej: 'Consecuencia', 'Actuación'
    GrupoImpacto NVARCHAR(100) NOT NULL,  -- Ej: 'Personas', 'Servicios básicos'
    SubgrupoImpacto NVARCHAR(100) NOT NULL,  -- Ej: 'Personas', 'Anomalías en los servicios básicos'
    ClaseImpacto NVARCHAR(100) NOT NULL,  -- Ej: 'Fallecidos', 'Heridos'
    Descripcion NVARCHAR(255) NOT NULL,  -- Ej: 'Fallecidos', 'Quemados'
    RelevanciaGeneral BIT NOT NULL -- 1 si es relevante, 0 si no lo es
);

CREATE TABLE dbo.ValidacionImpactoClasificado (
    Id INT PRIMARY KEY IDENTITY(1,1),
    IdImpactoClasificado INT NOT NULL FOREIGN KEY REFERENCES ImpactoClasificado(Id),  -- Relaciona con ImpactoClasificado
    Campo NVARCHAR(100) NOT NULL,  -- El nombre del campo a validar, como 'Numero', 'Causa', etc.
    TipoCampo NVARCHAR(100) NOT NULL,
    EsObligatorio BIT NOT NULL,  -- Indica si el campo es obligatorio (1 = Sí, 0 = No),
    Etiqueta NVARCHAR(100) NOT NULL
);

CREATE TABLE dbo.TipoDanio (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Descripcion NVARCHAR(100) NOT NULL
);

CREATE TABLE dbo.ImpactoEvolucion (
    Id INT PRIMARY KEY IDENTITY(1,1),
    IdEvolucion INT NOT NULL FOREIGN KEY REFERENCES Evolucion(Id), 
    IdImpactoClasificado INT NOT NULL FOREIGN KEY REFERENCES ImpactoClasificado(Id),

    -- Campos para almacenar datos específicos según el tipo de impacto
    Nuclear BIT NULL,
    ValorAD INT NULL,
    Numero INT NULL,
    Observaciones NVARCHAR(MAX) NULL,
    Fecha DATE NULL,
    FechaHora DATETIME NULL,
    FechaHoraInicio DATETIME NULL,
    FechaHoraFin DATETIME NULL,
    AlteracionInterrupcion CHAR(1) NULL CHECK (AlteracionInterrupcion IN ('A','I')),
    Causa NVARCHAR(255) NULL,
    NumeroGraves INT NULL,
    IdTipoDanio INT NULL FOREIGN KEY REFERENCES TipoDanio(Id),
    ZonaPlanificacion GEOMETRY NULL,
    NumeroUsuarios INT NULL,
    NumeroIntervinientes INT NULL,
    NumeroServicios INT NULL,
    NumeroLocalidades INT NULL,
    ---
    FechaCreacion DATETIME2(7) NOT NULL,
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0
);
