SET IDENTITY_INSERT dbo.TipoDocumento ON;

INSERT INTO dbo.TipoDocumento (Id, Descripcion) VALUES
	(1, N'Imágenes/Fotos '),
	(2, N'Comprimido.zip'),
	(3, N'Documentación General'),
	(4, N'Intervención de medios DGMNPF (Provisional)'),
    (8, N'Informe de situación'),
    (9, N'Intervenciones UME'),
    (10, N'Aportaciones de ayuda'),
    (11, N'Solicitud de ayuda'),
    (12, N'Nota de prensa'),
    (13, N'Simulación'),
    (15, N'Previsión de peligro de incendio');

SET IDENTITY_INSERT dbo.TipoDocumento OFF;
