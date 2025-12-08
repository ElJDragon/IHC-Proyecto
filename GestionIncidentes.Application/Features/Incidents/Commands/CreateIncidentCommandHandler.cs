using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Domain.Entities;
using GestionIncidentes.Domain.Events;
using MediatR;

namespace GestionIncidentes.Application.Features.Incidents.Commands;

public class CreateIncidentCommandHandler : IRequestHandler<CreateIncidentCommand, Guid>
{
    private readonly IIncidentRepository _incidentRepo;
    private readonly ICurrentUser _currentUser;
    private readonly IPublisher _publisher;

    public CreateIncidentCommandHandler(
        IIncidentRepository incidentRepo,
        ICurrentUser currentUser,
        IPublisher publisher)
    {
        _incidentRepo = incidentRepo;
        _currentUser = currentUser;
        _publisher = publisher;
    }

    public async Task<Guid> Handle(CreateIncidentCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedAccessException("Usuario no autenticado");

        var incident = Incident.Create(
            title: request.Title,
            description: request.Description,
            reportedByUserId: userId
        );

        await _incidentRepo.AddAsync(incident);

        // Publicar evento de incidente reportado
        await _publisher.Publish(new IncidentReported(incident.Id, incident.Title, userId), cancellationToken);

        return incident.Id;
    }
}
