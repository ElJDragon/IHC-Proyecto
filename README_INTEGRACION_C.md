# Integración C - Base de Conocimiento, Reportes, Notificaciones y Auditoría

Este documento describe la implementación de las funcionalidades de la Integración C del sistema de Gestión de Incidentes.

## Funcionalidades Implementadas

### 1. Base de Conocimiento (Knowledge Database)

La base de conocimiento permite documentar soluciones a problemas comunes para su reutilización.

#### Entidades
- **KnowledgeEntry**: Entrada en la base de conocimiento con título, problema, solución, categoría, tags y contador de uso.

#### Características
- Crear nuevas entradas en la base de conocimiento
- Buscar entradas por término, categoría o tags
- Incrementar contador de uso automáticamente al consultar
- Asociar entradas con tickets resueltos
- Categorización (Hardware, Software, Red, Otro)
- Sistema de etiquetas (tags)

#### Páginas Web
- `/knowledge/create` - Crear nueva entrada en la base de conocimiento
- `/knowledge/search` - Buscar en la base de conocimiento

#### Comandos y Queries
- `AddKnowledgeEntryCommand` - Agregar entrada a la base de conocimiento
- `SearchKnowledgeQuery` - Buscar en la base de conocimiento

---

### 2. Reportes de Tickets

Sistema para generar reportes detallados de resolución de tickets.

#### Entidades
- **TicketReport**: Reporte con diagnóstico, acciones realizadas, tiempo empleado y sugerencia de agregar a KDB.

#### Características
- Generar reportes de tickets resueltos
- Registrar diagnóstico del problema
- Documentar acciones realizadas
- Tiempo empleado en la resolución
- Sugerir agregar a base de conocimiento
- Formato HTML y Markdown para exportar

#### Páginas Web
- `/reports/create/{ticketId}` - Generar reporte para un ticket
- `/reports/viewer` - Ver todos los reportes generados

#### Comandos y Queries
- `GenerateTicketReportQuery` - Generar reporte de ticket

#### Servicios
- `ReportFormatter` - Formatear reportes a HTML/Markdown (preparado para exportar a PDF)

---

### 3. Sistema de Notificaciones

Sistema de notificaciones en tiempo real para usuarios del sistema.

#### Entidades
- **Notification**: Notificación con título, mensaje, tipo, estado de lectura y entidad relacionada.

#### Características
- Notificar eventos importantes a usuarios
- Notificaciones en tiempo real (actualización cada 30 segundos)
- Marcar notificaciones como leídas
- Tipos de notificación:
  - `IncidentReported` - Nuevo incidente reportado (para DITIC/Encargados)
  - `TicketAssigned` - Ticket asignado (para técnico)
  - `TicketResolved` - Ticket resuelto (para creador y técnico)

#### Páginas Web
- `/notifications` - Ver todas las notificaciones

#### Componentes
- `NotificationPanel.razor` - Panel de notificaciones en la barra de navegación

#### Comandos y Queries
- `GetNotificationsQuery` - Obtener notificaciones de un usuario
- `MarkNotificationReadCommand` - Marcar notificación como leída

#### Servicios
- `NotificationService` - Servicio para enviar notificaciones (puede extenderse con SignalR o correo)

---

### 4. Sistema de Auditoría

Sistema para registrar todas las operaciones importantes del sistema.

#### Entidades
- **AuditLog**: Registro de auditoría con usuario, acción, tipo de entidad, ID de entidad, detalles, timestamp e IP.

#### Características
- Registro automático de todas las operaciones importantes
- Pipeline behavior de MediatR para auditar comandos
- Búsqueda por usuario, entidad, rango de fechas
- Registro de IP del usuario

#### Behavior
- `AuditBehavior<TRequest, TResponse>` - Pipeline behavior que audita automáticamente todos los comandos

---

### 5. Event Handlers

Manejadores de eventos del dominio para automatizar procesos.

#### Handlers Implementados
- `OnIncidentReportedHandler` - Notifica a DITIC/Encargados cuando se reporta un incidente
- `OnTicketCreatedHandler` - Notifica al técnico asignado cuando se crea un ticket
- `OnTicketResolvedHandler` - Notifica al creador y técnico, sugiere generar reporte y agregar a KDB

---

## Arquitectura

El proyecto sigue Clean Architecture con las siguientes capas:

### Domain (GestionIncidentes.Domain)
- Entidades: `KnowledgeEntry`, `Notification`, `AuditLog`, `TicketReport`
- Eventos: `IncidentReported`, `TicketCreated`, `TicketResolved`

### Application (GestionIncidentes.Application)
- Features con Commands/Queries y Handlers
- Event Handlers
- Interfaces de repositorios
- Behaviors (AuditBehavior)

### Infrastructure (GestionIncidentes.Infrastructure)
- Repositorios: `KnowledgeRepository`, `NotificationRepository`, `AuditLogRepository`, `TicketReportRepository`
- Servicios: `NotificationService`, `ReportFormatter`
- DbContext con configuración de entidades

### Web (GestionIncidentes.Web)
- Páginas Blazor
- Componentes
- Configuración de servicios en `Program.cs`

---

## Base de Datos

### Tablas Creadas
- `KnowledgeEntries` - Entradas de la base de conocimiento
- `Notifications` - Notificaciones del sistema
- `AuditLogs` - Registros de auditoría
- `TicketReports` - Reportes de tickets

### Migración
La migración `AddKnowledgeReportsNotificationsAudit` crea todas las tablas necesarias.

Para aplicar la migración:
```bash
dotnet ef database update --project GestionIncidentes.Infrastructure --startup-project GestionIncidentes.Web
```

---

## Configuración

### Program.cs

Los repositorios y servicios se registran en `Program.cs`:

```csharp
// Repositorios
builder.Services.AddScoped<IKnowledgeRepository, KnowledgeRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddScoped<ITicketReportRepository, TicketReportRepository>();

// Servicios
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<ReportFormatter>();

// Usuario actual
builder.Services.AddScoped<ICurrentUser, HttpContextCurrentUser>();

// MediatR con Behaviors
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(
        typeof(ListUsersQueryHandler).Assembly,
        typeof(CreateUserCommandHandler).Assembly
    );
    cfg.AddOpenBehavior(typeof(AuditBehavior<,>));
});
```

---

## Flujo de Trabajo

### Reporte de Incidente ? Resolución ? Base de Conocimiento

1. **Estudiante reporta incidente**
   - Se crea un ticket
   - `OnIncidentReportedHandler` notifica a DITIC/Encargados

2. **Encargado asigna ticket a técnico**
   - `OnTicketCreatedHandler` notifica al técnico asignado

3. **Técnico busca en Base de Conocimiento**
   - Puede buscar soluciones previas en `/knowledge/search`
   - El contador de uso se incrementa automáticamente

4. **Técnico resuelve el ticket**
   - `OnTicketResolvedHandler` notifica al creador y técnico
   - Se sugiere generar reporte

5. **Técnico genera reporte**
   - Completa el formulario en `/reports/create/{ticketId}`
   - Puede marcar si sugiere agregar a base de conocimiento

6. **Crear entrada en Base de Conocimiento**
   - Si se marcó la sugerencia, se redirige a `/knowledge/create`
   - Se documenta el problema y solución para futuras consultas

---

## Tests

Los tests unitarios están en `GestionIncidentes.Tests/ApplicationTests/KnowledgeAndNotificationTests.cs`:

- `SearchKnowledge_ShouldIncrementUsageCount` - Verificar incremento de contador de uso
- `OnIncidentReported_ShouldCreateNotifications` - Verificar creación de notificaciones
- `KnowledgeEntry_ShouldIncrementUsage` - Verificar método de entidad
- `Notification_ShouldMarkAsRead` - Verificar marcado como leído
- `AuditLog_ShouldCreateWithAllProperties` - Verificar creación de audit log

Para ejecutar los tests:
```bash
cd GestionIncidentes.Tests
dotnet test
```

---

## Características Futuras (Opcional)

### SignalR para Notificaciones en Tiempo Real
Actualmente las notificaciones se actualizan cada 30 segundos. Se puede implementar SignalR para notificaciones instantáneas.

### Exportar Reportes a PDF
El `ReportFormatter` ya tiene métodos para HTML/Markdown. Se puede agregar una librería como iTextSharp o PuppeteerSharp para generar PDFs.

### Sistema de Votación en Base de Conocimiento
Permitir a los usuarios votar por las entradas más útiles.

### Búsqueda Avanzada
Implementar búsqueda por múltiples criterios, filtros y ordenamiento.

---

## Dependencias

- **MediatR** - Para CQRS y manejo de eventos
- **Entity Framework Core** - Para acceso a datos
- **PostgreSQL** - Base de datos
- **Blazor** - Framework web
- **AutoMapper** - Para mapeo de objetos
- **JWT Authentication** - Para autenticación

---

## Contacto y Soporte

Para preguntas o problemas, consultar con el equipo de desarrollo.

## Autor

Integración C desarrollada siguiendo Clean Architecture y mejores prácticas de desarrollo.
