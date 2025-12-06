# Script de Verificación Rápida de PostgreSQL
# Ejecuta este script para verificar que PostgreSQL está instalado y funcionando correctamente

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Verificación de PostgreSQL" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# 1. Verificar si psql está instalado
Write-Host "1. Verificando instalación de PostgreSQL..." -ForegroundColor Yellow
try {
    $version = psql --version 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   ✅ PostgreSQL instalado: $version" -ForegroundColor Green
    } else {
        Write-Host "   ❌ PostgreSQL no está instalado o no está en el PATH" -ForegroundColor Red
        Write-Host "   Por favor, sigue la guía GUIA_INSTALACION_POSTGRESQL.md" -ForegroundColor Yellow
        exit 1
    }
} catch {
    Write-Host "   ❌ Error al verificar PostgreSQL: $_" -ForegroundColor Red
    exit 1
}

Write-Host ""

# 2. Verificar si el servicio está corriendo
Write-Host "2. Verificando servicio de PostgreSQL..." -ForegroundColor Yellow
$service = Get-Service -Name "postgresql*" -ErrorAction SilentlyContinue

if ($service) {
    if ($service.Status -eq "Running") {
        Write-Host "   ✅ Servicio PostgreSQL corriendo: $($service.Name)" -ForegroundColor Green
    } else {
        Write-Host "   ⚠️  Servicio PostgreSQL detenido: $($service.Name)" -ForegroundColor Yellow
        Write-Host "   Intentando iniciar el servicio..." -ForegroundColor Yellow
        Start-Service $service.Name
        Write-Host "   ✅ Servicio iniciado" -ForegroundColor Green
    }
} else {
    Write-Host "   ⚠️  No se encontró el servicio de PostgreSQL" -ForegroundColor Yellow
}

Write-Host ""

# 3. Verificar conexión a la base de datos
Write-Host "3. Verificando conexión a la base de datos..." -ForegroundColor Yellow
Write-Host "   Usuario: postgres" -ForegroundColor Gray
Write-Host "   Base de datos: postgres (default)" -ForegroundColor Gray
Write-Host ""
Write-Host "   Por favor, ingresa la contraseña de PostgreSQL:" -ForegroundColor Cyan

$env:PGPASSWORD = Read-Host -AsSecureString "   Contraseña" | ConvertFrom-SecureString -AsPlainText

try {
    $result = psql -U postgres -d postgres -c "\l" 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   ✅ Conexión exitosa a PostgreSQL" -ForegroundColor Green
    } else {
        Write-Host "   ❌ Error de conexión: $result" -ForegroundColor Red
        Write-Host "   Verifica tu contraseña y configuración" -ForegroundColor Yellow
        exit 1
    }
} catch {
    Write-Host "   ❌ Error: $_" -ForegroundColor Red
    exit 1
}

Write-Host ""

# 4. Verificar si existe la base de datos del proyecto
Write-Host "4. Verificando base de datos 'GestionIncidentesDb'..." -ForegroundColor Yellow

$checkDb = psql -U postgres -d postgres -t -c "SELECT 1 FROM pg_database WHERE datname='GestionIncidentesDb';" 2>&1

if ($checkDb -match "1") {
    Write-Host "   ✅ Base de datos 'GestionIncidentesDb' existe" -ForegroundColor Green
    
    # Verificar tablas
    Write-Host "   Verificando tablas..." -ForegroundColor Gray
    $tables = psql -U postgres -d GestionIncidentesDb -t -c "\dt" 2>&1
    
    if ($tables -match "Incidents") {
        Write-Host "   ✅ Tabla 'Incidents' encontrada" -ForegroundColor Green
    } else {
        Write-Host "   ⚠️  Tabla 'Incidents' no encontrada" -ForegroundColor Yellow
    }
    
    if ($tables -match "Tickets") {
        Write-Host "   ✅ Tabla 'Tickets' encontrada" -ForegroundColor Green
    } else {
        Write-Host "   ⚠️  Tabla 'Tickets' no encontrada" -ForegroundColor Yellow
    }
    
    if ($tables -match "Users") {
        Write-Host "   ✅ Tabla 'Users' encontrada" -ForegroundColor Green
    } else {
        Write-Host "   ⚠️  Tabla 'Users' no encontrada" -ForegroundColor Yellow
    }
    
} else {
    Write-Host "   ❌ Base de datos 'GestionIncidentesDb' NO existe" -ForegroundColor Red
    Write-Host ""
    Write-Host "   ¿Deseas crearla ahora? (S/N)" -ForegroundColor Yellow
    $respuesta = Read-Host "   "
    
    if ($respuesta -eq "S" -or $respuesta -eq "s") {
        Write-Host "   Creando base de datos..." -ForegroundColor Cyan
        psql -U postgres -d postgres -c "CREATE DATABASE \"GestionIncidentesDb\";" 2>&1
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "   ✅ Base de datos creada exitosamente" -ForegroundColor Green
            Write-Host ""
            Write-Host "   Ahora ejecuta el script SQL:" -ForegroundColor Cyan
            Write-Host "   psql -U postgres -d GestionIncidentesDb -f GestionIncidentes.Infrastructure\Scripts\init-database.sql" -ForegroundColor White
        } else {
            Write-Host "   ❌ Error al crear la base de datos" -ForegroundColor Red
        }
    }
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Verificación Completada" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Limpiar contraseña del entorno
Remove-Item Env:\PGPASSWORD -ErrorAction SilentlyContinue

Write-Host "Presiona cualquier tecla para continuar..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
