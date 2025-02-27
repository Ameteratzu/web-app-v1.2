-- Insertar valores iniciales
INSERT INTO EstadoRegistro (Id, Nombre) VALUES 
(1, 'Creado'),
(2, 'Creado en registro anterior'),
(3, 'Creado y Modificado'),
(4, 'Modificado'),
(5, 'Permanente'),
(6, 'Eliminado');


-- Insertar Tipos de Registro
INSERT INTO TipoRegistroActualizacion (Id, Nombre) VALUES 
(1, 'Datos de evolución'),
(2, 'Dirección y Coordinación'),
(3, 'Actuaciones Relevantes'),
(4, 'Documentación'),
(5, 'Otra Información'),
(6, 'Sucesos Relacionados');


-- Insertar Apartados de Evolución
INSERT INTO ApartadoRegistro (Id, IdTipoRegistroActualizacion, Nombre) VALUES 
(1, 1, 'Registro'),
(2, 1, 'Datos principales'),
(3, 1, 'Parámetros'),
(4, 1, 'Área Afectada'),
(5, 1, 'Consecuencias / Actuaciones'),
(6, 1, 'Intervención de Medios');

-- Insertar Apartados de Direccion y Coordinación
INSERT INTO ApartadoRegistro (Id, IdTipoRegistroActualizacion, Nombre) VALUES 
(7, 2, 'Dirección'),
(8, 2, 'Coord. CECOPI'),
(9, 2, 'Coord. PMA');

-- Insertar Apartados de Actuaciones Relevantes
INSERT INTO ApartadoRegistro (Id, IdTipoRegistroActualizacion, Nombre) VALUES 
(10, 3, 'Movilización'),
(11, 3, 'Coord. CECOD'),
(12, 3, 'Activación Planes'),
(13, 3, 'Notificaciones oficiales'),
(14, 3, 'Activación de Sistemas'),
(15, 3, 'Declaración ZAGEP'),
(16, 3, 'Emergencia Nacional');

-- Insertar Apartados de Documentacion, Otra Información y Sucesos Relacionados
INSERT INTO ApartadoRegistro (Id, IdTipoRegistroActualizacion, Nombre) VALUES 
(17, 4, 'Documentación'),
(18, 5, 'Otra Información'),
(19, 6, 'Sucesos Relacionados');