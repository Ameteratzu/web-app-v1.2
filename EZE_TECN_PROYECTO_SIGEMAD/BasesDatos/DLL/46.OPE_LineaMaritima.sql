DROP TABLE IF EXISTS dbo.OPE_LineaMaritima;
GO


CREATE TABLE [dbo].[OPE_LineaMaritima](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](255) NOT NULL,
	[IdOpePuertoOrigen] int NOT NULL FOREIGN KEY REFERENCES dbo.OPE_Puerto(Id),
	[IdOpePuertoDestino] int NOT NULL FOREIGN KEY REFERENCES dbo.OPE_Puerto(Id),
	[IdOpeFase] int NOT NULL FOREIGN KEY REFERENCES dbo.OPE_Fase(Id),
	[FechaValidezDesde] [datetime2](7) NOT NULL,
	[FechaValidezHasta] [datetime2](7) NOT NULL,
	[NumeroRotaciones] int NOT NULL,
	[NumeroPasajeros] int NOT NULL,
	[NumeroTurismos] int NOT NULL,
	[NumeroAutocares] int NOT NULL,
	[NumeroCamiones] int NOT NULL,
	[NumeroTotalVehiculos] int NOT NULL,
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




