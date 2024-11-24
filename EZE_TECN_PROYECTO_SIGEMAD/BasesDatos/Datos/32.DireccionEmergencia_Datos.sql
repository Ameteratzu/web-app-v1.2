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
	(1	,'Estatal'),
	(2	,'Territorial'),
	(3	,'Especial'),
	(4	,'Normativa Básica'),
	(5	,'Autoprotección'),
	(6	,'Otros');

SET IDENTITY_INSERT dbo.TipoPlan OFF;


INSERT INTO TipoPlanMapeo (IdAntiguo, IdNuevo)
VALUES 
	(2, 1), --Estatal
	(5, 2),  --Territorial
	(6, 3), --Especial de CA
	(7, 5), --Autoprotección
	(8, 6); --Otros



-------

SET IDENTITY_INSERT TipoRiesgo ON;

INSERT INTO TipoRiesgo (Id,Descripcion, IdTipoSuceso) VALUES
	 (1, N'General', NULL),
	 (2, N'Accidentes con sustancias biológicas', 8),
	 (3, N'Accidentes con sustancias químicas', 10),
	 (4, N'Accidentes con sustancias radiactivas', 16),
	 (5, N'Accidentes de aviación civil', 7),
	 (6, N'Accidentes en centrales nucleares', 15),
	 (7, N'Accidentes en el transporte de mercancías peligrosas', 12),
	 (8, N'Accidentes en el transporte de viajeros por carretera y ferrocarril', NULL),
	 (9, N'Accidentes en túneles', NULL),
	 (10, N'Aludes', NULL),
	 (11, N'Bélico', NULL),
	 (12, N'Contaminación marina', NULL),
	 (13, N'Erupción volcánica', 18),
	 (14, N'Fenómenos Meteorológicos Adversos', 14),
	 (15, N'Incendios forestales', 6),
	 (16, N'Inundaciones', 9),
	 (17, N'Maremotos', 17),
	 (18, N'Nevadas', NULL),
	 (19, N'Otros', 13),
	 (20, N'Terremotos', 11),
	 (21, N'Viento', NULL);

SET IDENTITY_INSERT TipoRiesgo OFF;


insert into dbo.PlanEmergencia (Id, Codigo, Descripcion, IdCCAA, IdProvincia, IdMunicipio, IdTipoPlan, IdTipoRiesgo)
values  (1, N'01000000100', N'Plan Territorial de Emergencias de Protección Civil de Andalucía', 15, null, null, 2, 1),
        (2, N'01000000214', N'Plan de Emergencia por Riesgo de Incendios Forestales de Andalucía', 15, null, null, 3, 15),
        (3, N'01000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones en Andalucía', 15, null, null, 3, 16),
        (4, N'01000000206', N'Plan de Emergencia ante Accidentes en el Transporte de Mercancías Peligrosas por Carretera y Ferrocarril en Andalucía', 15, null, null, 3, 7),
        (5, N'01000000219', N'Plan de Emergencia ante el Riesgo Sísmico en Andalucía', 15, null, null, 3, 20),
        (6, N'01000000511', N'Plan de Emergencia ante el Riesgo de Contaminación del Litoral en Andalucía', 15, null, null, 6, 12),
        (7, N'01000000516', N'Plan de Emergencia ante el Riesgo de Maremotos en Andalucía', 15, null, null, 6, 17),
        (8, N'02000000100', N'Plan Territorial de Protección Civil de Aragón', 6, null, null, 2, 1),
        (9, N'02000000214', N'Plan de Emergencia por Incendios Forestales de Aragón', 6, null, null, 3, 15),
        (10, N'02000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones en Aragón', 6, null, null, 3, 16),
        (11, N'02000000206', N'Plan de Emergencia Producidas en el Transporte de Mercancías Peligrosas por Carretera y Ferrocarril en Andalucía', 6, null, null, 3, 7),
        (12, N'02000000203', N'Plan de Emergencia ante el Riesgo Radiológico en Aragón', 6, null, null, 3, 4),
        (13, N'02000000219', N'Plan de Emergencia ante el Riesgo Sísmico de Aragón', 6, null, null, 3, 20),
        (14, N'02000000202', N'Plan de Emergencia ante Accidentes en Gasoductos y Oleoductos de Aragón', 6, null, null, 3, 3),
        (15, N'03000000100', N'Plan Territorial de Protección Civil del Principado de Asturias', 2, null, null, 2, 1),
        (16, N'03000000214', N'Plan de Emergencia por Incendios Forestales del Principado de Asturias', 2, null, null, 3, 15),
        (17, N'03000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones del Principado de Asturias', 2, null, null, 3, 16),
        (18, N'03000000206', N'Plan de Emergencia ante el Transporte de Mercancías Peligrosas por Carretera y Ferrocarril del Principado de Asturias', 2, null, null, 3, 7),
        (19, N'03000000511', N'Plan de Emergencia frente a Contingencias por Contaminación Marina Accidental en Asturias', 2, null, null, 6, 12),
        (20, N'03000000517', N'Plan de Emergencia ante Nevadas en Asturias', 2, null, null, 6, 18),
        (21, N'04000000100', N'Plan Territorial de Protección Civil de Illes Balears', 13, null, null, 2, 1),
        (22, N'04000000214', N'Plan de Emergencia ante el Riesgo de Incendios Forestales en Illes Balears', 13, null, null, 3, 15),
        (23, N'04000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones en Illes Balears', 13, null, null, 3, 16),
        (24, N'04000000206', N'Plan de Emergencia ante el Riesgo de Transporte de Mercancías Peligrosas en Illes Balears', 13, null, null, 3, 7),
        (25, N'04000000203', N'Plan de Emergencia ante el Riesgo de Emergencias Radiológicas en Illes Balears', 13, null, null, 3, 4),
        (26, N'04000000219', N'Plan de Emergencia ante Riesgo Sísmico en Illes Balears', 13, null, null, 3, 20),
        (27, N'04000000213', N'Plan de Emergencia frente a Riesgo de Fenómenos Meteorológicos Adversos en Illes Balears', 13, null, null, 3, 14),
        (28, N'04000000511', N'Plan de Emergencia ante Contaminación de Aguas Marinas de Las Illes Balears', 13, null, null, 6, 12),
        (29, N'04000000218', N'Plan de Emergencia de Actuación en Situaciones de Alerta y Eventual Sequía de Illes Balears', 13, null, null, 3, 19),
        (30, N'05000000100', N'Plan Territorial de Protección Civil de Canarias', 17, null, null, 2, 1),
        (31, N'05000000214', N'Plan de Emergencia por Incendios Forestales en Canarias', 17, null, null, 3, 15),
        (32, N'05000000215', N'Plan de Emergencia por Riesgo de Inundaciones en Canarias', 17, null, null, 3, 16),
        (33, N'05000000206', N'Plan de Emergencia por Accidentes en el Transporte de Mercancías Peligrosas por Carretera de Canarias', 17, null, null, 3, 7),
        (34, N'05000000203', N'Plan de Emergencia por Riesgo Radiológico de Canarias', 17, null, null, 3, 4),
        (35, N'05000000219', N'Plan de Emergencia por Riesgo Sísmico en Canarias', 17, null, null, 3, 20),
        (36, N'05000000212', N'Plan de Emergencia por Riesgo Volcánico de Canarias', 17, null, null, 3, 13),
        (37, N'05000000202', N'Plan de Emergencia ante Riesgo Químico en Canarias', 17, null, null, 3, 3),
        (38, N'05000000513', N'Plan de Emergencia de Canarias ante Riesgos de Fenómenos Meteorológicos Adversos ', 17, null, null, 6, 14),
        (39, N'05000000202b', N'Plan de emergencia por Accidentes de Sustancias Explosivas de Canarias', 17, null, null, 3, 3),
        (40, N'06000000100', N'Plan Territorial de Protección Civil de Cantabria', 3, null, null, 2, 1),
        (41, N'06000000214', N'Plan de Emergencia Sobre Riesgo de Incendios Forestales de Cantabria', 3, null, null, 3, 15),
        (42, N'06000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones de Cantabria', 3, null, null, 3, 16),
        (43, N'06000000206', N'Plan de Emergencia Sobre Transporte de Mercancías Peligrosas por Carretera y Ferrocarril de Cantabria', 3, null, null, 3, 7),
        (44, N'07000000100', N'Plan Territorial de Protección Civil de Castilla y León', 8, null, null, 2, 1),
        (45, N'07000000214', N'Plan de Emergencia por Riesgo de Incendios Forestales en Castilla y León', 8, null, null, 3, 15),
        (46, N'07000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones en Castilla y León', 8, null, null, 3, 16),
        (47, N'07000000206', N'Plan de Emergencia ante el Riesgo de Transporte de Mercancías Peligrosas de Castilla y León', 8, null, null, 3, 7),
        (48, N'08000000100', N'Plan Territorial de Protección Civil de Castilla-La Mancha', 11, null, null, 2, 1),
        (49, N'08000000214', N'Plan de Emergencia por Incendios Forestales de Castilla-La Mancha', 11, null, null, 3, 15),
        (50, N'08000000215', N'Plan de Emergencia ante Inundaciones de Castilla-La Mancha', 11, null, null, 3, 16),
        (51, N'08000000206', N'Plan de Emergencia ante el Riesgo de Accidente en el Transporte de Mercancías Peligrosas por Carretera y Ferrocarril en Castilla-La Mancha', 11, null, null, 3, 7),
        (52, N'08000000203', N'Plan de Emergencia ante el Riesgo Radiológico en Castilla-la Mancha', 11, null, null, 3, 4),
        (53, N'08000000219', N'Plan de Emergencia por Riesgo Sísmico en Castilla-La Mancha', 11, null, null, 3, 20),
        (54, N'08000000513', N'Plan de Emergencia ante el Riesgo por Fenómenos Meteorológicos Adversos en Castilla La Mancha', 11, null, null, 6, 14),
        (55, N'08000000507', N'Plan de Respuesta ante Accidentes de Tráfico con Múltiples Víctimas en Castilla-La Mancha', 11, null, null, 6, 8),
        (56, N'09000000100', N'Plan Territorial de Protección Civil de Catalunya', 7, null, null, 2, 1),
        (57, N'09000000214', N'Plan de Emergencia ante Incendios Forestales de Catalunya', 7, null, null, 3, 15),
        (58, N'09000000215', N'Plan de Emergencia por Inundaciones en Catalunya', 7, null, null, 3, 16),
        (59, N'09000000206', N'Plan de emergencia por accidentes en el transporte de mercancías peligrosas por carretera y ferrocarril en Catalunya', 7, null, null, 3, 7),
        (60, N'09000000203', N'Plan de Emergencia ante Riesgos Radiológicos en Catalunya', 7, null, null, 3, 4),
        (61, N'09000000219', N'Plan de Emergencia por Riesgo Sísmico en Catalunya', 7, null, null, 3, 20),
        (62, N'09000000202', N'Plan de Emergencia Exterior del Sector Químico de Catalunya', 7, null, null, 3, 3),
        (63, N'09430000202', N'Plan de Emergencia Exterior del Sector Químico de Tarragona', 7, 43, null, 3, 3),
        (64, N'09000000511', N'Plan de Emergencia por Contaminación de las Aguas Marinas de Catalunya', 7, null, null, 6, 12),
        (65, N'09000000204', N'Plan de Emergencia Aeronáuticas de Catalunya', 7, null, null, 3, 5),
        (66, N'09000000509', N'Plan de Emergencia por Aludes en Catalunya', 7, null, null, 6, 10),
        (67, N'09000000217', N'Plan de Emergencia ante Nevadas en Catalunya', 7, null, null, 3, 18),
        (68, N'09000000220', N'Plan de Emergencia por Riesgo de Viento de Catalunya', 7, null, null, 3, 21),
        (69, N'09000000518', N'Plan de Emergencia por Pandemias en Catalunya', 7, null, null, 6, 19),
        (70, N'10000000100', N'Plan Territorial de Protección Civil de la Comunitat Valenciana', 12, null, null, 2, 1),
        (71, N'10000000214', N'Plan de Emergencia frente al riesgo de Incendios Forestales de la Comunitat Valenciana', 12, null, null, 3, 15),
        (72, N'10000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones en La Comunitat Valenciana', 12, null, null, 3, 16),
        (73, N'10000000206', N'Plan de Emergencia ante el Riesgo de Accidentes en el Transporte de Mercancías Peligrosas por Carretera y Ferrocarril en Comunitat Valenciana', 12, null, null, 3, 7),
        (74, N'10000000203', N'Plan de Emergencia ante el Riesgo Radiológico de la Comunitat Valenciana', 12, null, null, 3, 4),
        (75, N'10000000219', N'Plan de Emergencia Frente al Riesgo Sísmico en La Comunitat Valenciana', 12, null, null, 3, 20),
        (76, N'10000000511', N'Plan de Emergencia ante la Contaminación Marina Accidental en la Comunitat Valenciana', 12, null, null, 6, 12),
        (77, N'10000000517', N'Plan de Emergencia ante el Riesgo de Nevadas en la Comunitat Valenciana', 12, null, null, 6, 18),
        (78, N'11000000100', N'Plan Territorial de Protección Civil de Extremadura', 14, null, null, 2, 1),
        (79, N'11000000214', N'Plan de Emergencia ante Incendios Forestales en Extremadura', 14, null, null, 3, 15),
        (80, N'11000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones de Extremadura', 14, null, null, 3, 16),
        (81, N'11000000206', N'Plan de Emergencia Sobre Transporte de Mercancías Peligrosas por Carretera y Ferrocarril de Extremadura', 14, null, null, 3, 7),
        (82, N'11000000203', N'Plan de emergencia ante Riesgos radiológicos de Extremadura', 14, null, null, 3, 4),
        (83, N'11000000219', N'Plan de Emergencia ante el Riesgo Sísmico de Extremadura', 14, null, null, 3, 20),
        (84, N'12000000100', N'Plan Territorial de Protección Civil de Galicia', 1, null, null, 2, 1),
        (85, N'12000000214', N'Plan de Emergencia de Galicia ante el Riesgo de Incendios Forestales', 1, null, null, 3, 15),
        (86, N'12000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones de Galicia', 1, null, null, 3, 16),
        (87, N'12000000206', N'Plan de Emergencia por Accidentes en el Transporte de Mercancías Peligrosas de Galicia', 1, null, null, 3, 7),
        (88, N'12000000219', N'Plan de Emergencia ante el Riesgo Sísmico de Galicia', 1, null, null, 3, 20),
        (89, N'12000000511', N'Plan de Emergencia ante Contingencias por Contaminación Marina Accidental de Galicia', 1, null, null, 6, 12),
        (90, N'13000000100', N'Plan Territorial de Protección Civil de la Comunidad de Madrid', 10, null, null, 2, 1),
        (91, N'13000000214', N'Plan de Emergencia por Incendios Forestales en la Comunidad de Madrid', 10, null, null, 3, 15),
        (92, N'13000000215', N'Plan de Emergencia ante Inundaciones en la Comunidad de Madrid', 10, null, null, 3, 16),
        (93, N'13000000206', N'Plan de Emergencia ante el riesgo de accidentes en el transporte de mercancías peligrosas por carretera y ferrocarril de la Comunidad de Madrid', 10, null, null, 3, 7),
        (94, N'13000000203', N'Plan de Emergencia ante Riesgos Radiológicos en la Comunidad de Madrid', 10, null, null, 3, 4),
        (95, N'13000000213', N'Plan de Emergencia ante Inclemencias Invernales en la Comunidad de Madrid', 10, null, null, 3, 14),
        (96, N'14000000100', N'Plan Territorial de Protección Civil de la Región de Murcia', 16, null, null, 2, 1),
        (97, N'14000000214', N'Plan de Emergencia por Riesgo de Incendios Forestales de Murcia', 16, null, null, 3, 15),
        (98, N'14000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones de Murcia', 16, null, null, 3, 16),
        (99, N'14000000206', N'Plan de Emergencia Sobre Transporte de Mercancías Peligrosas por Carretera y Ferrocarril de Murcia', 16, null, null, 3, 7),
        (100, N'14000000219', N'Plan de Emergencia ante el Riesgo Sísmico de Murcia', 16, null, null, 3, 20),
        (101, N'14000000513', N'Plan de Emergencia ante Fenómenos Meteorológicos Adversos de Murcia', 16, null, null, 6, 14),
        (102, N'14000000511', N'Plan de Emergencia ante Contingencias por Contaminación Marina Accidental de Murcia', 16, null, null, 6, 12),
        (103, N'15000000100', N'Plan Territorial de Protección Civil de Navarra', 5, null, null, 2, 1),
        (104, N'15000000214', N'Plan de Emergencia por Riesgo de Incendios Forestales de Navarra.', 5, null, null, 3, 15),
        (105, N'15000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones de Navarra', 5, null, null, 3, 16),
        (106, N'15000000206', N'Plan de Emergencia por Accidentes en el Transporte de Mercancías Peligrosas por Carreteras y Ferrocarriles de Navarra', 5, null, null, 3, 7),
        (107, N'15000000203', N'Plan de Emergencia ante el Riesgo Radiológico de Navarra', 5, null, null, 3, 4),
        (108, N'15000000219', N'Plan de Emergencia ante el Riesgo Sísmico de Navarra', 5, null, null, 3, 20),
        (109, N'15000000213', N'Plan de Emergencia ante Fenómenos Meteorológicos Adversos en Navarra', 5, null, null, 3, 14),
        (110, N'15000000517', N'Plan de Emergencia por Nevadas en Navarra', 5, null, null, 6, 18),
        (111, N'15000000208', N'Plan de Emergencia para Túneles de La Red de Carreteras de Navarra', 5, null, null, 3, 9),
        (112, N'16000000100', N'Plan Territorial de Protección Civil de Euskadi', 4, null, null, 2, 1),
        (113, N'16000000214', N'Plan de Emergencia para Riesgo de Incendios Forestales de País Vasco', 4, null, null, 3, 15),
        (114, N'16000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones del País Vasco', 4, null, null, 3, 16),
        (115, N'16000000206', N'Plan de Emergencia ante el Riesgo de Accidentes en el Transporte de Mercancías Peligrosas por Carretera y Ferrocarril de País Vasco', 4, null, null, 3, 7),
        (116, N'16000000203', N'Plan de Emergencia ante el Riesgo Radiológico de País Vasco', 4, null, null, 3, 4),
        (117, N'16000000219', N'Plan de Emergencia ante el Riesgo Sísmico de País Vasco', 4, null, null, 3, 20),
        (118, N'16000000511', N'Plan de Emergencia ante la contaminación de la Ribera del Mar de País Vasco', 4, null, null, 6, 12),
        (119, N'16000000204', N'Plan de Emergencia Aeronáuticas de País Vasco', 4, null, null, 3, 5),
        (120, N'17000000100', N'Plan Territorial de Protección Civil de La Rioja', 9, null, null, 2, 1),
        (121, N'17000000214', N'Plan de Emergencia por Incendios Forestales en La Rioja', 9, null, null, 3, 15),
        (122, N'17000000215', N'Plan de Emergencia ante Inundaciones de La Rioja', 9, null, null, 3, 16),
        (123, N'17000000206', N'Plan de Emergencia Sobre Transporte de Mercancías Peligrosas por Carretera y Ferrocarril de La Rioja', 9, null, null, 3, 7),
        (124, N'17000000203', N'Plan de Emergencia ante Riesgos Radiológicos de La Rioja', 9, null, null, 3, 4),
        (125, N'18000000100', N'Plan Territorial de Protección Civil de Ceuta', 18, null, null, 2, 1),
        (126, N'18000000214', N'Plan de Emergencia de Incendios Forestales de Ceuta', 18, null, null, 3, 15),
        (127, N'18000000215', N'Plan de Emergencia ante Inundaciones de Ceuta', 18, null, null, 3, 16),
        (128, N'18000000219', N'Plan de Emergencia ante el Riesgo de Seísmos y Maremotos en Ceuta', 18, null, null, 3, 20),
        (129, N'19000000100', N'Plan Territorial de Protección Civil de la Ciudad Autónoma de Melilla', 19, null, null, 2, 1);