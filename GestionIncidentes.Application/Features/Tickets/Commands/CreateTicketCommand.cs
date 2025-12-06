using MediatR;

namespace GestionIncidentes.Application.Features.Tickets.Commands;

public record CreateTicketCommand(
    string Title,
    string Description,
    Guid? IncidentId = null
) : IRequest<Guid>;
