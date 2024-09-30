SET IDENTITY_INSERT dbo.ClaseSuceso ON;

INSERT INTO dbo.ClaseSuceso (Id, Descripcion) VALUES
	 (1, N'Suceso Real'),
	 (2, N'Ejercicio'),
	 (3, N'Simulacro');

SET IDENTITY_INSERT dbo.ClaseSuceso OFF;
