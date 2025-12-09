using MediatR;

namespace GestionIncidentes.Application.Features.Dashboard.Queries;

public record GetAdminDashboardStatsQuery : IRequest<AdminDashboardStats>;

public record AdminDashboardStats(
    // Métricas de Incidentes
    int TotalIncidents,
    int UnassignedIncidents,
    int IncidentsLast24Hours,
    
    // Métricas de Tickets
    int TotalTickets,
    int ActiveTickets,
    int PendingTickets,
    int ResolvedTickets,
    
    // Tiempos
    double AvgResolutionTimeHours,
    
    // Técnicos
    int TotalTechnicians,
    int AvailableTechnicians
);
