using MediatR;

namespace GestionIncidentes.Application.Features.Tickets.Commands;

public record AssignTicketCommand(Guid TicketId, Guid AssigneeUserId) : IRequest<Unit>;
