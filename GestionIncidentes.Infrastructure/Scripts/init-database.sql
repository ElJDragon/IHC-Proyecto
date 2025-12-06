-- =========================
-- Script de Base de Datos para Gestión de Incidentes
-- PostgreSQL
-- =========================

-- =========================
-- Tabla: Departments
-- =========================
CREATE TABLE IF NOT EXISTS "Departments" (
    "Id" uuid PRIMARY KEY,
    "Name" text NOT NULL
);

-- =========================
-- Tabla: Roles
-- =========================
CREATE TABLE IF NOT EXISTS "Roles" (
    "Id" uuid PRIMARY KEY,
    "Name" text NOT NULL,
    "Level" integer NOT NULL,
    CONSTRAINT roles_name_unique UNIQUE ("Name")
);

-- =========================
-- Tabla: Users
-- =========================
CREATE TABLE IF NOT EXISTS "Users" (
    "Id" uuid PRIMARY KEY,
    "Email" text NOT NULL,
    "FullName" text NOT NULL,
    "DepartmentId" uuid NOT NULL,
    "Workload" integer NOT NULL DEFAULT 0,
    "Role" text NOT NULL,
    "Password" text NOT NULL DEFAULT ''::text,
    CONSTRAINT users_department_fk FOREIGN KEY ("DepartmentId")
        REFERENCES "Departments" ("Id")
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
    CONSTRAINT users_role_fk FOREIGN KEY ("Role")
        REFERENCES "Roles" ("Name")
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);

-- Índice para búsqueda por email
CREATE UNIQUE INDEX IF NOT EXISTS users_email_idx ON "Users" ("Email");

-- =========================
-- Tabla: Incidents (Integrante A)
-- =========================
CREATE TABLE IF NOT EXISTS "Incidents" (
    "Id" uuid PRIMARY KEY,
    "Title" text NOT NULL,
    "Description" text NOT NULL,
    "ReportedByUserId" uuid NOT NULL,
    "ReportedAt" timestamptz NOT NULL DEFAULT now(),
    "Status" text NOT NULL DEFAULT 'Reported',
    "AssignedTicketId" uuid NULL,
    CONSTRAINT incidents_reportedby_fk FOREIGN KEY ("ReportedByUserId")
        REFERENCES "Users" ("Id")
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);

-- Índice para búsquedas por estado de incidente
CREATE INDEX IF NOT EXISTS incidents_status_idx ON "Incidents" ("Status");
CREATE INDEX IF NOT EXISTS incidents_reportedby_idx ON "Incidents" ("ReportedByUserId");

-- =========================
-- Tabla: Tickets
-- =========================
CREATE TABLE IF NOT EXISTS "Tickets" (
    "Id" uuid PRIMARY KEY,
    "Title" text NOT NULL,
    "Description" text NOT NULL,
    "CreatedByUserId" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "CreatedAt" timestamptz NOT NULL DEFAULT now(),
    "Status" text NOT NULL DEFAULT 'Open',
    CONSTRAINT tickets_createdby_fk FOREIGN KEY ("CreatedByUserId")
        REFERENCES "Users" ("Id")
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
    CONSTRAINT tickets_user_fk FOREIGN KEY ("UserId")
        REFERENCES "Users" ("Id")
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);

-- Índice para búsquedas frecuentes en tickets
CREATE INDEX IF NOT EXISTS tickets_status_idx ON "Tickets" ("Status");
CREATE INDEX IF NOT EXISTS tickets_user_idx ON "Tickets" ("UserId");

-- =========================
-- Tabla: KnowledgeEntries (Integrante C)
-- =========================
CREATE TABLE IF NOT EXISTS "KnowledgeEntries" (
    "Id" uuid PRIMARY KEY,
    "Title" text NOT NULL,
    "Problem" text NOT NULL,
    "Solution" text NOT NULL,
    "Category" text NOT NULL,
    "CreatedByUserId" uuid NOT NULL,
    "CreatedAt" timestamptz NOT NULL DEFAULT now(),
    CONSTRAINT knowledge_createdby_fk FOREIGN KEY ("CreatedByUserId")
        REFERENCES "Users" ("Id")
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);

CREATE INDEX IF NOT EXISTS knowledge_category_idx ON "KnowledgeEntries" ("Category");

-- =========================
-- Tabla: Notifications (Integrante C)
-- =========================
CREATE TABLE IF NOT EXISTS "Notifications" (
    "Id" uuid PRIMARY KEY,
    "UserId" uuid NOT NULL,
    "Title" text NOT NULL,
    "Message" text NOT NULL,
    "Type" text NOT NULL,
    "IsRead" boolean NOT NULL DEFAULT false,
    "CreatedAt" timestamptz NOT NULL DEFAULT now(),
    CONSTRAINT notifications_user_fk FOREIGN KEY ("UserId")
        REFERENCES "Users" ("Id")
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS notifications_user_read_idx ON "Notifications" ("UserId", "IsRead");

-- =========================
-- Tabla: AuditLogs (Integrante C)
-- =========================
CREATE TABLE IF NOT EXISTS "AuditLogs" (
    "Id" uuid PRIMARY KEY,
    "UserId" uuid NOT NULL,
    "Action" text NOT NULL,
    "EntityType" text NOT NULL,
    "EntityId" uuid NULL,
    "Details" text NULL,
    "Timestamp" timestamptz NOT NULL DEFAULT now(),
    CONSTRAINT auditlogs_user_fk FOREIGN KEY ("UserId")
        REFERENCES "Users" ("Id")
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);

CREATE INDEX IF NOT EXISTS auditlogs_user_timestamp_idx ON "AuditLogs" ("UserId", "Timestamp");
CREATE INDEX IF NOT EXISTS auditlogs_entityid_idx ON "AuditLogs" ("EntityId");

-- =========================
-- Tabla: TicketReports (Integrante C)
-- =========================
CREATE TABLE IF NOT EXISTS "TicketReports" (
    "Id" uuid PRIMARY KEY,
    "TicketId" uuid NOT NULL,
    "TechnicianId" uuid NOT NULL,
    "ResolutionDetails" text NOT NULL,
    "GeneratedAt" timestamptz NOT NULL DEFAULT now(),
    CONSTRAINT ticketreports_ticket_fk FOREIGN KEY ("TicketId")
        REFERENCES "Tickets" ("Id")
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
    CONSTRAINT ticketreports_technician_fk FOREIGN KEY ("TechnicianId")
        REFERENCES "Users" ("Id")
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);

-- =========================
-- Tabla: __EFMigrationsHistory
-- =========================
CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) PRIMARY KEY,
    "ProductVersion" character varying(32) NOT NULL
);

-- =========================
-- Datos de Ejemplo (Opcional)
-- =========================

-- Insertar Departamentos
INSERT INTO "Departments" ("Id", "Name") VALUES 
    ('11111111-1111-1111-1111-111111111111', 'TI'),
    ('22222222-2222-2222-2222-222222222222', 'Soporte')
ON CONFLICT ("Id") DO NOTHING;

-- Insertar Roles
INSERT INTO "Roles" ("Id", "Name", "Level") VALUES 
    ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'Admin', 1),
    ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 'Tecnico', 2),
    ('cccccccc-cccc-cccc-cccc-cccccccccccc', 'Usuario', 3)
ON CONFLICT ("Name") DO NOTHING;

-- Insertar Usuarios de ejemplo
INSERT INTO "Users" ("Id", "Email", "FullName", "DepartmentId", "Workload", "Role", "Password") VALUES 
    ('00000000-0000-0000-0000-000000000001', 'admin@empresa.com', 'Administrador Sistema', '11111111-1111-1111-1111-111111111111', 0, 'Admin', 'password123'),
    ('00000000-0000-0000-0000-000000000002', 'tecnico@empresa.com', 'Técnico Soporte', '22222222-2222-2222-2222-222222222222', 2, 'Tecnico', 'password123'),
    ('00000000-0000-0000-0000-000000000003', 'usuario@empresa.com', 'Usuario Final', '11111111-1111-1111-1111-111111111111', 0, 'Usuario', 'password123')
ON CONFLICT ("Id") DO NOTHING;

COMMIT;
