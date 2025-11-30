using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Domain.Events;
using MediatR;

namespace GestionIncidentes.Application.Features.Tickets.Commands;

public class UpdateTicketStatusCommandHandler : IRequestHandler<UpdateTicketStatusCommand, Unit>
{
    private readonly ITicketRepository _ticketRepo;
    private readonly IPublisher _publisher;

    public UpdateTicketStatusCommandHandler(
        ITicketRepository ticketRepo,
        IPublisher publisher)
    {
        _ticketRepo = ticketRepo;
        _publisher = publisher;
    }

    public async Task<Unit> Handle(UpdateTicketStatusCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _ticketRepo.GetByIdAsync(request.TicketId);
        if (ticket == null)
            throw new KeyNotFoundException("Ticket no encontrado");

        ticket.Status = request.NewStatus;
        await _ticketRepo.UpdateAsync(ticket);

        // Si se resuelve, publicar evento
        if (request.NewStatus == "Resolved")
        {
            await _publisher.Publish(new TicketResolved(ticket.Id), cancellationToken);
        }

        return Unit.Value;
    }
}
