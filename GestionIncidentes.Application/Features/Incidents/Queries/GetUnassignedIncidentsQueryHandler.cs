using GestionIncidentes.Application.Interfaces;
using MediatR;

namespace GestionIncidentes.Application.Features.Incidents.Queries;

public class GetUnassignedIncidentsQueryHandler : IRequestHandler<GetUnassignedIncidentsQuery, List<UnassignedIncidentDto>>
{
    private readonly IIncidentRepository _incidentRepo;
    private readonly IUserRepository _userRepo;

    public GetUnassignedIncidentsQueryHandler(IIncidentRepository incidentRepo, IUserRepository userRepo)
    {
        _incidentRepo = incidentRepo;
        _userRepo = userRepo;
    }

    public async Task<List<UnassignedIncidentDto>> Handle(GetUnassignedIncidentsQuery request, CancellationToken cancellationToken)
    {
        var incidents = await _incidentRepo.ListAllAsync();
        
        // Filtrar solo incidentes no asignados
        var unassigned = incidents.Where(i => !i.AssignedTicketId.HasValue).ToList();
        
        var result = new List<UnassignedIncidentDto>();
        
        foreach (var incident in unassigned)
        {
            var user = await _userRepo.GetByIdAsync(incident.ReportedByUserId);
            
            result.Add(new UnassignedIncidentDto(
                incident.Id,
                incident.Title,
                incident.Description,
                incident.ReportedByUserId,
                user?.FullName ?? "Usuario desconocido",
                incident.ReportedAt,
                incident.Status
            ));
        }
        
        return result.OrderByDescending(i => i.ReportedAt).ToList();
    }
}
