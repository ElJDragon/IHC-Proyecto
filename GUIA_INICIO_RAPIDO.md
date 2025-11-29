# Guía de Inicio Rápido - Sistema de Gestión de Incidentes

## Estado del Proyecto ?

- ? **Build exitoso**
- ? **Base de datos PostgreSQL configurada y funcionando**
- ? **Migraciones aplicadas correctamente**
- ? **Todas las tablas creadas:**
  - AuditLogs
  - Departments
  - KnowledgeEntries
  - Notifications
  - Roles
  - TicketReports
  - Tickets
  - Users

## Requisitos Previos

1. **.NET 9 SDK** instalado
2. **PostgreSQL** instalado y funcionando en `localhost:5050`
3. **Visual Studio 2022** o **VS Code** con extensiones C#

## Configuración de la Base de Datos

La aplicación está configurada para conectarse a PostgreSQL con los siguientes parámetros:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5050;Database=GestionIncidentesDb;Username=postgres;Password=postgres"
  }
}
```

**Las migraciones ya están aplicadas y las tablas creadas.**

## Ejecutar la Aplicación

### Opción 1: Desde la Terminal

```bash
cd "C:\Users\User\Documents\Joshua Universidad\Quinto\IHC\ProyectoIHC\IHC-Proyecto\GestionIncidentes.Web"
dotnet run
```

### Opción 2: Desde Visual Studio

1. Abre la solución en Visual Studio
2. Establece `GestionIncidentes.Web` como proyecto de inicio
3. Presiona `F5` o haz clic en el botón "Run"

### Opción 3: Con Hot Reload (Recomendado para Desarrollo)

```bash
cd "C:\Users\User\Documents\Joshua Universidad\Quinto\IHC\ProyectoIHC\IHC-Proyecto\GestionIncidentes.Web"
dotnet watch run
```

## URLs de la Aplicación

Una vez ejecutada, la aplicación estará disponible en:

- **HTTPS:** https://localhost:7287
- **HTTP:** http://localhost:5000

## Características de la Integración C

### 1. Base de Conocimiento (Knowledge Database)

#### Crear Entrada
- **URL:** `/knowledge/create`
- **Descripción:** Crear nueva entrada en la base de conocimiento
- **Campos:**
  - Título (requerido)
  - Categoría (Hardware, Software, Red, Otro)
  - Descripción del Problema
  - Solución
  - Tags (separados por coma)
  - Ticket Relacionado (opcional)

#### Buscar Entradas
- **URL:** `/knowledge/search`
- **Descripción:** Buscar entradas en la base de conocimiento
- **Funcionalidad:** El contador de uso se incrementa automáticamente al buscar

---

### 2. Reportes de Tickets

#### Crear Reporte
- **URL:** `/reports/create/{ticketId}`
- **Descripción:** Generar reporte de resolución de ticket
- **Campos:**
  - Diagnóstico del Problema
  - Acciones Realizadas
  - Detalles de Resolución
  - Tiempo Empleado (horas y minutos)
  - Checkbox: Sugerir agregar a Base de Conocimiento

#### Ver Reportes
- **URL:** `/reports/viewer`
- **Descripción:** Ver todos los reportes generados
- **Funcionalidad:** Lista de reportes con opción de ver detalles

---

### 3. Notificaciones

#### Ver Notificaciones
- **URL:** `/notifications`
- **Descripción:** Ver todas las notificaciones del usuario actual
- **Funcionalidad:** Marcar como leídas

#### Panel de Notificaciones
- **Ubicación:** Barra de navegación (icono de campana)
- **Actualización:** Cada 30 segundos
- **Badge:** Muestra cantidad de notificaciones no leídas

---

### 4. Sistema de Auditoría

El sistema de auditoría funciona automáticamente en segundo plano:
- Registra todas las operaciones importantes (comandos)
- Almacena usuario, acción, entidad, detalles y timestamp
- Implementado mediante `AuditBehavior<TRequest, TResponse>`

---

## Flujo de Trabajo Completo

### Escenario: Estudiante Reporta Problema de Hardware

1. **Estudiante reporta incidente**
   - Se crea un ticket
   - Sistema notifica a DITIC/Encargados

2. **Encargado asigna ticket a técnico**
   - Sistema notifica al técnico asignado

3. **Técnico busca en Base de Conocimiento**
   - Navega a `/knowledge/search`
   - Busca problema similar (ej: "impresora", "driver")
   - El sistema incrementa el contador de uso

4. **Técnico resuelve el problema**
   - Marca el ticket como resuelto
   - Sistema notifica al estudiante y técnico

5. **Técnico genera reporte**
   - Navega a `/reports/create/{ticketId}`
   - Completa diagnóstico, acciones y tiempo empleado
   - Marca "Sugerir agregar a Base de Conocimiento"

6. **Técnico crea entrada en Base de Conocimiento**
   - Sistema redirige a `/knowledge/create`
   - Documenta problema y solución
   - Agrega tags (ej: impresora, driver, windows)
   - Asocia con el ticket resuelto

---

## API Endpoints (Swagger)

La aplicación incluye documentación Swagger:
- **URL:** https://localhost:7287/swagger

### Endpoints Principales

#### Knowledge
- `POST /api/knowledge` - Crear entrada
- `GET /api/knowledge/search?term={term}` - Buscar

#### Reports
- `POST /api/reports` - Generar reporte
- `GET /api/reports` - Listar reportes
- `GET /api/reports/{id}` - Ver reporte

#### Notifications
- `GET /api/notifications` - Obtener notificaciones
- `PUT /api/notifications/{id}/read` - Marcar como leída

---

## Tests

Ejecutar tests:

```bash
cd "C:\Users\User\Documents\Joshua Universidad\Quinto\IHC\ProyectoIHC\IHC-Proyecto\GestionIncidentes.Tests"
dotnet test
```

**Tests disponibles:** 6 tests (todos pasando ?)

---

## Troubleshooting

### Error: No se puede conectar a PostgreSQL

```
Failed to connect to 127.0.0.1:5050
```

**Solución:**
1. Verifica que PostgreSQL esté ejecutándose
2. Verifica que el puerto sea 5050
3. Verifica usuario y contraseña en `appsettings.json`

### Error: Base de datos no existe

**Solución:**
```bash
cd GestionIncidentes.Infrastructure
dotnet ef database update --startup-project ..\GestionIncidentes.Web\GestionIncidentes.Web.csproj
```

### Error: Migraciones no aplicadas

**Solución:**
La aplicación aplica migraciones automáticamente al iniciar. Si hay problemas:

```bash
cd GestionIncidentes.Infrastructure
dotnet ef migrations list --startup-project ..\GestionIncidentes.Web\GestionIncidentes.Web.csproj
dotnet ef database update --startup-project ..\GestionIncidentes.Web\GestionIncidentes.Web.csproj
```

---

## Comandos Útiles

### Ver estado de migraciones
```bash
cd GestionIncidentes.Infrastructure
dotnet ef migrations list --startup-project ..\GestionIncidentes.Web\GestionIncidentes.Web.csproj
```

### Crear nueva migración
```bash
cd GestionIncidentes.Infrastructure
dotnet ef migrations add NombreMigracion --startup-project ..\GestionIncidentes.Web\GestionIncidentes.Web.csproj
```

### Revertir última migración
```bash
cd GestionIncidentes.Infrastructure
dotnet ef migrations remove --startup-project ..\GestionIncidentes.Web\GestionIncidentes.Web.csproj
```

### Actualizar base de datos a migración específica
```bash
cd GestionIncidentes.Infrastructure
dotnet ef database update NombreMigracion --startup-project ..\GestionIncidentes.Web\GestionIncidentes.Web.csproj
```

---

## Estructura del Proyecto

```
IHC-Proyecto/
??? GestionIncidentes.Domain/          # Entidades y eventos del dominio
??? GestionIncidentes.Application/     # Lógica de negocio (CQRS, handlers)
??? GestionIncidentes.Infrastructure/  # Repositorios, DbContext, servicios
??? GestionIncidentes.Web/             # Aplicación Blazor
??? GestionIncidentes.Tests/           # Tests unitarios
```

---

## Datos de Prueba (Opcional)

Para probar la aplicación, puedes crear datos de prueba manualmente:

1. **Crear Departamento**
2. **Crear Roles**
3. **Crear Usuarios** (Estudiante, Técnico, DITIC, Encargado)
4. **Reportar Incidente** como Estudiante
5. **Asignar Ticket** como Encargado
6. **Buscar en Knowledge DB** como Técnico
7. **Resolver Ticket** como Técnico
8. **Generar Reporte** como Técnico
9. **Crear Entrada en Knowledge DB** como Técnico

---

## Contacto y Soporte

Para preguntas o problemas, contactar al equipo de desarrollo.

---

## ? Checklist de Verificación

Antes de comenzar a usar la aplicación, verifica:

- [ ] PostgreSQL está ejecutándose en puerto 5050
- [ ] Base de datos `GestionIncidentesDb` existe
- [ ] Todas las tablas están creadas (ver lista arriba)
- [ ] La aplicación compila sin errores (`dotnet build`)
- [ ] Los tests pasan (`dotnet test`)
- [ ] La aplicación se ejecuta (`dotnet run`)
- [ ] Puedes acceder a https://localhost:7287
- [ ] Swagger está disponible en https://localhost:7287/swagger

---

## Estado Actual: TODO LISTO ?

¡La Integración C está completamente implementada y lista para usar!

**Última actualización:** 28 de noviembre de 2024
