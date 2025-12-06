using GestionIncidentes.Application.Features.Tickets.Dtos;
using GestionIncidentes.Application.Interfaces;
using MediatR;

namespace GestionIncidentes.Application.Features.Tickets.Queries;

public class ListTicketsByAssigneeQueryHandler : IRequestHandler<ListTicketsByAssigneeQuery, List<TicketDto>>
{
    private readonly ITicketRepository _ticketRepo;

    public ListTicketsByAssigneeQueryHandler(ITicketRepository ticketRepo)
    {
        _ticketRepo = ticketRepo;
    }

    public async Task<List<TicketDto>> Handle(ListTicketsByAssigneeQuery request, CancellationToken cancellationToken)
    {
        var tickets = await _ticketRepo.ListByUserAsync(request.AssigneeUserId);

        return tickets.Select(t => new TicketDto(
            t.Id,
            t.Title,
            t.Description,
            t.CreatedByUserId,
            t.UserId,
            t.CreatedAt,
            t.Status
        )).ToList();
    }
}
