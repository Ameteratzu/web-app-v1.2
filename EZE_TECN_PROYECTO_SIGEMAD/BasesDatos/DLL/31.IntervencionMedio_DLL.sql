DROP TABLE IF EXISTS dbo.IntervencionMedio;
GO

CREATE TABLE dbo.IntervencionMedio (
	Id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	IdEvolucion int NOT NULL FOREIGN KEY REFERENCES dbo.Evolucion(Id),
	IdTipoIntervencionMedio int NOT NULL FOREIGN KEY REFERENCES dbo.TipoIntervencionMedio(Id),
	IdCaracterMedio int NOT NULL FOREIGN KEY REFERENCES dbo.CaracterMedio(Id),
	IdClasificacionMedio int NOT NULL FOREIGN KEY REFERENCES dbo.ClasificacionMedio(Id),
	IdTitularidadMedio int NOT NULL FOREIGN KEY REFERENCES dbo.TitularidadMedio(Id),
	IdMunicipio int NOT NULL FOREIGN KEY REFERENCES dbo.Municipio(Id),
	Cantidad int NOT NULL,
	Unidad varchar(100) NOT NULL,
	Titular varchar(255) NOT NULL,
	GeoPosicion GEOMETRY,
	Observaciones text NULL,
	---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0
);
