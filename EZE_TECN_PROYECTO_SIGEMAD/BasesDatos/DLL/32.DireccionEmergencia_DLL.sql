DROP TABLE IF EXISTS dbo.ActivacionPlanesEmergencia;
GO

DROP TABLE IF EXISTS dbo.TipoPlan;
GO

DROP TABLE IF EXISTS dbo.DireccionCoordinacionEmergencia;
GO

DROP TABLE IF EXISTS dbo.TipoDireccionEmergencia;
GO



CREATE TABLE TipoDireccionEmergencia (
	Id INT NOT NULL IDENTITY(1,1),
	Descripcion NVARCHAR(255) NOT NULL,
	CONSTRAINT PK_TipoDireccionEmergencia PRIMARY KEY (Id)
);

-- Tabla principal para almacenar la información general de la dirección y coordinación
CREATE TABLE DireccionCoordinacionEmergencia (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdIncendio int NOT NULL FOREIGN KEY REFERENCES Incendio(Id),
    IdTipoDireccionEmergencia INT NOT NULL FOREIGN KEY REFERENCES dbo.TipoDireccionEmergencia(Id),  -- Tipo de dirección, ejemplo: "Autonómica"
    AutoridadQueDirige NVARCHAR(255) NOT NULL,  -- Autoridad que dirige la dirección
    FechaInicio DATETIME2(7) NULL,  -- Fecha de inicio de la dirección
    FechaFin DATETIME2(7) NULL,  -- Fecha de fin de la dirección
    
    -- Campos de Coordinacion CECOFI
    FechaInicioCECOPI DATETIME2(7) NULL,  -- Fecha de inicio de la coordinación en CECOFI
    FechaFinCECOPI DATETIME2(7) NULL,  -- Fecha de fin de la coordinación en CECOFI
    IdProvinciaCECOPI INT NOT NULL FOREIGN KEY REFERENCES dbo.Provincia(Id),
    IdMunicipioCECOPI INT NOT NULL FOREIGN KEY REFERENCES dbo.Municipio(Id),
    LugarCECOPI NVARCHAR(255) NULL,  -- Lugar de la coordinación en CECOFI
    GeoPosicionCECOPI GEOMETRY NULL,
    ObservacionesCECOPI NVARCHAR(MAX) NULL,  -- Observaciones adicionales para CECOFI

    -- Campos de Coordinacion PMA
    FechaInicioPMA DATETIME2(7) NOT NULL,  -- Fecha de inicio de la coordinación en PMA
    FechaFinPMA DATETIME2(7) NULL,  -- Fecha de fin de la coordinación en PMA
    IdProvinciaPMA INT NOT NULL FOREIGN KEY REFERENCES dbo.Provincia(Id),
    IdMunicipioPMA INT NOT NULL FOREIGN KEY REFERENCES dbo.Municipio(Id),
    LugarPMA NVARCHAR(255) NULL,  -- Lugar de la coordinación en PMA
    GeoPosicionPMA GEOMETRY NULL,
    ObservacionesPMA NVARCHAR(MAX) NULL,  -- Observaciones adicionales para PMA

    ---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0
);

CREATE TABLE dbo.TipoPlan (
	Id INT NOT NULL IDENTITY(1,1),
	Descripcion NVARCHAR(255) NOT NULL,
	CONSTRAINT TipoPlan_PK PRIMARY KEY (Id)
);

-- Tabla para almacenar la activación de planes de emergencia asociada a la dirección (Relación 1 a 1)
CREATE TABLE ActivacionPlanesEmergencia (
    IdDireccionCoordinacionEmergencia INT NOT NULL PRIMARY KEY FOREIGN KEY REFERENCES dbo.DireccionCoordinacionEmergencia(Id),
    IdTipoPlan INT NOT NULL FOREIGN KEY REFERENCES dbo.TipoPlan(Id),
    NombrePlan NVARCHAR(255) NOT NULL,  -- Nombre del plan
    AutoridadQueLoActiva NVARCHAR(255) NOT NULL,  -- Autoridad que activa el plan
    RutaDocumentoActivacion NVARCHAR(255) NULL,  -- Documento de activación asociado
    FechaInicio DATETIME2(7) NOT NULL,  -- Fecha de inicio del plan
    FechaFin DATETIME2(7) NULL,  -- Fecha de fin del plan
    Observaciones NVARCHAR(MAX) NULL,  -- Observaciones adicionales
    ---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0
);
