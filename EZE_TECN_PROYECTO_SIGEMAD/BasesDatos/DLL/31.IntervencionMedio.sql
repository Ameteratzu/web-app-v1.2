DROP TABLE IF EXISTS dbo.IntervencionMedio;
GO

CREATE TABLE dbo.IntervencionMedio (
	Id int NOT NULL IDENTITY(1,1),
	IdEvolucion int NOT NULL FOREIGN KEY REFERENCES Evolucion(Id),
	IdTipoIntervencionMedio int NOT NULL FOREIGN KEY REFERENCES TipoIntervencionMedio(Id),
	IdCaracterMedio int NOT NULL FOREIGN KEY REFERENCES CaracterMedio(Id),
	IdMunicipio int NOT NULL FOREIGN KEY REFERENCES Municipio(Id),
	Cantidad decimal(18,2) NULL,
	Unidad varchar(100) NULL,
	Titular varchar(1000) NULL,
	GeoPosicion GEOMETRY,
	Observaciones text NULL,
	---
    FechaCreacion datetime,
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion datetime,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaBorrado datetime,
	BorradoPor UNIQUEIDENTIFIER NULL,
	Borrado bit NULL
);
