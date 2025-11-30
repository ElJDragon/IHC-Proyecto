using GestionIncidentes.Application.Features.Tickets.Dtos;
using MediatR;

namespace GestionIncidentes.Application.Features.Tickets.Queries;

public record ListTicketsByAssigneeQuery(Guid AssigneeUserId) : IRequest<List<TicketDto>>;
