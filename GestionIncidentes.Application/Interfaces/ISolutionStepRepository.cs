using GestionIncidentes.Domain.Entities;

namespace GestionIncidentes.Application.Interfaces;

public interface ISolutionStepRepository
{
    Task<List<SolutionStep>> GetStepsByKnowledgeEntryIdAsync(Guid knowledgeEntryId);
    Task<SolutionStep?> GetByIdAsync(Guid id);
    Task<SolutionStep> CreateAsync(SolutionStep step);
    Task UpdateAsync(SolutionStep step);
    Task DeleteAsync(Guid id);
    Task ReorderStepsAsync(Guid knowledgeEntryId, List<(Guid StepId, int NewOrder)> reorderings);
}
