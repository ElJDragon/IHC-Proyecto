# Ejecutar desde la raíz donde está GestionIncidentes.sln
# Crea carpetas por capa y agrega .gitkeep para que VS las muestre

$paths = @(
  # Domain
  "GestionIncidentes.Domain\Entities",
  "GestionIncidentes.Domain\ValueObjects",
  "GestionIncidentes.Domain\Events",
  "GestionIncidentes.Domain\Common",

  # Application
  "GestionIncidentes.Application\Abstractions",
  "GestionIncidentes.Application\Behaviors",
  "GestionIncidentes.Application\Features\Incidents\Commands",
  "GestionIncidentes.Application\Features\Incidents\Queries",
  "GestionIncidentes.Application\Features\Incidents\Dtos",
  "GestionIncidentes.Application\Features\Tickets\Commands",
  "GestionIncidentes.Application\Features\Tickets\Queries",
  "GestionIncidentes.Application\Features\Tickets\Dtos",
  "GestionIncidentes.Application\Features\Users\Commands",
  "GestionIncidentes.Application\Features\Users\Queries",
  "GestionIncidentes.Application\Features\Users\Dtos",
  "GestionIncidentes.Application\Features\Workload\Commands",
  "GestionIncidentes.Application\Features\Workload\Queries",
  "GestionIncidentes.Application\Features\Workload\Dtos",
  "GestionIncidentes.Application\Features\Knowledge\Commands",
  "GestionIncidentes.Application\Features\Knowledge\Queries",
  "GestionIncidentes.Application\Features\Knowledge\Dtos",
  "GestionIncidentes.Application\Features\Reports\Queries",
  "GestionIncidentes.Application\Features\Reports\Dtos",
  "GestionIncidentes.Application\Features\Notifications\Commands",
  "GestionIncidentes.Application\Features\Notifications\Queries",
  "GestionIncidentes.Application\Features\Notifications\Dtos",

  # Infrastructure
  "GestionIncidentes.Infrastructure\Persistence",
  "GestionIncidentes.Infrastructure\Persistence\Configurations",
  "GestionIncidentes.Infrastructure\Persistence\Migrations",
  "GestionIncidentes.Infrastructure\Identity",
  "GestionIncidentes.Infrastructure\Repositories",
  "GestionIncidentes.Infrastructure\Services",

  # Web (Blazor)
  "GestionIncidentes.Web\Pages\Incidents",
  "GestionIncidentes.Web\Pages\Tickets",
  "GestionIncidentes.Web\Pages\Admin",
  "GestionIncidentes.Web\Pages\Knowledge",
  "GestionIncidentes.Web\Pages\Reports",
  "GestionIncidentes.Web\Pages\Notifications",
  "GestionIncidentes.Web\Components",
  "GestionIncidentes.Web\Auth",
  "GestionIncidentes.Web\Auth\AuthorizationHandlers",
  "GestionIncidentes.Web\Shared",

  # Tests (subcarpetas por tipo de test)
  "GestionIncidentes.Tests\DomainTests",
  "GestionIncidentes.Tests\ApplicationTests",
  "GestionIncidentes.Tests\InfrastructureTests",
  "GestionIncidentes.Tests\WebTests"
)

foreach ($p in $paths) {
  if (!(Test-Path $p)) {
    New-Item -ItemType Directory -Path $p -Force | Out-Null
  }
  $keep = Join-Path $p ".gitkeep"
  if (!(Test-Path $keep)) {
    New-Item -ItemType File -Path $keep -Force | Out-Null
  }
}

Write-Host "Estructura de carpetas creada con .gitkeep en cada una." -ForegroundColor Green