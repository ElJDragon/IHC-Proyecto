using GestionIncidentes.Application.Features.Incidents.Dtos;
using GestionIncidentes.Application.Interfaces;
using MediatR;

namespace GestionIncidentes.Application.Features.Incidents.Queries;

public class ListIncidentsQueryHandler : IRequestHandler<ListIncidentsQuery, List<IncidentDto>>
{
    private readonly IIncidentRepository _incidentRepo;

    public ListIncidentsQueryHandler(IIncidentRepository incidentRepo)
    {
        _incidentRepo = incidentRepo;
    }

    public async Task<List<IncidentDto>> Handle(ListIncidentsQuery request, CancellationToken cancellationToken)
    {
        var incidents = await _incidentRepo.ListAllAsync();

        return incidents.Select(i => new IncidentDto(
            i.Id,
            i.Title,
            i.Description,
            i.ReportedByUserId,
            i.ReportedAt,
            i.Status,
            i.AssignedTicketId
        )).ToList();
    }
}
