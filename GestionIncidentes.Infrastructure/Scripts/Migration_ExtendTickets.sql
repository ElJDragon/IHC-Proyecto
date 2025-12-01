-- ============================================================
-- Script de Migración: Extender Tabla Tickets
-- Fecha: 2025-11-30
-- Descripción: Agrega campos para categorización, prioridad,
--              ubicación, SLA, resolución y valoración
-- ============================================================

-- Agregar nuevos campos a la tabla Tickets
ALTER TABLE Tickets ADD AssignedToUserId uniqueidentifier NULL;
ALTER TABLE Tickets ADD Category nvarchar(50) NOT NULL DEFAULT 'general';
ALTER TABLE Tickets ADD Priority nvarchar(20) NOT NULL DEFAULT 'medium';
ALTER TABLE Tickets ADD Location nvarchar(100) NOT NULL DEFAULT '';
ALTER TABLE Tickets ADD AffectedType nvarchar(50) NOT NULL DEFAULT '';
ALTER TABLE Tickets ADD LocationDetail nvarchar(200) NULL;
ALTER TABLE Tickets ADD ProblemType nvarchar(20) NULL;
ALTER TABLE Tickets ADD EquipmentId nvarchar(50) NULL;
ALTER TABLE Tickets ADD ProgramName nvarchar(100) NULL;
ALTER TABLE Tickets ADD ErrorMessage nvarchar(max) NULL;
ALTER TABLE Tickets ADD AffectedParts nvarchar(200) NULL;
ALTER TABLE Tickets ADD TechnicianNotes nvarchar(max) NULL;
ALTER TABLE Tickets ADD ResolvedAt datetime2 NULL;
ALTER TABLE Tickets ADD SlaDeadline datetime2 NULL;
ALTER TABLE Tickets ADD Rating int NULL;
ALTER TABLE Tickets ADD FeedbackComment nvarchar(500) NULL;

-- Renombrar campos obsoletos si existen
IF EXISTS (SELECT * FROM sys.columns WHERE Name = 'UserId' AND Object_ID = Object_ID('Tickets'))
BEGIN
    EXEC sp_rename 'Tickets.UserId', 'CreatedByUserId_OLD', 'COLUMN';
END

-- Actualizar Status values existentes
UPDATE Tickets SET Status = 'Pendiente' WHERE Status = 'Open';
UPDATE Tickets SET Status = 'Resuelto' WHERE Status = 'Closed';

-- Agregar índices para mejorar performance
CREATE INDEX IX_Tickets_AssignedToUserId ON Tickets(AssignedToUserId);
CREATE INDEX IX_Tickets_Status ON Tickets(Status);
CREATE INDEX IX_Tickets_Priority ON Tickets(Priority);
CREATE INDEX IX_Tickets_Location ON Tickets(Location);
CREATE INDEX IX_Tickets_CreatedAt ON Tickets(CreatedAt);

-- Agregar constraint de foreign key para AssignedToUserId
ALTER TABLE Tickets 
ADD CONSTRAINT FK_Tickets_Users_AssignedTo 
FOREIGN KEY (AssignedToUserId) REFERENCES Users(Id);

PRINT 'Migración completada exitosamente';
