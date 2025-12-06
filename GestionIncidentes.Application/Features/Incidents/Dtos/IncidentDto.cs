namespace GestionIncidentes.Application.Features.Incidents.Dtos;

public record IncidentDto(
    Guid Id,
    string Title,
    string Description,
    Guid ReportedByUserId,
    DateTime ReportedAt,
    string Status,
    Guid? AssignedTicketId
);

public record ReportIncidentDto(
    string Title,
    string Description
);
