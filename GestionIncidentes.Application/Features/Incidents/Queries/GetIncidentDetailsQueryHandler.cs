using GestionIncidentes.Application.Features.Incidents.Dtos;
using GestionIncidentes.Application.Interfaces;
using MediatR;

namespace GestionIncidentes.Application.Features.Incidents.Queries;

public class GetIncidentDetailsQueryHandler : IRequestHandler<GetIncidentDetailsQuery, IncidentDto?>
{
    private readonly IIncidentRepository _incidentRepo;

    public GetIncidentDetailsQueryHandler(IIncidentRepository incidentRepo)
    {
        _incidentRepo = incidentRepo;
    }

    public async Task<IncidentDto?> Handle(GetIncidentDetailsQuery request, CancellationToken cancellationToken)
    {
        var incident = await _incidentRepo.GetByIdAsync(request.IncidentId);
        
        if (incident == null)
            return null;

        return new IncidentDto(
            incident.Id,
            incident.Title,
            incident.Description,
            incident.ReportedByUserId,
            incident.ReportedAt,
            incident.Status,
            incident.AssignedTicketId
        );
    }
}
