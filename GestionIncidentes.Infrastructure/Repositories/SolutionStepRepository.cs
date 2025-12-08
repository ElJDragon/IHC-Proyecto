using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Domain.Entities;
using GestionIncidentes.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace GestionIncidentes.Infrastructure.Repositories;

public class SolutionStepRepository : ISolutionStepRepository
{
    private readonly GestionIncidentesDbContext _context;

    public SolutionStepRepository(GestionIncidentesDbContext context)
    {
        _context = context;
    }

    public async Task<List<SolutionStep>> GetStepsByKnowledgeEntryIdAsync(Guid knowledgeEntryId)
    {
        return await _context.SolutionSteps
            .Where(s => s.KnowledgeEntryId == knowledgeEntryId)
            .OrderBy(s => s.StepNumber)
            .ToListAsync();
    }

    public async Task<SolutionStep?> GetByIdAsync(Guid id)
    {
        return await _context.SolutionSteps.FindAsync(id);
    }

    public async Task<SolutionStep> CreateAsync(SolutionStep step)
    {
        _context.SolutionSteps.Add(step);
        await _context.SaveChangesAsync();
        return step;
    }

    public async Task UpdateAsync(SolutionStep step)
    {
        _context.SolutionSteps.Update(step);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var step = await GetByIdAsync(id);
        if (step != null)
        {
            _context.SolutionSteps.Remove(step);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ReorderStepsAsync(Guid knowledgeEntryId, List<(Guid StepId, int NewOrder)> reorderings)
    {
        foreach (var (stepId, newOrder) in reorderings)
        {
            var step = await GetByIdAsync(stepId);
            if (step != null && step.KnowledgeEntryId == knowledgeEntryId)
            {
                step.UpdateStepNumber(newOrder);
            }
        }
        await _context.SaveChangesAsync();
    }
}
