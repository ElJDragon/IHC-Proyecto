# ============================================================
# Script PowerShell: Generar Migración y Actualizar BD
# Fecha: 2025-11-30
# ============================================================

Write-Host "================================================" -ForegroundColor Cyan
Write-Host "  Migración de Base de Datos - Extend Tickets" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

# Navegar al proyecto Infrastructure
$infrastructureProject = "GestionIncidentes.Infrastructure"
$webProject = "GestionIncidentes.Web"

Write-Host "1. Generando migración..." -ForegroundColor Yellow
try {
    dotnet ef migrations add ExtendTicketEntity `
        --project $infrastructureProject `
        --startup-project $webProject `
        --output-dir Migrations `
        --context GestionIncidentesDbContext

    Write-Host "   ✓ Migración generada exitosamente" -ForegroundColor Green
} catch {
    Write-Host "   ✗ Error al generar migración: $_" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "2. Aplicando migración a la base de datos..." -ForegroundColor Yellow
try {
    dotnet ef database update `
        --project $infrastructureProject `
        --startup-project $webProject `
        --context GestionIncidentesDbContext

    Write-Host "   ✓ Base de datos actualizada exitosamente" -ForegroundColor Green
} catch {
    Write-Host "   ✗ Error al actualizar BD: $_" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "================================================" -ForegroundColor Cyan
Write-Host "  Migración completada" -ForegroundColor Green
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Siguientes pasos:" -ForegroundColor Yellow
Write-Host "1. Ejecutar script de seed data:" -ForegroundColor White
Write-Host "   GestionIncidentes.Infrastructure\Scripts\SeedData_TestTickets.sql" -ForegroundColor Gray
Write-Host ""
Write-Host "2. Verificar los nuevos campos en la tabla Tickets" -ForegroundColor White
Write-Host ""
