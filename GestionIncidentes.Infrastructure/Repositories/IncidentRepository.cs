using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionIncidentes.Infrastructure.Repositories;

public class IncidentRepository : IIncidentRepository
{
    private readonly GestionIncidentesDbContext _context;

    public IncidentRepository(GestionIncidentesDbContext context)
    {
        _context = context;
    }

    public async Task<Incident?> GetByIdAsync(Guid id)
    {
        return await _context.Set<Incident>().FindAsync(id);
    }

    public async Task<List<Incident>> ListAllAsync()
    {
        return await _context.Set<Incident>()
            .OrderByDescending(i => i.ReportedAt)
            .ToListAsync();
    }

    public async Task<List<Incident>> ListByUserAsync(Guid userId)
    {
        return await _context.Set<Incident>()
            .Where(i => i.ReportedByUserId == userId)
            .OrderByDescending(i => i.ReportedAt)
            .ToListAsync();
    }

    public async Task AddAsync(Incident incident)
    {
        await _context.Set<Incident>().AddAsync(incident);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Incident incident)
    {
        _context.Set<Incident>().Update(incident);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var incident = await GetByIdAsync(id);
        if (incident != null)
        {
            _context.Set<Incident>().Remove(incident);
            await _context.SaveChangesAsync();
        }
    }
}
