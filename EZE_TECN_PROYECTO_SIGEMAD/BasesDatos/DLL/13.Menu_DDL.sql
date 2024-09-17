-- dbo.menu definition

-- Drop table

-- DROP TABLE dbo.menu;

CREATE TABLE dbo.Menu (
	Id int NOT NULL,
	Nombre varchar(30) NOT NULL,
	Tipo varchar(5) NOT NULL,
	IdGrupo int NOT NULL,
	NumOrden int NOT NULL,
	Icono varchar(100) NULL,
	ColorRgb varchar(6) NULL,
	CONSTRAINT PK_menu PRIMARY KEY (Id)
);