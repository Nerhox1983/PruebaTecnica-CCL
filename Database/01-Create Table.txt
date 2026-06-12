-- 1. Crear la tabla de productos si no existe
CREATE TABLE IF NOT EXISTS productos (
    id SERIAL PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    cantidad INT NOT NULL
);

-- 2. Insertar datos iniciales de prueba (Carga manual)
INSERT INTO productos (nombre, cantidad) VALUES 
('Teclado Mecánico', 50),
('Mouse Inalámbrico', 30),
('Monitor 24 pulgadas', 15)
ON CONFLICT DO NOTHING;