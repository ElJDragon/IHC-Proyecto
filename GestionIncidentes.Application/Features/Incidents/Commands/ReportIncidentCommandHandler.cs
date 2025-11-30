using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Domain.Entities;
using GestionIncidentes.Domain.Events;
using MediatR;

namespace GestionIncidentes.Application.Features.Incidents.Commands;

public class ReportIncidentCommandHandler : IRequestHandler<ReportIncidentCommand, Guid>
{
    private readonly IIncidentRepository _incidentRepo;
    private readonly ICurrentUser _currentUser;
    private readonly IPublisher _publisher;

    public ReportIncidentCommandHandler(
        IIncidentRepository incidentRepo,
        ICurrentUser currentUser,
        IPublisher publisher)
    {
        _incidentRepo = incidentRepo;
        _currentUser = currentUser;
        _publisher = publisher;
    }

    public async Task<Guid> Handle(ReportIncidentCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedAccessException("Usuario no autenticado");

        var incident = Incident.Create(
            request.Title,
            request.Description,
            userId
        );

        await _incidentRepo.AddAsync(incident);

        // Publicar evento de dominio
        await _publisher.Publish(new IncidentReported(incident.Id, incident.Title, userId), cancellationToken);

        return incident.Id;
    }
}
