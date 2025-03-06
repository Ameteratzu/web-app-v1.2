CREATE TABLE SucesoRelacionado (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdSucesoPrincipal INT NOT NULL FOREIGN KEY REFERENCES Suceso(Id), 
    ---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0

	CONSTRAINT UQ_OtraInformacion_IdSuceso UNIQUE (IdSucesoPrincipal)
);

CREATE TABLE DetalleSucesoRelacionado (
    IdCabeceraSuceso INT NOT NULL FOREIGN KEY REFERENCES SucesoRelacionado(Id),
    IdSucesoAsociado INT NOT NULL FOREIGN KEY REFERENCES Suceso(Id),
    PRIMARY KEY (IdCabeceraSuceso, IdSucesoAsociado), -- Clave compuesta
    ---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0
);