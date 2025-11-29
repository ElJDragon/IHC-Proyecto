using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionIncidentes.Infrastructure.Repositories;

public class TicketReportRepository : ITicketReportRepository
{
    private readonly GestionIncidentesDbContext _context;

    public TicketReportRepository(GestionIncidentesDbContext context)
    {
        _context = context;
    }

    public async Task<TicketReport?> GetByIdAsync(Guid id)
    {
        return await _context.TicketReports.FindAsync(id);
    }

    public async Task<TicketReport?> GetByTicketIdAsync(Guid ticketId)
    {
        return await _context.TicketReports
            .FirstOrDefaultAsync(r => r.TicketId == ticketId);
    }

    public async Task<List<TicketReport>> GetByTechnicianIdAsync(Guid technicianId)
    {
        return await _context.TicketReports
            .Where(r => r.TechnicianId == technicianId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<TicketReport>> GetAllAsync()
    {
        return await _context.TicketReports
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task AddAsync(TicketReport report)
    {
        await _context.TicketReports.AddAsync(report);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TicketReport report)
    {
        _context.TicketReports.Update(report);
        await _context.SaveChangesAsync();
    }
}
