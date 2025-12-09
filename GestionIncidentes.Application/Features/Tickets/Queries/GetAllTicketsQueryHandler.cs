using MediatR;
using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Application.Features.Tickets.Dtos;

namespace GestionIncidentes.Application.Features.Tickets.Queries;

public class GetAllTicketsQueryHandler : IRequestHandler<GetAllTicketsQuery, List<TicketDto>>
{
    private readonly ITicketRepository _ticketRepo;
    private readonly IUserRepository _userRepo;

    public GetAllTicketsQueryHandler(ITicketRepository ticketRepo, IUserRepository userRepo)
    {
        _ticketRepo = ticketRepo;
        _userRepo = userRepo;
    }

    public async Task<List<TicketDto>> Handle(GetAllTicketsQuery request, CancellationToken cancellationToken)
    {
        var tickets = (await _ticketRepo.ListAllAsync()).ToList();

        var ticketDtos = new List<TicketDto>();

        foreach (var ticket in tickets)
        {
            ticketDtos.Add(new TicketDto(
                Id: ticket.Id,
                Title: ticket.Title,
                Description: ticket.Description ?? "",
                CreatedByUserId: ticket.CreatedByUserId,
                AssignedToUserId: ticket.AssignedToUserId,
                CreatedAt: ticket.CreatedAt,
                Status: ticket.Status
            ));
        }

        // Ordenar por fecha de creación descendente (más recientes primero)
        return ticketDtos.OrderByDescending(t => t.CreatedAt).ToList();
    }
}
