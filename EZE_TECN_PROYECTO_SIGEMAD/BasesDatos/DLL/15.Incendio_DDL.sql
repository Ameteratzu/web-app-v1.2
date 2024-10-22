-- dbo.Incendio definition

-- Verificar si la tabla Incendio existe, si es as�, eliminarla
IF OBJECT_ID('dbo.Incendio', 'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.Incendio;
END
GO

CREATE TABLE dbo.Incendio (
	Id int NOT NULL IDENTITY(1,1),
	IdSuceso int NOT NULL,
	IdProvincia int NOT NULL,
	IdMunicipio int NOT NULL,
	IdTerritorio int NOT NULL,
	IdPais int NOT NULL,
	IdEstadoSuceso int NOT NULL,
	Denominacion varchar(255) NOT NULL,
	UTM_X decimal(18,9) NULL,
	UTM_Y decimal(18,9) NULL,
	Huso int NULL,
	GeoPosicion GEOMETRY,
	Contenido text NULL,
	Comentarios text NULL,
	IdClaseSuceso int NOT NULL,
	CoordenadasReales bit NOT NULL,
	FechaInicio datetime NOT NULL,
	RutaMapaRiesgo NVARCHAR(MAX),
	---
    FechaCreacion DATETIME2(7) NOT NULL,
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0
	CONSTRAINT Sucesos_PK PRIMARY KEY (Id),
	CONSTRAINT ClaseSucesoIncendio FOREIGN KEY (IdClaseSuceso) REFERENCES dbo.ClaseSuceso(Id),
	CONSTRAINT MunicipioIncendio FOREIGN KEY (IdMunicipio) REFERENCES dbo.Municipio(Id),
	CONSTRAINT ProvinciaIncendio FOREIGN KEY (IdProvincia) REFERENCES dbo.Provincia(Id),
	CONSTRAINT SucesoIncendio FOREIGN KEY (IdSuceso) REFERENCES dbo.Suceso(Id),
	CONSTRAINT SucesoTerritorio FOREIGN KEY (IdTerritorio) REFERENCES dbo.Territorio(Id),
	CONSTRAINT FK_Incendio_EstadoSuceso FOREIGN KEY (IdEstadoSuceso) REFERENCES dbo.EstadoSuceso(Id),
	CONSTRAINT FK_Incendio_Pais FOREIGN KEY (IdPais) REFERENCES dbo.Pais(Id)
);


CREATE INDEX IX_Incendio ON dbo.Incendio (Denominacion);
CREATE INDEX IX_Incendio_1 ON dbo.Incendio (IdSuceso);
CREATE INDEX IX_Incendio_2 ON dbo.Incendio (IdMunicipio);
CREATE INDEX IX_Incendio_3 ON dbo.Incendio (FechaCreacion,FechaModificacion);