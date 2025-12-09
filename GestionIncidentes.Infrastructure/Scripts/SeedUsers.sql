-- Script para crear los 5 usuarios del sistema
-- Ejecutar este script en la base de datos GestionIncidentesDb

-- Primero verificar si ya existen los usuarios
DO $$
DECLARE
    default_dept_id uuid;
BEGIN
    -- Obtener un DepartmentId existente de un usuario actual
    SELECT "DepartmentId" INTO default_dept_id FROM "Users" LIMIT 1;
    
    -- Crear Admin si no existe
    IF NOT EXISTS (SELECT 1 FROM "Users" WHERE "Email" = 'Admin@uta.edu.ec') THEN
        INSERT INTO "Users" ("Id", "Email", "Password", "FullName", "Role", "DepartmentId", "Workload")
        VALUES (
            gen_random_uuid(),
            'Admin@uta.edu.ec',
            'Admin123',
            'Administrador del Sistema',
            'Admin',
            default_dept_id,
            0
        );
        RAISE NOTICE 'Usuario Admin creado';
    ELSE
        RAISE NOTICE 'Usuario Admin ya existe';
    END IF;

    -- Crear Técnico 1 si no existe
    IF NOT EXISTS (SELECT 1 FROM "Users" WHERE "Email" = 'Tec1@uta.edu.ec') THEN
        INSERT INTO "Users" ("Id", "Email", "Password", "FullName", "Role", "DepartmentId", "Workload")
        VALUES (
            gen_random_uuid(),
            'Tec1@uta.edu.ec',
            '123456',
            'Técnico de Soporte 1',
            'Tecnico',
            default_dept_id,
            0
        );
        RAISE NOTICE 'Usuario Tec1 creado';
    ELSE
        RAISE NOTICE 'Usuario Tec1 ya existe';
    END IF;

    -- Crear Técnico 2 si no existe
    IF NOT EXISTS (SELECT 1 FROM "Users" WHERE "Email" = 'Tec2@uta.edu.ec') THEN
        INSERT INTO "Users" ("Id", "Email", "Password", "FullName", "Role", "DepartmentId", "Workload")
        VALUES (
            gen_random_uuid(),
            'Tec2@uta.edu.ec',
            '654321',
            'Técnico de Soporte 2',
            'Tecnico',
            default_dept_id,
            0
        );
        RAISE NOTICE 'Usuario Tec2 creado';
    ELSE
        RAISE NOTICE 'Usuario Tec2 ya existe';
    END IF;

    -- Crear Usuario 1 si no existe
    IF NOT EXISTS (SELECT 1 FROM "Users" WHERE "Email" = 'User1@uta.edu.ec') THEN
        INSERT INTO "Users" ("Id", "Email", "Password", "FullName", "Role", "DepartmentId", "Workload")
        VALUES (
            gen_random_uuid(),
            'User1@uta.edu.ec',
            '12456',
            'Usuario Estudiante 1',
            'Usuario',
            default_dept_id,
            0
        );
        RAISE NOTICE 'Usuario User1 creado';
    ELSE
        RAISE NOTICE 'Usuario User1 ya existe';
    END IF;

    -- Crear Usuario 2 si no existe
    IF NOT EXISTS (SELECT 1 FROM "Users" WHERE "Email" = 'User2@uta.edu.ec') THEN
        INSERT INTO "Users" ("Id", "Email", "Password", "FullName", "Role", "DepartmentId", "Workload")
        VALUES (
            gen_random_uuid(),
            'User2@uta.edu.ec',
            '654321',
            'Usuario Estudiante 2',
            'Usuario',
            default_dept_id,
            0
        );
        RAISE NOTICE 'Usuario User2 creado';
    ELSE
        RAISE NOTICE 'Usuario User2 ya existe';
    END IF;
END $$;

-- Verificar que los usuarios fueron creados
SELECT "Email", "FullName", "Role" 
FROM "Users" 
WHERE "Email" IN ('Admin@uta.edu.ec', 'Tec1@uta.edu.ec', 'Tec2@uta.edu.ec', 'User1@uta.edu.ec', 'User2@uta.edu.ec')
ORDER BY "Role", "Email";
