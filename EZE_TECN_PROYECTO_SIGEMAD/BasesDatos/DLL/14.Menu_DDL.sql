-- dbo.menu definition

-- Drop table

-- DROP TABLE dbo.menu;

CREATE TABLE dbo.Menu (
	id int NOT NULL,
	nombre varchar(30) NOT NULL,
	tipo varchar(5) NOT NULL,
	id_grupo int NOT NULL,
	num_orden int NOT NULL,
	icono varchar(100) NULL,
	color_RGB varchar(6) NULL,
	CONSTRAINT PK_menu PRIMARY KEY (id)
);