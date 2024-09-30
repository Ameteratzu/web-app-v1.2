SET IDENTITY_INSERT dbo.TipoImpacto ON;

INSERT INTO dbo.TipoImpacto (Id,Descripcion) VALUES
	 (1,N'Consecuencia'),
	 (2,N'Actuación');

SET IDENTITY_INSERT dbo.TipoImpacto OFF;

-- ==================================================================

SET IDENTITY_INSERT dbo.GrupoImpacto ON;

INSERT INTO GrupoImpacto (Id, Descripcion) VALUES (1, 'Personas');
INSERT INTO GrupoImpacto (Id, Descripcion) VALUES (2, 'Servicios básicos');
INSERT INTO GrupoImpacto (Id, Descripcion) VALUES (3, 'Daños');
INSERT INTO GrupoImpacto (Id, Descripcion) VALUES (4, 'Medio natural y otros');


SET IDENTITY_INSERT dbo.GrupoImpacto OFF;

-- ==================================================================

SET IDENTITY_INSERT dbo.SubgrupoImpacto ON;

INSERT INTO SubgrupoImpacto (Id, IdGrupoImpacto, Descripcion) 
VALUES (1, 1, 'Personas');

INSERT INTO SubgrupoImpacto (Id, IdGrupoImpacto, Descripcion) 
VALUES (2, 2, 'Anomalias en los servicios básicos');

INSERT INTO SubgrupoImpacto (Id, IdGrupoImpacto, Descripcion) 
VALUES (3, 2, 'Vialidad en carreteras');

INSERT INTO SubgrupoImpacto (Id, IdGrupoImpacto, Descripcion) 
VALUES (4, 3, 'Daños en edificaciones');

INSERT INTO SubgrupoImpacto (Id, IdGrupoImpacto, Descripcion) 
VALUES (5, 3, 'Daños en redes de distribución');

INSERT INTO SubgrupoImpacto (Id, IdGrupoImpacto, Descripcion) 
VALUES (6, 3, 'Daños en construcciones de especial importancia');

INSERT INTO SubgrupoImpacto (Id, IdGrupoImpacto, Descripcion) 
VALUES (7, 4, 'Medio natural');

INSERT INTO SubgrupoImpacto (Id, IdGrupoImpacto, Descripcion) 
VALUES (8, 4, 'Otros');

SET IDENTITY_INSERT dbo.SubgrupoImpacto OFF;

-- ==================================================================

SET IDENTITY_INSERT dbo.ClaseImpacto ON;

-- SUBGRUPO: PERSONAS
INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (1, 1, 'Fallecidos');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (2, 1, 'Desaparecidos');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (3, 1, 'Personas sin hogar');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (4, 1, 'Personas aisladas');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (5, 1, 'Heridos');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (6, 1, 'Rescate de personas');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (7, 1, 'Evacuación');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (8, 1, 'Albergue');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (9, 1, 'Confinamiento');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (10, 1, 'Realojo');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (11, 1, 'Asistencia psicosocial');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (12, 1, 'Búsqueda de personas');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (13, 1, 'Identificación de cadáveres');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (14, 1, 'Asistencia sanitaria');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (15, 1, 'Profilaxis radiológica');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (16, 1, 'Medidas de protección personal');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (17, 1, 'Activación de ECD/ABRS');

-- SUBGRUPO: Anomalias en los servicios básicos
INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (18, 2, 'Anomalias en el suministro de agua');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (19, 2, 'Anomalias en el suministro de electricidad');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (20, 2, 'Anomalias en el suministro de gas');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (21, 2, 'Anomalias en la red de alcantarillado');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (22, 2, 'Anomalias en la red de saneamiento');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (23, 2, 'Interrupción de servicios educativos');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (24, 2, 'Interrupción de servicios sanitarios');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (25, 2, 'Anomalias en comunicaciones telefónicas y telemáticas');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (26, 2, 'Anomalias en el tráfico ferroviario');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (27, 2, 'Anomalias en tráfico aéreo');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (28, 2, 'Interrupción de tráfico marítimo');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (29, 2, 'Anomalias en tráfico aéreo');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (30, 2, 'Restablecimiento de servicios básicos');

-- SUBGRUPO: Vialidad en carreteras
INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (31, 3, 'Corte de carreteras');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (32, 3, 'Incidencia en carreteras');

-- SUBGRUPO: Daños en edificaciones
INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (33, 4, 'Daños en edificaciones en zonas urbanas');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (34, 4, 'Daños en bienes agropecuarios');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (35, 4, 'Inspección de edificios');

-- SUBGRUPO: Daños en redes de distribución
INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (36, 5, 'Daños en la red de suministro de agua');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (37, 5, 'Daños en la red de distribución eléctrica');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (38, 5, 'Daños en la red de distribución de gas');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (39, 5, 'Daños en la red de saneamiento');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (40, 5, 'Daños en vías urbanas');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (41, 5, 'Daños en carreteras');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (42, 5, 'Daños en la red de ferrocarril');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (43, 5, 'Daños en las telecomunicaciones');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (44, 5, 'Daños en oleoductos');

-- SUBGRUPO: Daños en construcciones de especial importancia
INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (45, 6, 'Daños en puertos marítimos');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (46, 6, 'Daños en aeropuertos');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (47, 6, 'Daños en bienes industriales');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (48, 6, 'Daños en construcciones de especial importancia');

-- SUBGRUPO: Medio natural
INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (49, 7, 'Daños en bienes agropecuarios');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (50, 7, 'Efectos geomorfológicos');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (51, 7, 'Efectos hidrológicos');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (52, 7, 'Contaminación');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (53, 7, 'Vertido de sustancias químicas');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (54, 7, 'Control de accesos');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (55, 7, 'Control de agua y alimentos');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (56, 7, 'Estabulación de animales');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (57, 7, 'Otros');

-- SUBGRUPO: Otros
INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (58, 8, 'Daños en áreas urbanas');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (59, 8, 'Medidas administrativas públicas');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (60, 8, 'Monitorización y evaluación');

INSERT INTO ClaseImpacto (Id, IdSubgrupoImpacto, Descripcion) 
VALUES (61, 8, 'Medidas de reconstrucción');




SET IDENTITY_INSERT dbo.ClaseImpacto OFF;

-- ==================================================================

SET IDENTITY_INSERT dbo.Impacto ON;

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (1, 1, 1, 'Fallecidos', 1);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (2, 1, 2, 'Desaparecidos', 1);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (3, 1, 3, 'Personas sin hogar', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (4, 1, 4, 'Personas aisladas', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (5, 1, 5, 'Heridos', 1);

-- Add more as necessary based on your data

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (6, 1, 5, 'Quemados', 1);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (7, 1, 5, 'Intoxicados', 1);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (8, 1, 5, 'Contagiados', 1);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (9, 1, 5, 'Contaminados', 1);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (10, 1, 5, 'Irradiados', 1);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (11, 2, 6, 'Rescate de personas', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (12, 2, 7, 'Evacuación', 1);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (13, 2, 8, 'Albergue', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (14, 2, 9, 'Confinamiento', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (15, 2, 10, 'Realojo', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (16, 2, 11, 'Asistencia psicosocial', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (17, 2, 12, 'Búsqueda de personas', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (18, 2, 13, 'Identificación de cadáveres', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (19, 2, 14, 'Asistencia sanitaria', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (20, 2, 14, 'Hospitalización', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (21, 2, 14, 'Traslado de pacientes a otros hospitales', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (22, 2, 15, 'Profilaxis radiológica', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (23, 2, 16, 'Medidas de protección personal ', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (24, 2, 17, 'Activación de ECD', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (25, 2, 17, 'Activación de ABRS', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (26, 1, 18, 'Anomalias en el suministro de agua', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (27, 1, 19, 'Anomalias en el suministro de electricidad', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (28, 1, 20, 'Anomalias en el suministro de gas', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (29, 1, 21, 'Anomalias en la red de alcantarillado', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (30, 1, 22, 'Anomalias en la red de saneamiento', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (31, 2, 23, 'Interrupción de servicios educativos', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (32, 1, 24, 'Interrupción de servicios sanitarios', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (33, 1, 25, 'Anomalias en la telefonía fija', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (34, 1, 25, 'Anomalias en la telefonía móvil', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (35, 1, 25, 'Cortes de conexión a internet', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (36, 1, 26, 'Anomalias en el tráfico ferroviario de la red convencional', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (37, 1, 26, 'Anomalias en el tráfico ferroviario de alta velocidad', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (38, 1, 27, 'Anomalias en tráfico aéreo', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (39, 1, 28, 'Interrupción de tráfico marítimo', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (40, 2, 29, 'Seguridad en la navegación aérea', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (41, 2, 30, 'Restablecimiento de servicios básicos', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (42, 1, 31, 'Circulación nivel negro en vías de alta capacidad', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (43, 1, 31, 'Circulación nivel negro en carreteras ------', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (44, 1, 32, 'Circulación nivel rojo', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (45, 1, 32, 'Circulación nivel amarillo', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (46, 1, 33, 'Daños en viviendas unifamiliares', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (47, 1, 33, 'Daños en edificios de viviendas', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (48, 1, 34, 'Daños en viviendas en medio rural o forestal', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (49, 1, 34, 'Daños en instalaciones agropecuarias', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (50, 1, 33, 'Daños en garajes', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (51, 1, 33, 'Daños en oficinas, comercios y almacenes', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (52, 2, 35, 'Inspección de edificios', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (53, 1, 36, 'Daños en la red de suministro de agua', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (54, 1, 37, 'Daños en la red de distribución energía eléctrica', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (55, 1, 38, 'Daños en la red de distribución de gas', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (56, 1, 39, 'Daños en la red de saneamiento', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (57, 1, 40, 'Daños en vías urbanas', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (58, 1, 41, 'Daños en carreteras', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (59, 1, 42, 'Daños en la red de ferrocarril', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (60, 1, 43, 'Daños en las telecomunicaciones', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (61, 1, 44, 'Daños en oleoducto', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (62, 1, 45, 'Daños en puertos marítimos', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (63, 1, 46, 'Daños en aeropuertos', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (64, 1, 47, 'Daños en bienes industriales', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (65, 1, 48, 'Daños en presas', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (66, 1, 48, 'Daños en centros operativos FAS y C Seguridad', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (67, 1, 48, 'Daños en centros sanitarios', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (68, 1, 48, 'Daños en centros educativos', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (69, 1, 48, 'Daños en parques de bomberos', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (70, 1, 48, 'Daños en edificios de uso público', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (71, 1, 48, 'Daños en edificios histórico-artísticos', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (72, 1, 48, 'Daños en otras construcciones', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (73, 1, 49, 'Daños en agricultura', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (74, 1, 49, 'Daños a animales', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (75, 1, 50, 'Efectos geomorfológicos', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (76, 1, 51, 'Efectos hidrológicos', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (77, 1, 52, 'Contaminación hídrica', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (78, 1, 52, 'Nube tóxica', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (79, 1, 52, 'Contaminación del suelo', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (80, 1, 52, 'Contaminación marina', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (81, 1, 51, 'Desbordamiento de ríos', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (82, 1, 53, 'Vertido de sustancias químicas', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (83, 1, 52, 'Contaminación radiológica', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (84, 2, 54, 'Control de accesos', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (85, 2, 55, 'Control de agua y alimentos', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (86, 2, 56, 'Estabulación de animales', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (87, 1, 57, 'Otros efectos en el medio natural', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (88, 1, 58, 'Caída de árboles', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (89, 1, 58, 'Daños en vehículos automoviles', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (90, 1, 58, 'Daños en mobiliario urbano', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (91, 1, 57, 'Impacto económico', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (92, 1, 57, 'Otras consecuencias', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (93, 2, 59, 'Medidas administrativas públicas', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (94, 2, 60, 'Monitorización y evaluación', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (95, 2, 61, 'Medidas de reconstrucción', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (96, 2, 59, 'Concesión de subvenciones', 0);

INSERT INTO Impacto (Id, IdTipoImpacto, IdClaseImpacto, Descripcion, RelevanciaGeneral) 
VALUES (97, 2, 57, 'Otras actuaciones', 0);

SET IDENTITY_INSERT dbo.Impacto OFF;