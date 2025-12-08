using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Domain.Entities;
using GestionIncidentes.Domain.Events;
using MediatR;

namespace GestionIncidentes.Application.Features.Incidents.Commands;

public class AssignIncidentToTechnicianCommandHandler : IRequestHandler<AssignIncidentToTechnicianCommand, Guid>
{
    private readonly IIncidentRepository _incidentRepo;
    private readonly ITicketRepository _ticketRepo;
    private readonly IUserRepository _userRepo;
    private readonly IPublisher _publisher;

    public AssignIncidentToTechnicianCommandHandler(
        IIncidentRepository incidentRepo,
        ITicketRepository ticketRepo,
        IUserRepository userRepo,
        IPublisher publisher)
    {
        _incidentRepo = incidentRepo;
        _ticketRepo = ticketRepo;
        _userRepo = userRepo;
        _publisher = publisher;
    }

    public async Task<Guid> Handle(AssignIncidentToTechnicianCommand request, CancellationToken cancellationToken)
    {
        // Obtener el incidente
        var incident = await _incidentRepo.GetByIdAsync(request.IncidentId)
            ?? throw new InvalidOperationException($"Incidente {request.IncidentId} no encontrado");

        // Verificar que el técnico existe
        var technician = await _userRepo.GetByIdAsync(request.TechnicianId)
            ?? throw new InvalidOperationException($"Técnico {request.TechnicianId} no encontrado");

        // Verificar que no esté ya asignado
        if (incident.AssignedTicketId.HasValue)
            throw new InvalidOperationException($"El incidente ya está asignado al ticket {incident.AssignedTicketId}");

        // Crear el ticket
        var ticket = new Ticket
        {
            Id = Guid.NewGuid(),
            IncidentId = incident.Id, // Asociar al incidente
            Title = incident.Title,
            Description = incident.Description,
            CreatedByUserId = incident.ReportedByUserId,
            AssignedToUserId = request.TechnicianId, // Asignar al técnico
            CreatedAt = DateTime.UtcNow,
            Status = "En proceso",
            Priority = "medium" // Por defecto, se puede calcular después
        };

        await _ticketRepo.AddAsync(ticket);

        // Actualizar el incidente para asociarlo al ticket
        incident.AssignTicket(ticket.Id);
        await _incidentRepo.UpdateAsync(incident);

        // Publicar evento de ticket creado
        await _publisher.Publish(new TicketCreated(ticket.Id, ticket.Title, incident.ReportedByUserId), cancellationToken);

        return ticket.Id;
    }
}
