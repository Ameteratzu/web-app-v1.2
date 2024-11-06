SET IDENTITY_INSERT dbo.TipoDireccionEmergencia ON;

INSERT INTO dbo.TipoDireccionEmergencia (Id,Descripcion) VALUES
	 (1,N'Estatal'),
	 (2,N'Autonómica'),
	 (3,N'Municipal'),
	 (4,N'Provincial'),
	 (5,N'Sin especificar');

SET IDENTITY_INSERT dbo.TipoDireccionEmergencia OFF;


-- 

SET IDENTITY_INSERT dbo.TipoPlan ON;

INSERT INTO dbo.TipoPlan (Id,Descripcion) VALUES
	 (2,N'Estatal'),
	 --(3,N'Autonómica'),
	 --(4,N'Municipal');
	 (5,N'Territorial'),
	 (6,N'Especial de CA'),
	 (7,N'Autoprotección'),
	 (8,N'Otro');

SET IDENTITY_INSERT dbo.TipoPlan OFF;
