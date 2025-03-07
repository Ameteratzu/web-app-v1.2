CREATE TABLE TipoDireccionEmergencia (
	Id INT NOT NULL IDENTITY(1,1),
	Descripcion NVARCHAR(255) NOT NULL,
	CONSTRAINT PK_TipoDireccionEmergencia PRIMARY KEY (Id)
);

-- Tabla principal para almacenar la información general de la dirección y coordinación
CREATE TABLE DireccionCoordinacionEmergencia (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdSuceso int NOT NULL FOREIGN KEY REFERENCES Suceso(Id),
    ---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0,
    ---
    CONSTRAINT UQ_DireccionCoordinacionEmergencia_IdSuceso UNIQUE (IdSuceso)
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

