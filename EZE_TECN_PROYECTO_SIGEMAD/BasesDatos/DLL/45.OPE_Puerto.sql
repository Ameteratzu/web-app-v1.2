DROP TABLE IF EXISTS dbo.OPE_Puerto;
GO


CREATE TABLE [dbo].[OPE_Puerto](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](255) NOT NULL,
	[IdOpeFase] int NOT NULL FOREIGN KEY REFERENCES dbo.OPE_Fase(Id),
	[IdPais] int NOT NULL FOREIGN KEY REFERENCES dbo.Pais(Id),
	[IdCcaa] int NULL FOREIGN KEY REFERENCES dbo.CCAA(Id),
	[IdProvincia] int NULL FOREIGN KEY REFERENCES dbo.Provincia(Id),
	[IdMunicipio] int NULL FOREIGN KEY REFERENCES dbo.Municipio(Id),
	[CoordenadaUTM_X] [nvarchar](255) NOT NULL,
	[CoordenadaUTM_Y] [nvarchar](255) NOT NULL,
	[FechaValidezDesde] [datetime2](7) NOT NULL,
	[FechaValidezHasta] [datetime2](7) NOT NULL,
	[Capacidad] int NOT NULL,
	[FechaCreacion] [datetime2](7) NOT NULL,
	[CreadoPor] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime2](7) NULL,
	[ModificadoPor] [uniqueidentifier] NULL,
	[FechaEliminacion] [datetime2](7) NULL,
	[EliminadoPor] [uniqueidentifier] NULL,
	[Borrado] [bit] NOT NULL,
	
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO




