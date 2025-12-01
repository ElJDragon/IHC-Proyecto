# ============================================================
# README: Integración Backend con Frontend
# Fecha: 2025-11-30
# ============================================================

## 📋 RESUMEN DE CAMBIOS

### 1. Entidades Extendidas
- **Ticket.cs**: Agregados 15+ campos nuevos
  - Categorización: Category, Priority, Status
  - Ubicación: Location, LocationDetail, AffectedType
  - Estudiantes: ProblemType, EquipmentId, ProgramName, ErrorMessage, AffectedParts
  - Técnicos: AssignedToUserId, TechnicianNotes, ResolvedAt
  - SLA: SlaDeadline
  - Valoración: Rating, FeedbackComment

### 2. DTOs Nuevos (TicketDtos.cs)
- `CreateAdminTicketDto`: Crear tickets (Admin)
- `CreateStudentReportDto`: Reportar problemas (Usuario)
- `UpdateTicketDto`: Actualizar tickets
- `ChangeTicketStatusDto`: Cambiar estado (Técnico)
- `CloseTicketDto`: Cerrar ticket
- `TicketResponseDto`: Respuesta completa con datos calculados
- `AdminDashboardStatsDto`: Estadísticas del dashboard
- `TechnicianStatsDto`: Estadísticas del técnico

### 3. Repositorios Actualizados
- **ITicketRepository** + **EfTicketRepository**:
  - 10+ métodos nuevos para filtros y estadísticas
  - `ListByTechnicianAsync`, `ListByStatusAsync`, `ListByPriorityAsync`
  - `CountByStatusAsync`, `GetAverageResolutionTimeAsync`
  - `GetTicketsWithSlaViolationAsync`, `GetIncidentsByLocationAsync`

### 4. Controladores Nuevos
- **AdminTicketsController** (`/api/admin/tickets`):
  - CRUD completo de tickets
  - Filtros avanzados (técnico, estado, prioridad, búsqueda)
  - Dashboard stats con KPIs
  
- **TechnicianTicketsController** (`/api/technician/tickets`):
  - Mis tickets asignados
  - Cambiar estado, reasignar, cerrar ticket
  - Actualizar bitácora de solución
  - Mis estadísticas
  
- **StudentReportsController** (`/api/student/reports`):
  - Crear reporte (wizard de 3 pasos)
  - Mis reportes
  - Valorar tickets resueltos
  - Mis estadísticas

---

## 🚀 PASOS DE INSTALACIÓN

### Paso 1: Aplicar Migración de Base de Datos

**Opción A: Con Entity Framework CLI**
```powershell
cd "C:\Users\User\Documents\Joshua Universidad\Quinto\IHC\ProyectoIHC\IHC-Proyecto"
.\Scripts\MigrateDatabase.ps1
```

**Opción B: Manualmente con SQL**
```sql
-- Ejecutar en SQL Server Management Studio:
-- 1. Abrir: GestionIncidentes.Infrastructure\Scripts\Migration_ExtendTickets.sql
-- 2. Ejecutar contra tu base de datos
```

### Paso 2: Cargar Datos de Prueba
```sql
-- Ejecutar en SQL Server Management Studio:
-- GestionIncidentes.Infrastructure\Scripts\SeedData_TestTickets.sql
-- Esto creará:
-- - 3 técnicos (Carlos Méndez, Ana Torres, Luis García)
-- - 3 estudiantes
-- - 8 tickets de ejemplo con diferentes estados
```

### Paso 3: Verificar la Estructura
```sql
-- Verificar que los campos se agregaron correctamente
SELECT TOP 5 * FROM Tickets;

-- Verificar índices
EXEC sp_helpindex 'Tickets';
```

---

## 📡 ENDPOINTS API

### Admin Endpoints
```http
POST   /api/admin/tickets              # Crear ticket
GET    /api/admin/tickets              # Listar con filtros (?status=, ?priority=, ?technician=, ?search=)
GET    /api/admin/tickets/{id}         # Obtener por ID
PUT    /api/admin/tickets/{id}         # Actualizar
DELETE /api/admin/tickets/{id}         # Eliminar
GET    /api/admin/tickets/stats/dashboard  # Estadísticas del dashboard
```

### Technician Endpoints
```http
GET    /api/technician/tickets/my-tickets    # Mis tickets asignados
GET    /api/technician/tickets/{id}          # Obtener ticket
PATCH  /api/technician/tickets/{id}/status   # Cambiar estado
PATCH  /api/technician/tickets/{id}/reassign # Reasignar a otro técnico
POST   /api/technician/tickets/{id}/close    # Cerrar ticket
PATCH  /api/technician/tickets/{id}/notes    # Actualizar bitácora
GET    /api/technician/tickets/stats         # Mis estadísticas
```

### Student Endpoints
```http
POST   /api/student/reports                  # Crear reporte
GET    /api/student/reports/my-reports       # Mis reportes
GET    /api/student/reports/{id}             # Obtener reporte
POST   /api/student/reports/{id}/rate        # Valorar ticket resuelto
GET    /api/student/reports/stats            # Mis estadísticas
```

---

## 🧪 PRUEBAS

### 1. Probar con Postman/Insomnia

**Login (obtener token JWT)**:
```http
POST /api/auth/login
{
  "email": "admin@universidad.edu",
  "password": "Admin123!"
}
```

**Crear Ticket (Admin)**:
```http
POST /api/admin/tickets
Authorization: Bearer {tu_token}
{
  "title": "Problema de red en Lab 5",
  "description": "Sin conectividad",
  "category": "connectivity",
  "priority": "high",
  "affectedType": "classroom",
  "location": "Lab 5",
  "locationDetail": "Edificio B",
  "createdByUserId": "{admin_guid}",
  "assignedToUserId": "{tecnico_guid}"
}
```

**Listar Tickets con Filtros**:
```http
GET /api/admin/tickets?status=Pendiente&priority=critical
Authorization: Bearer {tu_token}
```

**Dashboard Stats**:
```http
GET /api/admin/tickets/stats/dashboard
Authorization: Bearer {tu_token}
```

### 2. Integración con Frontend Blazor

Las páginas Blazor ya están listas, ahora necesitan llamar a estos endpoints:

**AdminDashboard.razor** debe llamar a:
- `GET /api/admin/tickets/stats/dashboard` para KPIs y gráfico

**AdminIncidentes.razor** debe llamar a:
- `GET /api/admin/tickets?status=...&priority=...` para tabla con filtros

**TecnicoDashboard.razor** debe llamar a:
- `GET /api/technician/tickets/my-tickets` para lista SLA
- `PATCH /api/technician/tickets/{id}/status` para cambiar estado
- `POST /api/technician/tickets/{id}/close` para cerrar

**UsuarioReportar.razor** debe llamar a:
- `POST /api/student/reports` al enviar el formulario

**UsuarioDashboard.razor** debe llamar a:
- `GET /api/student/reports/my-reports` para tabla de historial

---

## 🔧 CONFIGURACIÓN ADICIONAL

### Actualizar appsettings.json si es necesario
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=GestionIncidentes;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "Jwt": {
    "Key": "tu_clave_secreta_muy_larga_y_segura_minimo_32_caracteres",
    "Issuer": "GestionIncidentesAPI",
    "Audience": "GestionIncidentesUI",
    "ExpirationHours": 24
  }
}
```

### Registrar servicios en Program.cs (si no están)
```csharp
builder.Services.AddScoped<ITicketRepository, EfTicketRepository>();
builder.Services.AddScoped<IUserRepository, EfUserRepository>();
```

---

## ✅ CHECKLIST DE VALIDACIÓN

- [ ] Migración aplicada correctamente
- [ ] Datos de prueba cargados
- [ ] API endpoints responden (probar con Postman)
- [ ] Frontend Blazor conectado a APIs
- [ ] Login funciona y genera token JWT
- [ ] Admin puede ver dashboard con estadísticas
- [ ] Admin puede crear/editar/eliminar tickets
- [ ] Técnico puede ver sus tickets asignados
- [ ] Técnico puede cambiar estado y cerrar tickets
- [ ] Usuario puede crear reportes
- [ ] Usuario puede ver historial de reportes

---

## 📞 CREDENCIALES DE PRUEBA

```
Admin:
  Email: admin@universidad.edu
  Password: Admin123!

Técnicos:
  carlos.mendez@universidad.edu / Tech123!
  ana.torres@universidad.edu / Tech123!
  luis.garcia@universidad.edu / Tech123!

Estudiantes:
  juan.perez@universidad.edu / Student123!
  maria.lopez@universidad.edu / Student123!
  pedro.gonzalez@universidad.edu / Student123!
```

---

## 🐛 TROUBLESHOOTING

**Error: "Column 'Category' does not exist"**
- Solución: Ejecutar el script de migración SQL

**Error: "Unauthorized" en APIs**
- Solución: Verificar que el token JWT sea válido y esté en el header

**Error: "Foreign key constraint"**
- Solución: Verificar que los GUIDs de usuarios existan en la tabla Users

**Performance lento con muchos tickets**
- Solución: Los índices ya están creados en el script de migración
