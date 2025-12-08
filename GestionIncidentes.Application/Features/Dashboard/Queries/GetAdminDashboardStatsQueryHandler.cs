using GestionIncidentes.Application.Interfaces;
using MediatR;

namespace GestionIncidentes.Application.Features.Dashboard.Queries;

public class GetAdminDashboardStatsQueryHandler : IRequestHandler<GetAdminDashboardStatsQuery, AdminDashboardStats>
{
    private readonly IIncidentRepository _incidentRepo;
    private readonly ITicketRepository _ticketRepo;
    private readonly IUserRepository _userRepo;

    public GetAdminDashboardStatsQueryHandler(
        IIncidentRepository incidentRepo,
        ITicketRepository ticketRepo,
        IUserRepository userRepo)
    {
        _incidentRepo = incidentRepo;
        _ticketRepo = ticketRepo;
        _userRepo = userRepo;
    }

    public async Task<AdminDashboardStats> Handle(GetAdminDashboardStatsQuery request, CancellationToken cancellationToken)
    {
        var allIncidents = await _incidentRepo.ListAllAsync();
        var allTickets = (await _ticketRepo.ListAllAsync()).ToList(); // Convert to List for LINQ operations
        var allUsers = await _userRepo.GetAllAsync();

        // Métricas de Incidentes
        var totalIncidents = allIncidents.Count;
        var unassignedIncidents = allIncidents.Count(i => !i.AssignedTicketId.HasValue);
        var incidentsLast24h = allIncidents.Count(i => (DateTime.UtcNow - i.ReportedAt).TotalHours < 24);

        // Métricas de Tickets
        var totalTickets = allTickets.Count;
        var activeTickets = allTickets.Count(t => t.Status == "En proceso");
        var pendingTickets = allTickets.Count(t => t.Status != "Resuelto" && t.Status != "Cerrado");
        var resolvedTickets = allTickets.Count(t => t.Status == "Resuelto" || t.Status == "Cerrado");

        // Tiempo promedio de resolución
        var resolvedWithTime = allTickets.Where(t => t.ResolvedAt.HasValue && t.CreatedAt != default).ToList();
        var avgResolutionTime = 0.0;
        if (resolvedWithTime.Any())
        {
            avgResolutionTime = resolvedWithTime.Average(t => (t.ResolvedAt!.Value - t.CreatedAt).TotalHours);
        }

        // Técnicos
        var technicians = allUsers.Where(u => u.Role == "Tecnico").ToList();
        var totalTechnicians = technicians.Count;
        var availableTechnicians = technicians.Count(t =>
        {
            var techTickets = allTickets.Count(ticket => ticket.AssignedToUserId == t.Id && ticket.Status == "En proceso");
            return techTickets < 5; // Disponible si tiene menos de 5 tickets activos
        });

        return new AdminDashboardStats(
            totalIncidents,
            unassignedIncidents,
            incidentsLast24h,
            totalTickets,
            activeTickets,
            pendingTickets,
            resolvedTickets,
            avgResolutionTime,
            totalTechnicians,
            availableTechnicians
        );
    }
}
