
-- =============================================
-- DATOS MAESTROS
-- =============================================
CREATE TABLE TipoRiesgo (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    Descripcion NVARCHAR(255) NOT NULL,
    IdTipoSuceso int NULL FOREIGN KEY REFERENCES TipoSuceso(Id),
    Codigo VARCHAR(5) NOT NULL
);


CREATE TABLE TipoPlan (
	Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Descripcion NVARCHAR(255) NOT NULL,
    Codigo VARCHAR(5) NOT NULL
);

CREATE TABLE TipoPlanMapeo (
    IdAntiguo INT NOT NULL PRIMARY KEY,
    IdNuevo INT NOT NULL
);

CREATE TABLE AmbitoPlan (
	Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Descripcion NVARCHAR(255) NOT NULL,
);


CREATE TABLE PlanEmergencia (
  Id INT NOT NULL PRIMARY KEY,
  Codigo NVARCHAR(15) NOT NULL,
  Descripcion NVARCHAR(255) NOT NULL,
  IdCCAA INT NULL FOREIGN KEY REFERENCES CCAA(Id),
  IdProvincia INT NULL FOREIGN KEY REFERENCES Provincia(Id),
  IdMunicipio INT NULL FOREIGN KEY REFERENCES Municipio(Id),
  IdTipoPlan INT NOT NULL FOREIGN KEY REFERENCES TipoPlan(Id),
  IdTipoRiesgo INT NOT NULL FOREIGN KEY REFERENCES TipoRiesgo(Id),
  IdAmbitoPlan INT NOT NULL FOREIGN KEY REFERENCES AmbitoPlan(Id),
);

CREATE INDEX IX_Codigo ON PlanEmergencia (Codigo);

-- =============================================
-- DATOS PANTALLAS
-- =============================================

CREATE TABLE ActivacionPlanEmergencia (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdDireccionCoordinacionEmergencia INT NOT NULL FOREIGN KEY REFERENCES DireccionCoordinacionEmergencia(Id),
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
