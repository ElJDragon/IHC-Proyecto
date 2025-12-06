using MediatR;

namespace GestionIncidentes.Application.Features.Tickets.Commands;

public record AddTicketActionCommand(
    Guid TicketId,
    string Action,
    string Details
) : IRequest<Unit>;
