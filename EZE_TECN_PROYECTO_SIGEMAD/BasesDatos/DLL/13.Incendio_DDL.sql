-- dbo.Incendio definition

-- Drop table

-- DROP TABLE dbo.Incendio;

CREATE TABLE dbo.Incendio (
	Id int NOT NULL IDENTITY(1,1),
	IdSuceso int NOT NULL,
	IdProvincia int NOT NULL,
	IdMunicipio int NOT NULL,
	Denominacion varchar(255) NOT NULL,
	UTM_X int NULL,
	UTM_Y int NULL,
	Huso varchar(3) NULL,
	GeoPosicion GEOMETRY,
	Comentarios text NULL,
	FechaAlta datetime NOT NULL,
	UsrAuditoria varchar(255) NOT NULL,
	FechaAuditoria datetime NOT NULL,
	Borrado bit NULL,
	UsrAltaAuditoria varchar(500) NOT NULL,
	IdTipoSuceso int NOT NULL,
	CoordenadasReales bit NOT NULL,
	IdPrevisionPeligroGravedad int NOT NULL,
	FechaAltaAuditoria datetime NULL,
	CONSTRAINT Sucesos_PK PRIMARY KEY (Id),
	CONSTRAINT TipoSucesoIncendio FOREIGN KEY (IdTipoSuceso) REFERENCES TipoSuceso(Id) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT FK_Incendio_NivelGravedad FOREIGN KEY (IdPrevisionPeligroGravedad) REFERENCES NivelGravedad(Id) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT MunicipioIncendio FOREIGN KEY (IdMunicipio) REFERENCES Municipio(Id) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT ProvinciaIncendio FOREIGN KEY (IdProvincia) REFERENCES Provincia(Id) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT SucesoIncendio FOREIGN KEY (IdSuceso) REFERENCES Suceso(Id) ON DELETE NO ACTION ON UPDATE NO ACTION
);
CREATE INDEX IX_Incendio ON dbo.Incendio (Denominacion);
CREATE INDEX IX_Incendio_1 ON dbo.Incendio (IdSuceso);
CREATE INDEX IX_Incendio_2 ON dbo.Incendio (IdMunicipio);
CREATE INDEX IX_Incendio_3 ON dbo.Incendio (FechaAlta,FechaAuditoria);