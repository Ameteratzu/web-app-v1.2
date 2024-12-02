CREATE TABLE TipoDireccionEmergencia (
	Id INT NOT NULL IDENTITY(1,1),
	Descripcion NVARCHAR(255) NOT NULL,
	CONSTRAINT PK_TipoDireccionEmergencia PRIMARY KEY (Id)
);

CREATE TABLE TipoPlan (
	Id INT NOT NULL IDENTITY(1,1),
	Descripcion NVARCHAR(255) NOT NULL,
	CONSTRAINT TipoPlan_PK PRIMARY KEY (Id)
);

CREATE TABLE TipoPlanMapeo (
    IdAntiguo INT NOT NULL PRIMARY KEY,
    IdNuevo INT NOT NULL
);


CREATE TABLE TipoRiesgo (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    Descripcion NVARCHAR(255) NOT NULL,
    IdTipoSuceso int NULL FOREIGN KEY REFERENCES TipoSuceso(Id)
);

-- Tabla principal para almacenar la información general de la dirección y coordinación
CREATE TABLE DireccionCoordinacionEmergencia (
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

CREATE TABLE Direccion (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdDireccionCoordinacionEmergencia INT NOT NULL FOREIGN KEY REFERENCES DireccionCoordinacionEmergencia(Id),
    IdTipoDireccionEmergencia INT NOT NULL FOREIGN KEY REFERENCES TipoDireccionEmergencia(Id),  -- Tipo de dirección, ejemplo: "Autonómica"
    AutoridadQueDirige NVARCHAR(255) NOT NULL,
    FechaInicio DATE NOT NULL,
    FechaFin DATE NULL,
    ---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0
);


CREATE TABLE CoordinacionCecopi (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdDireccionCoordinacionEmergencia INT NOT NULL FOREIGN KEY REFERENCES DireccionCoordinacionEmergencia(Id),
    FechaInicio DATE NOT NULL,
    FechaFin DATE NULL,
    IdProvincia INT NOT NULL FOREIGN KEY REFERENCES Provincia(Id),
    IdMunicipio INT NOT NULL FOREIGN KEY REFERENCES Municipio(Id),
    Lugar NVARCHAR(255) NOT NULL,
    GeoPosicion GEOMETRY NULL,
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


CREATE TABLE CoordinacionPMA (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdDireccionCoordinacionEmergencia INT NOT NULL FOREIGN KEY REFERENCES DireccionCoordinacionEmergencia(Id),
    FechaInicio DATE NOT NULL,
    FechaFin DATE NULL,
    IdProvincia INT NOT NULL FOREIGN KEY REFERENCES Provincia(Id),
    IdMunicipio INT NOT NULL FOREIGN KEY REFERENCES Municipio(Id),
    Lugar NVARCHAR(255) NOT NULL,
    GeoPosicion GEOMETRY NULL,
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


CREATE TABLE PlanEmergencia (
  Id INT NOT NULL PRIMARY KEY,
  Codigo VARCHAR(15) NOT NULL,
  Descripcion NVARCHAR(255) NOT NULL,
  IdCCAA INT NULL FOREIGN KEY REFERENCES CCAA(Id),
  IdProvincia INT NULL FOREIGN KEY REFERENCES Provincia(Id),
  IdMunicipio INT NULL FOREIGN KEY REFERENCES Municipio(Id),
  IdTipoPlan INT NOT NULL FOREIGN KEY REFERENCES TipoPlan(Id),
  IdTipoRiesgo INT NOT NULL FOREIGN KEY REFERENCES TipoRiesgo(Id)
);

CREATE INDEX IX_Codigo ON PlanEmergencia (Codigo);
