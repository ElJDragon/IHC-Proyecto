# ====================================================================
# Script para Aplicar Migraciones a PostgreSQL
# Base de Datos: GestionIncidentesDb
# ====================================================================

# INFORMACIÓN DE LA BASE DE DATOS
# --------------------------------
# Host: localhost
# Port: 5432
# Database: GestionIncidentesDb
# Username: postgres
# Password: postgres

# ====================================================================
# MÉTODO 1: Aplicar Migraciones desde Program.cs (RECOMENDADO)
# ====================================================================

Write-Host "==================================================================" -ForegroundColor Cyan
Write-Host "MÉTODO 1: Descomentar y ejecutar migraciones automáticas" -ForegroundColor Cyan
Write-Host "==================================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Actualmente las migraciones están COMENTADAS en Program.cs" -ForegroundColor Yellow
Write-Host "Ubicación: GestionIncidentes.Web\Program.cs (líneas 213-222)" -ForegroundColor Yellow
Write-Host ""
Write-Host "Para activar las migraciones automáticas:" -ForegroundColor Green
Write-Host "1. Descomenta estas líneas en Program.cs:" -ForegroundColor White
Write-Host @"
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GestionIncidentesDbContext>();
    await db.Database.MigrateAsync();
    
    // Seed de datos de prueba
    await GestionIncidentes.Infrastructure.Data.DbSeeder.SeedAsync(db);
}
"@ -ForegroundColor Cyan
Write-Host ""
Write-Host "2. Ejecuta la aplicación con:" -ForegroundColor White
Write-Host "   cd 'GestionIncidentes.Web'" -ForegroundColor Cyan
Write-Host "   dotnet run" -ForegroundColor Cyan
Write-Host ""
Write-Host "3. Las migraciones se aplicarán automáticamente al iniciar" -ForegroundColor Green
Write-Host ""

# ====================================================================
# MÉTODO 2: Usar dotnet-ef (Requiere instalación)
# ====================================================================

Write-Host "==================================================================" -ForegroundColor Cyan
Write-Host "MÉTODO 2: Usar dotnet-ef CLI Tool" -ForegroundColor Cyan
Write-Host "==================================================================" -ForegroundColor Cyan
Write-Host ""

# Verificar si dotnet-ef está instalado
$efInstalled = $false
try {
    $efVersion = dotnet ef --version 2>&1
    if ($LASTEXITCODE -eq 0) {
        $efInstalled = $true
        Write-Host "✓ dotnet-ef está instalado: $efVersion" -ForegroundColor Green
    }
} catch {
    Write-Host "✗ dotnet-ef NO está instalado" -ForegroundColor Red
}

if (-not $efInstalled) {
    Write-Host ""
    Write-Host "Para instalar dotnet-ef:" -ForegroundColor Yellow
    Write-Host "1. Desinstala cualquier versión corrupta:" -ForegroundColor White
    Write-Host "   dotnet tool uninstall --global dotnet-ef" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "2. Limpia el cache de NuGet:" -ForegroundColor White
    Write-Host "   dotnet nuget locals all --clear" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "3. Instala la versión correcta:" -ForegroundColor White
    Write-Host "   dotnet tool install --global dotnet-ef --version 9.0.0" -ForegroundColor Cyan
    Write-Host ""
} else {
    Write-Host ""
    Write-Host "Comandos para aplicar migraciones con dotnet-ef:" -ForegroundColor Green
    Write-Host ""
    Write-Host "1. Ver lista de migraciones:" -ForegroundColor White
    Write-Host "   cd GestionIncidentes.Web" -ForegroundColor Cyan
    Write-Host "   dotnet ef migrations list --project ..\GestionIncidentes.Infrastructure\GestionIncidentes.Infrastructure.csproj" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "2. Aplicar todas las migraciones:" -ForegroundColor White
    Write-Host "   dotnet ef database update --project ..\GestionIncidentes.Infrastructure\GestionIncidentes.Infrastructure.csproj" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "3. Aplicar una migración específica:" -ForegroundColor White
    Write-Host "   dotnet ef database update AddSolutionSteps --project ..\GestionIncidentes.Infrastructure\GestionIncidentes.Infrastructure.csproj" -ForegroundColor Cyan
    Write-Host ""
}

# ====================================================================
# MÉTODO 3: Script SQL Manual (Si PostgreSQL está instalado)
# ====================================================================

Write-Host "==================================================================" -ForegroundColor Cyan
Write-Host "MÉTODO 3: Verificar migraciones en PostgreSQL directamente" -ForegroundColor Cyan
Write-Host "==================================================================" -ForegroundColor Cyan
Write-Host ""

# Verificar si psql está disponible
$psqlInstalled = $false
try {
    $psqlVersion = psql --version 2>&1
    if ($LASTEXITCODE -eq 0) {
        $psqlInstalled = $true
        Write-Host "✓ PostgreSQL psql está instalado: $psqlVersion" -ForegroundColor Green
    }
} catch {
    Write-Host "✗ psql NO está disponible en PATH" -ForegroundColor Red
}

if ($psqlInstalled) {
    Write-Host ""
    Write-Host "Comandos para verificar el estado de la base de datos:" -ForegroundColor Green
    Write-Host ""
    Write-Host "1. Conectar a la base de datos:" -ForegroundColor White
    Write-Host "   psql -h localhost -p 5432 -U postgres -d GestionIncidentesDb" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "2. Ver todas las tablas:" -ForegroundColor White
    Write-Host "   \dt" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "3. Ver historial de migraciones aplicadas:" -ForegroundColor White
    Write-Host '   SELECT * FROM "__EFMigrationsHistory" ORDER BY "MigrationId";' -ForegroundColor Cyan
    Write-Host ""
    Write-Host "4. Ver estructura de la tabla SolutionSteps:" -ForegroundColor White
    Write-Host '   \d "SolutionSteps"' -ForegroundColor Cyan
    Write-Host ""
} else {
    Write-Host ""
    Write-Host "Para instalar PostgreSQL:" -ForegroundColor Yellow
    Write-Host "1. Descarga PostgreSQL desde: https://www.postgresql.org/download/windows/" -ForegroundColor White
    Write-Host "2. Instala y añade bin\ al PATH del sistema" -ForegroundColor White
    Write-Host "3. O usa pgAdmin para conectarte visualmente" -ForegroundColor White
    Write-Host ""
}

# ====================================================================
# RESUMEN DE MIGRACIONES DISPONIBLES
# ====================================================================

Write-Host "==================================================================" -ForegroundColor Cyan
Write-Host "MIGRACIONES DISPONIBLES EN EL PROYECTO" -ForegroundColor Cyan
Write-Host "==================================================================" -ForegroundColor Cyan
Write-Host ""

$migrations = @(
    @{Name="InitialCreate"; Date="2025-11-15 23:01"; Description="Tablas iniciales (Users, Departments, Tickets)"},
    @{Name="AddUserPassword"; Date="2025-11-15 23:17"; Description="Campo Password en User"},
    @{Name="AddKnowledgeReportsNotificationsAudit"; Date="2025-11-28 19:22"; Description="KnowledgeEntries, Notifications, AuditLogs, TicketReports"},
    @{Name="ExtendTicketEntity"; Date="2025-12-01 00:54"; Description="Campos adicionales en Ticket"},
    @{Name="CompleteTicketModelMigration"; Date="2025-12-01 14:48"; Description="Modelo completo de Ticket"},
    @{Name="AddSolutionSteps"; Date="2025-12-06 00:00"; Description="Tabla SolutionSteps con FK a KnowledgeEntries"}
)

$migrations | ForEach-Object {
    Write-Host ("[{0}] {1}" -f $_.Date, $_.Name) -ForegroundColor Yellow
    Write-Host ("    └─ {0}" -f $_.Description) -ForegroundColor Gray
}

Write-Host ""
Write-Host "==================================================================" -ForegroundColor Cyan
Write-Host "ESTRUCTURA ESPERADA EN LA BASE DE DATOS" -ForegroundColor Cyan
Write-Host "==================================================================" -ForegroundColor Cyan
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

Write-Host "Tablas que deben existir después de aplicar todas las migraciones:" -ForegroundColor Green
$tables | ForEach-Object {
    Write-Host "  ✓ $_" -ForegroundColor Cyan
}

Write-Host ""
Write-Host "==================================================================" -ForegroundColor Cyan
Write-Host "RECOMENDACIÓN FINAL" -ForegroundColor Cyan
Write-Host "==================================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "LA FORMA MÁS SIMPLE:" -ForegroundColor Green -BackgroundColor DarkGreen
Write-Host ""
Write-Host "1. Descomenta las líneas 213-222 en Program.cs" -ForegroundColor White
Write-Host "2. Ejecuta: dotnet run" -ForegroundColor Cyan
Write-Host "3. La aplicación aplicará todas las migraciones automáticamente" -ForegroundColor White
Write-Host "4. Se crearán datos de prueba (seed)" -ForegroundColor White
Write-Host ""
Write-Host "==================================================================" -ForegroundColor Cyan
