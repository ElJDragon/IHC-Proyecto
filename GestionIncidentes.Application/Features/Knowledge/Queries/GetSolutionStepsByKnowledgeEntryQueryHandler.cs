using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Domain.Entities;
using MediatR;

namespace GestionIncidentes.Application.Features.Knowledge.Queries;

public class GetSolutionStepsByKnowledgeEntryQueryHandler : IRequestHandler<GetSolutionStepsByKnowledgeEntryQuery, List<SolutionStep>>
{
    private readonly ISolutionStepRepository _repository;
    private readonly IKnowledgeRepository _knowledgeRepository;

    public GetSolutionStepsByKnowledgeEntryQueryHandler(
        ISolutionStepRepository repository,
        IKnowledgeRepository knowledgeRepository)
    {
        _repository = repository;
        _knowledgeRepository = knowledgeRepository;
    }

    public async Task<List<SolutionStep>> Handle(GetSolutionStepsByKnowledgeEntryQuery request, CancellationToken cancellationToken)
    {
        // Obtener el KnowledgeEntry para encontrar su SolutionId
        var knowledgeEntry = await _knowledgeRepository.GetByIdAsync(request.KnowledgeEntryId);
        
        if (knowledgeEntry == null || !knowledgeEntry.SolutionId.HasValue)
            return new List<SolutionStep>();
        
        return await _repository.GetStepsBySolutionIdAsync(knowledgeEntry.SolutionId.Value);
    }
}
