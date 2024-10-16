SET IDENTITY_INSERT dbo.TipoRegistro ON;

INSERT INTO dbo.TipoRegistro (Id, Descripcion) VALUES
	(1, N'Evolución'),
	(2, N'Resumen');


SET IDENTITY_INSERT dbo.TipoRegistro OFF;