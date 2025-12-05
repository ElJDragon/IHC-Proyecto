# Resumen Técnico del Proyecto - Sistema de Gestión de Incidentes

## 📋 Información General del Proyecto

**Nombre**: Sistema de Gestión de Incidentes para Laboratorios Universitarios  
**Versión**: 1.0  
**Fecha de Última Actualización**: 5 de diciembre de 2025  
**Estado**: En Desarrollo - Integración Backend-Frontend Completada

---

## 🏗️ Arquitectura del Sistema

### Stack Tecnológico

- **Framework Backend**: ASP.NET Core 9.0
- **Framework Frontend**: Blazor Server con InteractiveServer Rendermode
- **Base de Datos**: PostgreSQL 16.x
- **ORM**: Entity Framework Core 9.0
- **Patrón de Arquitectura**: Clean Architecture (DDD)
- **Patrón CQRS**: MediatR para separación de comandos y consultas
- **Autenticación**: ASP.NET Core Identity (en implementación)

### Estructura de Capas

```
┌─────────────────────────────────────┐
│     Presentation Layer (Web)        │
│  - Blazor Components                │
│  - HTTP Controllers                 │
│  - Services (HTTP Client Wrappers)  │
└─────────────────────────────────────┘
              ↓
┌─────────────────────────────────────┐
│     Application Layer               │
│  - Commands & Queries (MediatR)     │
│  - DTOs & Models                    │
│  - Handlers                         │
│  - Behaviors (Auditoría)            │
└─────────────────────────────────────┘
              ↓
┌─────────────────────────────────────┐
│     Domain Layer                    │
│  - Entities                         │
│  - Value Objects                    │
│  - Domain Events                    │
│  - Business Rules                   │
└─────────────────────────────────────┘
              ↓
┌─────────────────────────────────────┐
│     Infrastructure Layer            │
│  - EF Core DbContext                │
│  - Repositories                     │
│  - External Services                │
│  - Migrations                       │
└─────────────────────────────────────┘
```

---

## 📊 Modelo de Datos

### Entidades Principales

#### User (Usuario)
```csharp
- UserId: Guid (PK)
- Email: string
- FullName: string
- Password: string (hashed)
- RoleId: Guid (FK)
- DepartmentId: Guid? (FK)
- IsActive: bool
- CreatedAt: DateTime
```

#### Ticket (Incidente)
```csharp
- TicketId: Guid (PK)
- TicketNumber: string (TKT-YYYYMMDD-XXXX)
- ProblemType: string
- Status: string (Abierto/En Progreso/Resuelto/Cerrado)
- Priority: string (Baja/Media/Alta/Crítica)
- CreatedByUserId: Guid (FK)
- AssignedToUserId: Guid? (FK)
- LocationDetail: string
- Description: string
- TechnicianNotes: string?
- Rating: int? (1-5)
- FeedbackComment: string?
- ResolutionTime: TimeSpan?
- CreatedAt: DateTime
- UpdatedAt: DateTime
- ResolvedAt: DateTime?
```

#### Department (Departamento)
```csharp
- DepartmentId: Guid (PK)
- Name: string
- Description: string
- IsActive: bool
```

#### Role (Rol)
```csharp
- RoleId: Guid (PK)
- Name: string (Admin/Técnico/Estudiante)
- Description: string
```

#### Workload (Carga de Trabajo)
```csharp
- WorkloadId: Guid (PK)
- TechnicianId: Guid (FK)
- AssignedTickets: int
- PendingTickets: int
- LastUpdated: DateTime
```

#### AuditLog (Registro de Auditoría)
```csharp
- AuditLogId: Guid (PK)
- UserId: Guid? (FK)
- Action: string
- EntityType: string
- EntityId: Guid?
- Changes: string (JSON)
- Timestamp: DateTime
```

#### Notification (Notificación)
```csharp
- NotificationId: Guid (PK)
- UserId: Guid (FK)
- Message: string
- IsRead: bool
- Type: string
- CreatedAt: DateTime
```

### Relaciones Principales

- **User → Ticket**: 1:N (CreatedBy)
- **User → Ticket**: 1:N (AssignedTo)
- **User → Role**: N:1
- **User → Department**: N:1
- **User → Workload**: 1:1
- **User → Notification**: 1:N

---

## 🔧 Configuración de Base de Datos

### Connection String
```json
{
  "ConnectionStrings": {
    "PostgresConnection": "Host=localhost;Port=5432;Database=gestion_incidentes;Username=postgres;Password=admin"
  }
}
```

### Migraciones Aplicadas

1. **InitialCreate** - Estructura inicial del esquema
2. **AddInitialEntities** - Entidades base del sistema
3. **AddTicketLocation** - Campo LocationDetail en Ticket
4. **AddUserPassword** - Sistema de autenticación
5. **CompleteTicketModelMigration** - Modelo completo de tickets (última migración)

### Datos de Prueba (Seeding)

#### Usuarios de Prueba
- **Admin**: `admin@example.com` / Password: `Admin123!`
- **Técnico 1**: `tecnico1@example.com` / Password: `Tecnico123!`
- **Técnico 2**: `tecnico2@example.com` / Password: `Tecnico123!`
- **Estudiante 1**: `estudiante1@example.com` / Password: `Estudiante123!`
- **Estudiante 2**: `estudiante2@example.com` / Password: `Estudiante123!`

#### Tickets de Prueba
- 8 tickets creados con prefijo `[SEED]`
- Distribuidos entre laboratorios A, B y C
- Estados variados: Abierto, En Progreso, Resuelto
- Prioridades: Alta, Media, Baja

---

## 🎨 Componentes de UI (Blazor)

### Vistas Principales

#### 1. AdminDashboard.razor
**Propósito**: Dashboard principal del administrador con métricas del sistema

**Características**:
- Total de tickets por estado
- Gráficos de tendencias
- Lista de incidentes recientes
- Distribución por prioridad

**Estado de Integración**: ✅ Conectado a Backend
- Service: `AdminTicketService`
- Endpoint: `/api/admin/tickets/stats`

#### 2. AdminIncidentes.razor
**Propósito**: Gestión completa de todos los incidentes del sistema

**Características**:
- Tabla de todos los tickets
- Filtros por estado, prioridad, técnico
- Búsqueda por número de ticket
- Paginación
- Asignación de técnicos
- Cierre masivo de tickets

**Estado de Integración**: ✅ Conectado a Backend
- Service: `AdminTicketService`
- Métodos usados:
  - `ListTicketsAsync()` - Obtiene todos los tickets
  - `AssignTicketAsync(ticketId, technicianId)` - Asigna técnico
  - `CloseTicketAsync(ticketId, notes)` - Cierra ticket

**Código Clave**:
```csharp
protected override async Task OnInitializedAsync()
{
    await LoadTicketsAsync();
}

private async Task LoadTicketsAsync()
{
    allTickets = await AdminService.ListTicketsAsync();
    filteredTickets = GetFilteredTickets();
}
```

#### 3. TecnicoDashboard.razor
**Propósito**: Panel de trabajo para técnicos con tickets asignados

**Características**:
- Lista de tickets asignados
- Panel de detalles del ticket seleccionado
- Cambio de estado (Abierto → En Progreso → Resuelto)
- Agregar notas técnicas
- Cerrar tickets con calificación
- Cálculo de progreso SLA con barra visual

**Estado de Integración**: ✅ Conectado a Backend
- Service: `TechnicianTicketService`
- Métodos usados:
  - `GetAssignedTicketsAsync()` - Tickets del técnico
  - `ChangeStatusAsync(ticketId, newStatus, notes?)` - Cambia estado
  - `CloseTicketAsync(ticketId, notes, rating?)` - Cierra ticket

**Código Clave**:
```csharp
private async Task ChangeStatus(string newStatus)
{
    await TechnicianService.ChangeStatusAsync(
        selectedTicketId.Value, 
        new ChangeTicketStatusDto(newStatus, null)
    );
    await LoadTicketsAsync();
}

private int CalculateProgress(TicketResponseDto ticket)
{
    var elapsed = DateTime.Now - ticket.CreatedAt;
    var slaLimit = ticket.Priority == "Crítica" ? 4 : 
                   ticket.Priority == "Alta" ? 24 : 
                   ticket.Priority == "Media" ? 48 : 72;
    return Math.Min((int)(elapsed.TotalHours / slaLimit * 100), 100);
}
```

#### 4. UsuarioDashboard.razor
**Propósito**: Vista del estudiante con historial de reportes

**Características**:
- Estadísticas personales (reportes totales, resueltos, pendientes)
- Historial de reportes propios
- Sistema de calificación con estrellas (1-5)
- Modal para dejar comentarios
- Estados visuales con badges de color

**Estado de Integración**: ✅ Conectado a Backend
- Service: `StudentReportService`
- Métodos usados:
  - `GetMyReportsAsync()` - Reportes del estudiante
  - `RateTicketAsync(ticketId, rating, comment)` - Califica ticket

**Código Clave**:
```csharp
private async Task SubmitRating()
{
    await ReportService.RateTicketAsync(
        ticketToRate.Value,
        new RateTicketDto(selectedRating, ratingComment)
    );
    await LoadReportsAsync();
    CloseRatingModal();
}
```

#### 5. UsuarioReportar.razor
**Propósito**: Formulario para reportar nuevos incidentes

**Características**:
- Formulario multi-paso
- Selección de laboratorio y equipo
- Tipos de problema predefinidos
- Información adicional (programa afectado, mensaje de error)
- Selección de partes afectadas (monitor, teclado, mouse, CPU)

**Estado de Integración**: ✅ Conectado a Backend
- Service: `StudentReportService`
- Método usado:
  - `CreateReportAsync(CreateStudentReportDto)` - Crea reporte

**Código Clave**:
```csharp
private async Task SubmitReport()
{
    var reportDto = new CreateStudentReportDto(
        Lab: report.Lab,
        EquipmentId: report.EquipmentId,
        ProblemType: report.ProblemType,
        ProgramName: report.ProgramName,
        ErrorMessage: report.ErrorMessage,
        AffectedParts: string.Join(", ", report.AffectedParts),
        CreatedByUserId: new Guid("09c6afb9-90b7-4e4f-979b-1e40dcd6b513")
    );
    
    await ReportService.CreateReportAsync(reportDto);
}
```

---

## 🔌 API Controllers

### AdminController
**Ruta Base**: `/api/admin/tickets`

**Endpoints**:
```csharp
GET    /api/admin/tickets/stats          // Estadísticas generales
GET    /api/admin/tickets/list           // Lista todos los tickets
POST   /api/admin/tickets/{id}/assign    // Asigna técnico
POST   /api/admin/tickets/{id}/close     // Cierra ticket
```

### TechnicianController
**Ruta Base**: `/api/technician/tickets`

**Endpoints**:
```csharp
GET    /api/technician/tickets/assigned  // Tickets asignados al técnico actual
POST   /api/technician/tickets/{id}/status    // Cambia estado
POST   /api/technician/tickets/{id}/close     // Cierra ticket con notas
```

### StudentController
**Ruta Base**: `/api/student/reports`

**Endpoints**:
```csharp
POST   /api/student/reports               // Crea nuevo reporte
GET    /api/student/reports/my            // Obtiene reportes del estudiante
POST   /api/student/reports/{id}/rate    // Califica ticket resuelto
```

---

## 📦 DTOs (Data Transfer Objects)

### TicketResponseDto
```csharp
public record TicketResponseDto(
    Guid TicketId,
    string TicketNumber,          // TKT-YYYYMMDD-XXXX
    string ProblemType,
    string Status,
    string Priority,
    string CreatedByName,         // Nombre del creador
    string? AssignedToName,       // Nombre del técnico asignado
    string LocationDetail,        // Lab + Equipo
    string Description,
    string? TechnicianNotes,
    int? Rating,
    string? FeedbackComment,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? ResolvedAt
);
```

### CreateStudentReportDto
```csharp
public record CreateStudentReportDto(
    string Lab,
    string EquipmentId,
    string ProblemType,
    string ProgramName,
    string ErrorMessage,
    string AffectedParts,
    Guid CreatedByUserId
);
```

### ChangeTicketStatusDto
```csharp
public record ChangeTicketStatusDto(
    string NewStatus,
    string? TechnicianNotes
);
```

### RateTicketDto
```csharp
public record RateTicketDto(
    int Rating,          // 1-5 estrellas
    string? FeedbackComment
);
```

---

## 🔄 Servicios HTTP (Frontend)

### AdminTicketService
```csharp
public class AdminTicketService
{
    public async Task<List<TicketResponseDto>> ListTicketsAsync()
    public async Task<TicketStatsDto> GetStatsAsync()
    public async Task AssignTicketAsync(Guid ticketId, Guid technicianId)
    public async Task CloseTicketAsync(Guid ticketId, string notes)
}
```

### TechnicianTicketService
```csharp
public class TechnicianTicketService
{
    public async Task<List<TicketResponseDto>> GetAssignedTicketsAsync()
    public async Task ChangeStatusAsync(Guid ticketId, ChangeTicketStatusDto dto)
    public async Task CloseTicketAsync(Guid ticketId, CloseTicketDto dto)
}
```

### StudentReportService
```csharp
public class StudentReportService
{
    public async Task<List<TicketResponseDto>> GetMyReportsAsync()
    public async Task CreateReportAsync(CreateStudentReportDto dto)
    public async Task RateTicketAsync(Guid ticketId, RateTicketDto dto)
}
```

---

## 🎯 Features Implementados

### ✅ Completados

1. **Sistema de Usuarios y Roles**
   - Registro de usuarios
   - Roles: Administrador, Técnico, Estudiante
   - Departamentos

2. **Gestión de Tickets**
   - Creación de incidentes por estudiantes
   - Asignación automática/manual a técnicos
   - Estados: Abierto → En Progreso → Resuelto → Cerrado
   - Prioridades: Baja, Media, Alta, Crítica
   - Numeración automática (TKT-YYYYMMDD-XXXX)

3. **Dashboard Administrativo**
   - Estadísticas en tiempo real
   - Gestión completa de tickets
   - Filtros y búsqueda avanzada
   - Asignación de técnicos

4. **Panel de Técnico**
   - Vista de tickets asignados
   - Cambio de estados
   - Notas técnicas
   - Cierre de tickets
   - Indicadores SLA

5. **Portal del Estudiante**
   - Reporte de incidentes
   - Historial personal
   - Sistema de calificación
   - Seguimiento de estado

6. **Integración Backend-Frontend**
   - Todas las vistas conectadas a API
   - DTOs estandarizados
   - Manejo de errores
   - Carga de datos asíncrona

7. **Base de Datos**
   - Esquema completo implementado
   - Migraciones configuradas
   - Datos de prueba (seeding)
   - Relaciones establecidas

### 🚧 En Desarrollo

1. **Autenticación**
   - Reemplazar GUIDs hardcodeados
   - Implementar login/logout
   - Sesiones de usuario
   - Claims y políticas

2. **Sistema de Notificaciones**
   - Notificaciones en tiempo real
   - Email notifications
   - Push notifications

3. **Reportes y Analytics**
   - Exportación a PDF/Excel
   - Gráficos avanzados
   - Métricas de rendimiento

### 📋 Pendientes

1. **Base de Conocimiento**
   - Artículos de soluciones
   - FAQ
   - Búsqueda de conocimiento

2. **Sistema de Archivos**
   - Adjuntar imágenes a tickets
   - Documentos de soporte

3. **Configuración Avanzada**
   - SLA personalizables
   - Tipos de incidentes configurables
   - Plantillas de respuesta

---

## 🐛 Trabajo de Depuración Realizado

### Sesión de Integración (1 de diciembre de 2025)

#### Problema 1: Datos Hardcodeados
**Síntoma**: 4 de 5 vistas tenían datos estáticos en lugar de conectarse al backend

**Vistas Afectadas**:
- AdminIncidentes.razor
- TecnicoDashboard.razor
- UsuarioDashboard.razor
- UsuarioReportar.razor

**Solución**: Reemplazo completo de listas estáticas por llamadas async a servicios HTTP

#### Problema 2: Incompatibilidad de DTOs
**Síntoma**: Errores de compilación por propiedades inexistentes

**Errores Encontrados**:
```
Error: 'TicketResponseDto' no contiene una definición para 'AssignedToUser'
Error: 'TicketResponseDto' no contiene una definición para 'EquipmentId'
```

**Causa Raíz**: Las vistas referenciaban objetos navegacionales (`AssignedToUser.FullName`) pero el DTO devolvía strings directos (`AssignedToName`)

**Soluciones Aplicadas**:
- `ticket.AssignedToUser?.FullName` → `ticket.AssignedToName`
- `ticket.CreatedByUser?.FullName` → `ticket.CreatedByName`
- `ticket.EquipmentId` → `ticket.LocationDetail`
- `FormatTicketId(ticket.TicketId)` → `ticket.TicketNumber`

#### Problema 3: Sintaxis Razor
**Síntoma**: Error de parser en expresiones con comillas escapadas

**Código Problemático**:
```razor
@if (ticket.Status == \"Resuelto\")
```

**Solución**:
```razor
@if (ticket.Status == "Resuelto")
```

#### Problema 4: Parámetros Faltantes en DTOs
**Síntoma**: Errores al construir DTOs para cambio de estado

**Código Problemático**:
```csharp
new ChangeTicketStatusDto(newStatus)  // Falta TechnicianNotes
```

**Solución**:
```csharp
new ChangeTicketStatusDto(newStatus, null)  // Agregado parámetro opcional
new CloseTicketDto(technicianNotes, null)   // Rating opcional
```

#### Problema 5: Conflicto de Puerto
**Síntoma**: `System.IO.IOException: Failed to bind to address http://127.0.0.1:5238: address already in use`

**Estado**: Pendiente de resolución

**Opciones de Solución**:
1. Matar proceso usando puerto 5238
2. Cambiar puerto en `launchSettings.json`

---

## 🔐 Consideraciones de Seguridad

### Implementadas
- ✅ Contraseñas hasheadas en base de datos
- ✅ GUIDs para IDs (dificulta enumeración)
- ✅ Validación de DTOs
- ✅ Foreign Keys con integridad referencial

### Por Implementar
- ⚠️ Autenticación JWT/Cookie
- ⚠️ Autorización por roles (Policies)
- ⚠️ HTTPS enforcement
- ⚠️ CORS configuration
- ⚠️ Rate limiting
- ⚠️ SQL injection protection (EntityFramework ayuda)
- ⚠️ XSS protection

---

## 📈 Métricas de Desarrollo

### Líneas de Código (Aproximado)
- **Domain Layer**: ~800 líneas
- **Application Layer**: ~1,500 líneas
- **Infrastructure Layer**: ~1,200 líneas
- **Web Layer**: ~2,500 líneas
- **Tests**: ~500 líneas
- **Total**: ~6,500 líneas

### Archivos Modificados en Última Sesión
1. `AdminIncidentes.razor` - 450 líneas
2. `TecnicoDashboard.razor` - 380 líneas
3. `UsuarioDashboard.razor` - 320 líneas
4. `UsuarioReportar.razor` - 280 líneas
5. `TicketDtos.cs` - 150 líneas

### Compilación
- **Build Time**: ~1.8 segundos
- **Warnings**: 4 (nullable reference warnings)
- **Errors**: 0

---

## 🚀 Comandos de Ejecución

### Build
```powershell
dotnet build GestionIncidentes.sln
```

### Run (Development)
```powershell
cd GestionIncidentes.Web
dotnet watch run
```

### Migrations
```powershell
# Crear migración
dotnet ef migrations add MigrationName --project GestionIncidentes.Infrastructure --startup-project GestionIncidentes.Web

# Aplicar migraciones
dotnet ef database update --project GestionIncidentes.Infrastructure --startup-project GestionIncidentes.Web
```

### Tests
```powershell
dotnet test GestionIncidentes.Tests
```

---

## 🌐 URLs de Acceso

### Desarrollo Local
- **Aplicación**: http://localhost:5238
- **Swagger (si habilitado)**: http://localhost:5238/swagger
- **Base de Datos**: localhost:5432

### Endpoints API
- **Admin**: http://localhost:5238/api/admin/tickets/*
- **Técnico**: http://localhost:5238/api/technician/tickets/*
- **Estudiante**: http://localhost:5238/api/student/reports/*

---

## 📝 Notas Técnicas Adicionales

### Patrones de Diseño Utilizados
1. **Repository Pattern**: Abstracción de acceso a datos
2. **CQRS**: Separación de comandos y consultas con MediatR
3. **Dependency Injection**: IoC container nativo de ASP.NET Core
4. **DTO Pattern**: Transferencia de datos entre capas
5. **Unit of Work**: Transacciones con DbContext
6. **Factory Pattern**: Creación de entidades complejas

### Convenciones de Código
- **Naming**: PascalCase para clases, camelCase para variables
- **Async/Await**: Todos los métodos de I/O son asíncronos
- **Record Types**: DTOs inmutables con records
- **Nullable Reference Types**: Habilitado en todo el proyecto

### Configuración de EF Core
```csharp
// Connection pooling habilitado
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        connectionString,
        npgsqlOptions => npgsqlOptions.EnableRetryOnFailure()
    ));
```

### Logging
- **Provider**: Serilog (configurado para Console y File)
- **Niveles**: Debug, Information, Warning, Error, Critical
- **Formato**: JSON estructurado

---

## 🎓 Lecciones Aprendidas

1. **Sincronización DTO-UI**: Mantener DTOs alineados con las necesidades de la UI evita refactorización masiva
2. **Testing Temprano**: Conectar frontend al backend desde el inicio facilita detección de inconsistencias
3. **Migrations**: Aplicar migraciones frecuentemente evita conflictos de esquema
4. **Clean Architecture**: La separación de capas facilitó el testing y mantenimiento
5. **Async Todo**: Blazor Server requiere operaciones asíncronas para evitar bloqueos de UI

---

## 🔮 Roadmap Futuro

### Fase 2 (Próximos 2 meses)
- [ ] Sistema de autenticación completo
- [ ] Notificaciones en tiempo real (SignalR)
- [ ] Base de conocimiento
- [ ] Exportación de reportes

### Fase 3 (3-6 meses)
- [ ] Aplicación móvil (MAUI)
- [ ] Integración con Active Directory
- [ ] Dashboard de analytics avanzado
- [ ] API pública con rate limiting

### Fase 4 (6-12 meses)
- [ ] Machine Learning para asignación inteligente
- [ ] Chatbot de soporte
- [ ] Multi-tenancy
- [ ] Internacionalización (i18n)

---

## 👥 Contacto y Soporte

**Desarrollador Principal**: [Tu Nombre]  
**Universidad**: [Nombre de la Universidad]  
**Curso**: Interacción Humano-Computadora (IHC)  
**Semestre**: Quinto  

---

**Última Actualización**: 5 de diciembre de 2025  
**Versión del Documento**: 1.0
