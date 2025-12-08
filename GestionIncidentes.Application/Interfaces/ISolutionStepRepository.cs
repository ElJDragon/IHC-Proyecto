using GestionIncidentes.Domain.Entities;

namespace GestionIncidentes.Application.Interfaces;

public interface ISolutionStepRepository
{
    Task<List<SolutionStep>> GetStepsBySolutionIdAsync(Guid solutionId);
    Task<SolutionStep?> GetByIdAsync(Guid id);
    Task<SolutionStep> CreateAsync(SolutionStep step);
    Task UpdateAsync(SolutionStep step);
    Task DeleteAsync(Guid id);
    Task ReorderStepsAsync(Guid solutionId, List<(Guid StepId, int NewOrder)> reorderings);
}
