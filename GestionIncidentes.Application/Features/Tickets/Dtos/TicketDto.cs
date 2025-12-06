using GestionIncidentes.Domain.Entities;

namespace GestionIncidentes.Application.Features.Tickets.Dtos;

public record TicketDto(
    Guid Id,
    string Title,
    string Description,
    Guid CreatedByUserId,
    Guid? AssignedToUserId,
    DateTime CreatedAt,
    string Status
);

public record TicketActionDto(
    Guid Id,
    Guid TicketId,
    string Action,
    string PerformedBy,
    DateTime PerformedAt
);
