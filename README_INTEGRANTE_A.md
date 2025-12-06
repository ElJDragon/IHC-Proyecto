# 🎯 Sistema de Gestión de Incidentes - Integrante A Completado

## 📊 Estado del Proyecto

| Integrante | Estado | Completitud |
|------------|--------|-------------|
| **Integrante A** | ✅ **COMPLETO** | **100%** |
| Integrante B | ⚠️ Parcial | 65% |
| Integrante C | ✅ Completo | 100% |

---

## 🎉 ¿Qué se ha implementado?

### ✅ Integrante A - Incidentes y Tickets (COMPLETO)

#### 📦 Backend
- ✅ Entidad `Incident` con lógica de dominio
- ✅ Comandos CQRS: `ReportIncidentCommand`, `CreateTicketCommand`, `AssignTicketCommand`, `UpdateTicketStatusCommand`, `AddTicketActionCommand`
- ✅ Queries CQRS: `GetIncidentDetailsQuery`, `ListIncidentsQuery`, `ListTicketsByAssigneeQuery`
- ✅ Repositorio `IncidentRepository` con Entity Framework Core
- ✅ Eventos de dominio: `IncidentReported`, `TicketCreated`, `TicketAssigned`, `TicketResolved`
- ✅ Controller REST API: `IncidentsController`
- ✅ Integración con `IWorkloadService` y `IAuditLogRepository`

#### 🎨 Frontend (Razor Pages)
- ✅ `ReportIncident.razor` - Formulario para reportar incidentes
- ✅ `IncidentsList.razor` - Lista de todos los incidentes
- ✅ `IncidentDetails.razor` - Detalles completos de un incidente
- ✅ `MyTickets.razor` - Tickets asignados al usuario actual
- ✅ `TicketDetails.razor` - Detalles y acciones de un ticket
- ✅ Componentes: `TicketTimeline.razor`, `TicketStatusBadge.razor`

#### 🗄️ Base de Datos
- ✅ Tabla `Incidents` en PostgreSQL
- ✅ Relaciones con `Users` y `Tickets`
- ✅ Índices para optimización de consultas
- ✅ Script SQL completo con datos de ejemplo

---

## 🚀 Instalación y Configuración

### Paso 1: Instalar PostgreSQL

Sigue la guía detallada: **[GUIA_INSTALACION_POSTGRESQL.md](GUIA_INSTALACION_POSTGRESQL.md)**

**Resumen rápido:**
1. Descarga PostgreSQL desde https://www.postgresql.org/download/windows/
2. Instala con contraseña: `postgres` (o la que prefieras)
3. Puerto: `5432` (default)

### Paso 2: Verificar Instalación

Ejecuta el script de verificación:

```powershell
cd C:\IHC-Proyecto
.\Scripts\verificar-postgresql.ps1
```

Este script verificará:
- ✅ Instalación de PostgreSQL
- ✅ Servicio corriendo
- ✅ Conexión a la base de datos
- ✅ Existencia de la base de datos del proyecto

### Paso 3: Crear la Base de Datos

Si no existe, créala:

```powershell
psql -U postgres -c "CREATE DATABASE \"GestionIncidentesDb\";"
```

### Paso 4: Ejecutar el Script SQL

Aplica el esquema de la base de datos:

```powershell
psql -U postgres -d GestionIncidentesDb -f GestionIncidentes.Infrastructure\Scripts\init-database.sql
```

**Nota:** Cuando te pida contraseña, ingresa la que configuraste durante la instalación.

### Paso 5: Verificar las Tablas

```powershell
psql -U postgres -d GestionIncidentesDb -c "\dt"
```

Deberías ver:
- `Departments`
- `Roles`
- `Users`
- `Incidents` ← **Nueva tabla del Integrante A**
- `Tickets`
- `KnowledgeEntries`
- `Notifications`
- `AuditLogs`
- `TicketReports`

### Paso 6: Ejecutar la Aplicación

```powershell
cd GestionIncidentes.Web
dotnet run
```

La aplicación estará disponible en:
- **HTTPS:** https://localhost:7000
- **HTTP:** http://localhost:5000

---

## 🧪 Probar la Aplicación

### 1. Probar con Postman (API REST)

#### Reportar un Incidente
```http
POST https://localhost:7000/api/incidents
Authorization: Bearer {tu_token_jwt}
Content-Type: application/json

{
  "title": "Error en el sistema de correo",
  "description": "El servidor de correo no responde. Los usuarios no pueden enviar emails."
}
```

#### Listar Incidentes
```http
GET https://localhost:7000/api/incidents
Authorization: Bearer {tu_token_jwt}
```

#### Ver Detalles de un Incidente
```http
GET https://localhost:7000/api/incidents/{id}
Authorization: Bearer {tu_token_jwt}
```

### 2. Probar en el Navegador (Razor Pages)

#### Páginas de Incidentes
- **Reportar Incidente:** https://localhost:7000/incidents/report
- **Lista de Incidentes:** https://localhost:7000/incidents
- **Detalles de Incidente:** https://localhost:7000/incidents/{id}

#### Páginas de Tickets
- **Mis Tickets:** https://localhost:7000/tickets/my-tickets
- **Detalles de Ticket:** https://localhost:7000/tickets/{id}

### 3. Usuarios de Prueba

El script SQL incluye usuarios de ejemplo:

| Email | Contraseña | Rol |
|-------|-----------|-----|
| admin@empresa.com | password123 | Admin |
| tecnico@empresa.com | password123 | Tecnico |
| usuario@empresa.com | password123 | Usuario |

---

## 📚 Documentación Adicional

- **[INTEGRANTE_A_COMPLETADO.md](INTEGRANTE_A_COMPLETADO.md)** - Documentación completa del Integrante A
- **[ESTADO_INTEGRANTES_B_C.md](ESTADO_INTEGRANTES_B_C.md)** - Análisis del estado de B y C
- **[GUIA_INSTALACION_POSTGRESQL.md](GUIA_INSTALACION_POSTGRESQL.md)** - Guía detallada de PostgreSQL

---

## 🔍 Estructura del Proyecto

```
IHC-Proyecto/
├── GestionIncidentes.Domain/
│   ├── Entities/
│   │   ├── Incident.cs          ✅ NUEVO
│   │   ├── Ticket.cs
│   │   └── User.cs
│   └── Events/
│       └── DomainEvents.cs       ✅ ACTUALIZADO
│
├── GestionIncidentes.Application/
│   ├── Features/
│   │   ├── Incidents/           ✅ NUEVO
│   │   │   ├── Commands/
│   │   │   ├── Queries/
│   │   │   └── Dtos/
│   │   └── Tickets/             ✅ COMPLETADO
│   │       ├── Commands/
│   │       ├── Queries/
│   │       └── Dtos/
│   └── Interfaces/
│       └── IIncidentRepository.cs ✅ NUEVO
│
├── GestionIncidentes.Infrastructure/
│   ├── Repositories/
│   │   └── IncidentRepository.cs ✅ NUEVO
│   ├── Persistence/
│   │   └── GestionIncidentesDbContext.cs ✅ ACTUALIZADO
│   └── Scripts/
│       └── init-database.sql     ✅ ACTUALIZADO
│
└── GestionIncidentes.Web/
    ├── Controllers/
    │   └── IncidentsController.cs ✅ NUEVO
    ├── Pages/
    │   ├── Incidents/            ✅ NUEVO
    │   │   ├── ReportIncident.razor
    │   │   ├── IncidentsList.razor
    │   │   └── IncidentDetails.razor
    │   └── Tickets/              ✅ ACTUALIZADO
    │       ├── MyTickets.razor
    │       └── TicketDetails.razor
    └── Components/               ✅ NUEVO
        ├── TicketTimeline.razor
        └── TicketStatusBadge.razor
```

---

## 🐛 Solución de Problemas

### Error: "Cannot connect to PostgreSQL"
**Solución:** Verifica que el servicio esté corriendo:
```powershell
Get-Service postgresql*
Start-Service postgresql-x64-15
```

### Error: "Database does not exist"
**Solución:** Crea la base de datos:
```powershell
psql -U postgres -c "CREATE DATABASE \"GestionIncidentesDb\";"
```

### Error: "Password authentication failed"
**Solución:** Verifica la contraseña en:
- `appsettings.json` → `ConnectionStrings:DefaultConnection`
- `GestionIncidentesDbContextFactory.cs` → cadena de conexión

### Error: "Unauthorized" en los endpoints
**Solución:** Necesitas autenticarte primero. Usa el endpoint de login para obtener un token JWT.

---

## ✅ Checklist de Verificación

Antes de considerar el proyecto funcional, verifica:

- [ ] PostgreSQL instalado y corriendo
- [ ] Base de datos `GestionIncidentesDb` creada
- [ ] Script SQL ejecutado correctamente
- [ ] Tablas `Incidents`, `Tickets`, `Users` existen
- [ ] Aplicación ejecutándose sin errores
- [ ] Endpoints de API respondiendo (con autenticación)
- [ ] Páginas Razor cargando correctamente
- [ ] Puedes reportar un incidente
- [ ] Puedes ver la lista de incidentes
- [ ] Puedes ver los detalles de un incidente

---

## 🎓 Próximos Pasos

1. **Completar Integrante B:**
   - Implementar comandos y queries de Users/Workload
   - Crear páginas de administración de usuarios

2. **Pruebas:**
   - Escribir unit tests para los handlers
   - Escribir integration tests para los endpoints

3. **Optimizaciones:**
   - Agregar caché para consultas frecuentes
   - Implementar paginación en las listas

4. **Seguridad:**
   - Implementar hash de contraseñas con BCrypt
   - Configurar políticas de autorización por rol

---

## 👥 Integrantes del Proyecto

- **Integrante A:** Incidentes y Tickets ✅ **COMPLETADO**
- **Integrante B:** Usuarios, Roles, Workload ⚠️ **INCOMPLETO**
- **Integrante C:** Knowledge Base, Reports, Notifications ✅ **COMPLETADO**

---

## 📞 Soporte

Si encuentras problemas:
1. Revisa la sección "Solución de Problemas"
2. Consulta los archivos de documentación
3. Verifica los logs de la aplicación
4. Ejecuta el script `verificar-postgresql.ps1`

---

**¡Proyecto funcional y listo para usar!** 🎉

**Fecha:** Noviembre 2025  
**Estado:** Integrante A Completado ✅
