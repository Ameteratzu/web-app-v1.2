SET IDENTITY_INSERT dbo.SituacionOperativa ON;

INSERT INTO dbo.SituacionOperativa (Id, Descripcion) VALUES
	(1, N'Situación 0'),
	(2, N'Situación 1'),
	(3, N'Situación 2'),
	(4, N'Situación 4');

SET IDENTITY_INSERT dbo.SituacionOperativa OFF;


SET IDENTITY_INSERT SituacionEquivalente ON;

INSERT INTO SituacionEquivalente (Id, Descripcion, Obsoleto) VALUES
	(1, N'Situación 0', 1),
	(2, N'Situación 1', 1),
	(3, N'Situación 2', 1),
	(4, N'Situación 4', 1),
	(5, N'0', 0),
	(6, N'1', 0),
	(7, N'2', 0),
	(8, N'3', 0),
	(9, N'E', 0),
	(10, N'-', 0);

SET IDENTITY_INSERT SituacionEquivalente OFF;
