using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Domain.Entities;
using MediatR;

namespace GestionIncidentes.Application.Features.Knowledge.Queries;

public class GetSolutionStepsByKnowledgeEntryQueryHandler : IRequestHandler<GetSolutionStepsByKnowledgeEntryQuery, List<SolutionStep>>
{
    private readonly ISolutionStepRepository _repository;

    public GetSolutionStepsByKnowledgeEntryQueryHandler(ISolutionStepRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<SolutionStep>> Handle(GetSolutionStepsByKnowledgeEntryQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetStepsByKnowledgeEntryIdAsync(request.KnowledgeEntryId);
    }
}
