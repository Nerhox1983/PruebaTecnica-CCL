-- 2. Insertar datos iniciales de prueba (Carga manual)
INSERT INTO productos (nombre, cantidad) VALUES 
('Teclado Mecánico', 50),
('Mouse Inalámbrico', 30),
('Monitor 24 pulgadas', 15)
ON CONFLICT DO NOTHING;