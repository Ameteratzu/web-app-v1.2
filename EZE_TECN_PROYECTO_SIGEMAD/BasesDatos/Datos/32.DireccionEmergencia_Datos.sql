SET IDENTITY_INSERT dbo.TipoDireccionEmergencia ON;

INSERT INTO dbo.TipoDireccionEmergencia (Id,Descripcion) VALUES
	 (1,N'Estatal'),
	 (2,N'Autonómica'),
	 (3,N'Municipal'),
	 (4,N'Provincial'),
	 (5,N'Sin especificar');

SET IDENTITY_INSERT dbo.TipoDireccionEmergencia OFF;
