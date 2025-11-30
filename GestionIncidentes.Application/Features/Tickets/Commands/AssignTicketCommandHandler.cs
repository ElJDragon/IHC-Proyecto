using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Domain.Events;
using MediatR;

namespace GestionIncidentes.Application.Features.Tickets.Commands;

public class AssignTicketCommandHandler : IRequestHandler<AssignTicketCommand, Unit>
{
    private readonly ITicketRepository _ticketRepo;
    private readonly IWorkloadService _workloadService;
    private readonly IPublisher _publisher;

    public AssignTicketCommandHandler(
        ITicketRepository ticketRepo,
        IWorkloadService workloadService,
        IPublisher publisher)
    {
        _ticketRepo = ticketRepo;
        _workloadService = workloadService;
        _publisher = publisher;
    }

    public async Task<Unit> Handle(AssignTicketCommand request, CancellationToken cancellationToken)
    {
        // Validar que el usuario pueda recibir el ticket
        var canAssign = await _workloadService.CanAssignTicketAsync(request.AssigneeUserId);
        if (!canAssign)
            throw new InvalidOperationException("El usuario tiene demasiada carga de trabajo");

        var ticket = await _ticketRepo.GetByIdAsync(request.TicketId);
        if (ticket == null)
            throw new KeyNotFoundException("Ticket no encontrado");

        ticket.UserId = request.AssigneeUserId;
        ticket.Status = "Assigned";

        await _ticketRepo.UpdateAsync(ticket);

        // Recalcular carga de trabajo
        await _workloadService.RecalculateWorkloadAsync(request.AssigneeUserId);

        // Publicar evento
        await _publisher.Publish(new TicketAssigned(ticket.Id, request.AssigneeUserId), cancellationToken);

        return Unit.Value;
    }
}
