CREATE TABLE [dbo].[OPE_EstadoOcupacion](
	[Id] [int] NOT NULL,
	[Nombre] [nvarchar](255) NOT NULL,
	[PorcentajeInferior] [int] NOT NULL,
	[PorcentajeSuperior] [int] NOT NULL,
	[Borrado] [bit] NOT NULL,
	
PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
    
    -- Agregar restricción UNIQUE para la columna Nombre
    CONSTRAINT UC_OPE_EstadoOcupacion_Nombre UNIQUE ([Nombre])
) ON [PRIMARY]
GO






