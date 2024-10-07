DROP TABLE IF EXISTS dbo.Distrito;
GO

CREATE TABLE dbo.Distrito (
	Id int NOT NULL,
	IdPais int NOT NULL,
	Descripcion varchar(255) NOT NULL,
	CONSTRAINT Distrito_PK PRIMARY KEY (Id),
	CONSTRAINT PaisDistrito FOREIGN KEY (IdPais) REFERENCES Pais(Id) 
);
GO