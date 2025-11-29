-- Script para crear usuarios de prueba en PostgreSQL

-- Primero, crear un departamento
INSERT INTO "Departments" ("Id", "Name")
VALUES ('11111111-1111-1111-1111-111111111111', 'Departamento TI')
ON CONFLICT DO NOTHING;

-- Crear usuarios de prueba
INSERT INTO "Users" ("Id", "Email", "Password", "FullName", "DepartmentId", "Role", "Workload")
VALUES 
    ('00000000-0000-0000-0000-000000000001', 'admin@test.com', 'password123', 'Administrador', '11111111-1111-1111-1111-111111111111', 'DITIC', 0),
    ('00000000-0000-0000-0000-000000000002', 'tecnico@test.com', 'password123', 'Técnico', '11111111-1111-1111-1111-111111111111', 'Tecnico', 0),
    ('00000000-0000-0000-0000-000000000003', 'estudiante@test.com', 'password123', 'Estudiante', '11111111-1111-1111-1111-111111111111', 'Estudiante', 0)
ON CONFLICT DO NOTHING;

-- Verificar que se crearon
SELECT * FROM "Users";
