using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionIncidentes.Infrastructure.Repositories;

public class KnowledgeRepository : IKnowledgeRepository
{
    private readonly GestionIncidentesDbContext _context;

    public KnowledgeRepository(GestionIncidentesDbContext context)
    {
        _context = context;
    }

    public async Task<KnowledgeEntry?> GetByIdAsync(Guid id)
    {
        return await _context.KnowledgeEntries.FindAsync(id);
    }

    public async Task<List<KnowledgeEntry>> SearchAsync(string searchTerm)
    {
        var term = searchTerm.ToLower();
        return await _context.KnowledgeEntries
            .Where(k => k.IsPublished &&
                       (k.Title.ToLower().Contains(term) ||
                        k.Problem.ToLower().Contains(term) ||
                        k.SolutionDescription.ToLower().Contains(term) ||
                        k.Tags.Any(t => t.ToLower().Contains(term))))
            .OrderByDescending(k => k.UsageCount)
            .ToListAsync();
    }

    public async Task<List<KnowledgeEntry>> GetByCategoryAsync(string category)
    {
        return await _context.KnowledgeEntries
            .Where(k => k.IsPublished && k.Category == category)
            .OrderByDescending(k => k.UsageCount)
            .ToListAsync();
    }

    public async Task<List<KnowledgeEntry>> GetByTagsAsync(List<string> tags)
    {
        return await _context.KnowledgeEntries
            .Where(k => k.IsPublished && k.Tags.Any(t => tags.Contains(t)))
            .OrderByDescending(k => k.UsageCount)
            .ToListAsync();
    }

    public async Task<List<KnowledgeEntry>> GetAllAsync()
    {
        return await _context.KnowledgeEntries
            .Where(k => k.IsPublished)
            .OrderByDescending(k => k.CreatedAt)
            .ToListAsync();
    }

    public async Task AddAsync(KnowledgeEntry entry)
    {
        await _context.KnowledgeEntries.AddAsync(entry);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(KnowledgeEntry entry)
    {
        _context.KnowledgeEntries.Update(entry);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entry = await GetByIdAsync(id);
        if (entry != null)
        {
            _context.KnowledgeEntries.Remove(entry);
            await _context.SaveChangesAsync();
        }
    }
}
