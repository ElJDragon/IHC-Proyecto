using MediatR;

namespace GestionIncidentes.Application.Features.Tickets.Commands;

public record UpdateTicketStatusCommand(Guid TicketId, string NewStatus) : IRequest<Unit>;
