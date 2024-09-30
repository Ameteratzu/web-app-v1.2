SET IDENTITY_INSERT dbo.EstadoEvolucion ON;

INSERT INTO dbo.EstadoEvolucion (Id,Descripcion) VALUES
	 (1,N'Activo'),
	 (2,N'Estabilizado'),
	 (3,N'Controlado'),
	 (4,N'Extinguido');

SET IDENTITY_INSERT dbo.EstadoEvolucion OFF;
