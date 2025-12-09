# ====================================================================
# GUIA PARA APLICAR MIGRACIONES - GestionIncidentesDb
# ====================================================================

Write-Host "================================================================" -ForegroundColor Cyan
Write-Host "INFORMACION DE LA BASE DE DATOS" -ForegroundColor Cyan
Write-Host "================================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Host:     localhost" -ForegroundColor White
Write-Host "Port:     5432" -ForegroundColor White
Write-Host "Database: GestionIncidentesDb" -ForegroundColor White
Write-Host "Username: postgres" -ForegroundColor White
Write-Host "Password: postgres" -ForegroundColor White
Write-Host ""

Write-Host "================================================================" -ForegroundColor Cyan
Write-Host "METODO RECOMENDADO: Descomentar migraciones en Program.cs" -ForegroundColor Cyan
Write-Host "================================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "ESTADO ACTUAL:" -ForegroundColor Yellow
Write-Host "  Las migraciones estan COMENTADAS en Program.cs" -ForegroundColor Red
Write-Host "  Ubicacion: GestionIncidentes.Web\Program.cs (lineas 213-222)" -ForegroundColor White
Write-Host ""
Write-Host "SOLUCION:" -ForegroundColor Green
Write-Host "  1. Abre: GestionIncidentes.Web\Program.cs" -ForegroundColor White
Write-Host "  2. Ve a la linea 213" -ForegroundColor White
Write-Host "  3. Elimina los comentarios /* y */ que rodean el bloque" -ForegroundColor White
Write-Host "  4. Guarda el archivo" -ForegroundColor White
Write-Host "  5. Ejecuta: dotnet run" -ForegroundColor Cyan
Write-Host ""
Write-Host "El codigo a descomentar es:" -ForegroundColor Yellow
Write-Host ""
Write-Host "using (var scope = app.Services.CreateScope())" -ForegroundColor Gray
Write-Host "{" -ForegroundColor Gray
Write-Host "    var db = scope.ServiceProvider.GetRequiredService<GestionIncidentesDbContext>();" -ForegroundColor Gray
Write-Host "    await db.Database.MigrateAsync();" -ForegroundColor Gray
Write-Host "    " -ForegroundColor Gray
Write-Host "    // Seed de datos de prueba" -ForegroundColor Gray
Write-Host "    await GestionIncidentes.Infrastructure.Data.DbSeeder.SeedAsync(db);" -ForegroundColor Gray
Write-Host "}" -ForegroundColor Gray
Write-Host ""

Write-Host "================================================================" -ForegroundColor Cyan
Write-Host "MIGRACIONES DISPONIBLES EN EL PROYECTO" -ForegroundColor Cyan
Write-Host "================================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "1. InitialCreate (2025-11-15 23:01)" -ForegroundColor Yellow
Write-Host "   -> Tablas: Users, Departments, Tickets, Roles" -ForegroundColor Gray
Write-Host ""
Write-Host "2. AddUserPassword (2025-11-15 23:17)" -ForegroundColor Yellow
Write-Host "   -> Campo Password en User" -ForegroundColor Gray
Write-Host ""
Write-Host "3. AddKnowledgeReportsNotificationsAudit (2025-11-28 19:22)" -ForegroundColor Yellow
Write-Host "   -> Tablas: KnowledgeEntries, Notifications, AuditLogs, TicketReports" -ForegroundColor Gray
Write-Host ""
Write-Host "4. ExtendTicketEntity (2025-12-01 00:54)" -ForegroundColor Yellow
Write-Host "   -> Campos adicionales en Ticket" -ForegroundColor Gray
Write-Host ""
Write-Host "5. CompleteTicketModelMigration (2025-12-01 14:48)" -ForegroundColor Yellow
Write-Host "   -> Modelo completo de Ticket" -ForegroundColor Gray
Write-Host ""
Write-Host "6. AddSolutionSteps (2025-12-06 00:00) [ULTIMA]" -ForegroundColor Yellow
Write-Host "   -> Tabla SolutionSteps con FK a KnowledgeEntries" -ForegroundColor Gray
Write-Host ""

Write-Host "================================================================" -ForegroundColor Cyan
Write-Host "TABLAS QUE SE CREARAN EN PostgreSQL" -ForegroundColor Cyan
Write-Host "================================================================" -ForegroundColor Cyan
Write-Host ""
$tables = @(
    "Users",
    "Departments", 
    "Roles",
    "Tickets",
    "KnowledgeEntries",
    "SolutionSteps",
    "Notifications",
    "AuditLogs",
    "TicketReports",
    "__EFMigrationsHistory"
)
$tables | ForEach-Object { Write-Host "  - $_" -ForegroundColor Cyan }
Write-Host ""

Write-Host "================================================================" -ForegroundColor Cyan
Write-Host "PASOS COMPLETOS PARA APLICAR MIGRACIONES" -ForegroundColor Cyan
Write-Host "================================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "PASO 1: Descomentar migraciones en Program.cs" -ForegroundColor Green
Write-Host "  Archivo: GestionIncidentes.Web\Program.cs" -ForegroundColor White
Write-Host "  Lineas: 213-222" -ForegroundColor White
Write-Host "  Accion: Eliminar /* al inicio y */ al final del bloque" -ForegroundColor White
Write-Host ""
Write-Host "PASO 2: Ejecutar la aplicacion" -ForegroundColor Green
Write-Host "  cd GestionIncidentes.Web" -ForegroundColor Cyan
Write-Host "  dotnet run" -ForegroundColor Cyan
Write-Host ""
Write-Host "PASO 3: Verificar en la consola" -ForegroundColor Green
Write-Host "  Veras mensajes como:" -ForegroundColor White
Write-Host "  - Applying migration 'AddSolutionSteps'" -ForegroundColor Gray
Write-Host "  - Database migrated successfully" -ForegroundColor Gray
Write-Host ""
Write-Host "PASO 4: Verificar que funcione" -ForegroundColor Green
Write-Host "  Abre el navegador en: https://localhost:7287" -ForegroundColor Cyan
Write-Host "  La aplicacion deberia cargar correctamente" -ForegroundColor White
Write-Host ""

Write-Host "================================================================" -ForegroundColor Cyan
Write-Host "RESUMEN" -ForegroundColor Cyan
Write-Host "================================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "DATABASE: GestionIncidentesDb (PostgreSQL en localhost:5432)" -ForegroundColor Yellow
Write-Host "MIGRACIONES: 6 migraciones pendientes de aplicar" -ForegroundColor Yellow
Write-Host "SOLUCION: Descomentar lineas 213-222 en Program.cs y ejecutar" -ForegroundColor Green
Write-Host ""
Write-Host "================================================================" -ForegroundColor Cyan
