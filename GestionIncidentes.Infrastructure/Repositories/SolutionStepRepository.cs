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

    public async Task<List<SolutionStep>> GetStepsBySolutionIdAsync(Guid solutionId)
    {
        return await _context.SolutionSteps
            .Where(s => s.SolutionId == solutionId)
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

    public async Task ReorderStepsAsync(Guid solutionId, List<(Guid StepId, int NewOrder)> reorderings)
    {
        foreach (var (stepId, newOrder) in reorderings)
        {
            var step = await GetByIdAsync(stepId);
            if (step != null && step.SolutionId == solutionId)
            {
                step.UpdateStepNumber(newOrder);
            }
        }
        await _context.SaveChangesAsync();
    }
}
