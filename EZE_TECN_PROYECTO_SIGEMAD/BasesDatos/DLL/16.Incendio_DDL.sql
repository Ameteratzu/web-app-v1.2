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
	Denominacion varchar(255) NOT NULL,
	UTM_X decimal(18,9) NULL,
	UTM_Y decimal(18,9) NULL,
	Huso int NULL,
	GeoPosicion GEOMETRY,
	Comentarios text NULL,
	IdClaseSuceso int NOT NULL,
	CoordenadasReales bit NOT NULL,
	IdPrevisionPeligroGravedad int NOT NULL,
	FechaInicio datetime NOT NULL,
	Borrado bit NULL,
	FechaCreacion datetime,
	CreadoPor varchar(500),
	FechaModificacion datetime,
	ModificadoPor varchar(500),
	CONSTRAINT Sucesos_PK PRIMARY KEY (Id),
	CONSTRAINT ClaseSucesoIncendio FOREIGN KEY (IdClaseSuceso) REFERENCES dbo.ClaseSuceso(Id) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT FK_Incendio_NivelGravedad FOREIGN KEY (IdPrevisionPeligroGravedad) REFERENCES dbo.NivelGravedad(Id) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT MunicipioIncendio FOREIGN KEY (IdMunicipio) REFERENCES dbo.Municipio(Id) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT ProvinciaIncendio FOREIGN KEY (IdProvincia) REFERENCES dbo.Provincia(Id) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT SucesoIncendio FOREIGN KEY (IdSuceso) REFERENCES dbo.Suceso(Id) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT SucesoTerritorio FOREIGN KEY (IdTerritorio) REFERENCES dbo.Territorio(Id) ON DELETE NO ACTION ON UPDATE NO ACTION
);


CREATE INDEX IX_Incendio ON dbo.Incendio (Denominacion);
CREATE INDEX IX_Incendio_1 ON dbo.Incendio (IdSuceso);
CREATE INDEX IX_Incendio_2 ON dbo.Incendio (IdMunicipio);
CREATE INDEX IX_Incendio_3 ON dbo.Incendio (FechaCreacion,FechaModificacion);