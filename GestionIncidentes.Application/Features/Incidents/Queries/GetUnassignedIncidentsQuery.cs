using MediatR;

namespace GestionIncidentes.Application.Features.Incidents.Queries;

/// <summary>
/// Query para obtener incidentes no asignados (sin ticket)
/// </summary>
public record GetUnassignedIncidentsQuery : IRequest<List<UnassignedIncidentDto>>;

public record UnassignedIncidentDto(
    Guid Id,
    string Title,
    string Description,
    Guid ReportedByUserId,
    string ReportedByUserName,
    DateTime ReportedAt,
    string Status
);
