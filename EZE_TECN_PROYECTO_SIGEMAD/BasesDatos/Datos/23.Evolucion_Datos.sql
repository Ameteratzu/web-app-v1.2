SET IDENTITY_INSERT dbo.SituacionOperativa ON;

INSERT INTO dbo.SituacionOperativa (Id, Descripcion) VALUES
	(1, N'Situación 0'),
	(2, N'Situación 1'),
	(3, N'Situación 2'),
	(4, N'Situación 4');

SET IDENTITY_INSERT dbo.SituacionOperativa OFF;

/*
* FASE
*/

SET IDENTITY_INSERT dbo.Fase ON;

insert into dbo.Fase (Id, Descripcion)
values  (1, N'Alerta'),
        (2, N'Alerta (Preemergencia)'),
        (3, N'Alerta Máxima'),
        (4, N'Alerta/Alerta máxima'),
        (5, N'Anticipación'),
        (6, N'Categoría 0'),
        (7, N'Desactivación del Plan'),
        (8, N'Emergencia'),
        (9, N'Fase 0'),
        (10, N'Fase 1'),
        (11, N'Fase 2'),
        (12, N'Fase 3'),
        (13, N'Fase de Intesificación del seguimiento y la información'),
        (14, N'Fase de Normalización'),
        (15, N'Fin de emergencia'),
        (16, N'Fin de la Emergencia'),
        (17, N'Normalidad'),
        (18, N'Normalización'),
        (19, N'Prealerta'),
        (20, N'Prealerta/Alerta'),
        (21, N'Preemergencia'),
        (22, N'Prevención Operativa'),
        (23, N'Recuperación'),
        (24, N'Rehabilitación'),
        (25, N'Seguimiento'),
        (26, N'Vuelta a la normalidad');

SET IDENTITY_INSERT dbo.Fase OFF;

/*
* SITUACION EQUIVALENTE
*/

SET IDENTITY_INSERT dbo.SituacionEquivalente ON;

insert into dbo.SituacionEquivalente (Id, Descripcion)
values  (1, N'0'),
        (2, N'1'),
        (3, N'2'),
        (4, N'3'),
        (5, N'E'),
        (6, N'-');

SET IDENTITY_INSERT dbo.SituacionEquivalente OFF;