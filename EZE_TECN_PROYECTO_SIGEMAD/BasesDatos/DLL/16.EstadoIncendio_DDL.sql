-- dbo.EstadoIncendio definition

-- Verificar si la tabla EstadoIncendio existe, si es así, eliminarla
IF OBJECT_ID('dbo.EstadoIncendio', 'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.EstadoIncendio;
END
GO

CREATE TABLE dbo.EstadoIncendio (
	Id int NOT NULL IDENTITY(1,1),
	Descripcion varchar (255) NOT NULL,	
    Obsoleto BIT NOT NULL,
    CONSTRAINT PK_EstadoIncendio PRIMARY KEY (Id)	
);