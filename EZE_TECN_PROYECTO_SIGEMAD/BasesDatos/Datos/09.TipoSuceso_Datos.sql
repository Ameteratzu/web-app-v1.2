SET IDENTITY_INSERT dbo.TipoSuceso ON;

INSERT INTO dbo.TipoSuceso (Id, Descripcion) VALUES
	 (1, N'Incendio'),
	 (2, N'Suceso FMA'),
	 (3, N'Incendio Extranjero'),
	 (4, N'Ope'),
	 (5, N'Otra Información');

SET IDENTITY_INSERT dbo.TipoSuceso OFF;