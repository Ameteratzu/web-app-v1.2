
DROP TABLE IF EXISTS dbo.ImpactoEvolucion;
GO
DROP TABLE IF EXISTS dbo.Impacto;
GO
DROP TABLE IF EXISTS dbo.ClaseImpacto;
GO
DROP TABLE IF EXISTS dbo.SubgrupoImpacto;
GO
DROP TABLE IF EXISTS dbo.GrupoImpacto;
GO
DROP TABLE IF EXISTS dbo.TipoImpacto;
GO

-- Tabla TipoImpacto
CREATE TABLE TipoImpacto (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Descripcion VARCHAR(50) NOT NULL
);

-- Tabla GrupoImpacto
CREATE TABLE GrupoImpacto (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Descripcion VARCHAR(100) NOT NULL
);

-- Tabla SubgrupoImpacto
CREATE TABLE SubgrupoImpacto (
    Id INT PRIMARY KEY IDENTITY(1,1),
    IdGrupoImpacto INT NOT NULL FOREIGN KEY REFERENCES GrupoImpacto(Id),
    Descripcion VARCHAR(100) NOT NULL
);

-- Tabla ClaseImpacto
CREATE TABLE ClaseImpacto (
    Id INT PRIMARY KEY IDENTITY(1,1),
    IdSubgrupoImpacto INT NOT NULL FOREIGN KEY REFERENCES SubgrupoImpacto(Id),
    Descripcion VARCHAR(100) NOT NULL 
);

-- Tabla Impacto 
CREATE TABLE Impacto (
    Id INT PRIMARY KEY IDENTITY(1,1),
    IdTipoImpacto INT NOT NULL FOREIGN KEY REFERENCES TipoImpacto(Id),
    IdClaseImpacto INT NOT NULL FOREIGN KEY REFERENCES ClaseImpacto(Id),
    Descripcion VARCHAR(255) NOT NULL,
    RelevanciaGeneral BIT NOT NULL -- 1 si es relevante, 0 si no lo es
);


-- Tabla Impacto
CREATE TABLE ImpactoEvolucion (
    Id INT PRIMARY KEY IDENTITY(1,1),
    IdEvolucion INT NOT NULL FOREIGN KEY REFERENCES Evolucion(Id),
    IdClasificacionImpacto INT NOT NULL FOREIGN KEY REFERENCES Impacto(Id),    
);
