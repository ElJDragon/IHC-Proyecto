-- ============================================================
-- Script de Datos de Prueba para PostgreSQL
-- Sistema de Gestión de Incidentes
-- Fecha: 2025-11-30
-- ============================================================

-- NOTA: Ejecutar este script en pgAdmin o herramienta similar
-- Base de datos: GestionIncidentesDb

-- ============================================================
-- LIMPIAR DATOS EXISTENTES (OPCIONAL - DESCOMENTAR SI NECESITAS)
-- ============================================================
-- DELETE FROM "Tickets";
-- DELETE FROM "Users";
-- DELETE FROM "Departments";

-- ============================================================
-- 1. VERIFICAR Y CREAR DEPARTAMENTO
-- ============================================================
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
    -- Verificar si ya existe el departamento
    SELECT "Id" INTO dept_id 
    FROM "Departments" 
    WHERE "Name" = 'Soporte Técnico' 
    LIMIT 1;
    
    IF dept_id IS NULL THEN
        dept_id := gen_random_uuid();
        INSERT INTO "Departments" ("Id", "Name")
        VALUES (dept_id, 'Soporte Técnico');
        RAISE NOTICE '✓ Departamento "Soporte Técnico" creado: %', dept_id;
    ELSE
        RAISE NOTICE '✓ Departamento "Soporte Técnico" ya existe: %', dept_id;
    END IF;

    -- ============================================================
    -- 2. CREAR USUARIOS
    -- ============================================================
    
    -- ==================== ADMIN ====================
    SELECT "Id" INTO admin_id 
    FROM "Users" 
    WHERE "Email" = 'admin@universidad.edu' 
    LIMIT 1;
    
    IF admin_id IS NULL THEN
        admin_id := gen_random_uuid();
        INSERT INTO "Users" ("Id", "Email", "Password", "FullName", "DepartmentId", "Role", "Workload")
        VALUES (
            admin_id, 
            'admin@universidad.edu', 
            'Admin123!', 
            'Carlos Rodríguez', 
            dept_id, 
            'Admin', 
            0
        );
        RAISE NOTICE '✓ Admin creado: admin@universidad.edu';
    ELSE
        RAISE NOTICE '✓ Admin ya existe: admin@universidad.edu';
    END IF;

    -- ==================== TÉCNICO 1: Carlos Méndez ====================
    SELECT "Id" INTO tech1_id 
    FROM "Users" 
    WHERE "Email" = 'carlos.mendez@universidad.edu' 
    LIMIT 1;
    
    IF tech1_id IS NULL THEN
        tech1_id := gen_random_uuid();
        INSERT INTO "Users" ("Id", "Email", "Password", "FullName", "DepartmentId", "Role", "Workload")
        VALUES (
            tech1_id, 
            'carlos.mendez@universidad.edu', 
            'Tech123!', 
            'Carlos Méndez', 
            dept_id, 
            'Technician', 
            0
        );
        RAISE NOTICE '✓ Técnico 1 creado: carlos.mendez@universidad.edu';
    ELSE
        RAISE NOTICE '✓ Técnico 1 ya existe: carlos.mendez@universidad.edu';
    END IF;

    -- ==================== TÉCNICO 2: Ana Torres ====================
    SELECT "Id" INTO tech2_id 
    FROM "Users" 
    WHERE "Email" = 'ana.torres@universidad.edu' 
    LIMIT 1;
    
    IF tech2_id IS NULL THEN
        tech2_id := gen_random_uuid();
        INSERT INTO "Users" ("Id", "Email", "Password", "FullName", "DepartmentId", "Role", "Workload")
        VALUES (
            tech2_id, 
            'ana.torres@universidad.edu', 
            'Tech123!', 
            'Ana Torres', 
            dept_id, 
            'Technician', 
            0
        );
        RAISE NOTICE '✓ Técnico 2 creado: ana.torres@universidad.edu';
    ELSE
        RAISE NOTICE '✓ Técnico 2 ya existe: ana.torres@universidad.edu';
    END IF;

    -- ==================== TÉCNICO 3: Luis García ====================
    SELECT "Id" INTO tech3_id 
    FROM "Users" 
    WHERE "Email" = 'luis.garcia@universidad.edu' 
    LIMIT 1;
    
    IF tech3_id IS NULL THEN
        tech3_id := gen_random_uuid();
        INSERT INTO "Users" ("Id", "Email", "Password", "FullName", "DepartmentId", "Role", "Workload")
        VALUES (
            tech3_id, 
            'luis.garcia@universidad.edu', 
            'Tech123!', 
            'Luis García', 
            dept_id, 
            'Technician', 
            0
        );
        RAISE NOTICE '✓ Técnico 3 creado: luis.garcia@universidad.edu';
    ELSE
        RAISE NOTICE '✓ Técnico 3 ya existe: luis.garcia@universidad.edu';
    END IF;

    -- ==================== ESTUDIANTE 1: Juan Pérez ====================
    SELECT "Id" INTO student1_id 
    FROM "Users" 
    WHERE "Email" = 'juan.perez@universidad.edu' 
    LIMIT 1;
    
    IF student1_id IS NULL THEN
        student1_id := gen_random_uuid();
        INSERT INTO "Users" ("Id", "Email", "Password", "FullName", "DepartmentId", "Role", "Workload")
        VALUES (
            student1_id, 
            'juan.perez@universidad.edu', 
            'Student123!', 
            'Juan Pérez', 
            dept_id, 
            'Student', 
            0
        );
        RAISE NOTICE '✓ Estudiante 1 creado: juan.perez@universidad.edu';
    ELSE
        RAISE NOTICE '✓ Estudiante 1 ya existe: juan.perez@universidad.edu';
    END IF;

    -- ==================== ESTUDIANTE 2: María López ====================
    SELECT "Id" INTO student2_id 
    FROM "Users" 
    WHERE "Email" = 'maria.lopez@universidad.edu' 
    LIMIT 1;
    
    IF student2_id IS NULL THEN
        student2_id := gen_random_uuid();
        INSERT INTO "Users" ("Id", "Email", "Password", "FullName", "DepartmentId", "Role", "Workload")
        VALUES (
            student2_id, 
            'maria.lopez@universidad.edu', 
            'Student123!', 
            'María López', 
            dept_id, 
            'Student', 
            0
        );
        RAISE NOTICE '✓ Estudiante 2 creado: maria.lopez@universidad.edu';
    ELSE
        RAISE NOTICE '✓ Estudiante 2 ya existe: maria.lopez@universidad.edu';
    END IF;

    -- ==================== ESTUDIANTE 3: Pedro González ====================
    SELECT "Id" INTO student3_id 
    FROM "Users" 
    WHERE "Email" = 'pedro.gonzalez@universidad.edu' 
    LIMIT 1;
    
    IF student3_id IS NULL THEN
        student3_id := gen_random_uuid();
        INSERT INTO "Users" ("Id", "Email", "Password", "FullName", "DepartmentId", "Role", "Workload")
        VALUES (
            student3_id, 
            'pedro.gonzalez@universidad.edu', 
            'Student123!', 
            'Pedro González', 
            dept_id, 
            'Student', 
            0
        );
        RAISE NOTICE '✓ Estudiante 3 creado: pedro.gonzalez@universidad.edu';
    ELSE
        RAISE NOTICE '✓ Estudiante 3 ya existe: pedro.gonzalez@universidad.edu';
    END IF;

    -- ============================================================
    -- 3. CREAR TICKETS DE PRUEBA
    -- ============================================================

    -- ==================== TICKET 1: Router sin respuesta (CRÍTICO) ====================
    IF NOT EXISTS (SELECT 1 FROM "Tickets" WHERE "Title" = '[SEED] Router sin respuesta en Lab 1') THEN
        INSERT INTO "Tickets" (
            "Id", "Title", "Description", "Category", "Priority", "Status",
            "Location", "AffectedType", "CreatedByUserId", "AssignedToUserId",
            "CreatedAt", "SlaDeadline"
        )
        VALUES (
            gen_random_uuid(),
            '[SEED] Router sin respuesta en Lab 1',
            'El router principal del Laboratorio 1 no responde a pings. Los estudiantes no pueden acceder a internet para realizar sus prácticas de programación.',
            'connectivity',
            'critical',
            'En proceso',
            'Lab 1',
            'classroom',
            student1_id,
            tech1_id,
            NOW() - INTERVAL '2 hours',
            NOW() + INTERVAL '2 hours'
        );
        RAISE NOTICE '✓ Ticket 1 creado: Router sin respuesta (Crítico)';
    END IF;

    -- ==================== TICKET 2: Monitor sin señal (RESUELTO) ====================
    IF NOT EXISTS (SELECT 1 FROM "Tickets" WHERE "Title" = '[SEED] Monitor sin señal en Lab 3') THEN
        INSERT INTO "Tickets" (
            "Id", "Title", "Description", "Category", "Priority", "Status",
            "Location", "AffectedType", "ProblemType", "AffectedParts",
            "CreatedByUserId", "AssignedToUserId",
            "CreatedAt", "ResolvedAt", "SlaDeadline",
            "TechnicianNotes", "Rating", "FeedbackComment"
        )
        VALUES (
            gen_random_uuid(),
            '[SEED] Monitor sin señal en Lab 3',
            'El monitor de la estación 15 no muestra imagen. La PC enciende correctamente pero no hay señal en pantalla.',
            'hardware',
            'medium',
            'Resuelto',
            'Lab 3',
            'classroom',
            'hardware',
            'Monitor',
            student1_id,
            tech2_id,
            NOW() - INTERVAL '1 day',
            NOW() - INTERVAL '12 hours',
            NOW() - INTERVAL '12 hours',
            'Se identificó que el cable HDMI estaba dañado. Se reemplazó por uno nuevo y se verificó el funcionamiento correcto del monitor.',
            5,
            'Excelente servicio. El técnico fue muy rápido y profesional.'
        );
        RAISE NOTICE '✓ Ticket 2 creado: Monitor sin señal (Resuelto con rating 5)';
    END IF;

    -- ==================== TICKET 3: Error en Visual Studio (PENDIENTE) ====================
    IF NOT EXISTS (SELECT 1 FROM "Tickets" WHERE "Title" = '[SEED] Error de compilación en Visual Studio') THEN
        INSERT INTO "Tickets" (
            "Id", "Title", "Description", "Category", "Priority", "Status",
            "Location", "AffectedType", "ProblemType", "ProgramName", "ErrorMessage",
            "CreatedByUserId", "AssignedToUserId",
            "CreatedAt", "SlaDeadline"
        )
        VALUES (
            gen_random_uuid(),
            '[SEED] Error de compilación en Visual Studio',
            'Visual Studio 2022 no puede compilar proyectos de .NET y muestra error relacionado con MSBuild.',
            'software',
            'high',
            'Pendiente',
            'Lab 2',
            'classroom',
            'software',
            'Visual Studio 2022',
            'MSBuild error: Cannot find SDK Microsoft.NET.Sdk',
            student2_id,
            tech3_id,
            NOW() - INTERVAL '3 hours',
            NOW() + INTERVAL '6 hours'
        );
        RAISE NOTICE '✓ Ticket 3 creado: Error Visual Studio (Alta prioridad)';
    END IF;

    -- ==================== TICKET 4: Teclado no responde (EN PROCESO) ====================
    IF NOT EXISTS (SELECT 1 FROM "Tickets" WHERE "Title" = '[SEED] Teclado no responde en PC-08') THEN
        INSERT INTO "Tickets" (
            "Id", "Title", "Description", "Category", "Priority", "Status",
            "Location", "AffectedType", "ProblemType", "EquipmentId", "AffectedParts",
            "CreatedByUserId", "AssignedToUserId",
            "CreatedAt", "SlaDeadline", "TechnicianNotes"
        )
        VALUES (
            gen_random_uuid(),
            '[SEED] Teclado no responde en PC-08',
            'El teclado de la PC 08 no funciona correctamente. Algunas teclas no registran entrada.',
            'hardware',
            'low',
            'En proceso',
            'Lab 1',
            'classroom',
            'hardware',
            'PC-08',
            'Teclado',
            student2_id,
            tech1_id,
            NOW() - INTERVAL '5 hours',
            NOW() + INTERVAL '24 hours',
            'Se solicitó un teclado nuevo al almacén. En espera de llegada del equipo.'
        );
        RAISE NOTICE '✓ Ticket 4 creado: Teclado no responde (Baja prioridad)';
    END IF;

    -- ==================== TICKET 5: WiFi no disponible (CRÍTICO) ====================
    IF NOT EXISTS (SELECT 1 FROM "Tickets" WHERE "Title" = '[SEED] Red WiFi no disponible en Biblioteca') THEN
        INSERT INTO "Tickets" (
            "Id", "Title", "Description", "Category", "Priority", "Status",
            "Location", "LocationDetail", "AffectedType",
            "CreatedByUserId", "AssignedToUserId",
            "CreatedAt", "SlaDeadline"
        )
        VALUES (
            gen_random_uuid(),
            '[SEED] Red WiFi no disponible en Biblioteca',
            'La red WiFi "Universidad_Estudiantes" no aparece disponible en toda el área de la biblioteca. Los estudiantes no pueden conectarse para estudiar.',
            'connectivity',
            'critical',
            'Pendiente',
            'Biblioteca',
            'Segundo piso - Área de lectura',
            'library',
            student3_id,
            tech2_id,
            NOW() - INTERVAL '1 hour',
            NOW() + INTERVAL '2 hours'
        );
        RAISE NOTICE '✓ Ticket 5 creado: WiFi no disponible (Crítico)';
    END IF;

    -- ==================== TICKET 6: Monitor parpadeando (RESUELTO) ====================
    IF NOT EXISTS (SELECT 1 FROM "Tickets" WHERE "Title" = '[SEED] Monitor parpadeando constantemente') THEN
        INSERT INTO "Tickets" (
            "Id", "Title", "Description", "Category", "Priority", "Status",
            "Location", "AffectedType", "ProblemType", "AffectedParts",
            "CreatedByUserId", "AssignedToUserId",
            "CreatedAt", "ResolvedAt", "SlaDeadline",
            "TechnicianNotes", "Rating", "FeedbackComment"
        )
        VALUES (
            gen_random_uuid(),
            '[SEED] Monitor parpadeando constantemente',
            'El monitor de la estación 22 parpadea constantemente, causando molestias visuales.',
            'hardware',
            'medium',
            'Resuelto',
            'Lab 4',
            'classroom',
            'hardware',
            'Monitor',
            student3_id,
            tech3_id,
            NOW() - INTERVAL '2 days',
            NOW() - INTERVAL '1 day',
            NOW() - INTERVAL '1 day 12 hours',
            'Se ajustó la frecuencia de refresco del monitor de 50Hz a 60Hz. Problema solucionado.',
            4,
            'Bien resuelto, gracias.'
        );
        RAISE NOTICE '✓ Ticket 6 creado: Monitor parpadeando (Resuelto con rating 4)';
    END IF;

    -- ==================== TICKET 7: Licencia AutoCAD vencida (RESUELTO) ====================
    IF NOT EXISTS (SELECT 1 FROM "Tickets" WHERE "Title" = '[SEED] Licencia de AutoCAD vencida') THEN
        INSERT INTO "Tickets" (
            "Id", "Title", "Description", "Category", "Priority", "Status",
            "Location", "AffectedType", "ProblemType", "ProgramName",
            "CreatedByUserId", "AssignedToUserId",
            "CreatedAt", "ResolvedAt", "SlaDeadline",
            "TechnicianNotes", "Rating"
        )
        VALUES (
            gen_random_uuid(),
            '[SEED] Licencia de AutoCAD vencida',
            'AutoCAD 2024 muestra mensaje de licencia vencida en todas las PCs del laboratorio.',
            'software',
            'high',
            'Resuelto',
            'Lab 3',
            'classroom',
            'software',
            'AutoCAD 2024',
            student1_id,
            tech1_id,
            NOW() - INTERVAL '3 days',
            NOW() - INTERVAL '2 days',
            NOW() - INTERVAL '2 days 18 hours',
            'Se renovó la licencia educativa de AutoCAD con Autodesk. Se actualizaron todas las estaciones del laboratorio.',
            5
        );
        RAISE NOTICE '✓ Ticket 7 creado: Licencia AutoCAD (Resuelto con rating 5)';
    END IF;

    -- ==================== TICKET 8: Proyector no enciende (EN PROCESO) ====================
    IF NOT EXISTS (SELECT 1 FROM "Tickets" WHERE "Title" = '[SEED] Proyector del auditorio no enciende') THEN
        INSERT INTO "Tickets" (
            "Id", "Title", "Description", "Category", "Priority", "Status",
            "Location", "AffectedType", "ProblemType", "AffectedParts",
            "CreatedByUserId", "AssignedToUserId",
            "CreatedAt", "SlaDeadline", "TechnicianNotes"
        )
        VALUES (
            gen_random_uuid(),
            '[SEED] Proyector del auditorio no enciende',
            'El proyector del auditorio principal no responde al control remoto ni al botón de encendido físico.',
            'hardware',
            'medium',
            'En proceso',
            'Auditorio',
            'auditorium',
            'hardware',
            'Proyector',
            admin_id,
            tech2_id,
            NOW() - INTERVAL '4 hours',
            NOW() + INTERVAL '12 hours',
            'Se está verificando la fuente de poder del proyector. Posible reemplazo necesario si la fuente está dañada.'
        );
        RAISE NOTICE '✓ Ticket 8 creado: Proyector no enciende (En proceso)';
    END IF;

    -- ============================================================
    -- RESUMEN FINAL
    -- ============================================================
    RAISE NOTICE '';
    RAISE NOTICE '========================================';
    RAISE NOTICE '✅ DATOS DE PRUEBA CARGADOS EXITOSAMENTE';
    RAISE NOTICE '========================================';
    RAISE NOTICE '';
    RAISE NOTICE '📊 RESUMEN:';
    RAISE NOTICE '  • 1 Departamento: Soporte Técnico';
    RAISE NOTICE '  • 7 Usuarios: 1 Admin + 3 Técnicos + 3 Estudiantes';
    RAISE NOTICE '  • 8 Tickets de prueba';
    RAISE NOTICE '';
    RAISE NOTICE '🔐 CREDENCIALES DE ACCESO:';
    RAISE NOTICE '';
    RAISE NOTICE '👨‍💼 ADMIN:';
    RAISE NOTICE '   Email: admin@universidad.edu';
    RAISE NOTICE '   Password: Admin123!';
    RAISE NOTICE '';
    RAISE NOTICE '👨‍🔧 TÉCNICOS:';
    RAISE NOTICE '   Email: carlos.mendez@universidad.edu / Tech123!';
    RAISE NOTICE '   Email: ana.torres@universidad.edu / Tech123!';
    RAISE NOTICE '   Email: luis.garcia@universidad.edu / Tech123!';
    RAISE NOTICE '';
    RAISE NOTICE '🎓 ESTUDIANTES:';
    RAISE NOTICE '   Email: juan.perez@universidad.edu / Student123!';
    RAISE NOTICE '   Email: maria.lopez@universidad.edu / Student123!';
    RAISE NOTICE '   Email: pedro.gonzalez@universidad.edu / Student123!';
    RAISE NOTICE '';
    RAISE NOTICE '🎫 TICKETS CREADOS:';
    RAISE NOTICE '   1. Router sin respuesta (Crítico, Lab 1)';
    RAISE NOTICE '   2. Monitor sin señal (Resuelto ⭐5, Lab 3)';
    RAISE NOTICE '   3. Error Visual Studio (Alto, Lab 2)';
    RAISE NOTICE '   4. Teclado no responde (Bajo, Lab 1)';
    RAISE NOTICE '   5. WiFi no disponible (Crítico, Biblioteca)';
    RAISE NOTICE '   6. Monitor parpadeando (Resuelto ⭐4, Lab 4)';
    RAISE NOTICE '   7. Licencia AutoCAD (Resuelto ⭐5, Lab 3)';
    RAISE NOTICE '   8. Proyector no enciende (Medio, Auditorio)';
    RAISE NOTICE '';
    RAISE NOTICE '========================================';

END $$;
