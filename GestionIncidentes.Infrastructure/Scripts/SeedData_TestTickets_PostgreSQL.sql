-- ============================================================
-- Script de Datos de Prueba para PostgreSQL
-- Fecha: 2025-11-30
-- ============================================================

-- Verificar si ya existen datos de prueba
DO $$
DECLARE
    dept_id uuid;
    admin_id uuid;
    tech1_id uuid;
    tech2_id uuid;
    tech3_id uuid;
    student1_id uuid;
    student2_id uuid;
    student3_id uuid;
BEGIN
    -- ============================================================
    -- 1. CREAR DEPARTAMENTO
    -- ============================================================
    SELECT "Id" INTO dept_id FROM "Departments" WHERE "Name" = 'Soporte Técnico' LIMIT 1;
    
    IF dept_id IS NULL THEN
        dept_id := gen_random_uuid();
        INSERT INTO "Departments" ("Id", "Name", "CreatedAt")
        VALUES (dept_id, 'Soporte Técnico', NOW());
        RAISE NOTICE 'Departamento creado: %', dept_id;
    ELSE
        RAISE NOTICE 'Departamento ya existe: %', dept_id;
    END IF;

    -- ============================================================
    -- 2. CREAR USUARIOS
    -- ============================================================
    
    -- Admin
    SELECT "Id" INTO admin_id FROM "Users" WHERE "Email" = 'admin@universidad.edu' LIMIT 1;
    IF admin_id IS NULL THEN
        admin_id := gen_random_uuid();
        INSERT INTO "Users" ("Id", "Email", "Password", "FullName", "DepartmentId", "Role", "Workload", "CreatedAt")
        VALUES (admin_id, 'admin@universidad.edu', 'Admin123!', 'Carlos Rodríguez', dept_id, 'Admin', 0, NOW());
        RAISE NOTICE 'Admin creado: %', admin_id;
    END IF;

    -- Técnico 1: Carlos Méndez
    SELECT "Id" INTO tech1_id FROM "Users" WHERE "Email" = 'carlos.mendez@universidad.edu' LIMIT 1;
    IF tech1_id IS NULL THEN
        tech1_id := gen_random_uuid();
        INSERT INTO "Users" ("Id", "Email", "Password", "FullName", "DepartmentId", "Role", "Workload", "CreatedAt")
        VALUES (tech1_id, 'carlos.mendez@universidad.edu', 'Tech123!', 'Carlos Méndez', dept_id, 'Technician', 0, NOW());
        RAISE NOTICE 'Técnico 1 creado: %', tech1_id;
    END IF;

    -- Técnico 2: Ana Torres
    SELECT "Id" INTO tech2_id FROM "Users" WHERE "Email" = 'ana.torres@universidad.edu' LIMIT 1;
    IF tech2_id IS NULL THEN
        tech2_id := gen_random_uuid();
        INSERT INTO "Users" ("Id", "Email", "Password", "FullName", "DepartmentId", "Role", "Workload", "CreatedAt")
        VALUES (tech2_id, 'ana.torres@universidad.edu', 'Tech123!', 'Ana Torres', dept_id, 'Technician', 0, NOW());
        RAISE NOTICE 'Técnico 2 creado: %', tech2_id;
    END IF;

    -- Técnico 3: Luis García
    SELECT "Id" INTO tech3_id FROM "Users" WHERE "Email" = 'luis.garcia@universidad.edu' LIMIT 1;
    IF tech3_id IS NULL THEN
        tech3_id := gen_random_uuid();
        INSERT INTO "Users" ("Id", "Email", "Password", "FullName", "DepartmentId", "Role", "Workload", "CreatedAt")
        VALUES (tech3_id, 'luis.garcia@universidad.edu', 'Tech123!', 'Luis García', dept_id, 'Technician', 0, NOW());
        RAISE NOTICE 'Técnico 3 creado: %', tech3_id;
    END IF;

    -- Estudiante 1: Juan Pérez
    SELECT "Id" INTO student1_id FROM "Users" WHERE "Email" = 'juan.perez@universidad.edu' LIMIT 1;
    IF student1_id IS NULL THEN
        student1_id := gen_random_uuid();
        INSERT INTO "Users" ("Id", "Email", "Password", "FullName", "Role", "CreatedAt")
        VALUES (student1_id, 'juan.perez@universidad.edu', 'Student123!', 'Juan Pérez', 'Student', NOW());
        RAISE NOTICE 'Estudiante 1 creado: %', student1_id;
    END IF;

    -- Estudiante 2: María López
    SELECT "Id" INTO student2_id FROM "Users" WHERE "Email" = 'maria.lopez@universidad.edu' LIMIT 1;
    IF student2_id IS NULL THEN
        student2_id := gen_random_uuid();
        INSERT INTO "Users" ("Id", "Email", "Password", "FullName", "Role", "CreatedAt")
        VALUES (student2_id, 'maria.lopez@universidad.edu', 'Student123!', 'María López', 'Student', NOW());
        RAISE NOTICE 'Estudiante 2 creado: %', student2_id;
    END IF;

    -- Estudiante 3: Pedro González
    SELECT "Id" INTO student3_id FROM "Users" WHERE "Email" = 'pedro.gonzalez@universidad.edu' LIMIT 1;
    IF student3_id IS NULL THEN
        student3_id := gen_random_uuid();
        INSERT INTO "Users" ("Id", "Email", "Password", "FullName", "Role", "CreatedAt")
        VALUES (student3_id, 'pedro.gonzalez@universidad.edu', 'Student123!', 'Pedro González', 'Student', NOW());
        RAISE NOTICE 'Estudiante 3 creado: %', student3_id;
    END IF;

    -- ============================================================
    -- 3. CREAR TICKETS DE PRUEBA
    -- ============================================================

    -- Ticket 1: Problema crítico de conectividad
    IF NOT EXISTS (SELECT 1 FROM "Tickets" WHERE "Title" = '[SEED] Router sin respuesta') THEN
        INSERT INTO "Tickets" (
            "Id", "Title", "Description", "Category", "Priority", "Status",
            "Location", "CreatedByUserId", "AssignedToUserId",
            "CreatedAt", "SlaDeadline", "AffectedType"
        )
        VALUES (
            gen_random_uuid(),
            '[SEED] Router sin respuesta',
            'El router principal del laboratorio no responde a pings. Los estudiantes no pueden acceder a internet.',
            'connectivity', 'critical', 'En proceso',
            'Lab 1', student1_id, tech1_id,
            NOW() - INTERVAL '2 hours',
            NOW() + INTERVAL '2 hours',
            'classroom'
        );
        RAISE NOTICE 'Ticket 1 creado';
    END IF;

    -- Ticket 2: Problema de hardware resuelto
    IF NOT EXISTS (SELECT 1 FROM "Tickets" WHERE "Title" = '[SEED] Monitor sin señal') THEN
        INSERT INTO "Tickets" (
            "Id", "Title", "Description", "Category", "Priority", "Status",
            "Location", "CreatedByUserId", "AssignedToUserId",
            "CreatedAt", "ResolvedAt", "SlaDeadline", "TechnicianNotes",
            "Rating", "FeedbackComment", "AffectedType", "ProblemType", "AffectedParts"
        )
        VALUES (
            gen_random_uuid(),
            '[SEED] Monitor sin señal',
            'El monitor de la estación 15 no muestra imagen.',
            'hardware', 'medium', 'Resuelto',
            'Lab 3', student1_id, tech2_id,
            NOW() - INTERVAL '1 day',
            NOW() - INTERVAL '12 hours',
            NOW() - INTERVAL '12 hours' + INTERVAL '12 hours',
            'Se reemplazó el cable HDMI que estaba dañado.',
            5, 'Excelente servicio, muy rápido.',
            'classroom', 'hardware', 'Monitor'
        );
        RAISE NOTICE 'Ticket 2 creado';
    END IF;

    -- Ticket 3: Problema de software pendiente
    IF NOT EXISTS (SELECT 1 FROM "Tickets" WHERE "Title" = '[SEED] Error en Visual Studio') THEN
        INSERT INTO "Tickets" (
            "Id", "Title", "Description", "Category", "Priority", "Status",
            "Location", "CreatedByUserId", "AssignedToUserId",
            "CreatedAt", "SlaDeadline", "AffectedType", "ProblemType",
            "ProgramName", "ErrorMessage"
        )
        VALUES (
            gen_random_uuid(),
            '[SEED] Error en Visual Studio',
            'Visual Studio no compila proyectos y muestra error de MSBuild.',
            'software', 'high', 'Pendiente',
            'Lab 2', student2_id, tech3_id,
            NOW() - INTERVAL '3 hours',
            NOW() + INTERVAL '6 hours',
            'classroom', 'software',
            'Visual Studio 2022', 'MSBuild error: Cannot find SDK'
        );
        RAISE NOTICE 'Ticket 3 creado';
    END IF;

    -- Ticket 4: Problema de hardware en proceso
    IF NOT EXISTS (SELECT 1 FROM "Tickets" WHERE "Title" = '[SEED] Teclado no responde') THEN
        INSERT INTO "Tickets" (
            "Id", "Title", "Description", "Category", "Priority", "Status",
            "Location", "CreatedByUserId", "AssignedToUserId",
            "CreatedAt", "SlaDeadline", "TechnicianNotes",
            "AffectedType", "ProblemType", "EquipmentId", "AffectedParts"
        )
        VALUES (
            gen_random_uuid(),
            '[SEED] Teclado no responde',
            'El teclado de la PC 08 no funciona correctamente.',
            'hardware', 'low', 'En proceso',
            'Lab 1', student2_id, tech1_id,
            NOW() - INTERVAL '5 hours',
            NOW() + INTERVAL '24 hours',
            'Se solicitó un teclado nuevo al almacén.',
            'classroom', 'hardware', 'PC-08', 'Teclado'
        );
        RAISE NOTICE 'Ticket 4 creado';
    END IF;

    -- Ticket 5: Problema crítico de WiFi
    IF NOT EXISTS (SELECT 1 FROM "Tickets" WHERE "Title" = '[SEED] WiFi no disponible') THEN
        INSERT INTO "Tickets" (
            "Id", "Title", "Description", "Category", "Priority", "Status",
            "Location", "LocationDetail", "CreatedByUserId", "AssignedToUserId",
            "CreatedAt", "SlaDeadline", "AffectedType"
        )
        VALUES (
            gen_random_uuid(),
            '[SEED] WiFi no disponible',
            'La red WiFi "Universidad_Estudiantes" no aparece en el área de la biblioteca.',
            'connectivity', 'critical', 'Pendiente',
            'Biblioteca', 'Segundo piso', student3_id, tech2_id,
            NOW() - INTERVAL '1 hour',
            NOW() + INTERVAL '2 hours',
            'library'
        );
        RAISE NOTICE 'Ticket 5 creado';
    END IF;

    -- Ticket 6: Problema de hardware resuelto con valoración
    IF NOT EXISTS (SELECT 1 FROM "Tickets" WHERE "Title" = '[SEED] Monitor parpadeando') THEN
        INSERT INTO "Tickets" (
            "Id", "Title", "Description", "Category", "Priority", "Status",
            "Location", "CreatedByUserId", "AssignedToUserId",
            "CreatedAt", "ResolvedAt", "SlaDeadline", "TechnicianNotes",
            "Rating", "FeedbackComment", "AffectedType", "ProblemType", "AffectedParts"
        )
        VALUES (
            gen_random_uuid(),
            '[SEED] Monitor parpadeando',
            'El monitor parpadea constantemente.',
            'hardware', 'medium', 'Resuelto',
            'Lab 4', student3_id, tech3_id,
            NOW() - INTERVAL '2 days',
            NOW() - INTERVAL '1 day',
            NOW() - INTERVAL '2 days' + INTERVAL '12 hours',
            'Se ajustó la frecuencia de refresco a 60Hz.',
            4, 'Bien resuelto.',
            'classroom', 'hardware', 'Monitor'
        );
        RAISE NOTICE 'Ticket 6 creado';
    END IF;

    -- Ticket 7: Problema de software resuelto
    IF NOT EXISTS (SELECT 1 FROM "Tickets" WHERE "Title" = '[SEED] Licencia de AutoCAD vencida') THEN
        INSERT INTO "Tickets" (
            "Id", "Title", "Description", "Category", "Priority", "Status",
            "Location", "CreatedByUserId", "AssignedToUserId",
            "CreatedAt", "ResolvedAt", "SlaDeadline", "TechnicianNotes",
            "Rating", "AffectedType", "ProblemType", "ProgramName"
        )
        VALUES (
            gen_random_uuid(),
            '[SEED] Licencia de AutoCAD vencida',
            'AutoCAD muestra mensaje de licencia vencida en todas las PCs.',
            'software', 'high', 'Resuelto',
            'Lab 3', student1_id, tech1_id,
            NOW() - INTERVAL '3 days',
            NOW() - INTERVAL '2 days',
            NOW() - INTERVAL '3 days' + INTERVAL '6 hours',
            'Se renovó la licencia educativa con Autodesk.',
            5,
            'classroom', 'software', 'AutoCAD 2024'
        );
        RAISE NOTICE 'Ticket 7 creado';
    END IF;

    -- Ticket 8: Problema de hardware en proceso
    IF NOT EXISTS (SELECT 1 FROM "Tickets" WHERE "Title" = '[SEED] Proyector no enciende') THEN
        INSERT INTO "Tickets" (
            "Id", "Title", "Description", "Category", "Priority", "Status",
            "Location", "CreatedByUserId", "AssignedToUserId",
            "CreatedAt", "SlaDeadline", "TechnicianNotes",
            "AffectedType", "ProblemType", "AffectedParts"
        )
        VALUES (
            gen_random_uuid(),
            '[SEED] Proyector no enciende',
            'El proyector del auditorio no responde al control remoto ni al botón de encendido.',
            'hardware', 'medium', 'En proceso',
            'Auditorio', admin_id, tech2_id,
            NOW() - INTERVAL '4 hours',
            NOW() + INTERVAL '12 hours',
            'Se está verificando la fuente de poder. Posible reemplazo necesario.',
            'auditorium', 'hardware', 'Proyector'
        );
        RAISE NOTICE 'Ticket 8 creado';
    END IF;

    RAISE NOTICE '========================================';
    RAISE NOTICE 'Datos de prueba cargados exitosamente';
    RAISE NOTICE '========================================';
    RAISE NOTICE '';
    RAISE NOTICE 'CREDENCIALES DE ACCESO:';
    RAISE NOTICE '';
    RAISE NOTICE 'Admin:';
    RAISE NOTICE '  Email: admin@universidad.edu';
    RAISE NOTICE '  Password: Admin123!';
    RAISE NOTICE '';
    RAISE NOTICE 'Técnicos:';
    RAISE NOTICE '  carlos.mendez@universidad.edu / Tech123!';
    RAISE NOTICE '  ana.torres@universidad.edu / Tech123!';
    RAISE NOTICE '  luis.garcia@universidad.edu / Tech123!';
    RAISE NOTICE '';
    RAISE NOTICE 'Estudiantes:';
    RAISE NOTICE '  juan.perez@universidad.edu / Student123!';
    RAISE NOTICE '  maria.lopez@universidad.edu / Student123!';
    RAISE NOTICE '  pedro.gonzalez@universidad.edu / Student123!';
    RAISE NOTICE '';

END $$;
