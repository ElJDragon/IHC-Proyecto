-- ============================================================
-- Script de Datos de Prueba: Tickets y Usuarios
-- Fecha: 2025-11-30
-- Descripción: Inserta datos de ejemplo para testing del UI
-- ============================================================

-- Declarar variables para los IDs
DECLARE @AdminId uniqueidentifier = NEWID();
DECLARE @Tech1Id uniqueidentifier = NEWID();
DECLARE @Tech2Id uniqueidentifier = NEWID();
DECLARE @Tech3Id uniqueidentifier = NEWID();
DECLARE @Student1Id uniqueidentifier = NEWID();
DECLARE @Student2Id uniqueidentifier = NEWID();
DECLARE @Student3Id uniqueidentifier = NEWID();
DECLARE @DeptId uniqueidentifier = NEWID();

-- ==================== Departamento ====================
IF NOT EXISTS (SELECT 1 FROM Departments WHERE Name = 'Soporte Técnico')
BEGIN
    INSERT INTO Departments (Id, Name) VALUES (@DeptId, 'Soporte Técnico');
    PRINT 'Departamento creado';
END
ELSE
BEGIN
    SELECT @DeptId = Id FROM Departments WHERE Name = 'Soporte Técnico';
    PRINT 'Departamento ya existe';
END

-- ==================== Usuarios ====================
-- Admin
IF NOT EXISTS (SELECT 1 FROM Users WHERE Email = 'admin@universidad.edu')
BEGIN
    INSERT INTO Users (Id, Email, Password, FullName, DepartmentId, Role, Workload)
    VALUES (@AdminId, 'admin@universidad.edu', 'Admin123!', 'Carlos Rodríguez', @DeptId, 'Admin', 0);
    PRINT 'Admin creado';
END
ELSE
BEGIN
    SELECT @AdminId = Id FROM Users WHERE Email = 'admin@universidad.edu';
END

-- Técnicos
IF NOT EXISTS (SELECT 1 FROM Users WHERE Email = 'carlos.mendez@universidad.edu')
BEGIN
    INSERT INTO Users (Id, Email, Password, FullName, DepartmentId, Role, Workload)
    VALUES (@Tech1Id, 'carlos.mendez@universidad.edu', 'Tech123!', 'Carlos Méndez', @DeptId, 'Technician', 0);
    PRINT 'Técnico 1 creado';
END
ELSE
BEGIN
    SELECT @Tech1Id = Id FROM Users WHERE Email = 'carlos.mendez@universidad.edu';
END

IF NOT EXISTS (SELECT 1 FROM Users WHERE Email = 'ana.torres@universidad.edu')
BEGIN
    INSERT INTO Users (Id, Email, Password, FullName, DepartmentId, Role, Workload)
    VALUES (@Tech2Id, 'ana.torres@universidad.edu', 'Tech123!', 'Ana Torres', @DeptId, 'Technician', 0);
    PRINT 'Técnico 2 creado';
END
ELSE
BEGIN
    SELECT @Tech2Id = Id FROM Users WHERE Email = 'ana.torres@universidad.edu';
END

IF NOT EXISTS (SELECT 1 FROM Users WHERE Email = 'luis.garcia@universidad.edu')
BEGIN
    INSERT INTO Users (Id, Email, Password, FullName, DepartmentId, Role, Workload)
    VALUES (@Tech3Id, 'luis.garcia@universidad.edu', 'Tech123!', 'Luis García', @DeptId, 'Technician', 0);
    PRINT 'Técnico 3 creado';
END
ELSE
BEGIN
    SELECT @Tech3Id = Id FROM Users WHERE Email = 'luis.garcia@universidad.edu';
END

-- Estudiantes
IF NOT EXISTS (SELECT 1 FROM Users WHERE Email = 'juan.perez@universidad.edu')
BEGIN
    INSERT INTO Users (Id, Email, Password, FullName, DepartmentId, Role, Workload)
    VALUES (@Student1Id, 'juan.perez@universidad.edu', 'Student123!', 'Juan Pérez', @DeptId, 'Student', 0);
    PRINT 'Estudiante 1 creado';
END
ELSE
BEGIN
    SELECT @Student1Id = Id FROM Users WHERE Email = 'juan.perez@universidad.edu';
END

IF NOT EXISTS (SELECT 1 FROM Users WHERE Email = 'maria.lopez@universidad.edu')
BEGIN
    INSERT INTO Users (Id, Email, Password, FullName, DepartmentId, Role, Workload)
    VALUES (@Student2Id, 'maria.lopez@universidad.edu', 'Student123!', 'María López', @DeptId, 'Student', 0);
    PRINT 'Estudiante 2 creado';
END
ELSE
BEGIN
    SELECT @Student2Id = Id FROM Users WHERE Email = 'maria.lopez@universidad.edu';
END

IF NOT EXISTS (SELECT 1 FROM Users WHERE Email = 'pedro.gonzalez@universidad.edu')
BEGIN
    INSERT INTO Users (Id, Email, Password, FullName, DepartmentId, Role, Workload)
    VALUES (@Student3Id, 'pedro.gonzalez@universidad.edu', 'Student123!', 'Pedro González', @DeptId, 'Student', 0);
    PRINT 'Estudiante 3 creado';
END
ELSE
BEGIN
    SELECT @Student3Id = Id FROM Users WHERE Email = 'pedro.gonzalez@universidad.edu';
END

-- ==================== Tickets de Ejemplo ====================
-- Eliminar tickets de prueba anteriores
DELETE FROM Tickets WHERE Title LIKE '%[SEED]%';

-- Ticket 1: Crítico - En proceso
INSERT INTO Tickets (
    Id, Title, Description, Category, Priority, Status,
    Location, AffectedType, LocationDetail,
    CreatedByUserId, AssignedToUserId,
    CreatedAt, SlaDeadline
)
VALUES (
    NEWID(),
    '[SEED] Router sin respuesta',
    'El router principal del laboratorio no responde a pings. La red completa está caída.',
    'connectivity',
    'critical',
    'En proceso',
    'Lab 1',
    'classroom',
    'Edificio A - Piso 3',
    @Student1Id,
    @Tech1Id,
    DATEADD(HOUR, -2, GETUTCDATE()),
    DATEADD(HOUR, 2, DATEADD(HOUR, -2, GETUTCDATE()))
);

-- Ticket 2: Media - Resuelto
INSERT INTO Tickets (
    Id, Title, Description, Category, Priority, Status,
    Location, AffectedType, LocationDetail,
    ProblemType, AffectedParts,
    CreatedByUserId, AssignedToUserId,
    CreatedAt, ResolvedAt, SlaDeadline,
    TechnicianNotes, Rating
)
VALUES (
    NEWID(),
    '[SEED] Pantalla no enciende',
    'Monitor del equipo 15 no muestra imagen',
    'hardware',
    'medium',
    'Resuelto',
    'Lab 3',
    'classroom',
    'Equipo-015',
    'hardware',
    'Monitor',
    @Student2Id,
    @Tech2Id,
    DATEADD(HOUR, -5, GETUTCDATE()),
    DATEADD(HOUR, -2, GETUTCDATE()),
    DATEADD(HOUR, 12, DATEADD(HOUR, -5, GETUTCDATE())),
    'Cable VGA desconectado. Problema resuelto reconectando el cable.',
    5
);

-- Ticket 3: Alta - Pendiente
INSERT INTO Tickets (
    Id, Title, Description, Category, Priority, Status,
    Location, AffectedType, LocationDetail,
    ProblemType, ProgramName, ErrorMessage,
    CreatedByUserId, AssignedToUserId,
    CreatedAt, SlaDeadline
)
VALUES (
    NEWID(),
    '[SEED] Software no inicia',
    'Visual Studio no arranca en múltiples equipos',
    'software',
    'high',
    'Pendiente',
    'Lab 2',
    'classroom',
    'Equipos 10-20',
    'software',
    'Visual Studio 2022',
    'Error: The application failed to initialize properly (0xc0000142)',
    @Student3Id,
    @Tech3Id,
    DATEADD(HOUR, -8, GETUTCDATE()),
    DATEADD(HOUR, 6, DATEADD(HOUR, -8, GETUTCDATE()))
);

-- Ticket 4: Baja - En proceso
INSERT INTO Tickets (
    Id, Title, Description, Category, Priority, Status,
    Location, AffectedType, LocationDetail,
    ProblemType, AffectedParts,
    CreatedByUserId, AssignedToUserId,
    CreatedAt, SlaDeadline,
    TechnicianNotes
)
VALUES (
    NEWID(),
    '[SEED] Teclado defectuoso',
    'Varias teclas del teclado no responden',
    'hardware',
    'low',
    'En proceso',
    'Lab 1',
    'user',
    'Equipo-008',
    'hardware',
    'Teclado',
    @Student1Id,
    @Tech1Id,
    DATEADD(HOUR, -15, GETUTCDATE()),
    DATEADD(HOUR, 24, DATEADD(HOUR, -15, GETUTCDATE())),
    'Teclado de reemplazo solicitado al almacén. Pendiente de llegada.'
);

-- Ticket 5: Crítico - Pendiente
INSERT INTO Tickets (
    Id, Title, Description, Category, Priority, Status,
    Location, AffectedType, LocationDetail,
    CreatedByUserId, AssignedToUserId,
    CreatedAt, SlaDeadline
)
VALUES (
    NEWID(),
    '[SEED] WiFi intermitente',
    'La señal WiFi se cae cada 5-10 minutos en toda la biblioteca',
    'connectivity',
    'critical',
    'Pendiente',
    'Biblioteca',
    'building',
    'Edificio Central',
    @Student2Id,
    @Tech2Id,
    DATEADD(HOUR, -1, GETUTCDATE()),
    DATEADD(HOUR, 2, DATEADD(HOUR, -1, GETUTCDATE()))
);

-- Ticket 6: Media - Resuelto
INSERT INTO Tickets (
    Id, Title, Description, Category, Priority, Status,
    Location, AffectedType, LocationDetail,
    ProblemType, AffectedParts,
    CreatedByUserId, AssignedToUserId,
    CreatedAt, ResolvedAt, SlaDeadline,
    TechnicianNotes, Rating
)
VALUES (
    NEWID(),
    '[SEED] Monitor parpadeante',
    'El monitor parpadea constantemente',
    'hardware',
    'medium',
    'Resuelto',
    'Lab 4',
    'user',
    'Equipo-022',
    'hardware',
    'Monitor',
    @Student3Id,
    @Tech3Id,
    DATEADD(HOUR, -20, GETUTCDATE()),
    DATEADD(HOUR, -16, GETUTCDATE()),
    DATEADD(HOUR, 12, DATEADD(HOUR, -20, GETUTCDATE())),
    'Frecuencia de refresco incorrecta. Ajustada a 60Hz. Problema resuelto.',
    4
);

-- Ticket 7: Alta - Resuelto
INSERT INTO Tickets (
    Id, Title, Description, Category, Priority, Status,
    Location, AffectedType, LocationDetail,
    ProblemType, ProgramName, ErrorMessage,
    CreatedByUserId, AssignedToUserId,
    CreatedAt, ResolvedAt, SlaDeadline,
    TechnicianNotes, Rating
)
VALUES (
    NEWID(),
    '[SEED] Licencia expirada',
    'AutoCAD muestra error de licencia expirada',
    'software',
    'high',
    'Resuelto',
    'Lab 3',
    'classroom',
    'Todos los equipos',
    'software',
    'AutoCAD 2024',
    'Error: License expired',
    @Student1Id,
    @Tech1Id,
    DATEADD(HOUR, -30, GETUTCDATE()),
    DATEADD(HOUR, -28, GETUTCDATE()),
    DATEADD(HOUR, 6, DATEADD(HOUR, -30, GETUTCDATE())),
    'Licencia corporativa renovada. Todas las estaciones actualizadas.',
    5
);

-- Ticket 8: Media - En proceso
INSERT INTO Tickets (
    Id, Title, Description, Category, Priority, Status,
    Location, AffectedType, LocationDetail,
    ProblemType, AffectedParts,
    CreatedByUserId, AssignedToUserId,
    CreatedAt, SlaDeadline,
    TechnicianNotes
)
VALUES (
    NEWID(),
    '[SEED] Proyector no funciona',
    'El proyector del auditorio no enciende',
    'hardware',
    'medium',
    'En proceso',
    'Auditorio',
    'building',
    'Auditorio Principal',
    'hardware',
    'Otro',
    @Student2Id,
    @Tech2Id,
    DATEADD(HOUR, -12, GETUTCDATE()),
    DATEADD(HOUR, 12, DATEADD(HOUR, -12, GETUTCDATE())),
    'Lámpara fundida. Reemplazo solicitado. ETA: 24 horas.'
);

PRINT 'Se insertaron 8 tickets de ejemplo';
PRINT '===========================================';
PRINT 'Datos de prueba insertados correctamente';
PRINT 'Credenciales de prueba:';
PRINT 'Admin: admin@universidad.edu / Admin123!';
PRINT 'Técnico 1: carlos.mendez@universidad.edu / Tech123!';
PRINT 'Técnico 2: ana.torres@universidad.edu / Tech123!';
PRINT 'Técnico 3: luis.garcia@universidad.edu / Tech123!';
PRINT 'Estudiante: juan.perez@universidad.edu / Student123!';
PRINT '===========================================';
