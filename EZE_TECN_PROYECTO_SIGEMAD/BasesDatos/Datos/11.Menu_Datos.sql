INSERT INTO dbo.Menu (Id, Nombre, Tipo, IdGrupo, NumOrden, Icono, ColorRgb, Ruta) VALUES
(1, 'Panel Inicial', 'grupo', 0, 1, '/assets/img/dashboard.svg', '204, 204, 0', '/dashboard'),
(2, 'Buscador', 'grupo', 0, 2, '/assets/img/search.svg', '51, 153, 255', '/search'),
(3, 'Episodios', 'grupo', 0, 3, '/assets/img/episodes.svg', '102, 204, 0', '/episodes'),
(4, 'RIESGOS NATURALES', 'grupo', 0, 4, '', '', ''),
(5, 'Incendios forestales', 'item', 4, 1, '/assets/img/fire.svg', '255, 128, 0', '/fire'),
(6, 'Terremotos', 'item', 4, 2, '/assets/img/earthquakes.svg', '144, 73, 0', '/earthquakes'),
(7, 'Meteorología adversa', 'item', 4, 3, '/assets/img/adverse-weather.svg', '0, 0, 255', '/adverse-weather'),
(8, 'Fenómenos volcánicos', 'item', 4, 4, '/assets/img/volcanic-phenomena.svg', '255, 128, 0', '/volcanic-phenomena'),
(9, 'Inundaciones', 'item', 4, 5, '/assets/img/floods.svg', '0, 153, 153', '/floods'),
(10, 'RIESGOS TECNOLÓGICOS', 'grupo', 0, 5, '', '', ''),
(11, 'Riesgo químico', 'item', 10, 1, '/assets/img/chemical-risk.svg', '156, 161, 35', '/chemical-risk'),
(12, 'Mercancías peligrosas', 'item', 10, 2, '/assets/img/dangerous-goods.svg', '249, 215, 5', '/dangerous-goods'),
(13, 'Riesgo nuclear/radiológico', 'item', 10, 3, '/assets/img/nuclear-radiological-risk.svg', '204, 153, 255', '/nuclear-radiological-risk'),
(14, 'OTROS RIESGOS', 'grupo', 0, 6, '', '', ''),
(15, 'Otros riesgos', 'item', 14, 1, '/assets/img/other-risks.svg', '0, 134, 187', '/other-risks'),
(16, 'OPE', 'item', 14, 2, '/assets/img/ope.svg', '86, 130, 171', '/ope'),
(17, 'UTILIDADES', 'grupo', 0, 7, '', '', ''),
(18, 'Cuadro de Mando', 'item', 17, 1, '/assets/img/dashboard.svg', '102, 0, 102', '/dashboard'),
(19, 'Documentación', 'item', 17, 2, '/assets/img/documentation.svg', '0, 153, 0', '/documentation'),
(20, 'Incidencias', 'item', 17, 3, '/assets/img/incidents.svg', '153, 0, 76', '/incidents'),
(21, 'ADMINISTRACION', 'grupo', 0, 8, '', '', ''),
(22, 'Configuración', 'item', 21, 1, '/assets/img/config.svg', '169, 169, 169', '/config'),
(23, 'Usuarios', 'item', 21, 2, '/assets/img/users.svg', '102, 51, 0', '/users'),
(24, 'Catálogos', 'item', 21, 3, '/assets/img/catalogs.svg', '55, 3, 159', '/catalogs'),
(25, 'Administración OPE', 'item', 16, 1, NULL, NULL, NULL)
(26, 'Periodos', 'item', 25, 1, '/ope-administracion-periodos', '/assets/img/other-risks.svg', '169, 169, 169')
(27, 'Puertos', 'item', 25, 2, '/ope-administracion-puertos', '/assets/img/config.svg', '169, 169, 169')
(28, 'Líneas marítimas', 'item', 25, 3, '/ope-administracion-lineas-maritimas', '/assets/img/config.svg', '169, 169, 169')
(29, 'Fronteras', 'item', 25, 4, '/ope-administracion-fronteras', '/assets/img/config.svg', '169, 169, 169')
(30, 'Puntos de Control en carreteras', 'item', 25, 5, '/ope-administracion-puntos-control-carreteras', NULL, NULL)
(31, 'Áreas de descanso y puntos de información en carreteras', 'item', 25, 6, '/ope-administracion-areas-descanso', NULL, NULL)
(32, 'Áreas de estacionamiento', 'item', 25, 7, '/ope-administracion-areas-estacionamiento', NULL, NULL)
(33, 'Porcentajes de ocupación de áreas de estacionamiento', 'item', 25, 8, '/ope-administracion-ocupacion-areas-estacionamiento', NULL, NULL)
(34, 'Log', 'item', 25, 9, '/ope-administracion-log', NULL, NULL)
(35, 'Nuevo', 'item', 16, 2, NULL, NULL, NULL)
(36, 'Embarques diarios', 'item', 35, 1, '/ope-nuevo-embarques-diarios', NULL, NULL)
(37, 'Embarques. Funcionalidades', 'item', 35, 2, '/ope-nuevo-embarques-funcionalidades', NULL, NULL)
(38, 'Asistencias', 'item', 35, 3, '/ope-nuevo-asistencias', NULL, NULL)
(39, 'Asistencias. Funcionalidades', 'item', 35, 4, '/ope-nuevo-asistencias-funcionalidades', NULL, NULL)
(40, 'Fronteras', 'item', 35, 5, '/ope-nuevo-fronteras', NULL, NULL)
(41, 'Fronteras. Funcionalidades', 'item', 35, 6, '/ope-nuevo-fronteras-funcionalidades', NULL, NULL)
(42, 'Afluencia a puntos de control en carreteras', 'item', 35, 7, '/ope-nuevo-afluencia-puntos-control-carreteras', NULL, NULL)
(43, 'Afluencia a puntos de control en carreteras. Funcionalidades', 'item', 35, 8, '/ope-nuevo-afluencia-puntos-control-carreteras-funcionalidades', NULL, NULL)
(44, 'Áreas de descanso y puntos de información en carreteras', 'item', 35, 9, '/ope-nuevo-areas-descanso', NULL, NULL)
(45, 'Ocupación de áreas de estacionamiento', 'item', 35, 10, '/ope-nuevo-areas-estacionamiento', NULL, NULL)
(46, 'Ocupación de áreas de estacionamiento. Funcionalidades', 'item', 35, 11, '/ope-nuevo-areas-estacionamiento-funcionalidades', NULL, NULL)
(47, 'Buscar', 'item', 16, 3, NULL, NULL, NULL)
(48, 'Embarques diarios', 'item', 47, 1, '/ope-buscar-embarques-diarios', NULL, NULL)
(49, 'Embarques. Funcionalidades', 'item', 47, 2, '/ope-buscar-embarques-funcionalidades', NULL, NULL)
(50, 'Asistencias', 'item', 47, 3, '/ope-buscar-asistencias', NULL, NULL)
(51, 'Asistencias. Funcionalidades', 'item', 47, 4, '/ope-buscar-asistencias-funcionalidades', NULL, NULL)
(52, 'Fronteras', 'item', 47, 5, '/ope-buscar-fronteras', NULL, NULL)
(53, 'Fronteras. Funcionalidades', 'item', 47, 6, '/ope-buscar-fronteras-funcionalidades', NULL, NULL)
(54, 'Afluencia a puntos de control en carretera', 'item', 47, 7, '/ope-buscar-afluencia-puntos-control-carretera', NULL, NULL)
(55, 'Afluencia a puntos de control en carreteras. Funcionalidades', 'item', 47, 8, '/ope-buscar-afluencia-puntos-control-carretera-funcionalidades', NULL, NULL)
(56, 'Áreas de descanso y puntos de información en carreteras', 'item', 47, 9, '/ope-buscar-areas-descanso', NULL, NULL)
(57, 'Ocupación de áreas de estacionamiento', 'item', 47, 10, '/ope-buscar-ocupacion-areas-estacionamiento', NULL, NULL)
(58, 'Ocupación de áreas de estacionamiento. Funcionalidades', 'item', 47, 11, '/ope-buscar-ocupacion-areas-estacionamiento-funcionalidades', NULL, NULL)
(59, 'APBA-Entradas a puerto y embarques por horas', 'item', 16, 4, NULL, NULL, NULL)
(60, 'Entrada de vehículos en puertos APBA. Datos', 'item', 59, 1, '/ope-apba-entrada-vehiculos-puertos', NULL, NULL)
(61, 'Entrada de vehículos en puertos APBA. Funcionalidades', 'item', 59, 2, '/ope-apba-entrada-vehiculos-puertos-funcionalidades', NULL, NULL)
(62, 'Embarques de vehículos en APBA por intervalos horarios. Datos', 'item', 59, 3, '/ope-apba-embarques-vehiculos-intervalos-horarios', NULL, NULL)
(63, 'Embarques de vehículos por intervalos horarios. Funcionalidades', 'item', 59, 4, '/ope-apba-embarques-vehiculos-intervalos-horarios-funcionalidades', NULL, NULL)
(64, 'Planificación', 'item', 16, 5, NULL, NULL, NULL)
(65, 'Plan de flota', 'item', 64, 1, '/ope-planificacion-plan-flota', NULL, NULL)
(66, 'Plan de flota. Funcionalidades', 'item', 64, 2, '/ope-planificacion-plan-flota-funcionalidades', NULL, NULL)
(67, '	Participantes AGE', 'item', 64, 3, '/ope-planificacion-participantes-age', NULL, NULL)
(68, 'Incidencias', 'item', 16, 6, NULL, NULL, NULL)
(69, 'Incidencias. Datos de inicio', 'item', 68, 1, '/ope-incidencias-datos-inicio', NULL, NULL);

