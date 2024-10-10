SET IDENTITY_INSERT dbo.EstadoIncendio ON;

INSERT INTO dbo.EstadoIncendio (Id, Descripcion, Obsoleto) VALUES
	 (1, N'Activo', 0),
	 (2, N'Extinguido', 0),
	 (3, N'Controlado', 0),
	 (17, N'Reavivado', 0),
	 (18, N'Desconocido', 0),
	 (19, N'Estabilizado', 0),
	 (20, N'Sin seguimiento', 0),

SET IDENTITY_INSERT dbo.EstadoIncendio OFF;