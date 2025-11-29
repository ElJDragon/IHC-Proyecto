using GestionIncidentes.Domain.Entities;

namespace GestionIncidentes.Application.Interfaces;

public interface ITicketReportRepository
{
    Task<TicketReport?> GetByIdAsync(Guid id);
    Task<TicketReport?> GetByTicketIdAsync(Guid ticketId);
    Task<List<TicketReport>> GetByTechnicianIdAsync(Guid technicianId);
    Task<List<TicketReport>> GetAllAsync();
    Task AddAsync(TicketReport report);
    Task UpdateAsync(TicketReport report);
}
