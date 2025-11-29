# Postman - Pruebas de Integración C

## ?? Endpoints Disponibles

Todos los endpoints están en la ruta base: **http://localhost:5238/api/knowledge**

---

## 1?? Crear Entrada en Base de Conocimiento

**Método:** `POST`  
**URL:** `http://localhost:5238/api/knowledge/entries`

### Headers
```
Content-Type: application/json
```

### Body (JSON)
```json
{
  "title": "Problema con Impresora HP",
  "problem": "La impresora no imprime documentos desde la red",
  "solution": "1. Reiniciar el spooler de impresión con: net stop spooler && net start spooler\n2. Si persiste, reinstalar drivers\n3. Limpiar la cola de impresión",
  "category": "Hardware",
  "relatedTicketId": null,
  "tags": [
    "impresora",
    "hp",
    "drivers",
    "spooler"
  ]
}
```

### Respuesta Exitosa (200 OK)
```json
{
  "id": "8f7a5c3e-2b1d-4e9f-a6b2-1c5d8e3f9a7b",
  "message": "Entrada creada exitosamente"
}
```

---

## 2?? Buscar en Base de Conocimiento

**Método:** `GET`  
**URL:** `http://localhost:5238/api/knowledge/entries/search?searchTerm=impresora`

### Parámetros
- `searchTerm` (requerido): Término a buscar (ej: "impresora", "driver", "red")

### Respuesta Exitosa (200 OK)
```json
[
  {
    "id": "8f7a5c3e-2b1d-4e9f-a6b2-1c5d8e3f9a7b",
    "title": "Problema con Impresora HP",
    "problem": "La impresora no imprime documentos desde la red",
    "solution": "1. Reiniciar el spooler...",
    "category": "Hardware",
    "tags": ["impresora", "hp", "drivers"],
    "usageCount": 1,
    "createdAt": "2025-11-29T02:15:00Z"
  }
]
```

---

## 3?? Obtener Notificaciones del Usuario

**Método:** `GET`  
**URL:** `http://localhost:5238/api/knowledge/notifications`

### Respuesta Exitosa (200 OK)
```json
[
  {
    "id": "a1b2c3d4-e5f6-4a5b-9c8d-1e2f3a4b5c6d",
    "title": "Nuevo Incidente Reportado",
    "message": "Se ha reportado un nuevo incidente que requiere asignación.",
    "type": "IncidentReported",
    "isRead": false,
    "createdAt": "2025-11-29T02:10:00Z",
    "relatedEntityId": "ticket-id-123"
  }
]
```

---

## 4?? Marcar Notificación como Leída

**Método:** `PUT`  
**URL:** `http://localhost:5238/api/knowledge/notifications/{notificationId}/read`

### Parámetros de URL
- `notificationId`: ID de la notificación

### Ejemplo
```
http://localhost:5238/api/knowledge/notifications/a1b2c3d4-e5f6-4a5b-9c8d-1e2f3a4b5c6d/read
```

### Respuesta Exitosa (200 OK)
```json
{
  "message": "Notificación marcada como leída"
}
```

---

## 5?? Generar Reporte de Ticket

**Método:** `POST`  
**URL:** `http://localhost:5238/api/knowledge/reports`

### Headers
```
Content-Type: application/json
```

### Body (JSON)
```json
{
  "ticketId": "ticket-id-123",
  "resolutionDetails": "Se reinstalaron los drivers de la impresora HP LaserJet desde el sitio web oficial del fabricante",
  "problemDiagnosis": "Falta de drivers actualizados o corrupción del spooler de impresión",
  "actionsTaken": "1. Desinstalé los drivers antiguos\n2. Descargué e instalé los drivers más recientes\n3. Reinicié el servicio de spooler\n4. Probé la impresión exitosamente",
  "timeSpentMinutes": 45,
  "suggestKnowledgeEntry": true
}
```

### Respuesta Exitosa (200 OK)
```json
{
  "id": "report-id-456",
  "message": "Reporte generado exitosamente"
}
```

---

## ?? Ejemplo Completo de Flujo en Postman

### Paso 1: Crear Entrada de Conocimiento
```
POST http://localhost:5238/api/knowledge/entries
Content-Type: application/json

{
  "title": "Problema de Conectividad WiFi",
  "problem": "El ordenador no se conecta a la red WiFi",
  "solution": "1. Reiniciar el router\n2. Reiniciar el adaptador WiFi\n3. Olvidar la red y reconectar",
  "category": "Red",
  "tags": ["wifi", "conectividad", "red"]
}
```

### Paso 2: Buscar la Entrada
```
GET http://localhost:5238/api/knowledge/entries/search?searchTerm=wifi
```

### Paso 3: Generar Reporte
```
POST http://localhost:5238/api/knowledge/reports
Content-Type: application/json

{
  "ticketId": "00000000-0000-0000-0000-000000000099",
  "resolutionDetails": "Se reinició el router y el adaptador WiFi, conectándose exitosamente",
  "problemDiagnosis": "Fallo temporal en la conexión WiFi",
  "actionsTaken": "Reinicié el hardware de red",
  "timeSpentMinutes": 15,
  "suggestKnowledgeEntry": false
}
```

### Paso 4: Obtener Notificaciones
```
GET http://localhost:5238/api/knowledge/notifications
```

---

## ?? Códigos de Respuesta

| Código | Significado |
|--------|-------------|
| 200 OK | Solicitud exitosa |
| 400 Bad Request | Parámetros inválidos o faltantes |
| 500 Internal Server Error | Error en el servidor |

---

## ?? Notas Importantes

1. **El UserId se obtiene automáticamente** desde `ICurrentUser` (actualmente es `00000000-0000-0000-0000-000000000001`)
2. **No necesitas autenticación JWT** para la Integración C (en desarrollo)
3. **La categoría debe ser una de estas:** `Hardware`, `Software`, `Red`, `Otro`
4. **Los tags deben ser un array de strings**
5. **El timeSpentMinutes se convierte a TimeSpan**

---

## ?? Swagger UI

También puedes ver la documentación interactiva en:
**http://localhost:5238/swagger**

Ahí aparecerá el controlador `IntegracionC` con todos los endpoints documentados.
