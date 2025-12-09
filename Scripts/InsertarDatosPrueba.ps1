# Script para insertar tickets de ejemplo en la base de datos
# Ejecuta el script SQL con psql o usando dotnet

Write-Host "Insertando tickets de ejemplo en la base de datos..." -ForegroundColor Cyan

# Opción 1: Intentar con psql si está disponible
$psqlPath = Get-Command psql -ErrorAction SilentlyContinue

if ($psqlPath) {
    Write-Host "Usando psql..." -ForegroundColor Green
    $env:PGPASSWORD = "123"
    psql -h localhost -U postgres -d gestion_incidentes -f ".\Scripts\InsertSampleTickets.sql"
}
else {
    Write-Host "psql no está disponible. Usando dotnet ef para ejecutar comandos SQL..." -ForegroundColor Yellow
    
    # Crear un script temporal con Entity Framework
    $tempScript = @"
using Microsoft.EntityFrameworkCore;
using GestionIncidentes.Infrastructure.Data;
using GestionIncidentes.Domain.Entities;

var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
optionsBuilder.UseNpgsql("Host=localhost;Database=gestion_incidentes;Username=postgres;Password=123");

using var context = new ApplicationDbContext(optionsBuilder.Options);

// Leer y ejecutar el script SQL
var sqlScript = File.ReadAllText("Scripts/InsertSampleTickets.sql");
await context.Database.ExecuteSqlRawAsync(sqlScript);

Console.WriteLine("Tickets insertados correctamente");
var count = await context.Tickets.CountAsync();
Console.WriteLine(`"Total de tickets: {count}`");
"@

    # Por ahora, mostrar el mensaje para que el usuario ejecute manualmente
    Write-Host ""
    Write-Host "⚠️ No se pudo ejecutar automáticamente. Por favor ejecuta uno de estos comandos:" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Opción 1 - Con psql (si tienes PostgreSQL instalado):" -ForegroundColor White
    Write-Host '  $env:PGPASSWORD="123"; psql -h localhost -U postgres -d gestion_incidentes -f ".\Scripts\InsertSampleTickets.sql"' -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Opción 2 - Ejecutar manualmente las queries en pgAdmin o cualquier cliente PostgreSQL" -ForegroundColor White
    Write-Host ""
}
