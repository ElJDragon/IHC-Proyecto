using GestionIncidentes.Application.Interfaces;
using MediatR;

namespace GestionIncidentes.Application.Features.Knowledge;

public class SearchKnowledgeQueryHandler : IRequestHandler<SearchKnowledgeQuery, List<KnowledgeEntryDto>>
{
    private readonly IKnowledgeRepository _knowledgeRepository;

    public SearchKnowledgeQueryHandler(IKnowledgeRepository knowledgeRepository)
    {
        _knowledgeRepository = knowledgeRepository;
    }

    public async Task<List<KnowledgeEntryDto>> Handle(SearchKnowledgeQuery request, CancellationToken cancellationToken)
    {
        var entries = await _knowledgeRepository.SearchAsync(request.SearchTerm);

        // Incrementar contador de uso
        foreach (var entry in entries)
        {
            entry.IncrementUsage();
            await _knowledgeRepository.UpdateAsync(entry);
        }

        return entries.Select(e => new KnowledgeEntryDto
        {
            Id = e.Id,
            Title = e.Title,
            Problem = e.Problem,
            Solution = e.Solution,
            Category = e.Category,
            Tags = e.Tags,
            UsageCount = e.UsageCount,
            CreatedAt = e.CreatedAt
        }).ToList();
    }
}
