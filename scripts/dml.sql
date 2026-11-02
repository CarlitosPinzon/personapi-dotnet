-- =============================================
-- Laboratorio 1 - personapi-dotnet
-- Script DML: datos de prueba
-- Ejecutar despues de ddl.sql
-- =============================================

USE persona_db;
GO

INSERT INTO persona (cc, nombre, apellido, genero, edad) VALUES
(1010101010, 'Laura',    'Gomez',    'F', 22),
(1020202020, 'Andres',   'Martinez', 'M', 25),
(1030303030, 'Valentina','Rodriguez','F', 30),
(1040404040, 'Santiago', 'Lopez',    'M', 21),
(1050505050, 'Camila',   'Herrera',  'F', 27);
GO

INSERT INTO profesion (id, nom, des) VALUES
(1, 'Ingenieria de Sistemas',  'Diseno, desarrollo y gestion de sistemas de informacion y software.'),
(2, 'Ingenieria Electronica',  'Diseno de circuitos, sistemas embebidos y telecomunicaciones.'),
(3, 'Administracion de Empresas', 'Planeacion, organizacion y control de organizaciones.'),
(4, 'Medicina',                'Prevencion, diagnostico y tratamiento de enfermedades.'),
(5, 'Derecho',                 'Estudio y aplicacion de las normas juridicas.');
GO

INSERT INTO estudios (id_prof, cc_per, fecha, univer) VALUES
(1, 1010101010, '2024-06-15', 'Pontificia Universidad Javeriana'),
(2, 1020202020, '2022-12-10', 'Universidad Nacional de Colombia'),
(1, 1020202020, '2025-06-20', 'Universidad de los Andes'),
(4, 1030303030, '2019-11-30', 'Universidad del Rosario'),
(3, 1050505050, '2021-07-05', 'Universidad Externado de Colombia');
GO

INSERT INTO telefono (num, oper, duenio) VALUES
('3001234567', 'Claro',    1010101010),
('3109876543', 'Movistar', 1020202020),
('3204567890', 'Tigo',     1030303030),
('3157654321', 'Claro',    1030303030),
('3012223344', 'WOM',      1040404040);
GO
