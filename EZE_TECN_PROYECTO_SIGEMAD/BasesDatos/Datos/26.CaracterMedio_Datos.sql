SET IDENTITY_INSERT dbo.CaracterMedio ON;

INSERT INTO dbo.CaracterMedio (Id,Descripcion) VALUES
	 (1,N'Ordinario'),
	 (2,N'Extraordinario'),
	 (3,N'En Prácticas');

SET IDENTITY_INSERT dbo.CaracterMedio OFF;