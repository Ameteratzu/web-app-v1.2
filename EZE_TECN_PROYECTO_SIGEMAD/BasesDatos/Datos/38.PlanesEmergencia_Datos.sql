-- =============================================
-- TIPO DE RIESGO
-- =============================================

SET IDENTITY_INSERT TipoRiesgo ON;

INSERT INTO TipoRiesgo (Id,Descripcion, IdTipoSuceso, Codigo) VALUES
	 (1, N'General', NULL, '00'),
	 (2, N'Accidentes con sustancias biológicas', 8, '01'),
	 (3, N'Accidentes con sustancias químicas', 10, '02'),
	 (4, N'Accidentes con sustancias radiactivas', 16, '03'),
	 (5, N'Accidentes de aviación civil', 7, '04'),
	 (6, N'Accidentes en centrales nucleares', 15, '05'),
	 (7, N'Accidentes en el transporte de mercancías peligrosas', 12, '06'),
	 (8, N'Accidentes en el transporte de viajeros por carretera y ferrocarril', 19, '07'),
	 (9, N'Accidentes en túneles', 20, '08'),
	 (10, N'Aludes', 21, '09'),
	 (11, N'Bélico', 22, '10'),
	 (12, N'Contaminación marina', 23, '11'),
	 (13, N'Erupción volcánica', 18, '12'),
	 (14, N'Fenómenos Meteorológicos Adversos', 14, '13'),
	 (15, N'Incendios forestales', 6, '14'),
	 (16, N'Inundaciones', 9, '15'),
	 (17, N'Maremotos', 17, '16'),
	 (18, N'Nevadas', 24, '17'),
	 (19, N'Otros', 13, '18'),
	 (20, N'Terremotos', 11, '19'),
	 (21, N'Viento', 25, '20');

SET IDENTITY_INSERT TipoRiesgo OFF;

-- =============================================
-- TIPO DE PLAN
-- =============================================

SET IDENTITY_INSERT dbo.TipoPlan ON;

INSERT INTO dbo.TipoPlan (Id,Descripcion,Codigo) VALUES
	(1	,'Estatal', '00'),
	(2	,'Territorial', '01'),
	(3	,'Especial', '02'),
    (4	,'Normativa Básica', '03'),
    (5	,'Autoprotección', '04'),
    (6	,'Otros', '05');

SET IDENTITY_INSERT dbo.TipoPlan OFF;


INSERT INTO TipoPlanMapeo (IdAntiguo, IdNuevo)
VALUES 
	(2, 1), --Estatal
	(5, 2),  --Territorial
	(6, 3), --Especial de CA
	(7, 5), --Autoprotección
	(8, 6); --Otros


-- =============================================
-- AMBITO PLAN
-- =============================================

SET IDENTITY_INSERT dbo.AmbitoPlan ON;

INSERT INTO dbo.AmbitoPlan (Id,Descripcion) VALUES
    (1, 'Estatal'),
    (2, 'Autonómico');

SET IDENTITY_INSERT dbo.AmbitoPlan OFF;


-- =============================================
-- PLAN EMERGENCIA
-- =============================================

INSERT [dbo].[PlanEmergencia] ([Id], [Codigo], [Descripcion], [IdCCAA], [IdProvincia], [IdMunicipio], [IdTipoPlan], [IdTipoRiesgo], [IdAmbitoPlan]) VALUES 
(1, N'00000000000', N'Plan Estatal General de Emergencias de Protección Civil', NULL, NULL, NULL, 1, 1, 1),
(2, N'00000000015', N'Plan Estatal de Protección Civil ante el Riesgo de Inundaciones', NULL, NULL, NULL, 1, 16, 1),
(3, N'00000000014', N'Plan Estatal de Protección Civil para Emergencias por Incendios Forestales', NULL, NULL, NULL, 1, 15, 1),
(4, N'00000000019', N'Plan Estatal de Protección Civil ante el Riesgo Sísmico', NULL, NULL, NULL, 1, 20, 1),
(5, N'00000000002', N'Plan Estatal de Protección Civil ante el Riesgo Químico', NULL, NULL, NULL, 1, 3, 1),
(6, N'00000000012', N'Plan Estatal de Protección Civil ante el Riesgo Volcánico', NULL, NULL, NULL, 1, 13, 1),
(7, N'00000000003', N'Plan Estatal de Protección Civil ante el Riesgo Radiológico', NULL, NULL, NULL, 1, 4, 1),
(8, N'00000000016', N'Plan Estatal de Protección Civil ante el Riesgo de Maremotos', NULL, NULL, NULL, 1, 17, 1),
(9, N'00000000005', N'Plan de Emergencia Nuclear del Nivel Central de Respuesta y Apoyo', NULL, NULL, NULL, 1, 6, 1),
(10, N'07090000005', N'Plan de Emergencia Nuclear Exterior a la Central Nuclear de Santa María de Garoña en Burgos', 8, NULL, NULL, 1, 6, 1),
(11, N'11100000005', N'Plan de Emergencia Nuclear Exterior a la Central Nuclear de Almaraz en Cáceres', 14, 10, NULL, 1, 6, 1),
(12, N'08190000005', N'Plan de Emergencia Nuclear Exterior a las Centrales Nucleares de José Cabrera y Trillo en Guadalajara', 11, 19, NULL, 1, 6, 1),
(13, N'09430000005', N'Plan de Emergencia Nuclear Exterior a las Centrales Nucleares de Ascó y Vandellós en Tarragona', 7, 43, NULL, 1, 6, 1),
(14, N'10460000005', N'Plan de Emergencia Nuclear Exterior a la Central Nuclear de Cofrentes en Valencia', 12, 46, NULL, 1, 6, 1),
(15, N'00000000518', N'Plan Especial Operación Paso del Estrecho 2024', NULL, NULL, NULL, 6, 19, 1),
(16, N'02220000508', N'Plan de Socorro Binacional del Túnel de Somport', 6, 22, NULL, 6, 9, 1),
(17, N'09170000508', N'Plan de Socorro Binacional del Túnel del Perthus', 7, 17, NULL, 6, 9, 1),
(18, N'00000000513', N'Protocolo de Coordinación de la AGE para la Red de Carreteras del Estado (2023-2024)', NULL, NULL, NULL, 6, 14, 1),
(19, N'01000000100', N'Plan Territorial de Emergencias de Protección Civil de Andalucía', 15, NULL, NULL, 2, 1, 2),
(20, N'01000000214', N'Plan de Emergencia por Riesgo de Incendios Forestales de Andalucía', 15, NULL, NULL, 3, 15, 2),
(21, N'01000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones en Andalucía', 15, NULL, NULL, 3, 16, 2),
(22, N'01000000206', N'Plan de Emergencia ante Accidentes en el Transporte de Mercancías Peligrosas por Carretera y Ferrocarril en Andalucía', 15, NULL, NULL, 3, 7, 2),
(23, N'01000000219', N'Plan de Emergencia ante el Riesgo Sísmico en Andalucía', 15, NULL, NULL, 3, 20, 2),
(24, N'01000000511', N'Plan de Emergencia ante el Riesgo de Contaminación del Litoral en Andalucía', NULL, NULL, NULL, 6, 12, 2),
(25, N'01000000516', N'Plan de Emergencia ante el Riesgo de Maremotos en Andalucía', 15, NULL, NULL, 6, 17, 2),
(26, N'02000000100', N'Plan Territorial de Protección Civil de Aragón', 6, NULL, NULL, 2, 1, 2),
(27, N'02000000214', N'Plan de Emergencia por Incendios Forestales de Aragón', 6, NULL, NULL, 3, 15, 2),
(28, N'02000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones en Aragón', 6, NULL, NULL, 3, 16, 2),
(29, N'02000000206', N'Plan de Emergencia Producidas en el Transporte de Mercancías Peligrosas por Carretera y Ferrocarril en Andalucía', 6, NULL, NULL, 3, 7, 2),
(30, N'02000000203', N'Plan de Emergencia ante el Riesgo Radiológico en Aragón', 6, NULL, NULL, 3, 4, 2),
(31, N'02000000219', N'Plan de Emergencia ante el Riesgo Sísmico de Aragón', 6, NULL, NULL, 3, 20, 2),
(32, N'02000000202', N'Plan de Emergencia ante Accidentes en Gasoductos y Oleoductos de Aragón', 6, NULL, NULL, 3, 3, 2),
(33, N'03000000100', N'Plan Territorial de Protección Civil del Principado de Asturias', 2, NULL, NULL, 2, 1, 2),
(34, N'03000000214', N'Plan de Emergencia por Incendios Forestales del Principado de Asturias', 2, NULL, NULL, 3, 15, 2),
(35, N'03000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones del Principado de Asturias', 2, NULL, NULL, 3, 16, 2),
(36, N'03000000206', N'Plan de Emergencia ante el Transporte de Mercancías Peligrosas por Carretera y Ferrocarril del Principado de Asturias', 2, NULL, NULL, 3, 7, 2),
(37, N'03000000511', N'Plan de Emergencia frente a Contingencias por Contaminación Marina Accidental en Asturias', 2, NULL, NULL, 6, 12, 2),
(38, N'03000000517', N'Plan de Emergencia ante Nevadas en Asturias', 2, NULL, NULL, 6, 18, 2),
(39, N'04000000100', N'Plan Territorial de Protección Civil de Illes Balears', 13, NULL, NULL, 2, 1, 2);


INSERT [dbo].[PlanEmergencia] ([Id], [Codigo], [Descripcion], [IdCCAA], [IdProvincia], [IdMunicipio], [IdTipoPlan], [IdTipoRiesgo], [IdAmbitoPlan]) VALUES 
(40, N'04000000214', N'Plan de Emergencia ante el Riesgo de Incendios Forestales en Illes Balears', 13, NULL, NULL, 3, 15, 2),
(41, N'04000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones en Illes Balears', 13, NULL, NULL, 3, 16, 2),
(42, N'04000000206', N'Plan de Emergencia ante el Riesgo de Transporte de Mercancías Peligrosas en Illes Balears', 13, NULL, NULL, 3, 7, 2),
(43, N'04000000203', N'Plan de Emergencia ante el Riesgo de Emergencias Radiológicas en Illes Balears', 13, NULL, NULL, 3, 4, 2),
(44, N'04000000219', N'Plan de Emergencia ante Riesgo Sísmico en Illes Balears', 13, NULL, NULL, 3, 20, 2),
(45, N'04000000213', N'Plan de Emergencia frente a Riesgo de Fenómenos Meteorológicos Adversos en Illes Balears', 13, NULL, NULL, 3, 14, 2),
(46, N'04000000511', N'Plan de Emergencia ante Contaminación de Aguas Marinas de Las Illes Balears', 13, NULL, NULL, 6, 12, 2),
(47, N'04000000218', N'Plan de Emergencia de Actuación en Situaciones de Alerta y Eventual Sequía de Illes Balears', 13, NULL, NULL, 3, 19, 2),
(48, N'05000000100', N'Plan Territorial de Protección Civil de Canarias', 17, NULL, NULL, 2, 1, 2),
(49, N'05000000214', N'Plan de Emergencia por Incendios Forestales en Canarias', 17, NULL, NULL, 3, 15, 2),
(50, N'05000000215', N'Plan de Emergencia por Riesgo de Inundaciones en Canarias', 17, NULL, NULL, 3, 16, 2),
(51, N'05000000206', N'Plan de Emergencia por Accidentes en el Transporte de Mercancías Peligrosas por Carretera de Canarias', 17, NULL, NULL, 3, 7, 2),
(52, N'05000000203', N'Plan de Emergencia por Riesgo Radiológico de Canarias', 17, NULL, NULL, 3, 4, 2),
(53, N'05000000219', N'Plan de Emergencia por Riesgo Sísmico en Canarias', 17, NULL, NULL, 3, 20, 2),
(54, N'05000000212', N'Plan de Emergencia por Riesgo Volcánico de Canarias', 17, NULL, NULL, 3, 13, 2),
(55, N'05000000202/1', N'Plan de Emergencia ante Riesgo Químico en Canarias', 17, NULL, NULL, 3, 3, 2),
(56, N'05000000513', N'Plan de Emergencia de Canarias ante Riesgos de Fenómenos Meteorológicos Adversos ', 17, NULL, NULL, 6, 14, 2),
(57, N'05000000202/2', N'Plan de emergencia por Accidentes de Sustancias Explosivas de Canarias', 17, NULL, NULL, 3, 3, 2),
(58, N'06000000100', N'Plan Territorial de Protección Civil de Cantabria', 3, NULL, NULL, 2, 1, 2),
(59, N'06000000214', N'Plan de Emergencia Sobre Riesgo de Incendios Forestales de Cantabria', 3, NULL, NULL, 3, 15, 2),
(60, N'06000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones de Cantabria', 3, NULL, NULL, 3, 16, 2),
(61, N'06000000206', N'Plan de Emergencia sobre Transporte de Mercancías Peligrosas por Carretera y Ferrocarril de Cantabria', 3, NULL, NULL, 3, 7, 2),
(62, N'07000000100', N'Plan Territorial de Protección Civil de Castilla y León', 8, NULL, NULL, 2, 1, 2),
(63, N'07000000214', N'Plan de Emergencia por Riesgo de Incendios Forestales en Castilla y León', 8, NULL, NULL, 3, 15, 2),
(64, N'07000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones en Castilla y León', 8, NULL, NULL, 3, 16, 2),
(65, N'07000000206', N'Plan de Emergencia ante el Riesgo de Transporte de Mercancías Peligrosas de Castilla y León', 8, NULL, NULL, 3, 7, 2),
(66, N'08000000100', N'Plan Territorial de Protección Civil de Castilla-La Mancha', 11, NULL, NULL, 2, 1, 2),
(67, N'08000000214', N'Plan de Emergencia por Incendios Forestales de Castilla-La Mancha', 11, NULL, NULL, 3, 15, 2),
(68, N'08000000215', N'Plan de Emergencia ante Inundaciones de Castilla-La Mancha', 11, NULL, NULL, 3, 16, 2),
(69, N'08000000206', N'Plan de Emergencia ante el Riesgo de Accidente en el Transporte de Mercancías Peligrosas por Carretera y Ferrocarril en Castilla-La Mancha', 11, NULL, NULL, 3, 7, 2),
(70, N'08000000203', N'Plan de Emergencia ante el Riesgo Radiológico en Castilla-la Mancha', 11, NULL, NULL, 3, 4, 2),
(71, N'08000000219', N'Plan de Emergencia por Riesgo Sísmico en Castilla-La Mancha', 11, NULL, NULL, 3, 20, 2),
(72, N'08000000513', N'Plan de Emergencia ante el Riesgo por Fenómenos Meteorológicos Adversos en Castilla La Mancha', 11, NULL, NULL, 6, 14, 2),
(73, N'08000000507', N'Plan de Respuesta ante Accidentes de Tráfico con Múltiples Víctimas en Castilla-La Mancha', 11, NULL, NULL, 6, 8, 2),
(74, N'09000000100', N'Plan Territorial de Protección Civil de Catalunya', 7, NULL, NULL, 2, 1, 2),
(75, N'09000000214', N'Plan de Emergencia ante Incendios Forestales de Catalunya', 7, NULL, NULL, 3, 15, 2),
(76, N'09000000215', N'Plan de Emergencia por Inundaciones en Catalunya', 7, NULL, NULL, 3, 16, 2),
(77, N'09000000206', N'Plan de emergencia por accidentes en el transporte de mercancías peligrosas por carretera y ferrocarril en Catalunya', 7, NULL, NULL, 3, 7, 2),
(78, N'09000000203', N'Plan de Emergencia ante Riesgos Radiológicos en Catalunya', 7, NULL, NULL, 3, 4, 2),
(79, N'09000000219', N'Plan de Emergencia por Riesgo Sísmico en Catalunya', 7, NULL, NULL, 3, 20, 2),
(80, N'09000000202', N'Plan de Emergencia Exterior del Sector Químico de Catalunya', 7, NULL, NULL, 3, 3, 2),
(81, N'09430000202', N'Plan de Emergencia Exterior del Sector Químico de Tarragona', 7, 43, NULL, 3, 3, 2),
(82, N'09000000511', N'Plan de Emergencia por Contaminación de las Aguas Marinas de Catalunya', 7, NULL, NULL, 6, 12, 2),
(83, N'09000000204', N'Plan de Emergencia Aeronáuticas de Catalunya', 7, NULL, NULL, 3, 5, 2),
(84, N'09000000509', N'Plan de Emergencia por Aludes en Catalunya', 7, NULL, NULL, 6, 10, 2),
(85, N'09000000217', N'Plan de Emergencia ante Nevadas en Catalunya', 7, NULL, NULL, 3, 18, 2),
(86, N'09000000220', N'Plan de Emergencia por Riesgo de Viento de Catalunya', 7, NULL, NULL, 3, 21, 2),
(87, N'09000000518', N'Plan de Emergencia por Pandemias en Catalunya', 7, NULL, NULL, 6, 19, 2),
(88, N'10000000100', N'Plan Territorial de Protección Civil de la Comunitat Valenciana', 12, NULL, NULL, 2, 1, 2),
(89, N'10000000214', N'Plan de Emergencia frente al riesgo de Incendios Forestales de la Comunitat Valenciana', 12, NULL, NULL, 3, 15, 2),
(90, N'10000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones en La Comunitat Valenciana', 12, NULL, NULL, 3, 16, 2),
(91, N'10000000206', N'Plan de Emergencia ante el Riesgo de Accidentes en el Transporte de Mercancías Peligrosas por Carretera y Ferrocarril en Comunitat Valenciana', 12, NULL, NULL, 3, 7, 2),
(92, N'10000000203', N'Plan de Emergencia ante el Riesgo Radiológico de la Comunitat Valenciana', 12, NULL, NULL, 3, 4, 2),
(93, N'10000000219', N'Plan de Emergencia Frente al Riesgo Sísmico en La Comunitat Valenciana', 12, NULL, NULL, 3, 20, 2),
(94, N'10000000511', N'Plan de Emergencia ante la Contaminación Marina Accidental en la Comunitat Valenciana', 12, NULL, NULL, 6, 12, 2),
(95, N'10000000517', N'Plan de Emergencia ante el Riesgo de Nevadas en la Comunitat Valenciana', 12, NULL, NULL, 6, 18, 2),
(96, N'11000000100', N'Plan Territorial de Protección Civil de Extremadura', 14, NULL, NULL, 2, 1, 2),
(97, N'11000000214', N'Plan de Emergencia ante Incendios Forestales en Extremadura', 14, NULL, NULL, 3, 15, 2),
(98, N'11000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones de Extremadura', 14, NULL, NULL, 3, 16, 2),
(99, N'11000000206', N'Plan de Emergencia sobre Transporte de Mercancías Peligrosas por Carretera y Ferrocarril de Extremadura', 14, NULL, NULL, 3, 7, 2),
(100, N'11000000203', N'Plan de emergencia ante Riesgos radiológicos de Extremadura', 14, NULL, NULL, 3, 4, 2),
(101, N'11000000219', N'Plan de Emergencia ante el Riesgo Sísmico de Extremadura', 14, NULL, NULL, 3, 20, 2),
(102, N'12000000100', N'Plan Territorial de Protección Civil de Galicia', 1, NULL, NULL, 2, 1, 2),
(103, N'12000000214', N'Plan de Emergencia de Galicia ante el Riesgo de Incendios Forestales', 1, NULL, NULL, 3, 15, 2),
(104, N'12000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones de Galicia', 1, NULL, NULL, 3, 16, 2),
(105, N'12000000206', N'Plan de Emergencia por Accidentes en el Transporte de Mercancías Peligrosas de Galicia', 1, NULL, NULL, 3, 7, 2),
(106, N'12000000219', N'Plan de Emergencia ante el Riesgo Sísmico de Galicia', 1, NULL, NULL, 3, 20, 2),
(107, N'12000000511', N'Plan de Emergencia ante Contingencias por Contaminación Marina Accidental de Galicia', 1, NULL, NULL, 6, 12, 2),
(108, N'13000000100', N'Plan Territorial de Protección Civil de la Comunidad de Madrid', 10, NULL, NULL, 2, 1, 2),
(109, N'13000000214', N'Plan de Emergencia por Incendios Forestales en la Comunidad de Madrid', 10, NULL, NULL, 3, 15, 2),
(110, N'13000000215', N'Plan de Emergencia ante Inundaciones en la Comunidad de Madrid', 10, NULL, NULL, 3, 16, 2),
(111, N'13000000206', N'Plan de Emergencia ante el riesgo de accidentes en el transporte de mercancías peligrosas por carretera y ferrocarril de la Comunidad de Madrid', 10, NULL, NULL, 3, 7, 2),
(112, N'13000000203', N'Plan de Emergencia ante Riesgos Radiológicos en la Comunidad de Madrid', 10, NULL, NULL, 3, 4, 2),
(113, N'13000000213', N'Plan de Emergencia ante Inclemencias Invernales en la Comunidad de Madrid', 10, NULL, NULL, 3, 14, 2),
(114, N'14000000100', N'Plan Territorial de Protección Civil de la Región de Murcia', 16, NULL, NULL, 2, 1, 2),
(115, N'14000000214', N'Plan de Emergencia por Riesgo de Incendios Forestales de Murcia', 16, NULL, NULL, 3, 15, 2),
(116, N'14000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones de Murcia', 16, NULL, NULL, 3, 16, 2),
(117, N'14000000206', N'Plan de Emergencia sobre Transporte de Mercancías Peligrosas por Carretera y Ferrocarril de Murcia', 16, NULL, NULL, 3, 7, 2),
(118, N'14000000219', N'Plan de Emergencia ante el Riesgo Sísmico de Murcia', 16, NULL, NULL, 3, 20, 2),
(119, N'14000000513', N'Plan de Emergencia ante Fenómenos Meteorológicos Adversos de Murcia', 16, NULL, NULL, 6, 14, 2),
(120, N'14000000511', N'Plan de Emergencia ante Contingencias por Contaminación Marina Accidental de Murcia', 16, NULL, NULL, 6, 12, 2),
(121, N'15000000100', N'Plan Territorial de Protección Civil de Navarra', 5, NULL, NULL, 2, 1, 2),
(122, N'15000000214', N'Plan de Emergencia por Riesgo de Incendios Forestales de Navarra.', 5, NULL, NULL, 3, 15, 2),
(123, N'15000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones de Navarra', 5, NULL, NULL, 3, 16, 2),
(124, N'15000000206', N'Plan de Emergencia por Accidentes en el Transporte de Mercancías Peligrosas por Carreteras y Ferrocarriles de Navarra', 5, NULL, NULL, 3, 7, 2),
(125, N'15000000203', N'Plan de Emergencia ante el Riesgo Radiológico de Navarra', 5, NULL, NULL, 3, 4, 2),
(126, N'15000000219', N'Plan de Emergencia ante el Riesgo Sísmico de Navarra', 5, NULL, NULL, 3, 20, 2),
(127, N'15000000213', N'Plan de Emergencia ante Fenómenos Meteorológicos Adversos en Navarra', 5, NULL, NULL, 3, 14, 2),
(128, N'15000000517', N'Plan de Emergencia por Nevadas en Navarra', 5, NULL, NULL, 6, 18, 2),
(129, N'15000000208', N'Plan de Emergencia para Túneles de La Red de Carreteras de Navarra', 5, NULL, NULL, 3, 9, 2),
(130, N'16000000100', N'Plan Territorial de Protección Civil de Euskadi', 4, NULL, NULL, 2, 1, 2),
(131, N'16000000214', N'Plan de Emergencia para Riesgo de Incendios Forestales de País Vasco', 4, NULL, NULL, 3, 15, 2),
(132, N'16000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones del País Vasco', 4, NULL, NULL, 3, 16, 2),
(133, N'16000000206', N'Plan de Emergencia ante el Riesgo de Accidentes en el Transporte de Mercancías Peligrosas por Carretera y Ferrocarril de País Vasco', 4, NULL, NULL, 3, 7, 2),
(134, N'16000000203', N'Plan de Emergencia ante el Riesgo Radiológico de País Vasco', 4, NULL, NULL, 3, 4, 2),
(135, N'16000000219', N'Plan de Emergencia ante el Riesgo Sísmico de País Vasco', 4, NULL, NULL, 3, 20, 2),
(136, N'16000000511', N'Plan de Emergencia ante la contaminación de la Ribera del Mar de País Vasco', 4, NULL, NULL, 6, 12, 2),
(137, N'16000000204', N'Plan de Emergencia Aeronáuticas de País Vasco', 4, NULL, NULL, 3, 5, 2),
(138, N'17000000100', N'Plan Territorial de Protección Civil de La Rioja', 9, NULL, NULL, 2, 1, 2),
(139, N'17000000214', N'Plan de Emergencia por Incendios Forestales en La Rioja', 9, NULL, NULL, 3, 15, 2),
(140, N'17000000215', N'Plan de Emergencia ante Inundaciones de La Rioja', 9, NULL, NULL, 3, 16, 2),
(141, N'17000000206', N'Plan de Emergencia sobre Transporte de Mercancías Peligrosas por Carretera y Ferrocarril de La Rioja', 9, NULL, NULL, 3, 7, 2),
(142, N'17000000203', N'Plan de Emergencia ante Riesgos Radiológicos de La Rioja', 9, NULL, NULL, 3, 4, 2),
(143, N'18000000100', N'Plan Territorial de Protección Civil de Ceuta', 18, NULL, NULL, 2, 1, 2),
(144, N'18000000214', N'Plan de Emergencia de Incendios Forestales de Ceuta', 18, NULL, NULL, 3, 15, 2),
(145, N'18000000215', N'Plan de Emergencia ante Inundaciones de Ceuta', 18, NULL, NULL, 3, 16, 2),
(146, N'18000000219', N'Plan de Emergencia ante el Riesgo de Seísmos y Maremotos en Ceuta', 18, NULL, NULL, 3, 20, 2),
(147, N'19000000100', N'Plan Territorial de Protección Civil de la Ciudad Autónoma de Melilla', 19, NULL, NULL, 2, 1, 2);
