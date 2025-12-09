using MediatR;

namespace GestionIncidentes.Application.Features.Tickets.Queries;

/// <summary>
/// Query para obtener la carga de trabajo de los técnicos
/// </summary>
public record GetTechnicianWorkloadQuery : IRequest<List<TechnicianWorkloadDto>>;

public record TechnicianWorkloadDto(
    Guid TechnicianId,
    string TechnicianName,
    string Department,
    int ActiveTickets,      // Tickets en proceso
    int PendingTickets,     // Tickets pendientes (sin resolver)
    int ResolvedToday,      // Tickets resueltos hoy
    int TotalResolved       // Total de tickets resueltos
);
