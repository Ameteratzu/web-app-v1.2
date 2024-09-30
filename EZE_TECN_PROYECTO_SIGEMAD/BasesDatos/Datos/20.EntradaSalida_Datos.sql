SET IDENTITY_INSERT dbo.EntradaSalida ON;

INSERT INTO dbo.EntradaSalida (Id, Descripcion) VALUES
	 (1, N'Entrada'),
	 (2, N'Interior'),
	 (3, N'Salida');

SET IDENTITY_INSERT dbo.EntradaSalida OFF;