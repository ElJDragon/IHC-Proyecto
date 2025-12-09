-- Script para insertar tickets de ejemplo en la base de datos

-- Primero obtener los IDs de usuarios existentes
DO $$
DECLARE
    admin_id UUID;
    tech1_id UUID;
    tech2_id UUID;
    user1_id UUID;
BEGIN
    -- Obtener IDs de usuarios
    SELECT "Id" INTO admin_id FROM "Users" WHERE "Email" = 'admin@universidad.edu' LIMIT 1;
    SELECT "Id" INTO tech1_id FROM "Users" WHERE "Role" = 'Tecnico' LIMIT 1;
    SELECT "Id" INTO tech2_id FROM "Users" WHERE "Role" = 'Tecnico' OFFSET 1 LIMIT 1;
    SELECT "Id" INTO user1_id FROM "Users" WHERE "Role" = 'Usuario' LIMIT 1;

    -- Insertar tickets de ejemplo
    -- Ticket 1: Internet no funciona en laboratorio (Resuelto)
    INSERT INTO "Tickets" ("Id", "Title", "Description", "Status", "Priority", "Category", "CreatedAt", "CreatedByUserId", "AssignedToUserId", "SlaDeadline", "Location", "LocationDetail", "ProblemType", "ResolvedAt", "Rating")
    VALUES (gen_random_uuid(), 'Internet no funciona en Laboratorio 301', 'Los estudiantes no pueden acceder a internet en el laboratorio de computación 301', 'Resuelto', 'Alta', 'Conectividad', NOW() - INTERVAL '5 days', user1_id, tech1_id, NOW() - INTERVAL '4 days', 'Pabellón A', 'Laboratorio 301', 'hardware', NOW() - INTERVAL '4 days 2 hours', 4.5);

    -- Ticket 2: Impresora no imprime (Resuelto)
    INSERT INTO "Tickets" ("Id", "Title", "Description", "Status", "Priority", "Category", "CreatedAt", "CreatedByUserId", "AssignedToUserId", "SlaDeadline", "Location", "LocationDetail", "ProblemType", "ResolvedAt", "Rating")
    VALUES (gen_random_uuid(), 'Impresora HP LaserJet no imprime', 'La impresora del área administrativa no responde', 'Resuelto', 'Media', 'Hardware', NOW() - INTERVAL '7 days', admin_id, tech2_id, NOW() - INTERVAL '6 days', 'Administración', 'Oficina 105', 'hardware', NOW() - INTERVAL '6 days 3 hours', 5.0);

    -- Ticket 3: Proyector sin señal (En Proceso)
    INSERT INTO "Tickets" ("Id", "Title", "Description", "Status", "Priority", "Category", "CreatedAt", "CreatedByUserId", "AssignedToUserId", "SlaDeadline", "Location", "LocationDetail", "ProblemType")
    VALUES (gen_random_uuid(), 'Proyector no muestra señal en aula 202', 'El proyector del aula 202 enciende pero no muestra imagen', 'En Proceso', 'Alta', 'Hardware', NOW() - INTERVAL '2 days', user1_id, tech1_id, NOW() + INTERVAL '1 day', 'Pabellón B', 'Aula 202', 'hardware');

    -- Ticket 4: Software de diseño no inicia (Pendiente)
    INSERT INTO "Tickets" ("Id", "Title", "Description", "Status", "Priority", "Category", "CreatedAt", "CreatedByUserId", "SlaDeadline", "Location", "LocationDetail", "ProblemType", "ProgramName")
    VALUES (gen_random_uuid(), 'AutoCAD 2024 no inicia correctamente', 'Al intentar abrir AutoCAD aparece un error de licencia', 'Pendiente', 'Media', 'Software', NOW() - INTERVAL '1 day', user1_id, NOW() + INTERVAL '2 days', 'Laboratorio de Arquitectura', 'Lab Diseño', 'software', 'AutoCAD 2024');

    -- Ticket 5: Computadora lenta (Resuelto)
    INSERT INTO "Tickets" ("Id", "Title", "Description", "Status", "Priority", "Category", "CreatedAt", "CreatedByUserId", "AssignedToUserId", "SlaDeadline", "Location", "LocationDetail", "ProblemType", "ResolvedAt", "Rating")
    VALUES (gen_random_uuid(), 'Computadora PC-15 muy lenta', 'La computadora tarda mucho en iniciar y abrir programas', 'Resuelto', 'Baja', 'Hardware', NOW() - INTERVAL '10 days', user1_id, tech2_id, NOW() - INTERVAL '8 days', 'Sala de Profesores', 'Pabellón C', 'hardware', NOW() - INTERVAL '8 days 1 hour', 4.0);

    -- Ticket 6: Error en plataforma educativa (En Proceso)
    INSERT INTO "Tickets" ("Id", "Title", "Description", "Status", "Priority", "Category", "CreatedAt", "CreatedByUserId", "AssignedToUserId", "SlaDeadline", "Location", "LocationDetail", "ProblemType", "ProgramName", "ErrorMessage")
    VALUES (gen_random_uuid(), 'No puedo subir archivos a Moodle', 'Al intentar subir tareas aparece error de tamaño de archivo', 'En Proceso', 'Media', 'Software', NOW() - INTERVAL '3 days', user1_id, tech1_id, NOW() + INTERVAL '1 day', 'Virtual', 'Plataforma Moodle', 'software', 'Moodle', 'File size exceeds maximum');

    -- Ticket 7: Teclado no funciona (Pendiente)
    INSERT INTO "Tickets" ("Id", "Title", "Description", "Status", "Priority", "Category", "CreatedAt", "CreatedByUserId", "SlaDeadline", "Location", "LocationDetail", "ProblemType")
    VALUES (gen_random_uuid(), 'Teclado inalámbrico no responde', 'El teclado de la estación 08 no funciona', 'Pendiente', 'Media', 'Hardware', NOW() - INTERVAL '1 hour', user1_id, NOW() + INTERVAL '3 days', 'Laboratorio 402', 'Estación 08', 'hardware');

    -- Ticket 8: Wi-Fi intermitente (Resuelto)
    INSERT INTO "Tickets" ("Id", "Title", "Description", "Status", "Priority", "Category", "CreatedAt", "CreatedByUserId", "AssignedToUserId", "SlaDeadline", "Location", "LocationDetail", "ProblemType", "ResolvedAt", "Rating")
    VALUES (gen_random_uuid(), 'Señal WiFi intermitente en biblioteca', 'La conexión WiFi se cae cada 10 minutos', 'Resuelto', 'Alta', 'Conectividad', NOW() - INTERVAL '6 days', admin_id, tech2_id, NOW() - INTERVAL '5 days', 'Biblioteca Central', 'Planta Alta', 'hardware', NOW() - INTERVAL '5 days 4 hours', 4.8);

    -- Ticket 9: Excel no guarda archivos (En Proceso)
    INSERT INTO "Tickets" ("Id", "Title", "Description", "Status", "Priority", "Category", "CreatedAt", "CreatedByUserId", "AssignedToUserId", "SlaDeadline", "Location", "LocationDetail", "ProblemType", "ProgramName", "ErrorMessage")
    VALUES (gen_random_uuid(), 'Microsoft Excel no permite guardar', 'Al intentar guardar documentos en Excel aparece error', 'En Proceso', 'Alta', 'Software', NOW() - INTERVAL '4 hours', user1_id, tech1_id, NOW() + INTERVAL '2 days', 'Oficinas Administrativas', 'Finanzas', 'software', 'Microsoft Excel', 'Permission denied');

    -- Ticket 10: Pantalla parpadeante (Resuelto)
    INSERT INTO "Tickets" ("Id", "Title", "Description", "Status", "Priority", "Category", "CreatedAt", "CreatedByUserId", "AssignedToUserId", "SlaDeadline", "Location", "LocationDetail", "ProblemType", "ResolvedAt", "Rating")
    VALUES (gen_random_uuid(), 'Monitor parpadea constantemente', 'El monitor de la PC-23 parpadea y se ve mal', 'Resuelto', 'Media', 'Hardware', NOW() - INTERVAL '8 days', user1_id, tech2_id, NOW() - INTERVAL '7 days', 'Laboratorio Multimedia', 'PC-23', 'hardware', NOW() - INTERVAL '6 days 5 hours', 5.0);

END $$;

-- Verificar los tickets insertados
SELECT 'Tickets insertados correctamente' as message;
SELECT COUNT(*) as total_tickets FROM "Tickets";
