using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Domain.Entities;
using GestionIncidentes.Domain.Events;
using MediatR;

namespace GestionIncidentes.Application.Features.Tickets.Commands;

public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, Guid>
{
    private readonly ITicketRepository _ticketRepo;
    private readonly IIncidentRepository _incidentRepo;
    private readonly ICurrentUser _currentUser;
    private readonly IPublisher _publisher;

    public CreateTicketCommandHandler(
        ITicketRepository ticketRepo,
        IIncidentRepository incidentRepo,
        ICurrentUser currentUser,
        IPublisher publisher)
    {
        _ticketRepo = ticketRepo;
        _incidentRepo = incidentRepo;
        _currentUser = currentUser;
        _publisher = publisher;
    }

    public async Task<Guid> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedAccessException("Usuario no autenticado");

        var ticket = new Ticket
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            CreatedByUserId = userId,
            AssignedToUserId = null, // Sin asignar inicialmente
            CreatedAt = DateTime.UtcNow,
            Status = "Open"
        };

        await _ticketRepo.AddAsync(ticket);

        // Si está asociado a un incidente, actualizar el incidente
        if (request.IncidentId.HasValue)
        {
            var incident = await _incidentRepo.GetByIdAsync(request.IncidentId.Value);
            if (incident != null)
            {
                incident.AssignTicket(ticket.Id);
                await _incidentRepo.UpdateAsync(incident);
            }
        }

        // Publicar evento
        await _publisher.Publish(new TicketCreated(ticket.Id, ticket.Title, userId), cancellationToken);

        return ticket.Id;
    }
}
