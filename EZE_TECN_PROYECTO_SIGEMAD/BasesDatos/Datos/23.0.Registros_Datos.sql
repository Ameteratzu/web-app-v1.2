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
(2, 1, 'Área afectada'),
(3, 1, 'Consecuencias / Actuaciones'),
(4, 1, 'Intervención de Medios');

-- Insertar Apartados de Direccion y Coordinación
INSERT INTO ApartadoRegistro (Id, IdTipoRegistroActualizacion, Nombre) VALUES 
(5, 2, 'Dirección'),
(6, 2, 'Coordinación Cecopi'),
(7, 2, 'Coordinación PMA');