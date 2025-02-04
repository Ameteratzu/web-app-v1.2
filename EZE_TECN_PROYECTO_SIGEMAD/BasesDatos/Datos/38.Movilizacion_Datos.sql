-- =============================================
-- PROCEDENCIA
-- =============================================

INSERT INTO ProcedenciaMedio (Id,Descripcion) VALUES
    (1, 'CCAA'),
	(2, 'Órgano SNPC'),
	(3, 'Iniciativa DGPCE');

-- =============================================
-- DESTINO
-- =============================================

INSERT INTO DestinoMedio (Id,Descripcion) VALUES
    (1, 'CCAA'),
	(2, 'UME'),
	(3, 'Otros SNPC'),
	(4, 'DGPCE'),
	(5, 'Países bilaterales'),
	(6, 'UCPM');


INSERT INTO EstadoMovilizacion (Id, Descripcion)
VALUES 
(1, 'Inicio'),
(2, 'En Proceso'),
(3, 'Fin');

INSERT INTO PasoMovilizacion (Id, Descripcion, IdEstadoMovilizacion)
VALUES 
(1, 'Solicitud', 1),
(2, 'Tramitación', 2),
(3, 'Ofrecimiento', 2),
(4, 'Cancelación', 3),
(5, 'Aportación', 2),
(6, 'Despliegue', 2),
(7, 'Fin de intervención', 2),
(8, 'Llegada a Base', 3);

INSERT INTO FlujoPasoMovilizacion (Id, IdPasoActual, IdPasoSiguiente, Orden)
VALUES 
(1, NULL, 1, 1), -- Inicio "Solicitud"
(2, 1, 2, 1), -- Desde "Solicitud" a "Tramitación"
(3, 1, 3, 2), -- Desde "Solicitud" a "Ofrecimiento"
(4, 1, 4, 3), -- Desde "Solicitud" a "Cancelación"
(5, 2, 2, 1), -- Desde "Tramitación" a "Tramitación"
(6, 2, 3, 2), -- Desde "Tramitación" a "Ofrecimiento"
(7, 2, 5, 3), -- Desde "Tramitación" a "Aprobación"
(8, 2, 4, 4), -- Desde "Tramitación" a "Cancelación"
(9, 3, 2, 1), -- Desde "Ofrecimiento" a "Tramitación"
(10, 3, 3, 2), -- Desde "Ofrecimiento" a "Ofrecimiento"
(11, 3, 5, 3), -- Desde "Ofrecimiento" a "Aprobación"
(12, 3, 4, 4), -- Desde "Ofrecimiento" a "Cancelación"
(13, 5, 6, 4), -- Desde "Aprobación" a "Despliegue"
(14, 5, 7, 4), -- Desde "Aprobación" a "Fin de intervención"
(15, 6, 7, 4), -- Desde "Despliegue" a "Fin de intervención"
(16, 7, 8, 4); -- Desde "Fin de intervención" a "Repliegue"


INSERT INTO TipoAdministracion (Id, Nombre, Codigo)
VALUES 
(1, 'Estatal', 'AE'),
(2, 'Autonómica', 'CA');

INSERT INTO Administracion (Id, Codigo, Nombre, IdTipoAdministracion)
VALUES
(1, 'E00003301', 'AGE', 1),
(2, 'E05068001', 'AGE', 1),
(3, 'A01002820', 'Junta de Andalucía', 2),
(4, 'A13002908', 'Comunidad de Madrid', 2),
(5, 'A10002983', 'Generalitat Valenciana', 2),
(6, 'A04003003', 'Gobierno de las Illes Balears', 2),
(7, 'A07002862', 'Junta de Castilla y León', 2),
(8, 'A02002834', 'Gobierno de Aragón', 2),
(9, 'A08002880', 'Comunidad Autónoma de Castilla-La Mancha', 2),
(10, 'A11002926', 'Junta de Extremadura', 2),
(11, 'A12002994', 'Comunidad Autónoma de Galicia', 2),
(12, 'A14002961', 'Región de Murcia', 2),
(13, 'A05003638', 'Comunidad Autónoma de Canarias', 2);

INSERT INTO Organismo (Id, Codigo, Nombre, IdAdministracion)
VALUES
(1, 'E00003301', 'Ministerio de Defensa', 1),
(2, 'E05068001', 'Ministerio para la Transición Ecológica y el Reto Demográfico', 2),
(3, 'A01002820', 'Junta de Andalucía', 3),
(4, 'A13002908', 'Comunidad de Madrid', 4),
(5, 'A10002983', 'Generalitat Valenciana', 5),
(6, 'A04003003', 'Gobierno de las Illes Balears', 6),
(7, 'A07002862', 'Junta de Castilla y León', 7),
(8, 'A02002834', 'Gobierno de Aragón', 8),
(9, 'A08002880', 'Comunidad Autónoma de Castilla-La Mancha', 9),
(10, 'A11002926', 'Junta de Extremadura', 10),
(11, 'A12002994', 'Comunidad Autónoma de Galicia', 11),
(12, 'A14002961', 'Región de Murcia', 12),
(13, 'A05003638', 'Comunidad Autónoma de Canarias', 13);



INSERT INTO Entidad (Id, Codigo, Nombre, IdOrganismo)
VALUES
(1, 'E05077401', 'Dirección General de Biodiversidad, Bosques y Desertificación', 2),
(2, 'A01025641', 'Consejería de Agricultura, Pesca, Agua y Desarrollo Rural', 3),
(3, 'TEST0001', 'ADCIF', 2),
(4, 'TEST0002', 'Bomberos Madrid', 4),
(5, 'A10023472', 'Servicio de Extinción de Incendios Forestales (AVSRE)', 5),
(6, 'A04013548', 'Instituto Balear de la Naturaleza (IBANAT)', 6),
(7, 'A07023555', 'Servicio de Incendios Forestales', 7),
(8, 'TEST0003', 'INFOAR', 8),
(9, 'TEST0004', 'INFOCA', 3),
(10, 'A08047750', 'Centro Operativo Regional de Lucha contra Incendios Forestales', 9),
(11, 'A11030455', 'Servicio de Prevención y Extinción de Incendios Forestales', 10),
(12, 'TEST0005', 'INFOGA', 11),
(13, 'A14029799', 'Consorcio de Extinción de Incendios y Salvamento de la Región de Murcia', 12),
(14, 'TEST0006', 'Servico Andaluz de Salud', 3),
(15, 'TEST0007', 'Test - Canarias', 13),
(16, 'E04720503', 'Unidad militar de emergencias', 1);


INSERT INTO TipoCapacidad (Id, Nombre)
VALUES
(1, 'BRIF/A'),
(2, 'BRIF/i  BRIF/B'),
(3, 'AA (FOCA)'),
(4, 'HK'),
(5, 'ACO'),
(6, 'Aa (ALFA)'),
(7, 'ACT (TANGO)'),
(8, 'UMAP'),
(9, 'BLP'),
(10, 'MIKE'),
(11, 'HOTEL'),
(12, 'LIMA'),
(13, 'Técnicos'),
(14, 'Cuadrillas helitransportadas'),
(15, 'Cuadrillas terrestres');

INSERT INTO Capacidad (Id, Nombre, Descripcion, Gestionado, IdTipoCapacidad, IdEntidad)
VALUES
(1	, 'ACO  (Matacan)',	'Aeronave de Comunicaciones y Observación',	0, 5, 1),
(2	, 'ACO Leon', 'Aeronave de Comunicaciones y Observación', 0, 5, 1),
(3	, 'ACO Mutxamiel', 'Aeronave de Comunicaciones y Observación', 0, 5, 1),
(4	, 'ACO Talavera la Real', 'Aeronave de Comunicaciones y Observación', 0, 5, 1),
(5	, 'ACO Zaragoza', 'Aeronave de Comunicaciones y Observación', 0, 5, 1),
(6	, 'Aeronave de Coordinación (Andalucía)', 'Aeronave de Coordinación', 0, NULL, 2),
(7	, 'Aeronave de Coordinación ACO (I.Baleares)', 'Aeronave de Coordinación', 0, NULL,	6),
(8	, 'Agente Forestal/Medioambiental (Aragón)', 'Agente forestal/medioambiental', 0, NULL, 8),
(9	, 'Agente Forestal/Medioambiental (Castilla y León)', 'Agente forestal/medioambiental',	0, NULL, 7),
(10,	'Agente Forestal/Medioambiental (Extremadura)',	'Agente forestal/medioambiental', 0, NULL,11),
(11,	'Agente Forestal/Medioambiental (Extremadura)',	'Agente forestal/medioambiental', 0, NULL, 11),
(12,	'Autobombas  (Extremadura)', 'Camión autobomba para extinción de incendios', 0, NULL,11),
(13,	'Autobombas  (I.Baleares)',	'Camión autobomba para extinción de incendios',	0, NULL,6),
(14,	'Autobombas (Andalucia)', 'Camión autobomba para extinción de incendios', 0, NULL,9),
(15,	'Autobombas (Castilla-La Mancha)', 'Camión autobomba para extinción de incendios', 0, NULL,10),
(16,	'Avion anfibio Comunidad Valenciana (ALFA/Aa) ', 'Avión anfibio tipo 2 (ALFA)',	0, NULL,5),
(17,	'Avión anfibio La Gomera (ALFA/Aa)', 'Avión anfibio tipo 2 (ALFA)',	0, NULL,3),
(18,	'Avión anfibio Labacolla (FOCA/AA)', 'Avión anfibio tipo 1 (FOCA) Canadair', 0, 3, 1),
(19,	'Avión anfibio Los Llanos-Albacete (FOCA/AA)', 'Avión anfibio tipo 1 (FOCA) Canadair', 0, 3, 1),
(20,	'Avión anfibio Málaga (FOCA/AA)', 'Avión anfibio tipo 1 (FOCA) Canadair', 0, 3,	1),
(21,	'Avión anfibio Manises (ALFA/Aa)', 'Avión anfibio tipo 2 (ALFA)', 0, 6,	1),
(22,	'Avión anfibio Matacan (FOCA/AA)', 'Avión anfibio tipo 1 (FOCA) Canadair', 0, 3, 1),
(23,	'Avión anfibio Mirabel (ALFA/Aa)', 'Avión anfibio tipo 2 (ALFA)', 0, 6, 1),
(24,	'Avión anfibio Pollensa (FOCA/AA)', 'Avión anfibio tipo 1 (FOCA) Canadair',	0, 3, 1),
(25,	'Avión anfibio Requena (ALFA/Aa)', 'Avión anfibio tipo 2 (ALFA)', 0, 6, 1),
(26,	'Avión anfibio Reus (ALFA/Aa)', 'Avión anfibio tipo 2 (ALFA)',	0, 6, 1),
(27,	'Avión anfibio Rosinos (ALFA/Aa)', 'Avión anfibio tipo 2 (ALFA)', 0, 6, 1),
(28,	'Avión anfibio Talavera la Real (FOCA/AA)', 'Avión anfibio tipo 1 (FOCA) Canadair',	0, 3, 1),
(29,	'Avión anfibio Torrejón (FOCA/AA)', 'Avión anfibio tipo 1 (FOCA) Canadair',	0, 3, 1),
(30,	'Avión anfibio Viver (ALFA/Aa)', 'Avión anfibio tipo 2 (ALFA)',	0, 6, 1),
(31,	'Avión anfibio Zaragoza (FOCA/AA)', 'Avión anfibio tipo 1 (FOCA) Canadair',	0, 3, 1),
(32,	'Avión de extinción Agoncillo (TANGO/ACT)', 'Avión de extinción de carga en tierra MITECO',	0, 7, 1),
(33,	'Avión de extinción Ampuriabrava (TANGO/ACT)', 'Avión de extinción de carga en tierra MITECO',0, 7, 1),
(34,	'Avión de extinción La Centenera (TANGO/ACT)', 'Avión de extinción de carga en tierra MITECO',0, 7, 1),
(35,	'Avión extinción La Gomera (TANGO/ACT)', 'Avión de extinción de carga en tierra MITECO',0, 7, 1),
(36,	'Avión extinción Niebla (TANGO/ACT)', 'Avión de extinción de carga en tierra MITECO',0, 7, 1),
(37,	'Avión extinción Noain (TANGO/ACT)', 'Avión de extinción de carga en tierra MITECO',0, 7, 1),
(38,	'Avión extinción Son Bonet (TANGO/ACT)', 'Avión de extinción de carga en tierra MITECO',0, 7, 1),
(39,	'Avión extinción Xinzo (TANGO/ACT)', 'Avión de extinción de carga en tierra MITECO',0, 7, 1),
(40,	'BLP Tineo', 'Brigada de Labores Preventivas', 0, 9, 1),
(41,	'BRIF/A  Cuenca/Prado de los Esquiladores', 'Brigada de Refuerzo Contra Incendios Forestales', 0, 1, 1),
(42,	'BRIF/A  La Palma/Puntagorda', 'Brigada de Refuerzo Contra Incendios Forestales', 0, 1, 1),
(43,	'BRIF/A  Laza', 'Brigada de Refuerzo Contra Incendios Forestales', 0, 1, 1),
(44,	'BRIF/A  Lubia', 'Brigada de Refuerzo Contra Incendios Forestales', 0, 1, 1),
(45,	'BRIF/A  Pinofranqueado', 'Brigada de Refuerzo Contra Incendios Forestales', 0, 1, 1),
(46,	'BRIF/A  Tabuyo', 'Brigada de Refuerzo Contra Incendios Forestales', 0, 1, 1),
(47,	'BRIF/A  Tineo', 'Brigada de Refuerzo Contra Incendios Forestales', 0, 1, 1),
(48,	'BRIF/A Daroca', 'Brigada de Refuerzo Contra Incendios Forestales', 0, 1, 1),
(49,	'BRIF/A La Iglesuela', 'Brigada de Refuerzo Contra Incendios Forestales', 0, 1, 1),
(50,	'BRIF/B  Puerto El Pico', 'Brigada de Refuerzo Contra Incendios Forestales', 0, 2, 1),
(51,	'BRIF-i  (Laza)', 'Brigada de Refuerzo Contra Incendios Forestales', 0, 2, 1),
(52,	'BRIF-i  (Pinofranqueado)', 'Brigada de Refuerzo Contra Incendios Forestales', 0, 2, 1),
(53,	'BRIF-i  (Ruente)', 'Brigada de Refuerzo Contra Incendios Forestales', 0, 2, 1),
(54,	'BRIF-i  (Tabuyo)', 'Brigada de Refuerzo Contra Incendios Forestales', 0, 2, 1),
(55,	'BRIF-i (Tineo)', 'Brigada de Refuerzo Contra Incendios Forestales', 0, 2, 1),
(56,	'Camión Nodriza (Castilla-La Mancha)', 'Camión nodriza para suministro de agua', 0, NULL, 10),
(57,	'Camiones de transporte Extremadura', 'Camiones TT para transporte Extremadura', 0, NULL, 11),
(58,	'Helicóptero de extinción Almoraima (KILO/HK)', 'Helicóptero de transporte y extinción pesado', 0, 4, 1),
(59,	'Helicóptero de extinción Andalucía (KILO/HK)', 'Helicóptero de extinción de gran capacidad', 0, 4, 2),
(60,	'Helicóptero de extinción Caravaca (KILO/HK)', 'Helicóptero de transporte y extinción pesado', 0, 4, 1),
(61,	'Helicóptero de extinción Castilla-La Mancha (KILO/HK)', 'Helicóptero de extinción de gran capacidad', 0, 4, 10),
(62,	'Helicóptero de extinción Huelma (KILO/HK)', 'Helicóptero de transporte y extinción pesado', 0, 4, 1),
(63,	'Helicóptero de extinción Ibias (KILO/HK)', 'Helicóptero de transporte y extinción pesado', 0, 4, 1),
(64,	'Helicóptero de extinción Plasencia de Cáceres (KILO/HK)', 'Helicóptero de transporte y extinción pesado', 0, 4, 1),
(65,	'Helicóptero de extinción Plasencia del Monte (KILO/HK)', 'Helicóptero de transporte y extinción pesado', 0, 4, 1),
(66,	'Helicóptero de extinción Tenerife Sur (KILO/HK)', 'Helicóptero de transporte y extinción pesado', 0, 4, 1),
(67,	'Helicóptero de extinción Villlares/Las Minas (KILO/HK)', 'Helicóptero de transporte y extinción pesado', 0, 4, 1),
(68,	'Helicóptero de transporte y extinción MIKE- Andalucía', '	Helicóptero de transporte y extinción medio',  0, 10, 2),
(69,	'Helicóptero de transporte y extinción MIKE- Aragón', '	Helicóptero de transporte y extinción medio', 	0, 10, 8),
(70,	'Helicóptero de transporte y extinción MIKE- Canarias', '	Helicóptero de transporte y extinción medio', 	0, 10, 15),
(71,	'Helicóptero de transporte y extinción MIKE- Castilla y León', 'Helicóptero de transporte y extinción medio', 0, 10, 7),
(72,	'Helicóptero de transporte y extinción MIKE- Castilla-La Mancha', '	Helicóptero de transporte y extinción medio', 0, 10, 10),
(73,	'Helicóptero de transporte y extinción MIKE- Comunidad Valenciana', 'Helicóptero de transporte y extinción medio', 0, 10, 5),
(74,	'Helicóptero de transporte y extinción MIKE- Extremadura', 'Helicóptero de transporte y extinción medio', 0, 10, 11),
(75,	'Helicóptero de transporte y extinción MIKE- Galicia', 'Helicóptero de transporte y extinción medio', 0, 10, 12),
(76,	'Helicóptero de transporte y extinción MIKE- Galicia', 'Helicóptero de transporte y extinción medio', 0, 10, 12),
(77,	'Helicoptero de transporte y extincion MIKE- I.Baleares', 'Helicóptero de transporte y extinción medio', 0, 10, 6),
(78,	'Helicóptero de transporte y extinción MIKE- Madrid', 'Helicóptero de transporte y extinción medio', 0, 10, 4),
(79,	'Helicóptero de transporte y extinción MIKE- Murcia', 'Helicóptero de transporte y extinción medio', 0, 10, 13),
(80,	'Helicóptero medio Caravaca (MIKE/HT)', 'Helicóptero de transporte y extinción medio', 0, 10, 1),
(81,	'Helicóptero medio Huelma (MIKE/HT)', 'Helicóptero de transporte y extinción medio', 0, 10, 1),
(82,	'Helicóptero medio Ibias (MIKE/HT)', 'Helicóptero de transporte y extinción medio', 0, 10, 1),
(83,	'Helicóptero medio La Almoraima (MIKE/HT)', 'Helicóptero de transporte y extinción medio', 0, 10, 1),
(84,	'Helicóptero medio Plasencia de Cáceres (MIKE/HT)', 'Helicóptero de transporte y extinción medio', 0, 10, 1),
(85,	'Helicóptero medio Plasencia del Monte (MIKE/HT)', 'Helicóptero de transporte y extinción medio', 0, 10, 1),
(86,	'Helicóptero medio Tenerife Sur (MIKE/HT)', 'Helicóptero de transporte y extinción medio', 0, 10, 1),
(87,	'Personal sanitario Andalucía', 'Personal sanitario Andalucía', 0, NULL, 14),
(88,	'Técnicos de incendios - Andalucía', 'Personal Técnico', 0, 13, 2),
(89,	'Técnicos de incendios - Aragón', 'Personal Técnico',0,	13,	8),
(90,	'Técnicos de incendios - Castilla y León', 'Personal Técnico', 0, 13, 7),
(91,	'UME', 'Unidad Militar de Emergencias', 1, NULL, 16),
(92,	'Otros', 'Medios sin catalogar', 0, NULL, NULL);
