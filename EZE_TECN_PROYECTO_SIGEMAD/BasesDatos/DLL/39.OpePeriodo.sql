DROP TABLE IF EXISTS dbo.OpePeriodo;
GO


CREATE TABLE [dbo].[OpePeriodo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](255) NOT NULL,
	[FechaInicioFaseSalida] [datetime2](7) NOT NULL,
	[FechaFinFaseSalida] [datetime2](7) NOT NULL,
	[FechaInicioFaseRetorno] [datetime2](7) NOT NULL,
	[FechaFinFaseRetorno] [datetime2](7) NOT NULL,
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




