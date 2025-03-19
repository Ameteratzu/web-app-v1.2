DROP TABLE IF EXISTS dbo.OPE_PeriodoTipo;
GO


CREATE TABLE [dbo].[OPE_PeriodoTipo](
	[Id] [int] NOT NULL,
	[Nombre] [nvarchar](255) NOT NULL,
	[Borrado] [bit] NOT NULL,
	
PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
    
    -- Agregar restricción UNIQUE para la columna Nombre
    CONSTRAINT UC_OPE_PeriodoTipo_Nombre UNIQUE ([Nombre])
) ON [PRIMARY]
GO






