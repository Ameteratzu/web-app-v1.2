CREATE TABLE [dbo].[OPE_DatoFrontera](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdOpeFrontera] int NOT NULL FOREIGN KEY REFERENCES dbo.OPE_Frontera(Id),
	[FechaHoraInicioIntervalo] [datetime2](7) NOT NULL,
	[FechaHoraFinIntervalo] [datetime2](7) NOT NULL,
	[NumeroVehiculos] int NOT NULL,
	[Afluencia] [nvarchar](255) NOT NULL,
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




