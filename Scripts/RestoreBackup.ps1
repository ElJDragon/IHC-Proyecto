param(
    [string]$BackupFile = "C:\Users\Jona\Downloads\IncidenesBackup (1).sql",
    [string]$Server = "localhost",
    [string]$Database = "gestion_incidentes",
    [string]$Username = "postgres",
    [string]$Password = "123"
)

Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "  RESTAURACION DE BASE DE DATOS" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Archivo: $BackupFile" -ForegroundColor Yellow
Write-Host "Base de datos: $Database" -ForegroundColor Yellow
Write-Host ""

# Verificar que el archivo existe
if (-not (Test-Path $BackupFile)) {
    Write-Host "ERROR: El archivo de backup no existe: $BackupFile" -ForegroundColor Red
    exit 1
}

# Buscar psql en rutas comunes
$possiblePaths = @(
    "C:\Program Files\PostgreSQL\*\bin\psql.exe",
    "C:\Program Files (x86)\PostgreSQL\*\bin\psql.exe"
)

$psqlPath = $null
foreach ($pattern in $possiblePaths) {
    $found = Get-ChildItem $pattern -ErrorAction SilentlyContinue | Select-Object -First 1
    if ($found) {
        $psqlPath = $found.FullName
        break
    }
}

if ($psqlPath) {
    Write-Host "psql encontrado en: $psqlPath" -ForegroundColor Green
    Write-Host ""
    Write-Host "Ejecutando restauracion..." -ForegroundColor Cyan
    
    $env:PGPASSWORD = $Password
    & $psqlPath -h $Server -U $Username -d $Database -f $BackupFile 2>&1 | Select-Object -Last 20
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "Restauracion completada exitosamente" -ForegroundColor Green
    }
} else {
    Write-Host "No se encontro psql.exe" -ForegroundColor Red
    Write-Host "Busca la instalacion de PostgreSQL manualmente" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "==========================================" -ForegroundColor Cyan
