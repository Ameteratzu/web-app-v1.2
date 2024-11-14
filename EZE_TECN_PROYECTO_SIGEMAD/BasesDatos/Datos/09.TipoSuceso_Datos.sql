SET IDENTITY_INSERT dbo.TipoSuceso ON;

INSERT INTO dbo.TipoSuceso (Id, Nombre, Descripcion, EsMigrado) VALUES
	 (1, N'Incendios', N'Incendio', 1),
	 (2, N'Fma', N'Suceso FMA', 1),
	 (3, N'IncendiosEnExtranjero', N'Incendio Extranjero', 1),
	 (4, N'Ope', N'Ope', 1),
	 (5, N'OtraInformacion', N'Otra Información', 1),
	 (6, N'Incendios forestales', N'Incendios forestales', 0),
	 (7, N'Accidentes de aviación civil', N'Accidentes de aviación civil', 0),
	 (8, N'Accidentes sustancias biológicas', N'Accidentes sustancias biológicas', 0),
	 (9, N'Inundaciones', N'Inundaciones', 0),
	 (10, N'Químico', N'Químico', 0),
	 (11, N'Terremotos', N'Terremotos', 0),
	 (12, N'TMP', N'TMP', 0),
	 (13, N'Otros riesgos', N'Otros riesgos', 0),
	 (14, N'FMA', N'FMA', 0),
	 (15, N'Nuclear', N'Nuclear', 0),
	 (16, N'Radiológico', N'Radiológico', 0),
	 (17, N'Maremoto', N'Maremoto', 0),
	 (18, N'Volcanes', N'Volcanes', 0);

SET IDENTITY_INSERT dbo.TipoSuceso OFF;