--SET IDENTITY_INSERT dbo.EstadoSuceso ON;

INSERT INTO dbo.EstadoSuceso (Id, Descripcion) VALUES
	 (1, N'En curso'),
	 (2, N'Sin seguimiento'),
	 (3, N'Cerrado');

--SET IDENTITY_INSERT dbo.EstadoSuceso OFF;